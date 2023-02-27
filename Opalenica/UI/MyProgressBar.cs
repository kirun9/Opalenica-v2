namespace Opalenica.UI;

using System;
using System.ComponentModel;

public class MyProgressBar : ProgressBar
{
	[Bindable(true)]
	[Browsable(true)]
	[Category("Appearance")]
	[EditorBrowsable(EditorBrowsableState.Always)]
	public ProgressBarDisplayText DisplayStyle { get; set; }

	[Category("Appearance")]
	public ContentAlignment TextAlign { get; set; } = ContentAlignment.TopLeft;

	[Bindable(true)]
	[Browsable(true)]
	[Category("Appearance")]
	[EditorBrowsable(EditorBrowsableState.Always)]
	public override string Text { get; set; }

	[Bindable(true)]
	[Browsable(true)]
	[Category("Appearance")]
	[EditorBrowsable(EditorBrowsableState.Always)]
	public override Font Font { get; set; } = DefaultFont;
	public Color TextColor { get; set; } = Color.Black;

	[Bindable(true)]
	[Browsable(true)]
	[Category("Layout")]
	[EditorBrowsable(EditorBrowsableState.Always)]
	public new Padding Padding { get; set; }

	public MyProgressBar()
	{
		// Modify the ControlStyles flags
		//http://msdn.microsoft.com/en-us/library/system.windows.forms.controlstyles.aspx
		SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer, true);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		Rectangle rect = ClientRectangle;
		Graphics g = e.Graphics;

		ProgressBarRenderer.DrawHorizontalBar(g, rect);
		rect.Inflate(-3, -3);
		if (Value > 0)
		{
			// As we doing this ourselves we need to draw the chunks on the progress bar
			Rectangle clip = new Rectangle(rect.X, rect.Y, (int)Math.Round(((float)Value / Maximum) * rect.Width), rect.Height);
			ProgressBarRenderer.DrawHorizontalChunks(g, clip);
		}

		// Set the Display text (Either a % amount or our custom text
		int percent = (int)(((double)this.Value / (double)this.Maximum) * 100);
		string text = DisplayStyle == ProgressBarDisplayText.Percentage ? percent.ToString() + '%' : Text;

		using Brush brush = new SolidBrush(TextColor);

		SizeF len = g.MeasureString(text, Font);

		PointF location = TextAlign switch
		{
			ContentAlignment.TopLeft => new PointF(Padding.Left, Padding.Top),
			ContentAlignment.MiddleLeft => new PointF(Padding.Left, (Height / 2) - (len.Height / 2)),
			ContentAlignment.BottomLeft => new PointF(Padding.Left, Height - Padding.Bottom - len.Height),

			ContentAlignment.TopCenter => new PointF((Width / 2) - (len.Width / 2), Padding.Top),
			ContentAlignment.MiddleCenter => new PointF((Width / 2) - (len.Width / 2), (Height / 2) - (len.Height / 2)),
			ContentAlignment.BottomCenter => new PointF((Width / 2) - (len.Width / 2), Height - Padding.Bottom - len.Height),

			ContentAlignment.TopRight => new PointF(Width - Padding.Right - len.Width, Padding.Top),
			ContentAlignment.MiddleRight => new PointF(Width - Padding.Right - len.Width, (Height / 2) - (len.Height / 2)),
			ContentAlignment.BottomRight => new PointF(Width - Padding.Right - len.Width, Height - Padding.Bottom - len.Height),
			_ => new PointF(Padding.Left, Padding.Top),
		};

		g.DrawString(text, Font, brush, location);
	}
}

public enum ProgressBarDisplayText
{
	Percentage,
	CustomText
}