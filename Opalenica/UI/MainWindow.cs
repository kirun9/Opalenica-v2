// Copyright (c) PKMK. All rights reserved.

namespace Opalenica.UI;

using Kirun9.CommandParser.Results;

using Microsoft.Extensions.DependencyInjection;

using Opalenica.Commands;
using Opalenica.Logging;
using Opalenica.Updater;

using System.Reflection;

using Timer = System.Windows.Forms.Timer;

public partial class MainWindow : Form
{
    private Timer updateTimer;
    private static Screen mainScreen;
    private SWDRForm swdrForm;

#if EFU
    private Timer EFUTimer;
#endif

    public MainWindow(IServiceProvider ServiceProvider)
    {
        InitializeComponent();
        updateTimer = new Timer() { Interval = 100, Enabled = true };
        updateTimer.Tick += (_, _) => {
            Invalidate(true);
        };
        updateTimer.Start();

#if EFU
        EFUTimer = new Timer() { Interval = (int) TimeSpan.FromMinutes(10).TotalMilliseconds, Enabled = true };
        EFUTimer.Tick += (_, _) => { CheckForUpdate(); };
        EFUTimer.Start();
#endif

        if (Settings.Default.EnableSWDR)
        {
            ServiceProvider.GetService<ILogger>().Log(new LogMessage("Starting SWDR ...", Program.LOG_SOURCE, MessageLevel.Info));
            swdrForm = new SWDRForm(ServiceProvider);
            swdrForm.Show();
        }
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
#if !EFU
        CheckForUpdate();
#endif
    }

    private void CheckForUpdate()
    {
        if (Settings.Default.CheckForUpdates)
        {
            if (UpdateManager.IsServerAvaible())
            {
                if (UpdateManager.DownloadUpdateInfo(Assembly.GetEntryAssembly()) is UpdateInfo updateInfo)
                {
                    using UpdateQuestionDialog updateQuestionDialog = new UpdateQuestionDialog();
                    updateQuestionDialog.newVersonLabel.Text = updateInfo.Version;
#if EFU
                    if (updateInfo.ForcedUpdate)
                    {
                        PerformUpdate(updateInfo);
                        return;
                    }
#endif
                    var result = updateQuestionDialog.ShowDialog(this);
                    if (result == DialogResult.Yes)
                    {
                        PerformUpdate(updateInfo);
                    }
                }
            }
        }
    }

    private void PerformUpdate(UpdateInfo updateInfo)
    {
        using UpdateDialog updateDialog = new UpdateDialog(updateInfo);
        var result = updateDialog.ShowDialog(this);
        if (result == DialogResult.Continue)
        {
            // I don't know XD
            // Maybe not needed ???
            // Or maybe update cancel implementation ...
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

    internal void ToggleSWDRForm()
    {
        if (swdrForm is not null)
        {
            swdrForm.Hide();
            swdrForm.Close();
            swdrForm.Dispose();
            swdrForm = null;
        }
        else
        {
            swdrForm = new SWDRForm(Program.ServiceProvider);
            swdrForm.Show(this);
        }
    }
}