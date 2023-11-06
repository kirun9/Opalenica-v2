namespace Opalenica.Graphic.Base;

public struct Colors
{
    public static readonly Color None = Color.Empty;

    public static Color Black { get; } = Color.FromArgb(0, 0, 0);
    public static Color White { get; } = Color.FromArgb(255, 255, 255);
    public static Color Red { get; } = Color.FromArgb(255, 0, 0);
    public static Color Yellow { get; } = Color.FromArgb(255, 255, 0);
    public static Color Green { get; } = Color.FromArgb(0, 255, 0);
    public static Color Cyan { get; } = Color.FromArgb(0, 255, 255);
    public static Color Blue { get; } = Color.FromArgb(0, 0, 255);
    public static Color Pink { get; } = Color.FromArgb(255, 0, 255);
    public static Color Gray { get; } = Color.FromArgb(169, 169, 169);
    public static Color Orange { get; } = Color.FromArgb(255, 169, 0);
    public static Color LightCyan { get; } = Color.FromArgb(0, 169, 169);
    public static Color Azure { get; } = Color.FromArgb(0, 169, 255);
    public static Color DarkRed { get; } = Color.FromArgb(128, 0, 0);

    public struct Pens
    {
        public static readonly Pen None = new Pen(Colors.None);
        public static Pen Black { get; } = new Pen(Colors.Black);
        public static Pen White { get; } = new Pen(Colors.White);
        public static Pen Red { get; } = new Pen(Colors.Red);
        public static Pen Yellow { get; } = new Pen(Colors.Yellow);
        public static Pen Green { get; } = new Pen(Colors.Green);
        public static Pen Cyan { get; } = new Pen(Colors.Cyan);
        public static Pen Blue { get; } = new Pen(Colors.Blue);
        public static Pen Pink { get; } = new Pen(Colors.Pink);
        public static Pen Gray { get; } = new Pen(Colors.Gray);
        public static Pen Orange { get; } = new Pen(Colors.Orange);
        public static Pen LightCyan { get; } = new Pen(Colors.LightCyan);
        public static Pen Azure { get; } = new Pen(Colors.Azure);
        public static Pen DarkRed { get; } = new Pen(Colors.DarkRed);
    }

    public struct Brushes
    {
        public static readonly Brush None = new SolidBrush(Colors.None);
        public static Brush Black { get; } = new SolidBrush(Colors.Black);
        public static Brush White { get; } = new SolidBrush(Colors.White);
        public static Brush Red { get; } = new SolidBrush(Colors.Red);
        public static Brush Yellow { get; } = new SolidBrush(Colors.Yellow);
        public static Brush Green { get; } = new SolidBrush(Colors.Green);
        public static Brush Cyan { get; } = new SolidBrush(Colors.Cyan);
        public static Brush Blue { get; } = new SolidBrush(Colors.Blue);
        public static Brush Pink { get; } = new SolidBrush(Colors.Pink);
        public static Brush Gray { get; } = new SolidBrush(Colors.Gray);
        public static Brush Orange { get; } = new SolidBrush(Colors.Orange);
        public static Brush LightCyan { get; } = new SolidBrush(Colors.LightCyan);
        public static Brush Azure { get; } = new SolidBrush(Colors.Azure);
        public static Brush DarkRed { get; } = new SolidBrush(Colors.DarkRed);
    }
}
