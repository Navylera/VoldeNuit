using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VoldeNuit.Framework.Drawing;

using static Heart;
using static Configuration;

public partial class Sprite {

    internal Texture2D? _texture = null;

    public Texture2D? texture {

        get { 

            if (_texture == null && GetType().Name == "Sprite") { return null; }

            if (_texture == null && texture_path != null && File.Exists(texture_path)) {

                if (texture_path.EndsWith("xnb") &&
                    CONTENT_PATH == $".{separator}Content{separator}") {

                    _texture = _main.Content.Load<Texture2D>(texture_path[10..^4]);
                }

                if (texture_path.EndsWith("png")) {
                    
                    _texture = _import(texture_path);
                }
            }

            if (_preloaded && _texture == null) {

                _texture = new Texture2D(_graphicsDeviceManager.GraphicsDevice,
                                         _cache_partial_texture.Length/(4*_sprite_height), 
                                         _sprite_height
                );

                _texture.SetData(_cache_partial_texture);

                return _texture;
            }
            
            _texture = _texture?? load_texture(GetType().Name); return _texture;
        }

        set { 

            if (GetType() != typeof(Sprite)) { return; }
            
            _texture?.Dispose(); _flag_cache_modified = true; _texture = value;
        }
    }

    public string? texture_path { get; init; } = null;

    public int x { get; init; } = 0;
    public int y { get; init; } = 0;

    public int sprite_width { get; init; }
    private int _sprite_height = -1;
    public int sprite_height { 

        get { 

            if (_texture == null) { return 0; }
            
            return _sprite_height <= 0? _texture.Height: _sprite_height;
        } 
        
        init => _sprite_height = value; 
    }

    public int image_number {

        get {

            if (texture == null) { return 0; }

            return (int)float.Floor(texture.Width/sprite_width)*
                   (int)float.Round(texture.Height/sprite_height);
        }
    }

    public Rectangle? bbox;

    public bool precise { get => bbox == null; }
}