using Microsoft.Xna.Framework.Audio;

namespace VoldeNuit.Framework.Audio;

using static Heart;

public partial class Sound {

    public static SoundInstance audio_play_sound(Sound sound, bool loop = false) { 

        return new SoundInstance(sound, loop);
    }

    public static SoundInstance audio_play_sound_ext(Sound sound, Emitter emitter, Listener listener, bool loop = false) {

        return new SoundInstance(sound, emitter, listener, loop);
    }

    public static void audio_pause_sound(Sound sound) { 

        foreach (SoundInstance si in _soundinstance) {

            if (si.sound == sound) { si._sfxi?.Pause(); }
        }

        return;
    }

    public static void audio_pause_sound(SoundInstance soundinstance) { 

        foreach (SoundInstance si in _soundinstance) {

            if (si == soundinstance) { si._sfxi?.Pause(); return; }
        }

        return;
    }

    public static void audio_pause_all() { 

        foreach (SoundInstance si in _soundinstance) {

            if (si._sfxi?.State == SoundState.Playing) { si._sfxi?.Pause(); }
        }

        return;
    }

    public static void audio_resume_sound(Sound sound) { 

        foreach (SoundInstance si in _soundinstance) {

            if (si.sound == sound && si._sfxi?.State == SoundState.Paused) { si._sfxi?.Resume(); }
        }

        return;
    }

    public static void audio_resume_sound(SoundInstance soundinstance) { 

        foreach (SoundInstance si in _soundinstance) {

            if (si == soundinstance && si._sfxi?.State == SoundState.Paused) { si._sfxi?.Resume(); return; }
        }

        return;
    }

    public static void audio_resume_all() { 

        foreach (SoundInstance si in _soundinstance) {

            if (si._sfxi?.State == SoundState.Paused) { si._sfxi?.Resume(); }
        }

        return;
    }

    public static void audio_stop_sound(Sound sound) { 

        foreach (SoundInstance si in _soundinstance) {

            if (si.sound == sound) { si._sfxi?.Stop(); }
        }

        return;
    }

    public static void audio_stop_sound(SoundInstance soundinstance) { 

        foreach (SoundInstance si in _soundinstance) {

            if (si == soundinstance) { si._sfxi?.Stop(); return; }
        }

        return;
    }

    public static void audio_stop_all() { 

        foreach (SoundInstance si in _soundinstance) { si._sfxi?.Stop(); } return;
    }

    public static bool audio_is_playing(Sound sound) {

        foreach (SoundInstance si in _soundinstance) {

            if (si.sound == sound && si._sfxi?.State == SoundState.Playing) { return true; }
        }

        return false;
    }

    public static bool audio_is_playing(SoundInstance soundinstance) {

        return soundinstance._sfxi?.State == SoundState.Playing;
    }

    public static bool audio_is_paused(Sound sound) {

        foreach (SoundInstance si in _soundinstance) {

            if (si.sound == sound && si._sfxi?.State == SoundState.Paused) { return true; }
        }

        return false;
    }

    public static bool audio_is_paused(SoundInstance soundinstance) {

        return soundinstance._sfxi?.State == SoundState.Playing;
    }

    
}