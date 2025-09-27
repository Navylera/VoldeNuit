using MonoInput = Microsoft.Xna.Framework.Input;

using Microsoft.Xna.Framework.Input;
using static Microsoft.Xna.Framework.Input.Keys;

namespace VoldeNuit.Framework.Input;

public static partial class Keyboard {

    public static byte ord(char c) { return (byte)c; }

    public static bool keyboard_check(byte key) {

        switch (key) {

            case vk_nokey: {

                return MonoInput.Keyboard.GetState().GetPressedKeyCount() == 0;
            }

            case vk_anykey: {

                return MonoInput.Keyboard.GetState().GetPressedKeyCount() > 0;
            }

            case vk_shift: {

                return MonoInput.Keyboard.GetState().IsKeyDown(LeftShift) ||
                       MonoInput.Keyboard.GetState().IsKeyDown(RightShift)
                ;
            }

            case vk_control: {

                return MonoInput.Keyboard.GetState().IsKeyDown(LeftControl) ||
                       MonoInput.Keyboard.GetState().IsKeyDown(RightControl)
                ;
            }

            case vk_alt: {

                return MonoInput.Keyboard.GetState().IsKeyDown(LeftAlt) ||
                       MonoInput.Keyboard.GetState().IsKeyDown(RightAlt)
                ;
            }
        }

        if (key >= 'a' && key <= 'z') { key = (byte)(key-32); }

        return MonoInput.Keyboard.GetState().IsKeyDown((Keys)key);
    }

    public static bool keyboard_check_pressed(byte key) {

        switch (key) {

            case vk_nokey: {

                return Enumerable.SequenceEqual(MonoInput.Keyboard.GetState().GetPressedKeys(), 
                                                _keyboard_lastkey
                );
            }

            case vk_anykey: {

                foreach (Keys k in MonoInput.Keyboard.GetState().GetPressedKeys()) {

                    if (!_keyboard_lastkey.Contains(k)) { return true; }
                }

                return false;
            }

            case vk_shift: {

                return (!_keyboard_lastkey.Contains(LeftShift) &&
                        MonoInput.Keyboard.GetState().IsKeyDown(LeftShift)) ||
                       (!_keyboard_lastkey.Contains(RightShift) &&
                        MonoInput.Keyboard.GetState().IsKeyDown(RightShift))
                ;
            }

            case vk_control: {

                return (!_keyboard_lastkey.Contains(LeftControl) &&
                        MonoInput.Keyboard.GetState().IsKeyDown(LeftControl)) ||
                       (!_keyboard_lastkey.Contains(RightControl) &&
                        MonoInput.Keyboard.GetState().IsKeyDown(RightControl))
                ;
            }

            case vk_alt: {

                return (!_keyboard_lastkey.Contains(LeftAlt) &&
                        MonoInput.Keyboard.GetState().IsKeyDown(LeftAlt)) ||
                       (!_keyboard_lastkey.Contains(RightAlt) &&
                        MonoInput.Keyboard.GetState().IsKeyDown(RightAlt))
                ;
            }
        }

        if (key >= 'a' && key <= 'z') { key = (byte)(key-32); }

        return !_keyboard_lastkey.Contains((Keys)key) && 
               MonoInput.Keyboard.GetState().IsKeyDown((Keys)key)
        ;
    }

    public static bool keyboard_check_released(byte key) {

        switch (key) {

            case vk_nokey: {

                return Enumerable.SequenceEqual(MonoInput.Keyboard.GetState().GetPressedKeys(), 
                                                _keyboard_lastkey
                );
            }

            case vk_anykey: {

                return _keyboard_lastkey.Length > 0 &&
                       MonoInput.Keyboard.GetState().GetPressedKeyCount() == 0
                ;
            }

            case vk_shift: {

                return (_keyboard_lastkey.Contains(LeftShift) &&
                        MonoInput.Keyboard.GetState().IsKeyUp(LeftShift)) ||
                       (_keyboard_lastkey.Contains(RightShift) &&
                        MonoInput.Keyboard.GetState().IsKeyUp(RightShift))
                ;
            }

            case vk_control: {

                return (_keyboard_lastkey.Contains(LeftControl) &&
                        MonoInput.Keyboard.GetState().IsKeyUp(LeftControl)) ||
                       (_keyboard_lastkey.Contains(RightControl) &&
                        MonoInput.Keyboard.GetState().IsKeyUp(RightControl))
                ;
            }

            case vk_alt: {

                return (_keyboard_lastkey.Contains(LeftAlt) &&
                        MonoInput.Keyboard.GetState().IsKeyUp(LeftAlt)) ||
                       (_keyboard_lastkey.Contains(RightAlt) &&
                        MonoInput.Keyboard.GetState().IsKeyUp(RightAlt))
                ;
            }
        }

        if (key >= 'a' && key <= 'z') { key = (byte)(key-32); }

        return _keyboard_lastkey.Contains((Keys)key) && 
               MonoInput.Keyboard.GetState().IsKeyUp((Keys)key)
        ;
    }

    public static void keyboard_key_press(byte key) {

        switch (key) {

            case vk_shift: {

                _keyboard_lastkey = [.._keyboard_lastkey.Where(key => key != LeftShift && key != RightShift), 
                                     LeftShift, RightShift];

                break;
            }

            case vk_control: {

                _keyboard_lastkey = [.._keyboard_lastkey.Where(key => key != LeftControl && key != RightControl), 
                                     LeftControl, RightControl];

                break;
            }

            case vk_alt: {

                _keyboard_lastkey = [.._keyboard_lastkey.Where(key => key != LeftAlt && key != RightAlt), 
                                     LeftAlt, RightAlt];

                break;
            }
        }

        if (key >= 'a' && key <= 'z') { key = (byte)(key-32); }

        _keyboard_lastkey = [.._keyboard_lastkey.Where(_key => (byte)_key != key), 
                             (Keys)key];

        vk_lastkey = _keyboard_lastkey.Length == 0? vk_nokey: (byte)_keyboard_lastkey[^1];

        return;
    }

    public static void keyboard_key_release(byte key) {

        switch (key) {

            case vk_shift: {

                _keyboard_lastkey = [.._keyboard_lastkey.Where(key => key != LeftShift && key != RightShift)];

                break;
            }

            case vk_control: {

                _keyboard_lastkey = [.._keyboard_lastkey.Where(key => key != LeftControl && key != RightControl)];

                break;
            }

            case vk_alt: {

                _keyboard_lastkey = [.._keyboard_lastkey.Where(key => key != LeftAlt && key != RightAlt)];

                break;
            }
        }

        if (key >= 'a' && key <= 'z') { key = (byte)(key-32); }

        _keyboard_lastkey = [.._keyboard_lastkey.Where(_key => (byte)_key != key)];

        vk_lastkey = _keyboard_lastkey.Length == 0? vk_nokey: (byte)_keyboard_lastkey[^1];

        return;
    }

    public static void keyboard_clear(byte key) {

        _keyboard_lastkey = [.._keyboard_lastkey.Where(k => k != (Keys)key)]; 
        
        vk_lastkey = _keyboard_lastkey.Length == 0? vk_nokey: (byte)_keyboard_lastkey[^1];

        return;
    }

    [Obsolete("This property will be removed in the next major update. Please use the Input.io_clear() instead.")]
    public static void io_clear() {

        Mouse.mouse_lastbutton = Mouse.mb_none;

        _keyboard_lastkey = [];

        return;
    }
}