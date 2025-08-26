using System.Reflection;

using Microsoft.Xna.Framework.Graphics;

using ImageMagick;

using VoldeNuit.Framework.Drawing;

namespace VoldeNuit.Framework.Preload.DefaultFont;

using static Heart;

internal class DefaultFont: Font {

    // Galmuri9

    // © 2019–2024 Lee Minseo (quiple@quiple.dev)
    // https://quiple.dev/galmuri

    public DefaultFont() {

        string directory;

        string range = 
            "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890"+
            "~`!@#$%^&*(){}[]-_=+;:"+
            "\'\",<.>/?\\|";

        size_font = 10;

        _vmod = 12;

        Stream stream;

        Assembly asb = Assembly.GetExecutingAssembly();

        for (int i=0; i<2; i=i+1) {

            directory = asb.GetManifestResourceNames().First(n => n.EndsWith($"Galmuri9.ttf_font_texture_{i}.png"));

            stream = asb.GetManifestResourceStream(directory);

            using MagickImage image = new MagickImage(stream, MagickFormat.Png);

            using IPixelCollection<byte> pdata = image.GetPixels();

            _texture_font[i] = new Texture2D(_graphicsDeviceManager.GraphicsDevice,
                                             (int)image.Width, (int)image.Height
            );

            _texture_font[i].SetData(pdata.ToByteArray(PixelMapping.RGBA));

            stream.Close();
            stream.Dispose();
        }

        directory = asb.GetManifestResourceNames().First(n => n.EndsWith("Galmuri9.ttf_data"));

        stream = asb.GetManifestResourceStream(directory);

        stream.Seek(0, SeekOrigin.Begin);

        using StreamReader sreader = new StreamReader(stream);

        foreach (char c in range) { _dict_char_data.Add(c, uint.Parse(sreader.ReadLine())); }

        stream.Close();
        stream.Dispose();
    }
}