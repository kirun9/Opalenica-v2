namespace Opalenica.UI;

using Opalenica.Updater;

using System.Net;
using System.Windows.Forms;

using DownloadProgressChangedEventArgs = Updater.DownloadProgressChangedEventArgs;

public partial class UpdateDialog : Form
{
    private UpdateInfo updateInfo;

    public UpdateDialog(UpdateInfo updateInfo)
    {
        this.updateInfo = updateInfo;
        InitializeComponent();
        myProgressBar1.Maximum = updateInfo.FileList.Count;
        myProgressBar2.Maximum = 2000;
        myProgressBar2.Text = updateInfo.FileList[0].FileName;
        Timer timer = new Timer();
        timer.Interval = 500;
        timer.Tick += (_, _) =>
        {
            timer.Stop();
            timer.Dispose();
            UpdateManager.DownloadUpdate(updateInfo, DownloadProgressChanged, DownloadFileCompleted, DownloadingCompleted);
        };
        timer.Start();
    }

    private void DownloadProgressChanged(object? sender, DownloadProgressChangedEventArgs e)
    {
        var value = (int)((e.BytesReceived / e.TotalBytesToReceive) * 2000);
        if (value > 2000) value = 2000;
        if (value < 0) value = 0;
        myProgressBar2.Value = value;
        myProgressBar2.Maximum = 2000;
    }

    private void DownloadFileCompleted(object? sender, EventArgs e)
    {
        myProgressBar1.Value++;
        myProgressBar2.Text = updateInfo.FileList[myProgressBar1.Value - 1].FileName;
    }

    private void DownloadingCompleted(object? sender, EventArgs e)
    {
        UpdateManager.FinishUpdate(updateInfo);
        Application.Exit();
    }
}
