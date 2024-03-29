﻿// Copyright (c) Krystian Pawełek PKMK. All rights reserved.

namespace Opalenica.Graphic;

public class ApplicationSettings : Singleton<ApplicationSettings>
{
    public bool CaseSensitiveCommands { get; set; }
    public bool IgnoreMultiMatchElements { get; set; }
    public char CommandSeparatorChar { get; set; }
    public bool DebugMode { get; set; }
    public bool EnableSWDR { get; set; }

    public ApplicationSettings() : base()
    {
        CaseSensitiveCommands = false;
        IgnoreMultiMatchElements = true;
        CommandSeparatorChar = '|';
        DebugMode = true;
        EnableSWDR = false;
    }
}