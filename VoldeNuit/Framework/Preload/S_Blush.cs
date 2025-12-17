using System.Reflection;

using Microsoft.Xna.Framework.Graphics;

using ImageMagick;

using VoldeNuit.Framework.Drawing;

namespace VoldeNuit.Framework.Preload;

using static Heart;

internal class S_Blush: Sprite {

    public S_Blush() {

        embedded = true;

        texture_path = _assembly.GetManifestResourceNames().First(n => n.EndsWith("S_Blush.png"));

        sprite_width  = 4;
        sprite_height = 64;

        x = 0;
        y = 0;
    }
}