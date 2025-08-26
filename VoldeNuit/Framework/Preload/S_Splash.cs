using System.Reflection;

using Microsoft.Xna.Framework.Graphics;

using ImageMagick;

using VoldeNuit.Framework.Drawing;

namespace VoldeNuit.Framework.Preload;

using static Heart;

internal class S_Splash: Sprite {

    public S_Splash() {

        Assembly asb = Assembly.GetExecutingAssembly();

        string directory = asb.GetManifestResourceNames().First(n => n.EndsWith("VoldeNuit.png"));

        using Stream stream = asb.GetManifestResourceStream(directory);

        using MagickImage image = new MagickImage(stream, MagickFormat.Png);

        stream.Close();
        stream.Dispose();

        using IPixelCollection<byte> pdata = image.GetPixels();

        _texture = new Texture2D(_graphicsDeviceManager.GraphicsDevice, 
                                 (int)image.Width, (int)image.Height
        );
        
        _texture.SetData(pdata.ToByteArray(PixelMapping.RGBA));

        sprite_width  = 300;
        sprite_height = 100;

        x = 150;
        y = 61;
    }
}