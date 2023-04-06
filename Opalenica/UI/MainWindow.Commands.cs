// Copyright (c) PKMK. All rights reserved.

namespace Opalenica.UI;

using Kirun9.CommandParser;
using Kirun9.CommandParser.Attributes;

using Opalenica.Commands;

public class MainWindowCommands : ModuleBase<ICommandContext>
{
    public static MainWindow MainWindow { get; set; }

    [Command("Fullscreen")]
    [Alias("fs")]
    [DebugCommand]
    public void Fullscreen()
    {
        MainWindow.WindowState = MainWindow.WindowState switch
        {
            FormWindowState.Maximized => FormWindowState.Normal,
            FormWindowState.Minimized => FormWindowState.Maximized,
            FormWindowState.Normal    => FormWindowState.Maximized,
            _                         => FormWindowState.Maximized
        };
    }
}