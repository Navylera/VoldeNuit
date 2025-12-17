using System.Reflection;

using Microsoft.Xna.Framework.Graphics;

using ImageMagick;

using VoldeNuit.Framework.Drawing;

namespace VoldeNuit.Framework.Preload;

using static Heart;

internal class S_Splash: Sprite {

    public S_Splash() {

        embedded = true;

        texture_path = _assembly.GetManifestResourceNames().First(n => n.EndsWith("VoldeNuit.png"));

        sprite_width  = 300;
        sprite_height = 100;

        x = 150;
        y = 61;
    }
}