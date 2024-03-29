﻿// Copyright (c) Krystian Pawełek PKMK. All rights reserved.

namespace Opalenica.Updater;

using System.Diagnostics;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

using FInfo = System.IO.FileInfo;

public class UpdateManager
{
    public delegate void DownloadProgressChangedEventHandler(object? sender, DownloadProgressChangedEventArgs e);

    private static readonly string UpdateURL = "http://kirun9.com/programs/";

    public static event DownloadProgressChangedEventHandler? DownloadProgressChanged;
    public static event EventHandler? DownloadingFileCompleted;
    public static event EventHandler? DownloadingCompleted;

    public static bool IsServerAvaible()
    {
        try
        {
            using HttpClient client = new HttpClient();
            HttpRequestMessage message = new HttpRequestMessage();
            message.RequestUri = new Uri(UpdateURL + "/updates.xml");
            return client.Send(message).IsSuccessStatusCode;
        }
        catch { return false; }
    }

    public static UpdateInfo? DownloadUpdateInfo(Assembly assembly)
    {
        try
        {
            XmlSerializer serializer = new XmlSerializer(typeof(UpdatesXml));
            XmlReader reader = XmlReader.Create(UpdateURL + "/updates.xml");
            if (serializer.CanDeserialize(reader))
            {
                if (serializer.Deserialize(reader) is not UpdatesXml updateData)
                    throw new ApplicationException("Readed data is not in correct format. Unsuccessful parse");
                // Select only updateInfo that we are interested in (from the same application)
                var info = updateData.Updates.Where(e => e.ApplicationID == assembly.GetName().Name);
                // Sort versions
                info = info.OrderByDescending(e => e.Version);
                if (info.First() is UpdateInfo newestInfo and not null)
                    if (new Version(newestInfo.Version) > assembly.GetName().Version) return newestInfo;
                return null;
            }
            throw new XmlException("Cannot deserialize XML data");
        }
        catch (Exception ex)
        {
            throw new Exception("Something went wrong white getting update information. Anyway, here are details: " + ex.Message, ex);
        }
    }

    public static void DownloadUpdate(UpdateInfo info)
    {
        Thread thread = new Thread(() =>
        {
            var tempLocation = GetCurrentLocation();
            var baseUri = new Uri(UpdateURL);
            var updateUri = new Uri(baseUri, info.Location);
            foreach (var file in info.FileList)
            {
                var fileInfo = new FInfo(Path.Combine(tempLocation, file.FileName));
                if (!fileInfo.Directory?.Exists ?? false) fileInfo.Directory?.Create();
                var fileStream = fileInfo.Create();
                fileStream.Close();
                fileStream.Dispose();
                var fileUri = new Uri(updateUri, file.FileLocation);
                DownloadFile(fileUri, Path.Combine(tempLocation, file.FileLocalLocation), file.FileHash, HashType.MD5).GetAwaiter().GetResult();
                DownloadingFileCompleted?.Invoke(null, new EventArgs());
            }
            DownloadingCompleted?.Invoke(null, new EventArgs());
        });
        thread.Start(); // VERY VERY IMPORTANT
    }

    private static async Task DownloadFile(Uri uri, string fileName, string expectedHash, HashType hashType, int attempt = 0)
    {
        if (attempt >= 5)
        {
            throw new Exception("Cannot complete downloading of file :" + fileName + " . Reason: mismatch hashes");
        }
        using HttpClient client = new HttpClient();
        client.Timeout = TimeSpan.FromMinutes(10);
        using HttpResponseMessage response = client.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead).Result;
        response.EnsureSuccessStatusCode();
        var total = response.Content.Headers.ContentLength ?? -1L;
        var canReportProgress = total != -1;
        using Stream contentStream = await response.Content.ReadAsStreamAsync(), fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write, FileShare.ReadWrite, 8192);

        var totalRead = 0L;
        var totalReads = 0L;
        var buffer = new byte[8192];
        var isMoreToRead = true;
        do
        {
            var read = await contentStream.ReadAsync(buffer, 0, buffer.Length);
            if (read == 0)
            {
                isMoreToRead = false;
                continue;
            }
            fileStream.Write(buffer, 0, read);
            totalRead += read;
            totalReads++;
            if (totalReads % 2000 == 0 || canReportProgress)
            {
                DownloadProgressChanged?.Invoke(null, new DownloadProgressChangedEventArgs(totalRead, total));
            }
            await Task.Delay(1);
        }
        while (isMoreToRead);
        fileStream.Close();
        var hash = Hasher.HashFile(fileName, hashType);
        if (hash != expectedHash)
        {
            await DownloadFile(uri, fileName, expectedHash, hashType, ++attempt);
        }
    }

    public static void FinishUpdate(UpdateInfo info)
    {
        var currentPath = GetCurrentLocation();
        var tempPath = Path.Combine(currentPath, "temp");
        var startFile = Path.Combine(currentPath, info.StartFileInfo.FileLocalLocation);

        string[] args = new[]
        {
            currentPath,
            tempPath,
            Path.GetFileName(startFile),
            ""
        };

        ProcessStartInfo p = new ProcessStartInfo();
        p.Arguments = CommandFormatter(args);
        p.WindowStyle = ProcessWindowStyle.Hidden;
        p.CreateNoWindow = true;
        p.FileName = "cmd.exe";
        Process.Start(p);
    }

    private static string CommandFormatter(string[] args)
    {
        string TimeoutCommand(int time)
        {
            return $"choice /C Y /N /D Y /T {time}";
        }

        string MoveCommand(string from, string to)
        {
            return $"move /y \"{from}\" \"{to}\"";
        }

        string DelCommand(string location)
        {
            return $"del /f /q \"{location}\"";
        }

        string RDCommand(string location)
        {
            return $"rd /s /q \"{location}\"";
        }

        string StartCommand(string dir, string file, string arg)
        {
            return $"start \"\" /d \"{dir}\" \"{file}\" \"{arg}\"";
        }

        List<string> commands = new List<string>();
        commands.Add(TimeoutCommand(2));
        commands.Add(DelCommand(args[0]));
        foreach (var systemInfo in new DirectoryInfo(args[1]).GetFileSystemInfos())
        {
            commands.Add(MoveCommand(systemInfo.FullName, args[0]));
        }
        commands.Add(RDCommand(args[1]));
        commands.Add(StartCommand(args[0], args[2], args[3]));

        return "/C " + String.Join(" & ", commands.ToArray());
    }

    private static string GetCurrentLocation()
    {
        var currentPath = new FInfo(Assembly.GetEntryAssembly()?.Location ?? "./").Directory?.FullName;
        if (currentPath is null) throw new NullReferenceException("Cannot get current path");
        return currentPath;
    }
}