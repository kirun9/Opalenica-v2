namespace Opalenica.Graphic.Base.Interfaces;

using System.Drawing;

public interface IPaintContext
{
    bool DebugMode { get; }
    Graphics Graphics { get; }
    Size PaintArea { get; }
    bool Pulse { get; set; }
    ViewType ViewType { get; }
}