// Copyright (c) Krystian Pawełek PKMK. All rights reserved.

namespace Opalenica.Graphic.Interfaces;

using System.Collections.Generic;
using System.Drawing;

public interface ITile
{
    Point Location { get; }
    Dictionary<string, Point> Locations { get; set; }
    string Name { get; set; }
    Size TileSize { get; set; }
    string ViewID { get; set; }
    IEnumerable<string> ViewIDs { get; }

    void Paint(PaintContext context);
}