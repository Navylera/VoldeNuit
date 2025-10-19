using System.Buffers.Binary;
using System.Text;

using Microsoft.Xna.Framework.Audio;

using NVorbis;

namespace VoldeNuit.Framework.Audio;

using static Heart;
using static Configuration;
using static Exception;

public partial class Sound {

    internal float _volume = 1f;

    public float volume {

        get => _volume; set {

            _volume = value;

            foreach (SoundInstance si in _soundinstance.Where(si => si.sound == this)) {

                si._volume = volume;
            }
        }
    }

    public string? sound_path { get; init; } = null;

    private SoundEffect _sfx = null!;

    internal SoundEffect sfx { 
        
        get {

            if (_sfx == null && sound_path != null && File.Exists(sound_path)) {

                string ext = sound_path[^3..];

                switch (ext) {

                    case "xnb": {

                        if (CONTENT_PATH != $".{separator}Content{separator}") { break; }

                        _sfx = _main.Content.Load<SoundEffect>(sound_path[10..^4]);

                        break;
                    }

                    case "wav": {

                        _sfx = SoundEffect.FromFile($"{sound_path}.wav"); break;
                    }

                    case "ogg": {

                        _sfx = _read_ogg($"{sound_path}.ogg");

                        break;
                    }
                }
            }

            _sfx = _sfx?? load_sfx(GetType().Name)!;

            return _sfx;
        }

        set { _sfx?.Dispose(); _sfx = value; }
    }

    private static SoundEffect? _load_sfx(string directory, string target) {

        string path_target = directory+separator+target;

        if (File.Exists($"{path_target}.xnb") &&
            CONTENT_PATH == $".{separator}Content{separator}") { 
            
            return _main.Content.Load<SoundEffect>(path_target[10..]);
        }

        if (File.Exists($"{path_target}.wav")) {

            return SoundEffect.FromFile($"{path_target}.wav");
        }

        if (File.Exists($"{path_target}.ogg")) {

            return _read_ogg($"{path_target}.ogg");
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

    internal static SoundEffect _read_ogg(string sound_path) {

        FileStream fstream = File.OpenRead(sound_path);

        VorbisReader vreader = new VorbisReader(fstream, true);

        int samplerate = vreader.SampleRate;
        int channels = vreader.Channels;

        float[] rbuffer = new float[samplerate*channels];
        byte[]  output  = new byte[sizeof(short)*vreader.TotalSamples*channels];

        int count;

        int offset = 0;

        while ((count = vreader.ReadSamples(rbuffer, 0, rbuffer.Length)) > 0) {

            for (int i=0; i<count; i=i+1) {

                short data = short.Clamp((short)(short.MaxValue*rbuffer[i]), short.MinValue, short.MaxValue);

                BinaryPrimitives.WriteInt16LittleEndian(output.AsSpan(offset, 2), data);
                
                offset = offset+2;
            }
        }

        SoundEffect sfx = new SoundEffect(output, samplerate, 
                                          channels == 1? AudioChannels.Mono: AudioChannels.Stereo);

        fstream.Close();
        fstream.Dispose();

        vreader.Dispose();

        return sfx;
    }
}