namespace Opalenica.Updater;

public class FileInfo
{
    public string FileName { get; set; }
    public string FileHash { get; set; }
    public string FileDirectory { get; set; }
    public string FileLocation { get; set; }
    public string FileLocalLocation => FileDirectory + Path.DirectorySeparatorChar + FileName;
}
