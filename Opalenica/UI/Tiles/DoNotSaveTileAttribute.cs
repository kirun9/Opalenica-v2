namespace Opalenica.UI.Tiles;

using System;

[AttributeUsage(AttributeTargets.Class, Inherited = true, AllowMultiple = false)]
sealed class DoNotSaveTileAttribute : Attribute
{
    public DoNotSaveTileAttribute() { }
}