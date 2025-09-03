namespace VoldeNuit.Framework;

using static Heart;

public static class Configuration {

    internal static int QTREE_MINSIZE { get; set; } = 8;

    internal static float EPSILON { get; set; } = .0001f;
    
    public static MidpointRounding ROUNDING { get; set; } = MidpointRounding.AwayFromZero;

    internal static bool PRINT_STACKTRACE { get; set; } = true;
    
    public static ColorFormat COLOR_FORMAT { get; set; } = ColorFormat.ARGB;
    public enum ColorFormat {

        ARGB,
        BGRA,
        ABGR
    }

    public static AngleFormat ANGLE_FORMAT { get; set; } = AngleFormat.RADIAN;
    public enum AngleFormat {

        RADIAN,
        DEGREE,
        LEGACY
    }

    public static string CONTENT_PATH { get; set; } = $".{separator}Content{separator}";
}