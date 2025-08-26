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

    public static void keyboard_clear(byte key) {

        _keyboard_lastkey = _keyboard_lastkey.Where(k => k != (Keys)key).ToArray(); return;
    }

    public static void io_clear() {

        Mouse.mouse_lastbutton = Mouse.mb_none;

        _keyboard_lastkey = [];

        return;
    }
}