// Copyright (c) PKMK. All rights reserved.

namespace Opalenica.UI;

using Opalenica.Updater;
using Opalenica.UI;

using System.Reflection;

public partial class MainWindow : Form
{
    public MainWindow(IServiceProvider ServiceProvider)
    {
        InitializeComponent();
    }

    private void MainWindow_Shown(Object sender, EventArgs e)
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

                        }
                    }
                }
            }
        }
    }
}