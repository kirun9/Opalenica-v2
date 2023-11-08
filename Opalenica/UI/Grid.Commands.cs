// Copyright (c) Krystian Pawełek PKMK. All rights reserved.

namespace Opalenica.UI;

using Kirun9.CommandParser;
using Kirun9.CommandParser.Attributes;

using Opalenica.Commands.Attributes;

public class GridCommands : ModuleBase<ICommandContext>
{
    [Command("reload grid")]
    [Alias("rlg")]
    [DebugCommand]
    public void ReloadGrid()
    {
        var grid = Grid.Instance;
        TileViewManager.UnregisterViews();
        TileViewManager.ReadViews();
        //grid.Load(grid.CurrentView);
    }
}