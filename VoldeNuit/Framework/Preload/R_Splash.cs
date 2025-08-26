using VoldeNuit.Framework.Display;

namespace VoldeNuit.Framework.Preload;

using static Heart;

internal class R_Splash: Room {

    public R_Splash() {

        float scale = _entry.width/640;

        int width = (int)float.Ceiling(640*scale);
        int height = (int)float.Ceiling(480*scale);

        // room_width  = 640;
        // room_height = 480;

        if (scale >= 1) {

            room_width  = _entry.width;
            room_height = _entry.height;

            camera.Add(new Camera(0, 0, 4, 4, 0, 0, _entry.width, _entry.height));

            camera.Add(new Camera(0, 0, 640, 480,
                                  (_entry.width-width)/2, (_entry.height-height)/2, width, height
            ));
        }

        room_speed = 60;

        color_background = 0xff74569bu;

        new I_Splash() { x = 320, y = 240 };
    }
}