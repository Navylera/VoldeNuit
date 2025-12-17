using System.Reflection;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

using MonoColor = Microsoft.Xna.Framework.Color;

using VoldeNuit.Framework.Instances;
using VoldeNuit.Framework.Drawing;
using VoldeNuit.Framework.Display;
using VoldeNuit.Framework.Input;
using VoldeNuit.Framework.Audio;

namespace VoldeNuit.Framework;

using static Room;
using static Mouse;
using static Camera;
using static Drawing.Color;
using static Keyboard;
using static Configuration;

public static partial class Heart {

    internal static Game _main = null!;

    internal static GraphicsDeviceManager _graphicsDeviceManager = null!;

    public static GraphicsDevice graphicsDevice {

        get => _graphicsDeviceManager.GraphicsDevice;
    }

    internal static List<Instance> _beat_copy = [];

    internal static readonly HashSet<Instance> _instance_id = [];

    public static ReadOnlySpan<Instance> instance_id {
        
        get { return new ReadOnlySpan<Instance>([.._instance_id]); }
    }

    internal static readonly HashSet<Instance> _instance_id_deactivated = [];
    
    public static ReadOnlySpan<Instance> instance_id_deactivated {
        
        get { return new ReadOnlySpan<Instance>([.._instance_id_deactivated]); }
    }

    internal static HashSet<Instance> instance_id_solid = [];

    internal static readonly HashSet<Sprite> _sprite = [];

    // internal static readonly List<Room> _room = [];

    internal static Room _room_current = null!;

    public static Room room_current { get => _room_current; }

    public static int room_width {

        get => room_current.room_width; set => room_current.room_width = value;
    }

    public static int room_height {

        get => room_current.room_height; set => room_current.room_height = value;
    }
    
    public static int room_speed {

        get => room_current.room_speed; set => room_current.room_speed = value;
    }

    public static int instance_number { 
        
        get => _instance_id.Where(i => !i._disposed).Count();
    }

    internal static readonly List<Font> _font = [];

    public static Font font_current { get; set; } = null!;

    internal static readonly List<DrawData> _draw = [];

    internal static Texture2D _primitive = null!;

    internal static readonly List<Emitter> emitter = [];

    internal static readonly List<Listener> listener = [];

    internal static readonly List<Sound> _sound = [];

    internal static readonly List<SoundInstance> _soundinstance = [];

    internal static GameWindow window = null!;

    internal static SpriteBatch _spritebatch = null!;

    internal static (int width, int height, Type point, uint bcolor, uint tcolor) _entry = (1, 1, null!, 0xffde5bu, 0x0u);

    internal static Assembly assembly = null!;

    internal static Assembly _assembly = Assembly.GetExecutingAssembly();

    internal static char separator = Path.DirectorySeparatorChar;

    internal static string version = _assembly.GetName().Version!.ToString()[..^2];

    internal static Progress _progress = Progress.BEGIN_STEP;

    internal enum Progress {

        BEGIN_STEP = 1, 
        STEP       = 2,
        END_STEP   = 4,
        BEGIN_DRAW = 8,
        DRAW       = 16,
        END_DRAW   = 32,
        GUI_DRAW   = 64,
    }
    
    // Core functions
    public static void InitMonoGameEnvironment(Assembly assembly, Game game, GraphicsDeviceManager gdeviceManager) { 

        if (Heart.assembly != null) { return; }
        
        Heart.assembly = assembly;

        _main = game;

        window = game.Window;

        _graphicsDeviceManager = gdeviceManager;

        _primitive = new Texture2D(gdeviceManager.GraphicsDevice, 1, 1);
        _primitive.SetData(new byte[] { 255, 255, 255, 255 } );

        if (listener.Count == 0) { listener.Add(new Listener()); }

        room_goto(typeof(Preload.R_Splash));
    }

    public static void InitResolution(int width, int height) {

        _entry.width = width; _entry.height = height;
    }    

    public static void InitSplashBgColor(uint color) {

        _entry.bcolor = color;
    }

    public static void InitSplashTextColor(uint color) {

        _entry.tcolor = color;
    }

    public static void InitEntryPoint(Type room) {

        _entry.point = room;
    }

    public static void InitSpriteBatch(SpriteBatch spriteBatch) {

        _spritebatch = spriteBatch;
    }

