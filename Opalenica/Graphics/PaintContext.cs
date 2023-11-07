// Copyright (c) Krystian Pawełek PKMK. All rights reserved.

using Opalenica.Graphic.Interfaces;

namespace Opalenica.Graphic;

public class PaintContext : IPaintContext
{
    public PaintContext(Boolean debugMode, Graphics g, Size clip, bool pulse, ViewType viewType = ViewType.Normal)
    {
        DebugMode = debugMode;
        this.Graphics = g;
        this.PaintArea = clip;
        this.ViewType = viewType;
        this.Pulse = pulse;
    }

    public Graphics Graphics { get; private set; }
    public Size PaintArea { get; private set; }
    public bool DebugMode { get; private set; }
    public ViewType ViewType { get; private set; }
    public bool Pulse { get; set; }

    public static implicit operator Rectangle(PaintContext context) => new Rectangle(new Point(0, 0), context.PaintArea);
    public static implicit operator Graphics(PaintContext context) => context.Graphics;
}