using Microsoft.Xna.Framework.Graphics;

namespace VoldeNuit.Framework.Display;

using static Heart;

public partial class View {

    internal View(int x, int y, int width, int height) {

        _x = x;
        _y = y;

        _width  = width;
        _height = height;

        _render_target = new RenderTarget2D(_graphicsDeviceManager.GraphicsDevice, 
                                            _width, _height
        );
    }

    private int _x;
    public int x { get => _x; set { 
        
            _x = int.Clamp(value, 0, room_current.room_width-width);
        }
    }

    private int _y;
    public int y { get => _y; set { 
        
            _y = int.Clamp(value, 0, room_current.room_height-height);
        }
    }

    private int _width;
    public int width { get => _width; set { _width = value; _updated = true; } }

    private int _height;
    public int height { get => _height; set { _height = value; _updated = true; } }

    public RenderTarget2D _render_target;

    internal bool _updated = false;
}