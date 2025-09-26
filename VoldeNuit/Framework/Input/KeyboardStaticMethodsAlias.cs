namespace VoldeNuit.Framework.Input;

public static partial class Keyboard {

    public static byte CharToByte(char c) { 
        
        return (byte)c;
    }

    public static bool HasKeyInput(byte key) {
        
        return keyboard_check(key);
    }

    public static bool HasKeyPressed(byte key) {
        
        return keyboard_check_pressed(key);
    }

    public static bool HasKeyReleased(byte key) {
        
        return keyboard_check_released(key);
    }

    public static void PressKey(byte key) {

        keyboard_key_press(key);
    }

    public static void ReleaseKey(byte key) {

        keyboard_key_release(key);
    }

    public static void ClearKey(byte key) {
        
        keyboard_clear(key);
    }

    [Obsolete("It is not recommended to use. Please use the Input.ClearIO() instead.")]
    public static void ClearIO() {
        
        io_clear();
    }
}