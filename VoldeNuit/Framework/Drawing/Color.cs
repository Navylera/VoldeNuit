namespace VoldeNuit.Framework.Drawing;

using static Configuration;

public static class Color {

    internal static uint _color = 0xffffffffu;

    public static uint color { get => _color; set => _color = value; }

    public static byte A {

        get {

            switch (COLOR_FORMAT) {   
            
                case ColorFormat.ARGB: { return (byte)(_color>>24); }

                case ColorFormat.BGRA: { return (byte)(_color); }

                case ColorFormat.ABGR: { return (byte)(_color>>24); }
            }
            
            return 0;
        }
        
        set {
            
            switch (COLOR_FORMAT) {   
            
                case ColorFormat.ARGB: { _color = (_color&0x00ffffffu)|(uint)(value<<24); return; }

                case ColorFormat.BGRA: { _color = (_color&0xffffff00u)|(uint)(value); return; }

                case ColorFormat.ABGR: { _color = (_color&0x00ffffffu)|(uint)(value<<24); return; }
            }
        }
    }

    public static byte R {

        get {

            switch (COLOR_FORMAT) {   
            
                case ColorFormat.ARGB: { return (byte)(_color>>16); }

                case ColorFormat.BGRA: { return (byte)(_color>>8); }

                case ColorFormat.ABGR: { return (byte)(_color); }
            }
            
            return 0;
        }
        
        set {
            
            switch (COLOR_FORMAT) {   
            
                case ColorFormat.ARGB: { _color = (_color&0xff00ffffu)|(uint)(value<<16); return; }

                case ColorFormat.BGRA: { _color = (_color&0xffff00ffu)|(uint)(value<<8); return; }

                case ColorFormat.ABGR: { _color = (_color&0xffffff00u)|(uint)(value); return; }
            }
        }
    }

    public static byte G {

        get {

            switch (COLOR_FORMAT) {   
            
                case ColorFormat.ARGB: { return (byte)(_color>>8); }

                case ColorFormat.BGRA: { return (byte)(_color>>16); }

                case ColorFormat.ABGR: { return (byte)(_color>>8); }
            }
            
            return 0;
        }
        
        set {
            
            switch (COLOR_FORMAT) {   
            
                case ColorFormat.ARGB: { _color = (_color&0xffff00ffu)|(uint)(value<<8); return; }

                case ColorFormat.BGRA: { _color = (_color&0xff00ffffu)|(uint)(value<<16); return; }

                case ColorFormat.ABGR: { _color = (_color&0xffff00ffu)|(uint)(value<<8); return; }
            }
        }
    }

    public static byte B {

        get {

            switch (COLOR_FORMAT) {   
            
                case ColorFormat.ARGB: { return (byte)(_color); }

                case ColorFormat.BGRA: { return (byte)(_color>>24); }

                case ColorFormat.ABGR: { return (byte)(_color>>16); }
            }
            
            return 0;
        }
        
        set {
            
            switch (COLOR_FORMAT) {   
            
                case ColorFormat.ARGB: { _color = (_color&0xffffff00u)|(uint)(value); return; }

                case ColorFormat.BGRA: { _color = (_color&0x00ffffffu)|(uint)(value<<24); return; }

                case ColorFormat.ABGR: { _color = (_color&0xff00ffffu)|(uint)(value<<16); return; }
            }
        }
    }

    public static Microsoft.Xna.Framework.Color color_to_xna(uint color) {

        switch (COLOR_FORMAT) {   
            
            // Monogame default: ABGR
            case ColorFormat.ARGB: {

                return new Microsoft.Xna.Framework.Color(
                           (color&0xff000000u)|
                           (color&0x00ff0000u) >> 16|
                           (color&0x0000ff00u)|
                           (color&0x000000ffu) << 16
                );
            }

            case ColorFormat.BGRA: {

                return new Microsoft.Xna.Framework.Color(
                           (0xff000000u >> 24)|
                           (color&0x00ff0000u) >> 8|
                           (color&0x0000ff00u) >> 8|
                           (color&0x000000ffu) << 16
                );
            }

            case ColorFormat.ABGR: {

                return new Microsoft.Xna.Framework.Color(color);
            }
        }

        return new Microsoft.Xna.Framework.Color();
    }

    public static Microsoft.Xna.Framework.Color ColorToXna(uint color) {

        return color_to_xna(color);
    }
}