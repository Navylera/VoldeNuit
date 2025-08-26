using MonoInput = Microsoft.Xna.Framework.Input;

namespace VoldeNuit.Framework.Input;

using static Heart;

public static partial class Mouse {

    internal static void _update_mouse() {

        if (!_main.IsActive) { return; }

        MonoInput.MouseState ms = MonoInput.Mouse.GetState(window);

        _mb_middle = MonoInput.ButtonState.Released;
        _mb_right  = MonoInput.ButtonState.Released;
        _mb_left   = MonoInput.ButtonState.Released;

        if (ms.MiddleButton == MonoInput.ButtonState.Pressed) {

            mouse_lastbutton = mb_middle;

            _mb_middle = MonoInput.ButtonState.Pressed;
        }

        if (ms.RightButton == MonoInput.ButtonState.Pressed) {

            mouse_lastbutton = mb_right;

            _mb_right = MonoInput.ButtonState.Pressed;
        }

        if (ms.LeftButton == MonoInput.ButtonState.Pressed) {

            mouse_lastbutton = mb_left;

            _mb_left = MonoInput.ButtonState.Pressed;
        }
    }
}