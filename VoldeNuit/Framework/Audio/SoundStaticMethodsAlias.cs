using System.Runtime.CompilerServices;

namespace VoldeNuit.Framework.Audio;

public partial class Sound {
    
    public static SoundInstance SoundPlay(Sound sound, bool loop = false) { 

        return new SoundInstance(sound, loop);
    }

    public static SoundInstance SoundPlay(Sound sound, Emitter emitter, Listener listener, bool loop = false) {

        return new SoundInstance(sound, emitter, listener, loop);
    }

    public static void SoundPause() {

        audio_pause_all();
    }

    public static void SoundPause(Sound sound) {

        audio_pause_sound(sound);
    }

    public static void SoundPause(SoundInstance soundInstance) {

        audio_pause_sound(soundInstance);
    }

    public static void SoundResume() {

        audio_resume_all();
    }

    public static void SoundResume(Sound sound) {

        audio_resume_sound(sound);
    }

    public static void SoundResume(SoundInstance soundInstance) {

        audio_resume_sound(soundInstance);
    }

    public static void SoundStop() {

        audio_stop_all();
    }

    public static void SoundStop(Sound sound) {

        audio_stop_sound(sound);
    }

    public static void SoundStop(SoundInstance soundInstance) {

        audio_stop_sound(soundInstance);
    }

    public static void IsSoundPlaying(Sound sound) {

        audio_is_playing(sound);
    }

    public static void IsSoundPlaying(SoundInstance soundInstance) {

        audio_is_playing(soundInstance);
    }

    public static void IsSoundPaused(Sound sound) {

        audio_is_paused(sound);
    }

    public static void IsSoundPaused(SoundInstance soundInstance) {

        audio_is_paused(soundInstance);
    }
}