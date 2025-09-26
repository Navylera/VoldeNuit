using Microsoft.Xna.Framework.Graphics;

namespace VoldeNuit.Framework.Display;

public static partial class Display {

    public static int display_get_width() {

        return GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
    }

    public static int display_get_height() {

        return GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
    }
}