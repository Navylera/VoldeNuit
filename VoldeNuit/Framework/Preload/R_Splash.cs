using VoldeNuit.Framework.Display;

namespace VoldeNuit.Framework.Preload;

using static Heart;

internal class R_Splash: Room {

    public R_Splash() {

        float scale = _entry.width/640;

        room_width  = 640;
        room_height = 480;

        if (scale >= 1) {

            room_width  = _entry.width;
            room_height = _entry.height;
        }

        room_speed = 60;

        color_background = _entry.color;

        new I_Splash() {
            
            x = room_width/2, y = room_height/2,
            
            image_xscale = float.Floor(scale), image_yscale = float.Floor(scale)
        };
    }
}