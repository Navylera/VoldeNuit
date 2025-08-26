namespace VoldeNuit.Framework.Drawing;

public static partial class Draw {

    public const int HorizontalAlignLeft   = 0;
    public const int HorizontalAlignCenter = 1;
    public const int HorizontalAlignRight  = 2;

    public const int VerticalAlignTop    = 3;
    public const int VerticalAlignMiddle = 4;
    public const int VerticalAlignBottom = 5;

    public static int HorizontalAlign { get => halign; set => halign = value; }
    public static int VerticalAlign { get => valign; set => valign = value; }
}