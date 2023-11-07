// Copyright (c) PKMK. All rights reserved.

namespace Opalenica.Commands;

using System;

using Kirun9.CommandParser;

public class ChainedCommandContext : ICommandContext
{
    public CommandInfo Command { get; internal set; }
    public string Content { get; internal set; }
    public CommandType CommandType { get; internal set; }
    public ICommandSender Sender { get; internal set; }
    public ICommandContext PrevContext { get; internal set; }

    public bool IsSameSender => PrevContext?.Sender?.Equals(Sender) ?? false;
    public bool IsSameCommandType => PrevContext?.CommandType == CommandType;

    public bool IsSameCommand
    {
        get
        {
            return PrevContext switch
            {
                ChainedCommandContext ccc => ccc.Command.Name.Equals(Command.Name),
                CommandContext cc => cc.Command.Name.Equals(Command.Name),
                _ => false
            };
        }
    }

    public bool IsSameContent => PrevContext?.Content?.Equals(Content) ?? false;

    public ChainedCommandContext(string content, CommandType commandType, ICommandSender sender, CommandInfo command, ICommandContext prevContext)
    {
        Content = content;
        CommandType = commandType;
        Sender = sender;
        Command = command;
        PrevContext = prevContext;
    }
}
