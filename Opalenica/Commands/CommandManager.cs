// Copyright (c) Krystian Pawełek PKMK. All rights reserved.

namespace Opalenica.Commands;

using Kirun9.CommandParser;
using Kirun9.CommandParser.Results;

using Microsoft.Extensions.DependencyInjection;

using Opalenica.Commands.Attributes;
using Opalenica.Elements;
using Opalenica.Logging;

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
        CommandService.AddTypeReader<Element>(new Element.ElementReader());
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

    private static void CommandExecuted(Optional<CommandInfo> command, ICommandContext context, IResult result)
    {
        if (command.IsSpecified)
        {
            CommandExecuted(command.Value, context, result);
        }
        else
        {
            Log("Command not specified.");
        }
    }

    private static void CommandExecuted(CommandInfo command, ICommandContext context, IResult result)
    {
        if (result.IsSuccess)
        {
            Log($"Command {command.Name} executed successfully.");
        }
        else
        {
            Log($"Command {command.Name} failed to execute.");
        }
    }

    private static void Log(string message)
    {
        Program.ServiceProvider.GetService<ILogger>().Log(new LogMessage(message, "CommandHandler", MessageLevel.Info));
    }
}