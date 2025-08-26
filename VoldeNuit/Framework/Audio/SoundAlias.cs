namespace VoldeNuit.Framework.Audio;

public partial class Sound {

    public float Volume { get => volume; set => volume = value; }

    public string? SoundPath { get => sound_path; init => sound_path = value; }
}