    public static void Beat() {

        // Use copy of existing list => new instances are not updated in this step

        _beat_copy = [.._instance_id];

        _progress = Progress.BEGIN_STEP;
        foreach (Instance instance in _beat_copy) { 

            if (instance != null && !instance._disposed) { instance._Begin_Step(); };

            if (_beat_copy.Count == 0) { break; }
        }

        _progress = Progress.STEP;
        foreach (Instance instance in _beat_copy) { 

            if (instance != null && !instance._disposed) { instance._Step(); };

            if (_beat_copy.Count == 0) { break; }
        }

        _progress = Progress.END_STEP;
        foreach (Instance instance in _beat_copy) { 
        
            if (instance != null && !instance._disposed) { instance._End_Step(); };

            if (_beat_copy.Count == 0) { break; }
        }

        List<SoundInstance> _sicp = [.._soundinstance];
        foreach (SoundInstance si in _sicp) {

            if (!si.loop && si._sfxi?.State == SoundState.Stopped) { si.Dispose(); continue; }

            if (si._update) { si._update_sound(); }
        }

        _update_window_size();
        
        // Methods in Draw cannot get reference of the Instance
        // Sort Instances in descending order

        _beat_copy = [.._instance_id.OrderByDescending(i => i.depth)];

        _progress = Progress.BEGIN_DRAW;
        foreach (Instance instance in _beat_copy) { 

            instance.Begin_Draw(); 
        }

        _progress = Progress.DRAW;
        foreach (Instance instance in _beat_copy) { 

            instance.Draw(); 
        }

        _progress = Progress.END_DRAW;
        foreach (Instance instance in _beat_copy) { 

            instance.End_Draw(); 
        }

        instance_id_solid.Clear();

        foreach (Instance instance in _instance_id) {

            if (instance.solid) { instance_id_solid.Add(instance); }
        }

        _progress = Progress.GUI_DRAW;
        foreach (Instance instance in _beat_copy) {

            instance.GUI_Draw();
        }

        _update_keyboard();

        _update_mouse();

        return;
    }

    public static void Draw() {

        List<DrawData> _ndraw = [];
        List<DrawData> _gdraw = [];

        foreach (Camera c in room_current.camera) {

            _graphicsDeviceManager.GraphicsDevice.SetRenderTarget(c.view._render_target);

            MonoColor color = color_to_xna(room_current.color_background);

            _graphicsDeviceManager.GraphicsDevice.Clear(color);

            _spritebatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);

            _ndraw.Clear();

            foreach (DrawData d in _draw) { 
                
                if (!d.gui) { _ndraw.Add(d); continue; }

                _gdraw.Add(d);
            }

            foreach (DrawData drd in _ndraw) { 

                float radian = drd.angle%(2*float.Pi);

                switch (ANGLE_FORMAT) {

                    case AngleFormat.RADIAN: { 
                        
                        radian = drd.angle%(2*float.Pi); break;
                    }

                    case AngleFormat.DEGREE: {

                        radian = (drd.angle/180f*float.Pi)%(2*float.Pi); break;
                    }

                    case AngleFormat.LEGACY: {

                        radian = (-drd.angle/180f*float.Pi)%(2*float.Pi);

                        if (radian < 0) { radian = radian+(2*float.Pi); } break;
                    }
                }

                SpriteEffects spe = SpriteEffects.None;

                if (drd.image_xscale < 0) { spe = spe|SpriteEffects.FlipHorizontally; }

                if (drd.image_yscale < 0) { spe = spe|SpriteEffects.FlipVertically; }

                _spritebatch.Draw(drd.texture,
                                  new Vector2(drd.x-c.view.x, drd.y-c.view.y), 
                                  drd.region, 
                                  color_to_xna(drd.color), 
                                  radian, 
                                  new Vector2(drd.vx, drd.vy), 
                                  new Vector2(float.Abs(drd.image_xscale), 
                                              float.Abs(drd.image_yscale)
                                  ), spe, 0f
                );
            }

            _spritebatch.End();
        }

        _draw.Clear();

        _graphicsDeviceManager.GraphicsDevice.SetRenderTarget(null);

        _spritebatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp);

        foreach (Camera c in room_current.camera) {

            if (!c.visible) { continue; }

            _spritebatch.Draw(c.view._render_target,
                              new Rectangle(c.viewport.x, c.viewport.y, 
                                            c.viewport.width, c.viewport.height),
                              color_to_xna(c.colorfilter)
            );
        }

        _ndraw.Clear();

        foreach (DrawData drd in _gdraw) {

            float radian = drd.angle%(2*float.Pi);

            switch (ANGLE_FORMAT) {

                case AngleFormat.RADIAN: { 
                    
                    radian = drd.angle%(2*float.Pi); break;
                }

                case AngleFormat.DEGREE: {

                    radian = (drd.angle/180f*float.Pi)%(2*float.Pi); break;
                }

                case AngleFormat.LEGACY: {

                    radian = (-drd.angle/180f*float.Pi)%(2*float.Pi);

                    if (radian < 0) { radian = radian+(2*float.Pi); } break;
                }
            }

            SpriteEffects spe = SpriteEffects.None;

            if (drd.image_xscale < 0) { spe = spe|SpriteEffects.FlipHorizontally; }

            if (drd.image_yscale < 0) { spe = spe|SpriteEffects.FlipVertically; }

            _spritebatch.Draw(drd.texture,
                              new Vector2(drd.x, drd.y), 
                              drd.region, 
                              color_to_xna(drd.color), 
                              radian, 
                              new Vector2(drd.vx, drd.vy), 
                              new Vector2(float.Abs(drd.image_xscale), 
                                          float.Abs(drd.image_yscale)
                              ), spe, 0f
            );
        }

        _gdraw.Clear();

        _spritebatch.End();
    }
}