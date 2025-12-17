using System.Text;
using System.Reflection;

using Microsoft.Xna.Framework.Graphics;

using MonoColor = Microsoft.Xna.Framework.Color;

using VoldeNuit.Framework.Drawing;
using VoldeNuit.Framework.Instances;
using VoldeNuit.Framework.Audio;
using VoldeNuit.Framework.Display;

namespace VoldeNuit.Framework.Preload;

using static Heart;
using static Configuration;
using static Room;
using static Draw;

internal class I_Splash: Instance {

    internal Assembly assembly;

    internal string message = "Searching for class files...";

    internal int tcount = 0;
    internal static int pcount { get; set; } = 0;

    internal bool completeload = false;
    internal bool completeset = false;

    internal ColorFormat cformat = COLOR_FORMAT;

    internal int _halign = halign;

    internal float progressindex = 0f;

    internal RenderTarget2D message_white = new RenderTarget2D(
        _graphicsDeviceManager.GraphicsDevice, 240, 11
    );

    internal RenderTarget2D message_violet = new RenderTarget2D(
        _graphicsDeviceManager.GraphicsDevice, 240, 11
    );

    internal float blush = 0f;

    MonoColor color = Color._color_to_xna(0);

    internal int scale = 1;

    public I_Splash() {

        halign = fa_right;

        sprite_index = Instantiate(typeof(S_Splash));

        image_speed = 0.4f;

        draw_set_font(new DefaultFont.DefaultFont());

        assembly = Heart.assembly;

        _splash();
    }

    public override void Step() {

        room_speed = 60;

        if (completeload && !completeset) {

            completeset = true;

            foreach (Sprite s in _sprite) { _ = s.texture; }

            foreach (Font f in _font) { f._update_texture(); }
        }

        image_index = float.Clamp(image_index, 0, 50);

        progressindex = progressindex-.025f;

        if (progressindex <= 0) { progressindex = 3.975f; }

        if (completeload && image_index >= 49.9f) {

            blush = blush+0.3f;
        }

        if (blush >= 20) {

            halign = _halign;

            COLOR_FORMAT = cformat;

            message_white .Dispose();
            message_violet.Dispose();

            sprite_index?.Dispose();

            Instantiate(typeof(S_Blush))?.Dispose();

            room_goto(_entry.point);
        }
    }

    internal async void _splash() {

        StringBuilder sbuilder = new StringBuilder();

        sbuilder.Clear().Append(CONTENT_PATH);

        if (CONTENT_PATH[^1] != separator) { sbuilder.Append(separator); }

        List<Type> types = [..assembly.GetTypes().Where(t => t.IsClass)];

        Dictionary<string, string> dsprite = [];
        Dictionary<string, string> dsound = [];

        List<Type> spritetypes = [..types.Where(t => t.BaseType == typeof(Sprite))];
        List<Type> fonttypes   = [..types.Where(t => t.BaseType == typeof(Font))];
        List<Type> soundtypes  = [..types.Where(t => t.BaseType == typeof(Sound))];

        _traversal(dsprite, $"{sbuilder}Sprite", ["xnb", "png"]);
        _traversal(dsound, $"{sbuilder}Sound", ["wav", "xnb", "ogg"]);

        tcount = spritetypes.Count+fonttypes.Count+soundtypes.Count;

        message = "Loading Textures...";

        await _sprpreload(dsprite, spritetypes);

        message = "Loading Fonts...";

        await _fntpreload(fonttypes);

        message = "Loading Sounds...";

        await _sndpreload(dsound, soundtypes);

        completeload = true;

        message = "Finishing Tasks...";
    }

    private static void _traversal(Dictionary<string, string> dictionary, string directory, string[] extlist) {

        string[] directories = Directory.GetFiles(directory);
        
        foreach (string s in directories) {

            foreach (string ext in extlist) {

                if (!s.EndsWith(ext)) { continue; }

                string classname = s[(directory.Length+1)..^4];

                dictionary.TryAdd(classname, s);
            }
        }

        string[] array_directories = Directory.GetDirectories(directory);

        foreach (string d in array_directories) {

            _traversal(dictionary, d, extlist);
        }
    }

    private static async Task _sprpreload(Dictionary<string, string> dictionary, List<Type> types) {

        await Task.Run(() => {

            foreach (Type t in types) {

                if (dictionary.TryGetValue(t.Name, out string? path)) {

                    Sprite? _s = (Sprite?)Convert.ChangeType(Activator.CreateInstance(t), t);

                    if (_s == null) { pcount = pcount+1; continue; }

                    _s._preload($"{path}");

                    _sprite.Add(_s);
                }
                
                pcount = pcount+1;
            }
        });
    }

