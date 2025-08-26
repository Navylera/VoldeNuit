namespace VoldeNuit.Framework.Display;

public partial class Viewport {

    internal Viewport(int x, int y, int width, int height) {

        this.x = x;
        this.y = y;

        _width  = width;
        _height = height;
    }

    public int x;
    public int y;

    private int _width;
    public int width {

        get => _width; set { _width = value; }
    }
    
    private int _height;
    public int height {

        get => _height; set { _height = value; }
    }
}