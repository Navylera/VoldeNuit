namespace VoldeNuit.Framework.Drawing;

public static partial class Draw {

    public const int fa_left   = 0;
    public const int fa_center = 1;
    public const int fa_right  = 2;

    public const int fa_top    = 3;
    public const int fa_middle = 4;
    public const int fa_bottom = 5;

    public static int halign { get; set; } = fa_left;
    public static int valign { get; set; } = fa_top;
}