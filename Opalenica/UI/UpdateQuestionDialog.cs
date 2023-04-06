namespace Opalenica.UI;

using Kirun9.CustomUI;

using System.Collections.ObjectModel;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Windows.Forms;

public partial class UpdateQuestionDialog : ResizableForm
{
    public UpdateQuestionDialog()
    {
        InitializeComponent();

        SuspendLayout();
        this.DragControls = new Collection<Control>() { this.label1 };
        ResumeLayout(false);
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