using System.Runtime.InteropServices;
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

    internal float alpha = 0;

    internal bool completeload = false;
    internal bool completeset = false;

    internal ColorFormat cformat = COLOR_FORMAT;

    internal int _halign = halign;

    internal float progressindex = 0f;

    internal string version = "1.0.1.1";

    internal RenderTarget2D message_white = new RenderTarget2D(
        _graphicsDeviceManager.GraphicsDevice, 240, 11
    );

    internal RenderTarget2D message_violet = new RenderTarget2D(
        _graphicsDeviceManager.GraphicsDevice, 240, 11
    );

    MonoColor color = Color._color_to_xna(0x00000000u);

    public I_Splash() {

        halign = fa_right;

        sprite_index = Instantiate(typeof(S_Splash));

        image_speed = 0.3f;

        draw_set_font(new DefaultFont.DefaultFont());

        //TODO: Indicate Assembly directory when import manually

        assembly = Assembly.LoadFrom($"{projectname}.dll");

        _splash();
    }

    public override void Step() {

        if (completeload && !completeset) {

            completeset = true;

            foreach (Sprite s in _sprite) { Console.WriteLine("!"+s.GetType()); _ = s.texture; }

            foreach (Font f in _font) { f._update_texture(); }
        }

        image_index = float.Clamp(image_index, 0, 50);

        progressindex = progressindex-.025f;

        if (progressindex <= 0) { progressindex = 3.975f; }

        if (completeload && image_index >= 49.9f) {

            alpha = float.Clamp(alpha+.0084f, 0, 1);
        }

        if (alpha >= 1) {

            halign = _halign;

            COLOR_FORMAT = cformat;

            message_white .Dispose();
            message_violet.Dispose();

            room_goto(_entry.point);
        }
    }

    internal async void _splash() {

        char split = '/';

        StringBuilder sbuilder = new StringBuilder();

        bool definded = false;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) {

            sbuilder.Clear().Append(CONTENT_PATH_LINUX);

            if (CONTENT_PATH_LINUX[^1] != '/') { sbuilder.Append('/'); }
            
            definded = true;
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {

            split = '\\';

            sbuilder.Clear().Append(CONTENT_PATH_WINDOWS);

            if (CONTENT_PATH_WINDOWS[^1] != '\\') { sbuilder.Append('\\'); }
            
            definded = true;
        }

        if (!definded) {

            sbuilder.Clear().Append(CONTENT_PATH_OTHERS);

            if (CONTENT_PATH_LINUX[^1] != '/') {

                sbuilder.Append('/');
            }
        }

        List<Type> types = [..assembly.GetTypes().Where(t => t.IsClass)];

        Dictionary<string, string> dsprite = [];
        Dictionary<string, string> dsound = [];

        List<Type> spritetypes = [..types.Where(t => t.BaseType == typeof(Sprite))];
        List<Type> fonttypes   = [..types.Where(t => t.BaseType == typeof(Font))];
        List<Type> soundtypes  = [..types.Where(t => t.BaseType == typeof(Sound))];

        _traversal(dsprite, $"{sbuilder}Sprite", "png");
        _traversal(dsound, $"{sbuilder}Sound", "wav");

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

    private static void _traversal(Dictionary<string, string> dictionary, string directory, string ext) {

        string[] directories = Directory.GetFiles(directory);
        
        foreach (string s in directories) {

            if (s[^3..] == ext) {

                string classname = s[(directory.Length+1)..^3];
                
                dictionary.Add(classname, s);
            }
        }

        string[] array_directories = Directory.GetDirectories(directory);

        foreach (string d in array_directories) {

            _traversal(dictionary, d, ext);
        }
    }

    private static async Task _sprpreload(Dictionary<string, string> dictionary, List<Type> types) {

        await Task.Run(() => {

            foreach (Type t in types) {

                if (dictionary.TryGetValue(t.Name, out string path)) {

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

                if (dictionary.TryGetValue(t.Name, out string path)) {

                    Sound? _s = (Sound?)Convert.ChangeType(Activator.CreateInstance(t), t);

                    if (_s == null) { pcount = pcount+1; continue; }

                    _sound.Add(_s);
                }
                
                pcount = pcount+1;
            }
        });
    }

    public override void Draw() {

        draw_self();
        
        draw_set_alpha(1f);
        draw_set_color(0xffffff);

        draw_rectangle(x-120, y+50, 240, 14, true);

        int progress = 240;

        if (tcount > 0) {
            
            progress = 240*(pcount/tcount);
        }

        draw_rectangle(x-120, y+51, progress, 13, false);

        draw_set_halign(fa_center);        

        _graphicsDeviceManager.GraphicsDevice.SetRenderTarget(message_white);

        _graphicsDeviceManager.GraphicsDevice.Clear(color);

        draw_set_color(0xffffffu);

        draw_text(120, 0, message[..^(int)(float.Floor(progressindex))]);

        _graphicsDeviceManager.GraphicsDevice.SetRenderTarget(message_violet);

        _graphicsDeviceManager.GraphicsDevice.Clear(color);

        draw_set_color(0x74569bu);

        draw_text(120, 0, message[..^(int)(float.Floor(progressindex))]);

        draw_set_color(0xffffffu);

        _graphicsDeviceManager.GraphicsDevice.SetRenderTarget(null);
        
        draw_texture_part(message_white, progress, 0, 240-progress, 11, x-120+progress, y+51);
        draw_texture_part(message_violet, 0, 0, progress, 11, x-120, y+51);

        draw_set_halign(fa_right);

        draw_text(640-10, 480-35, $"VoldeNuit Framework v.{version}");
        draw_text(640-10, 480-20, "https://github.com/Navylera/VoldeNuit");
    }

    public override void End_Draw() {

        draw_set_color(room_current.color_background);

        draw_rectangle(0, 0, room_current.room_width, room_current.room_height, true);

        draw_set_color(0xffffffu);

        draw_set_alpha(alpha);
        draw_set_color(0x000000);

        draw_rectangle(0, 0, room_current.room_width, room_current.room_height, false);
    }
}