namespace VoldeNuit.Framework.Calculation;

public static partial class Calculation {

    public static float GetDirection(float x1, float y1, float x2, float y2) {

        return point_direction(x1, y1, x2, y2);
    }

    public static float GetDistance(float x1, float y1, float x2, float y2) {

        return point_distance(x1, y1, x2, y2);
    }

    public static float RadiansToLegacyDegrees(float radian) {

        return radian_to_legacy(radian);
    }

    public static float LegacyDegreesToRadians(float degree) {

        return legacy_to_radian(degree);
    }
}