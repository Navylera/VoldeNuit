using System.Reflection;

using Microsoft.Xna.Framework.Graphics;

using ImageMagick;

using VoldeNuit.Framework.Drawing;

namespace VoldeNuit.Framework.Preload;

using static Heart;

internal class S_Blush: Sprite {

    public S_Blush() {

        Assembly asb = Assembly.GetExecutingAssembly();

        string directory = asb.GetManifestResourceNames().First(n => n.EndsWith("S_Blush.png"));

        using Stream stream = asb.GetManifestResourceStream(directory);

        using MagickImage image = new MagickImage(stream, MagickFormat.Png);

        stream.Close();
        stream.Dispose();

        using IPixelCollection<byte> pdata = image.GetPixels();

        byte[] parray = pdata.ToByteArray(PixelMapping.RGBA);

        for (int i=3; i<parray.Length; i=i+4) {
            
            if (parray[i] != 0) { continue; }

            parray[i-1] = 0;
            parray[i-2] = 0;
            parray[i-3] = 0;
        }

        _texture = new Texture2D(_graphicsDeviceManager.GraphicsDevice, 
                                 (int)image.Width, (int)image.Height
        );
        
        _texture.SetData(parray);

        sprite_width  = 4;
        sprite_height = 64;

        x = 0;
        y = 0;
    }
}