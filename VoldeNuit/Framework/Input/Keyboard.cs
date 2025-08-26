using Microsoft.Xna.Framework.Input;

namespace VoldeNuit.Framework.Input;

using static Microsoft.Xna.Framework.Input.Keys;

public static partial class Keyboard {

    public const byte vk_nokey       = (byte)None;
    public const byte vk_anykey      = (byte)255;
    public const byte AnyKey         = (byte)255;
    public const byte vk_left        = (byte)Left;
    public const byte vk_right       = (byte)Right;
    public const byte vk_up          = (byte)Up;
    public const byte vk_down 	     = (byte)Down;
    public const byte vk_enter 	     = (byte)Enter;
    public const byte vk_escape      = (byte)Escape;
    public const byte vk_space 	     = (byte)Space;
    public const byte vk_shift 	     = (byte)159;
    public const byte Shift          = (byte)159;
    public const byte vk_control     = (byte)158;
    public const byte Control        = (byte)158;
    public const byte vk_alt         = (byte)157;
    public const byte Alt            = (byte)157;
    public const byte vk_backspace   = (byte)Back;
    public const byte vk_tab 	     = (byte)Tab;
    public const byte vk_home 	     = (byte)Home;
    public const byte vk_end 	     = (byte)End;
    public const byte vk_delete      = (byte)Delete;
    public const byte vk_insert      = (byte)Insert;
    public const byte vk_pageup      = (byte)PageUp;
    public const byte vk_pagedown    = (byte)PageDown;
    public const byte vk_pause       = (byte)Pause;
    public const byte vk_printscreen = (byte)PrintScreen;
    public const byte vk_f1          = (byte)F1;
    public const byte vk_f2          = (byte)F2;
    public const byte vk_f3          = (byte)F3;
    public const byte vk_f4          = (byte)F4;
    public const byte vk_f5          = (byte)F5;
    public const byte vk_f6          = (byte)F6;
    public const byte vk_f7          = (byte)F7;
    public const byte vk_f8          = (byte)F8;
    public const byte vk_f9          = (byte)F9;
    public const byte vk_f10         = (byte)F10;
    public const byte vk_f11         = (byte)F11;
    public const byte vk_f12         = (byte)F12;
    public const byte vk_numpad0     = (byte)NumPad0;
    public const byte vk_numpad1     = (byte)NumPad1;
    public const byte vk_numpad2     = (byte)NumPad2;
    public const byte vk_numpad3     = (byte)NumPad3;
    public const byte vk_numpad4     = (byte)NumPad4;
    public const byte vk_numpad5     = (byte)NumPad5;
    public const byte vk_numpad6     = (byte)NumPad6;
    public const byte vk_numpad7     = (byte)NumPad7;
    public const byte vk_numpad8     = (byte)NumPad8;
    public const byte vk_numpad9     = (byte)NumPad9;
    public const byte vk_multiply    = (byte)Multiply;
    public const byte vk_divide      = (byte)Divide;
    public const byte vk_add 	     = (byte)Add;
    public const byte vk_subtract 	 = (byte)Subtract;
    public const byte vk_decimal 	 = (byte)Keys.Decimal;
    public const byte vk_lshift 	 = (byte)LeftShift;
    public const byte vk_lcontrol 	 = (byte)LeftControl;
    public const byte vk_lalt 	     = (byte)LeftAlt;
    public const byte vk_rshift 	 = (byte)RightShift;
    public const byte vk_rcontrol 	 = (byte)RightControl;
    public const byte vk_ralt 	     = (byte)RightAlt;


    // Legacy keyboard variables

    public static byte vk_lastkey { get; set; } = vk_nokey;
    public static byte LastKey { get => vk_lastkey; set => vk_lastkey = value; }
}