using System.Runtime.InteropServices;
using System.Text;

using ImageMagick;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VoldeNuit.Framework.Drawing;

using static Heart;
using static Configuration;
using static Draw;

public partial class Sprite {

    private bool _disposed = false;

    internal virtual void Dispose() {

        if (_sprite.Contains(this) || _disposed) { return; }

        _texture?.Dispose();

        _disposed = true; return;
    }

    internal byte[] _cache_partial_texture = null!;

    internal bool _flag_cache_modified = true;

    internal bool _preloaded = false;

    internal void _preload(string imagepath) {

        if (_preloaded) { return; }

        using MagickImage image = new MagickImage(imagepath, MagickFormat.Png);

        using IPixelCollection<byte> pdata = image.GetPixels();

        _cache_partial_texture = pdata.ToByteArray(PixelMapping.RGBA)!;

        // If alpha == 0 => clear all RGB data

        for (int i=3; i<_cache_partial_texture.Length; i=i+4) {
            
            if (_cache_partial_texture[i] != 0) { continue; }

            _cache_partial_texture[i-1] = 0;
            _cache_partial_texture[i-2] = 0;
            _cache_partial_texture[i-3] = 0;
        }

        _flag_cache_modified = false;

        if (_sprite_height == -1) { _sprite_height = (int)image.Height; }

        _preloaded = true;
    }

    private static Texture2D _import(string path_target) {

        // TODO: add FileStream & modify texture to internal

        using MagickImage image = new MagickImage(path_target, MagickFormat.Png);

        using IPixelCollection<byte> pdata = image.GetPixels();

        Texture2D texture = new Texture2D(_graphicsDeviceManager.GraphicsDevice, 
                                          (int)image.Width, (int)image.Height
        );

        byte[] parray = pdata.ToByteArray(PixelMapping.RGBA)!;

        // If alpha == 0 => clear all RGB data

        for (int i=3; i<parray.Length; i=i+4) {
            
            if (parray[i] != 0) { continue; }

            parray[i-1] = 0;
            parray[i-2] = 0;
            parray[i-3] = 0;
        }

        texture.SetData(pdata.ToByteArray(PixelMapping.RGBA));

        return texture;
    }

    private static Texture2D? _load_texture(string directory, string target) {

        string path_target = directory+separator+target;

        if (File.Exists($"{path_target}.xnb") &&
            CONTENT_PATH == $".{separator}Content{separator}") {

            return _main.Content.Load<Texture2D>(path_target[10..]);
        }

        if (File.Exists($"{path_target}.png")) { return _import($"{path_target}.png"); }

        string[] array_directories = Directory.GetDirectories(directory);

        Texture2D? ret = null;

        foreach (string d in array_directories) {

            if (ret != null) { break; }

            ret = ret?? _load_texture(d, target);
        }

        return ret;
    }

    internal static Texture2D? load_texture(string name_file) {

        StringBuilder sbuilder = new StringBuilder();     

        sbuilder.Clear().Append(CONTENT_PATH);

        if (CONTENT_PATH[^1] != separator) { sbuilder.Append(separator); }
        
        sbuilder.Append("Sprite");

        return _load_texture(sbuilder.ToString(), name_file);
    }

    internal byte[] this[float i] {

        get {

            if (texture == null) { return []; }

            int index = (int)float.Floor(i)%image_number;

            Rectangle region = new Rectangle((index*sprite_width)%texture.Width,
                                             sprite_height*((index*sprite_width)/texture.Width),
                                             sprite_width, sprite_height
            );

            if (!_flag_cache_modified) { 

                return _get_byte_data_region(_cache_partial_texture, region);
            }

            _cache_partial_texture = _cache_partial_texture?? 
                                     new byte[4*texture.Width*texture.Height];

            texture.GetData(_cache_partial_texture);

            return _get_byte_data_region(_cache_partial_texture, region);
        }
    }
}