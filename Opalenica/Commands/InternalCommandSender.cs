// Copyright (c) Krystian Pawełek PKMK. All rights reserved.

namespace Opalenica.Commands;

using Kirun9.CommandParser;

internal class InternalCommandSender : ICommandSender
{
    public string ID { get; set; }
    public bool IsAdmin { get; internal set; } = false;
}
