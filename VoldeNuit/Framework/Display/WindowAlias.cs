using Microsoft.Xna.Framework;

namespace VoldeNuit.Framework.Display;

using static Heart;

public static partial class Window {

    public static int X { get => window.ClientBounds.X; }
    public static int Y { get => window.ClientBounds.Y; }

    public static int Width { get => window.ClientBounds.Width; }
    public static int Height { get => window.ClientBounds.Height; }

    public static GameWindow GetGameWindow() { return window; }
}