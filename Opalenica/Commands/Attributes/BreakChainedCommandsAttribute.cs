// Copyright (c) PKMK. All rights reserved.

namespace Opalenica.Commands.Attributes;

using System;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public class BreakChainedCommandsAttribute : Attribute { }