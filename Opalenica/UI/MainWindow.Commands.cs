// Copyright (c) PKMK. All rights reserved.

namespace Opalenica.UI;

using Kirun9.CommandParser;
using Kirun9.CommandParser.Attributes;

using Opalenica.Commands;

public class MainWindowCommands : ModuleBase<ICommandContext>
{
    [Command("Fullscreen")]
    [Alias("fs")]
    [DebugCommand]
    public void Fullscreen()
    {
        Program.MainWindow.WindowState = Program.MainWindow.WindowState switch
        {
            FormWindowState.Maximized => FormWindowState.Normal,
            FormWindowState.Minimized => FormWindowState.Maximized,
            FormWindowState.Normal    => FormWindowState.Maximized,
            _                         => FormWindowState.Maximized
        };
    }

    [Command("RepaintScreen")]
    [DebugCommand]
    public void RepaintScreen()
    {
        Program.MainWindow.Invoke(() => Program.MainWindow.Invalidate(true));
    }
}