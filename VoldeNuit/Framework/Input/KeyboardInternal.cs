using MonoInput = Microsoft.Xna.Framework.Input;

namespace VoldeNuit.Framework.Input;

using Microsoft.Xna.Framework.Input;

public static partial class Keyboard {

    internal static Keys[] _keyboard_lastkey = MonoInput.Keyboard.GetState().GetPressedKeys();

    internal static void _update_keyboard() {

        _keyboard_lastkey = MonoInput.Keyboard.GetState().GetPressedKeys();

        vk_lastkey = _keyboard_lastkey.Length == 0? vk_nokey: (byte)_keyboard_lastkey[^1];
    }
}