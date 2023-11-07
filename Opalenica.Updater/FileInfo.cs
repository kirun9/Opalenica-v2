// Copyright (c) Krystian Pawełek PKMK. All rights reserved.

namespace Opalenica.Updater;

public class FileInfo
{
    public string FileName { get; set; } = string.Empty;
    public string FileHash { get; set; } = string.Empty;
    public string FileDirectory { get; set; } = string.Empty;
    public string FileLocation { get; set; } = string.Empty;
    public string FileLocalLocation => FileDirectory + Path.DirectorySeparatorChar + FileName;
}
