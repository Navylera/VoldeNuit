namespace VoldeNuit.Framework.Input;

public static partial class UniversalInput {

    public static void io_clear() {

        Mouse.mouse_lastbutton = Mouse.mb_none;

        Keyboard._keyboard_lastkey = [];

        return;
    }
}