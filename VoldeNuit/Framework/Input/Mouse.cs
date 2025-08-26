using MonoInput = Microsoft.Xna.Framework.Input;

namespace VoldeNuit.Framework.Input;

using static Heart;

public static partial class Mouse {

    // Legacy mouse variables

    public static int mouse_x {
        
        get => MonoInput.Mouse.GetState(window).X; 
        
        set => MonoInput.Mouse.SetPosition(value, mouse_y);
    }

    public static int mouse_y {
        
        get => MonoInput.Mouse.GetState(window).Y; 
        
        set => MonoInput.Mouse.SetPosition(mouse_x, value);
    }

    public static int mouse_lastbutton { get; set; } = mb_none;

    internal static MonoInput.ButtonState _mb_left   = MonoInput.ButtonState.Released;
    internal static MonoInput.ButtonState _mb_middle = MonoInput.ButtonState.Released;
    internal static MonoInput.ButtonState _mb_right  = MonoInput.ButtonState.Released;

    public const int mb_left   = 0;
    public const int mb_middle = 1;
    public const int mb_right  = 2;
    public const int mb_none   = 3;
    public const int mb_any    = 4;
}