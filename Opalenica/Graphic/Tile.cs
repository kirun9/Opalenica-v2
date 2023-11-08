// Copyright (c) Krystian Pawełek PKMK. All rights reserved.

namespace Opalenica.Graphic;

using Newtonsoft.Json;

using Opalenica.Graphic.Interfaces;
using Opalenica.UI;
using System.Drawing;

public class Tile
{
    public string Name { get; set; }
    public Dictionary<string, Point> Locations { get; set; } = new Dictionary<string, Point>();
    [JsonIgnore]
    public IEnumerable<string> ViewIDs => Locations.Keys;
    [JsonIgnore]
    public string ViewID { get; set; }
    [JsonIgnore]
    public Point Location => ViewID is not "" or null ? Locations[ViewID] : new Point(0, 0);
    public Size TileSize { get; set; } = new Size(1, 1);

    public virtual void Paint(PaintContext context)
    {
        if (this is IHasElements element)
        {
            if (element.IsSelected())
            {
                context.Graphics.DrawRectangle(Colors.Pens.Cyan, context);
            }
        }
    }

    public void SetContext(TileView view)
    {
        ViewID = view.ViewID;
    }

    public bool CheckContext(TileView view)
    {
        return view.ViewID == ViewID;
    }
}