// Copyright (c) PKMK. All rights reserved.

namespace Opalenica.Commands;

using System;

using Kirun9.CommandParser;

public class CommandContext : ICommandContext
{
    public CommandInfo Command { get; internal set; }
    public string Content { get; internal set; }
    public CommandType CommandType { get; internal set; }
    public ICommandSender Sender { get; internal set; }
    public CommandContext(string content, CommandType commandType, ICommandSender sender, CommandInfo command)
    {
        Content = content;
        CommandType = commandType;
        Sender = sender;
        Command = command;
    }
}
