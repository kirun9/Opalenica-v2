namespace Opalenica.UI;

using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows.Forms;

public partial class UpdateQuestionDialog : Form
{
    public UpdateQuestionDialog()
    {
        InitializeComponent();
        currentVersionLabel.Text = Assembly.GetExecutingAssembly().GetName().Version.ToString();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        using Pen pen = new Pen(Color.White, 2);
        pen.Alignment = PenAlignment.Inset;
        e.Graphics.DrawRectangle(pen, 0, 0, Width, Height);
    }
}
