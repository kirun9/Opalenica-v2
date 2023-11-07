// Copyright (c) Krystian Pawełek PKMK. All rights reserved.

namespace Opalenica.Updater;

public class DownloadProgressChangedEventArgs
{
    internal DownloadProgressChangedEventArgs(long bytesReceived, long totalBytesToReceive)
    {
        BytesReceived = bytesReceived;
        TotalBytesToReceive = totalBytesToReceive;
    }

    public long BytesReceived { get; }
    public long TotalBytesToReceive { get; }
}