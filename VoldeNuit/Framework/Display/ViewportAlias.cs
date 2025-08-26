namespace VoldeNuit.Framework.Display;

public partial class Viewport {

    public int X { get => x; set => x = value; }
    public int Y { get => y; set => y = value; }

    public int Width {

        get => _width; set { _width = value; }
    }

    public int Height {

        get => _height; set { _height = value; }
    }
}