using MonoInput = Microsoft.Xna.Framework.Input;

namespace VoldeNuit.Framework.Input;

using VoldeNuit.Framework.Display;

using static Exception;
using static Heart;

public static partial class Mouse {

    public static int mouse_x_on(Camera camera) {

        int x = MonoInput.Mouse.GetState(window).X;

        float resolution = (float)camera.view.width/camera.viewport.width;

        return (int)float.Floor(camera.view.x+camera.viewport.x+((x-camera.viewport.x)*resolution));
    }

    public static int mouse_y_on(Camera camera) {

        int y = MonoInput.Mouse.GetState(window).Y;

        float resolution = (float)camera.view.height/camera.viewport.height;

        return (int)float.Floor(camera.view.y+camera.viewport.y+((y-camera.viewport.y)*resolution));
    }

    public static bool mouse_check_button(int mb) {

        if (!_main.IsActive) { return false; }

        MonoInput.MouseState ms = MonoInput.Mouse.GetState(window);

        switch (mb) {

            case mb_left: {

                return ms.LeftButton == MonoInput.ButtonState.Pressed;
            }

            case mb_middle: {

                return ms.MiddleButton == MonoInput.ButtonState.Pressed;
            }

            case mb_right: {

                return ms.RightButton == MonoInput.ButtonState.Pressed;
            }

            case mb_any: {

                return ms.LeftButton   == MonoInput.ButtonState.Pressed ||
                       ms.MiddleButton == MonoInput.ButtonState.Pressed ||
                       ms.RightButton  == MonoInput.ButtonState.Pressed;
            }

            case mb_none: {

                return ms.LeftButton   == MonoInput.ButtonState.Released &&
                       ms.MiddleButton == MonoInput.ButtonState.Released &&
                       ms.RightButton  == MonoInput.ButtonState.Released;
            }


            default: {

                _stacktrace(ExConstants.ARGUMENT_NOT_ALLOWED); return false;
            }
        }
    }

    public static bool mouse_check_button_pressed(int mb) {

        if (!_main.IsActive) { return false; }

        MonoInput.MouseState ms = MonoInput.Mouse.GetState(window);

        switch (mb) {

            case mb_left: {

                return (ms.LeftButton == MonoInput.ButtonState.Pressed) &&
                       (_mb_left != MonoInput.ButtonState.Pressed);
            }

            case mb_middle: {

                return (ms.MiddleButton == MonoInput.ButtonState.Pressed) && 
                       (_mb_middle != MonoInput.ButtonState.Pressed);
            }

            case mb_right: {

                return (ms.RightButton == MonoInput.ButtonState.Pressed) &&
                       (_mb_right != MonoInput.ButtonState.Pressed);
            }

            case mb_any: {

                return ((ms.LeftButton == MonoInput.ButtonState.Pressed) && (ms.LeftButton != _mb_left)) ||
                       ((ms.MiddleButton == MonoInput.ButtonState.Pressed) && (ms.MiddleButton != _mb_middle)) ||
                       ((ms.RightButton == MonoInput.ButtonState.Pressed) && (ms.RightButton != _mb_right));
            }

            case mb_none: {

                return (ms.LeftButton != MonoInput.ButtonState.Pressed) && (ms.LeftButton != _mb_left) &&
                       (ms.MiddleButton != MonoInput.ButtonState.Pressed) && (ms.MiddleButton != _mb_middle) &&
                       (ms.RightButton != MonoInput.ButtonState.Pressed) && (ms.RightButton != _mb_right);
            }


            default: { _stacktrace(ExConstants.ARGUMENT_NOT_ALLOWED); return false; }
        }
    }

    public static bool mouse_check_button_released(int mb) {

        if (!_main.IsActive) { return true; }

        MonoInput.MouseState ms = MonoInput.Mouse.GetState(window);

        switch (mb) {

            case mb_left: {

                return (ms.LeftButton == MonoInput.ButtonState.Released) &&
                       (_mb_left != MonoInput.ButtonState.Released);
            }

            case mb_middle: {

                return (ms.MiddleButton == MonoInput.ButtonState.Released) && 
                       (_mb_middle != MonoInput.ButtonState.Released);
            }

            case mb_right: {

                return (ms.RightButton == MonoInput.ButtonState.Released) &&
                       (_mb_right != MonoInput.ButtonState.Released);
            }

            case mb_any: {

                return ((ms.LeftButton == MonoInput.ButtonState.Released) && (_mb_left != MonoInput.ButtonState.Released)) ||
                       ((ms.MiddleButton == MonoInput.ButtonState.Released) && (_mb_middle != MonoInput.ButtonState.Released)) ||
                       ((ms.RightButton == MonoInput.ButtonState.Released) && (_mb_right != MonoInput.ButtonState.Released));
            }

            case mb_none: {

                return (ms.LeftButton != MonoInput.ButtonState.Released) && (ms.LeftButton != _mb_left) &&
                       (ms.MiddleButton == MonoInput.ButtonState.Released) && (ms.MiddleButton != _mb_middle) &&
                       (ms.RightButton == MonoInput.ButtonState.Released) && (ms.RightButton != _mb_right);
            }
        }

        _stacktrace(ExConstants.ARGUMENT_NOT_ALLOWED); return false;
    }
}