// Copyright (c) Krystian Pawełek from PKMK. All rights reserved.

namespace Opalenica;

using System;
using System.Runtime.CompilerServices;

using Kirun9.CommandParser;
using Kirun9.CommandParser.Attributes;

using Opalenica.Commands.Attributes;
using Opalenica.Logging;

public class BasicCommandsModule : ModuleBase<ICommandContext>
{
    public ILogger Logger { get; set; }

    [Command("exit")]
    [Alias("koniec")]
    [Alias("wyjście")]
    [Alias("wyjscie")]
    [ChainedCommand]
    [RepeatCommand(Count = 2)]
    [BreakChainedCommands]
    public void Exit()
    {
        Log("Exiting program ...");
        Application.ExitThread();
        Environment.Exit(0);
    }

    [Command("debugmode")]
    public void DebugMode()
    {
        Program.Settings.DebugMode = !Program.Settings.DebugMode;
        Log("Debug mode is " + (Program.Settings.DebugMode ? "enabled" : "disabled"));
    }

    [Command("debugmode")]
    public void DebugMode(bool value)
    {
        Program.Settings.DebugMode = value;
        Log("Debug mode is " + (Program.Settings.DebugMode ? "enabled" : "disabled"));
    }

    private void Log(string message, [CallerMemberName] string callerName = "")
    {
        Logger.Log(new LogMessage(message, callerName, MessageLevel.Info));
    }
}
