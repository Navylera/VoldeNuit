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

    internal byte[] _cache_partial_texture;

    internal bool _flag_cache_modified = false;

    internal bool _preloaded = false;

    internal void _preload(string path_target) {

        string tpath = path_target.Replace("Class", "")+".png";

        if (!File.Exists($"{path_target}.cs") ||
            !File.Exists(tpath)) { 
            
            return;
        }

        using MagickImage image = new MagickImage(tpath, MagickFormat.Png);

        using IPixelCollection<byte> pdata = image.GetPixels();

        _cache_partial_texture = pdata.ToByteArray(PixelMapping.RGBA);

        if (_sprite_height == -1) { _sprite_height = (int)image.Height; }

        _preloaded = true;
    }

    private static Texture2D _import(string path_target) {

        using MagickImage image = new MagickImage(path_target, MagickFormat.Png);

        using IPixelCollection<byte> pdata = image.GetPixels();

        Texture2D texture = new Texture2D(_graphicsDeviceManager.GraphicsDevice, 
                                          (int)image.Width, (int)image.Height
        );

        texture.SetData(pdata.ToByteArray(PixelMapping.RGBA));

        return texture;
    }

    private static Texture2D _load_texture(string directory, string target, char split) {

        string path_target = directory+split+target;
        
        if (File.Exists($"{path_target}.xnb")) { return _main.Content.Load<Texture2D>($"{path_target}.xnb"); }

        if (File.Exists($"{path_target}.png")) { return _import($"{path_target}.png"); }

        string[] array_directories = Directory.GetDirectories(directory);

        Texture2D? ret = null;

        foreach (string d in array_directories) {

            if (ret != null) { break; }

            ret = ret?? _load_texture(d, target, split);
        }

        return ret;
    }

    internal static Texture2D load_texture(string name_file) {

        char split = '/';

        StringBuilder sbuilder = new StringBuilder();     

        bool definded = false;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {

            sbuilder.Clear().Append(CONTENT_PATH_LINUX);

            if (CONTENT_PATH_LINUX[^1] != '/') {

                sbuilder.Append('/');
            }
            
            sbuilder.Append("Sprite"); definded = true;
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {

            split = '\\';

            sbuilder.Clear().Append(CONTENT_PATH_WINDOWS);

            if (CONTENT_PATH_WINDOWS[^1] != '\\') {

                sbuilder.Append('\\');
            }
            
            sbuilder.Append("Sprite"); definded = true;
        }

        if (!definded) {

            sbuilder.Clear().Append(CONTENT_PATH_OTHERS);

            if (CONTENT_PATH_OTHERS[^1] != '/') {

                sbuilder.Append('/');
            }
            
            sbuilder.Append("Sprite");
        }

        return _load_texture(sbuilder.ToString(), name_file, split);
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