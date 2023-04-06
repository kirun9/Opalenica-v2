// Copyright (c) Krystian Pawełek PKMK. All rights reserved.

namespace Opalenica.Commands;

using Kirun9.CommandParser;
using Kirun9.CommandParser.Results;

using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

public static class CommandManager
{
    private static Settings Settings { get; set; }
    private static CommandService CommandService { get; set; }
    private static ICommandContext PrevContext { get; set; }

    internal static void Initialize(Settings settings)
    {
        Settings = settings;
        var commandServiceConfig = new CommandServiceConfig();
        commandServiceConfig.CaseSensitiveCommands = settings.CaseSensitiveCommands;
        commandServiceConfig.IgnoreExtraArgs = false;
        commandServiceConfig.ThrowOnError = false;
        CommandService = new CommandService(commandServiceConfig);
        Program.ServiceCollection.AddSingleton(CommandService);
        CommandService.CommandExecuted += CommandExecuted;
        CommandService.AddModules(typeof(Program).Assembly, Program.ServiceProvider);
        CommandService.AddTypeReader<Element>(new ElementReader());
    }

    public static IReadOnlyCollection<IResult> ExecuteCommand(string input)
    {
        return ExecuteCommand(input, null);
    }

    internal static IReadOnlyCollection<IResult> ExecuteCommandAsAdmin(string input)
    {
        return ExecuteCommand(input.Split(Settings.CommandSeparatorChar, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries), new InternalCommandSender() { ID = "Internal", IsAdmin = true });
    }

    internal static IReadOnlyCollection<IResult> ExecuteCommand(string input, ICommandSender sender)
    {
        return ExecuteCommand(input.Split(Settings.CommandSeparatorChar, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries), sender);
    }

    private static IReadOnlyCollection<IResult> ExecuteCommand(string[] commands, ICommandSender sender)
    {
        List<IResult> results = new List<IResult>();
        foreach (var command in commands)
        {
            var searchResult = CommandService.Search(command);
            if (!searchResult.IsSuccess)
            {
                results.Add(searchResult);
                PrevContext = null;
                continue;
            }

            var foundCommands = searchResult.Commands.OrderByDescending(e => e.Command.Priority);
            var parsedCommands = foundCommands.Select(foundCommand =>
            {
                bool chainedCommand = false;
                foreach (var attribute in foundCommand.Command.Attributes)
                {
                    if (attribute is ChainedCommandAttribute cca) chainedCommand = cca.IsChained;
                }
                ICommandContext context = chainedCommand
                        ? new ChainedCommandContext(command, CommandType.User, sender, foundCommand.Command, PrevContext)
                        : new CommandContext(command, CommandType.User, sender, foundCommand.Command);
                var commandPreconditionResult = foundCommand.CheckPreconditions(context, Program.ServiceProvider);
                var score = (commandPreconditionResult.IsSuccess ? foundCommand.Command.Priority : -1);
                if (commandPreconditionResult is StoppedPreconditionResult and { IsSuccess: false })
                    score = -2;
                var parseResult = foundCommand.Parse(context, SearchResult.FromSuccess(command, foundCommands.ToImmutableArray()));
                return (score, foundCommand, context, parseResult);
            }).OrderByDescending((e) => e.score).Where(e => e.score >= 0 || e.score == -2);

            if (parsedCommands.Count() is > 1)
            {
                if (!Settings.IgnoreMultiMatchElements)
                {
                    results.Add(SearchResult.FromError(CommandError.MultipleMatches, "Found multiple matched commands"));
                    PrevContext = null;
                    continue;
                }
            }
            if (parsedCommands.Count() is 0)
            {
                results.Add(SearchResult.FromError(CommandError.UnknownCommand, "Command not found"));
                continue;
            }

            var selectedCommand = parsedCommands.First();
            if (selectedCommand.score is -2)
            {
                if (selectedCommand.context is ChainedCommandContext)
                    PrevContext = selectedCommand.context;
                else
                    PrevContext = null;
                results.Add(selectedCommand.parseResult);
                continue;
            }
            var executeResult = selectedCommand.foundCommand.Execute(selectedCommand.context, selectedCommand.parseResult, Program.ServiceProvider);
            if (executeResult.IsSuccess)
                if (selectedCommand.context is ChainedCommandContext)
                    PrevContext = selectedCommand.foundCommand.Command.HasAttribute<BreakChainedCommandsAttribute>() ? null : selectedCommand.context;
                else
                    PrevContext = null;
            else PrevContext = null;
            results.Add(executeResult);
        }
        return results;
    }

