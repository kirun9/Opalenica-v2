// Copyright (c) Krystian Pawełek PKMK. All rights reserved.

namespace Opalenica.Graphic.Interfaces;

using Opalenica.Elements;

public interface IHasElements
{
    public Element[] GetElements();

    public bool IsSelected();
}