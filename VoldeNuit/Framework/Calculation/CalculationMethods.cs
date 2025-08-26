namespace VoldeNuit.Framework.Calculation;

using static Configuration;

public static partial class Calculation {

    public static float point_direction(float x1, float y1, float x2, float y2) {

        float direction = float.Atan2(y2-y1, x2-x1);

        switch (ANGLE_FORMAT) {

            case AngleFormat.RADIAN: {

                return direction;
            }

            case AngleFormat.DEGREE: {

                return direction*180f/float.Pi;
            }

            case AngleFormat.LEGACY: {

                if (-direction < 0) { return ((2*float.Pi)-direction)*180f/float.Pi; }

                return -direction*180f/float.Pi;
            }
        }

        return direction;
    }

    public static float point_distance(float x1, float y1, float x2, float y2) {

        return float.Sqrt(float.Pow(x2-x1, 2)+float.Pow(y2-y1, 2));
    }

    public static float radian_to_legacy(float radian) {

        float direction = (float.Pi*(-radian)/180f)%(2*float.Pi);

        if (direction < 0) { direction = direction+(2*float.Pi); }

        return direction;
    }

    public static float legacy_to_radian(float degree) {

        if (-degree < 0) { return ((2*float.Pi)-degree)*180f/float.Pi; }

        return -degree*180f/float.Pi;
    }
}