    private static async Task _fntpreload(List<Type> types) {

        await Task.Run(() => {

            foreach (Type t in types) {

                Font? _f = (Font?)Convert.ChangeType(Activator.CreateInstance(t), t);

                if (_f == null) { pcount = pcount+1; continue; }

                _f._init_font(_f.name, _f.size_font, _f.range);

                _font.Add(_f);

                pcount = pcount+1;
            }
        });
    }

    private static async Task _sndpreload(Dictionary<string, string> dictionary, List<Type> types) {

        await Task.Run(() => {

            foreach (Type t in types) {

                if (dictionary.TryGetValue(t.Name, out string? _)) {

                    Sound? _s = (Sound?)Convert.ChangeType(Activator.CreateInstance(t), t);

                    if (_s == null) { pcount = pcount+1; continue; }

                    _ = _s.sfx;

                    _sound.Add(_s);
                }
                
                pcount = pcount+1;
            }
        });
    }

    public override void Draw() {

        draw_self();
        
        draw_set_color(0xffffff);

        draw_rectangle(x-(120*scale), y+(50*scale), 240*scale, 14*scale, true);

        int progress = 240;

        if (tcount > 0) { progress = 240*(pcount/tcount); }

        draw_rectangle(x-(120*scale), y+(50*scale), progress*scale, 14*scale, false);

        draw_set_halign(fa_center);        

        _graphicsDeviceManager.GraphicsDevice.SetRenderTarget(message_white);

        _graphicsDeviceManager.GraphicsDevice.Clear(color);

        draw_set_color(0xffffffu);

        draw_text(120, 0, message[..^(int)(float.Floor(progressindex))]);

        _graphicsDeviceManager.GraphicsDevice.SetRenderTarget(message_violet);

        _graphicsDeviceManager.GraphicsDevice.Clear(color);

        draw_set_color(_entry.color);

        draw_text(120, 0, message[..^(int)(float.Floor(progressindex))]);

        draw_set_color(0xffffffu);

        _graphicsDeviceManager.GraphicsDevice.SetRenderTarget(null);
        
        // draw_texture_part(message_white, progress, 0, 
        //                   240-progress, 11, x-120+progress, y+51);

        draw_texture_ext(message_white, x-(120*scale), y+(51*scale),
                         scale, scale, 0, 0, 0f, 0xffffffu, 1f
        );

        // draw_texture_part(message_violet, 0, 0, progress, 11, x-120, y+51);

        draw_texture_part_ext(message_violet, 0, 0, progress, 11, 
                              x-(120*scale), y+(51*scale),
                              scale, scale, 0xffffffu, 1f
        );

        draw_set_halign(fa_right);

        draw_text((X-10)*scale, (Y-35)*scale, $"VoldeNuit Framework v.{version}\nhttps://github.com/Navylera/VoldeNuit", scale, scale);
        // draw_text((X-10)*scale, (Y-20)*scale, "https://github.com/Navylera/VoldeNuit", scale, scale);

        int block = room_width/20;
        
        int blushwidth = block*(int)MathF.Pow(2, float.Floor(blush));
        int fillwidth = blush < 6? 0: block*(1<<(int)float.Floor(blush-5));

        for (int i=fillwidth; i<fillwidth+blushwidth; i=i+4) {

            if (i > room_width) { goto EXITCOLOR; }

            for (int k=0; k<room_height; k=k+64) {

                float iindex = 16*(1-(i-fillwidth)/(float)blushwidth);

                draw_sprite_ext(Instantiate(typeof(S_Blush)), iindex,
                                i, k, 1, 1, 0, _entry.color, 1
                );
            }
        }

        EXITCOLOR: 

        draw_set_color(_entry.color);
        draw_rectangle(0, 0, int.Clamp(fillwidth, 0, room_width), room_height);

        if (blush <= 6) { return; }

        blushwidth = block*(int)MathF.Pow(2, float.Floor(blush-6));
        fillwidth = blush < 12? 0: block*(1<<(int)float.Floor(blush-11));

        for (int i=fillwidth; i<fillwidth+blushwidth; i=i+4) {

            if (i > room_width) { goto EXITBLACK; }

            for (int k=0; k<room_height; k=k+64) {

                float iindex = 15*(1-(i-fillwidth)/(float)blushwidth);

                draw_sprite_ext(Instantiate(typeof(S_Blush)), iindex,
                                i, k, 1, 1, 0, 0x0u, 1
                );
            }
        }

        EXITBLACK: 

        draw_set_color(0x0u);
        draw_rectangle(0, 0, int.Clamp(fillwidth, 0, room_width), room_height);
    }
}