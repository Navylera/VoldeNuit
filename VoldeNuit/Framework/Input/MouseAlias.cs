using MonoInput = Microsoft.Xna.Framework.Input;

namespace VoldeNuit.Framework.Input;

using static Heart;

public static partial class Mouse {

    public static int X {
        
        get => MonoInput.Mouse.GetState(window).X; set => MonoInput.Mouse.SetPosition(value, mouse_y);
    }
    public static int Y {
        
        get => MonoInput.Mouse.GetState(window).Y; set => MonoInput.Mouse.SetPosition(mouse_x, value);
    }

    public static int LastButton { get => mouse_lastbutton; set => mouse_lastbutton = value; }

    public const int Left = 0;
    public const int Middle = 1;
    public const int Right = 2;
    public const int None = 3;
    public const int Any = 4;
}