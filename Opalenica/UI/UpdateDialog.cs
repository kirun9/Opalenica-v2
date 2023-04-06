namespace Opalenica.UI;

using Kirun9.CustomUI;

using Opalenica.Updater;

using System.Collections.ObjectModel;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

using DownloadProgressChangedEventArgs = Updater.DownloadProgressChangedEventArgs;

public partial class UpdateDialog : ResizableForm
{
    private UpdateInfo updateInfo;

    public UpdateDialog(UpdateInfo updateInfo)
    {
        this.updateInfo = updateInfo;
        InitializeComponent();
        SuspendLayout();
        this.DragControls = new Collection<Control>() { this.label1 };
        ResumeLayout(false);
        myProgressBar1.Maximum = updateInfo.FileList.Count;
        myProgressBar2.Maximum = 2000;
        myProgressBar2.Text = updateInfo.FileList[0].FileName;
        Timer timer = new Timer();
        timer.Interval = 500;
        timer.Tick += (_, _) =>
        {
            timer.Stop();
            timer.Dispose();
            UpdateManager.DownloadingCompleted += DownloadingCompleted;
            UpdateManager.DownloadingFileCompleted += DownloadFileCompleted;
            UpdateManager.DownloadProgressChanged += DownloadProgressChanged;
            UpdateManager.DownloadUpdate(updateInfo);
        };
        timer.Start();
    }

    private void DownloadProgressChanged(object? sender, DownloadProgressChangedEventArgs e)
    {
        if (InvokeRequired)
        {
            Invoke(DownloadProgressChanged, sender, e);
            return;
        }
        var value = (int)(((float)e.BytesReceived / (float)e.TotalBytesToReceive) * 2000);
        if (value > 2000) value = 2000;
        if (value < 0) value = 0;
        myProgressBar2.Value = value;
        myProgressBar2.Maximum = 2000;
        myProgressBar2.Invalidate();
    }

    private void DownloadFileCompleted(object? sender, EventArgs e)
    {
        if (InvokeRequired)
        {
            Invoke(DownloadFileCompleted, sender, e);
            return;
        }
        myProgressBar1.Value++;
        myProgressBar2.Text = updateInfo.FileList[myProgressBar1.Value - 1].FileName;
        myProgressBar1.Invalidate();
    }

    private void DownloadingCompleted(object? sender, EventArgs e)
    {
        if (InvokeRequired)
        {
            Invoke(DownloadingCompleted, sender, e);
            return;
        }
        UpdateManager.DownloadingCompleted -= DownloadingCompleted;
        UpdateManager.DownloadingFileCompleted -= DownloadFileCompleted;
        UpdateManager.DownloadProgressChanged -= DownloadProgressChanged;
        UpdateManager.FinishUpdate(updateInfo);
        Application.Exit();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        using Pen pen = new Pen(Color.White, 2);
        pen.Alignment = PenAlignment.Inset;
        e.Graphics.DrawRectangle(pen, 0, 0, Width, Height);
    }
}
