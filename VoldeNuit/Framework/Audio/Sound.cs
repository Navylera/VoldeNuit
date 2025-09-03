using System.Runtime.InteropServices;
using System.Text;

using Microsoft.Xna.Framework.Audio;

namespace VoldeNuit.Framework.Audio;

using static Heart;
using static Configuration;
using static Exception;

public partial class Sound {

    internal float _volume = 1f;

    public float volume {

        get => _volume; set {

            foreach (SoundInstance si in _soundinstance.Where(si => si.sound == this)) {

                si._volume = volume;
            }
        }
    }

    public string? sound_path { get; init; } = null;

    private SoundEffect _sfx;

    internal SoundEffect sfx { 
        
        get {

            if (_sfx == null && sound_path != null && File.Exists(sound_path)) {

                string ext = sound_path[^3..];

                if (ext == "xnb") {

                    _sfx = _main.Content.Load<SoundEffect>(sound_path);
                }
                
                if (ext == "wav") {
                    
                    _sfx = SoundEffect.FromFile($"{sound_path}.wav");
                }
            }

            _sfx = _sfx?? load_sfx(GetType().Name);

            return _sfx;
        }

        set { _sfx?.Dispose(); _sfx = value; }
    }

    private static SoundEffect? _load_sfx(string directory, string target) {

        string path_target = directory+separator+target;

        if (File.Exists($"{path_target}.xnb")) { 
            
            return _main.Content.Load<SoundEffect>($"{path_target}.xnb");
        }

        if (File.Exists($"{path_target}.wav")) {

            return SoundEffect.FromFile($"{path_target}.wav");
        }

        string[] array_directories = Directory.GetDirectories(directory);

        SoundEffect? ret = null;

        foreach (string d in array_directories) {

            if (ret != null) { break; }

            ret = ret?? _load_sfx(d, target);
        }

        return ret;
    }

    internal static SoundEffect? load_sfx(string name_file) {

        StringBuilder sbuilder = new StringBuilder();

        sbuilder.Clear().Append(CONTENT_PATH);

        if (CONTENT_PATH[^1] != separator) { sbuilder.Append(separator); }
        
        sbuilder.Append("Sound");

        SoundEffect? sfx = _load_sfx(sbuilder.ToString(), name_file);

        if (sfx == null) { _stacktrace(ExConstants.SOUND_NOT_ACCESSABLE); return null; }

        return sfx;
    }
}