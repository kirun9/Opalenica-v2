// Copyright (c) Krystian Pawełek PKMK. All rights reserved.

namespace Opalenica.Graphic.Interfaces;

using System.Drawing;
using Opalenica.UI;

public interface IPaintContext
{
    bool DebugMode { get; }
    Graphics Graphics { get; }
    Size PaintArea { get; }
    bool Pulse { get; set; }
    ViewType ViewType { get; }
}