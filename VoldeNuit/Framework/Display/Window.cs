using Microsoft.Xna.Framework;

namespace VoldeNuit.Framework.Display;

using static Heart;

public static partial class Window {

    public static int window_get_x() { return window.ClientBounds.X; }

    public static int window_get_y() { return window.ClientBounds.Y; }

    public static int window_get_width() { return window.ClientBounds.Width; }

    public static int window_get_height() { return window.ClientBounds.Height; }

    public static GameWindow window_device() { return window; }
}