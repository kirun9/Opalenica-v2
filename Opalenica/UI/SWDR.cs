namespace Opalenica.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

public partial class SWDRForm : Form
{
    private static Screen mainScreen;

    public SWDRForm(IServiceProvider ServiceProvider)
    {
        InitializeComponent();
    }

    private void SWDRForm_Load(object sender, EventArgs e)
    {
        mainScreen = (Screen.AllScreens.Where(e => !e.Primary).FirstOrDefault());
        if (mainScreen is null)
        {
            Close();
            return;
        }
        var location = mainScreen.WorkingArea.Location;
        var dimensions = mainScreen.WorkingArea.Size;
        this.Location = new Point(location.X + (dimensions.Width - this.Width) / 2, location.Y + (dimensions.Height - this.Height) / 2);
    }
}
