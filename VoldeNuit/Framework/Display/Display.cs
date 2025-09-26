using Microsoft.Xna.Framework.Graphics;

namespace VoldeNuit.Framework.Display;

public static partial class Display {

    public static int DisplayWidth { 
        
        get => GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
    }

    public static int DisplayHeight {

        get => GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
    }
}