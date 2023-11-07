// Copyright (c) Krystian Pawełek PKMK. All rights reserved.

namespace Opalenica.Updater;

public static class FileInfoExtension
{
    public static bool Contains(this IEnumerable<FileInfo> list, FileInfo fileInfo)
    {
        return list.Select(e => e.FileLocalLocation).Contains(fileInfo.FileLocalLocation);
    }
}
