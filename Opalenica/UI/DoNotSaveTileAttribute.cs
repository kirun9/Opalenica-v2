// Copyright (c) Krystian Pawełek PKMK. All rights reserved.

namespace Opalenica.UI;

using System;

[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
sealed class DoNotSaveTileAttribute : Attribute
{
    public DoNotSaveTileAttribute() { }
}