    /*internal static void ExecuteCommand(string input)
    {
        var commands = CommandService.Search(input);
        if (commands.IsSuccess)
        {
            var command = commands.Commands.First();
            var isChained = false;
            foreach (var attribute in command.Command.Attributes)
            {
                if (attribute is ChainedCommandAttribute)
                    isChained = isChained || attribute is ChainedCommandAttribute;
            }
            ICommandContext context = isChained
                ? new ChainedCommandContext(input, CommandType.User, null, command.Command, prevContext)
                : new CommandContext(input, CommandType.User, null, command.Command);
            if (context is ChainedCommandContext ccc)
            {
                if (ccc.IsSameCommand)
                {

                }
            }
            try
            {
                var commandResult = CommandService.Execute(context, 0, Program.ServiceProvider);
                if (!commandResult.IsSuccess)
                {
                    if (commandResult is ExecuteResult executeResult)
                    {
                        var ex = executeResult.Exception;
                        Console.Error.WriteLine($"Exception in {ex.TargetSite.DeclaringType.Name} inside {ex.TargetSite.DeclaringType.Assembly.GetName()}\n{typeof(Program).FullName}: {ex.Message}\nCommand: {context.Content}\nBy: {context.Sender?.ID}\nError Reason: {commandResult.ErrorMessage}");
#if DEBUG
                        Debugger.Break();
#endif
                    }
                    else
                    {
                        Console.Error.WriteLine($"Unrecognized exception:\nCommand: {context.Content}\nBy: {context.Sender?.ID}\nError Reason: {commandResult.ErrorMessage}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Exception in {ex.TargetSite.DeclaringType.Name} inside {ex.TargetSite.DeclaringType.Assembly.GetName().Name}:\n{typeof(Program).FullName}: {ex.Message}\nCommand: {context.Content}\nBy: {context.Sender?.ID}");
#if DEBUG
                Debugger.Break();
#endif
            }
        }
    }*/

    private static void CommandExecuted(Optional<CommandInfo> command, ICommandContext context, IResult result)
    {
        if (command.IsSpecified)
        {
            CommandExecuted(command.Value, context, result);
        }
        else
        {
            Console.WriteLine("Command not specified.");
        }
    }

    private static void CommandExecuted(CommandInfo command, ICommandContext context, IResult result)
    {
        if (result.IsSuccess)
        {
            Console.WriteLine($"Command {command.Name} executed successfully.");
        }
        else
        {
            Console.WriteLine($"Command {command.Name} failed to execute.");
        }
    }
}

public static class CommandInfoExtensions
{
    public static bool HasAttribute<T>(this CommandInfo commandInfo)
    {
        foreach (var attribute in commandInfo.Attributes)
        {
            if (attribute is T) return true;
        }
        return false;
    }

    public static T GetAttribute<T>(this CommandInfo commandInfo) where T : Attribute
    {
        foreach (var attribute in commandInfo.Attributes)
        {
            if (attribute is T) return attribute as T;
        }
        return null;
    }

    public static IEnumerable<T> GetAttributes<T>(this CommandInfo commandInfo) where T : Attribute
    {
        foreach (var attribute in commandInfo.Attributes)
        {
            if (attribute is T) yield return attribute as T;
        }
    }
}