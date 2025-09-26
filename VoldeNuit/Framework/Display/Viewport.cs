namespace VoldeNuit.Framework.Display;

public partial class Viewport {

    internal Viewport(int x, int y, int width, int height) {

        this.x = x;
        this.y = y;

        _width  = width;
        _height = height;
    }

    private int _x;
    public int x { get => _x; set { 
        
            _x = value < 0? 0 : value;
        }
    }

    private int _y;
    public int y { get => _y; set { 
        
            _y = value < 0? 0 : value;
        }
    }

    private int _width;
    public int width {

        get => _width; set { _width = value; }
    }
    
    private int _height;
    public int height {

        get => _height; set { _height = value; }
    }
}