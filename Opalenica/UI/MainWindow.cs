// Copyright (c) PKMK. All rights reserved.

namespace Opalenica.UI;

using Opalenica.Commands;
using Opalenica.Updater;

using System.Reflection;

using Timer = System.Windows.Forms.Timer;

public partial class MainWindow : Form
{
    private Timer updateTimer;
    private static Screen mainScreen;

    public MainWindow(IServiceProvider ServiceProvider)
    {
        InitializeComponent();
        updateTimer = new Timer() { Interval = 100, Enabled = true };
        updateTimer.Tick += (_, _) => {
            Invalidate(true);
        };
        updateTimer.Start();
    }

    private void MainWindow_Load(object sender, EventArgs e)
    {
        mainScreen = (/*Screen.AllScreens.Where(e => !e.Primary).FirstOrDefault() ??*/ Screen.PrimaryScreen);
        var location = mainScreen.WorkingArea.Location;
        var dimensions = mainScreen.WorkingArea.Size;
        this.Location = new Point(location.X + (dimensions.Width - this.Width) / 2, location.Y + (dimensions.Height - this.Height) / 2);
    }

    private void MainWindow_Shown(object sender, EventArgs e)
    {
        if (Settings.Default.CheckForUpdates)
        {
            if (UpdateManager.IsServerAvaible())
            {
                if (UpdateManager.DownloadUpdateInfo(Assembly.GetEntryAssembly()) is UpdateInfo updateInfo)
                {
                    using UpdateQuestionDialog updateQuestionDialog = new UpdateQuestionDialog();
                    updateQuestionDialog.newVersonLabel.Text = updateInfo.Version;
                    var result = updateQuestionDialog.ShowDialog(this);
                    if (result == DialogResult.Yes)
                    {
                        using UpdateDialog updateDialog = new UpdateDialog(updateInfo);
                        result = updateDialog.ShowDialog(this);
                        if (result == DialogResult.Continue)
                        {
                            // I don't know XD
                            // Maybe not needed ???
                            // Or maybe update cancel implementation ...
                        }
                    }
                }
            }
        }
    }

    private void MainWindow_KeyDown(Object sender, KeyEventArgs e)
    {
        e.Handled = true;
        e.SuppressKeyPress = true;
    }

    private void ExitButton_Click(Object sender, EventArgs e)
    {
        CommandManager.ExecuteCommand("exit", new InternalCommandSender()
        {
            IsAdmin = true
        });
    }
}