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

    public static void ClearKey(byte key) {
        
        keyboard_clear(key);
    }

    public static void ClearIO() {
        
        io_clear();
    }
}