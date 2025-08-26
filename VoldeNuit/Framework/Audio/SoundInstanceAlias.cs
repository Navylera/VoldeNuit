namespace VoldeNuit.Framework.Audio;

public partial class SoundInstance {

    public Sound Sound { get => sound; set => sound = value; }
    public Emitter? Emitter { get => emitter; set => emitter = value; }
    public Listener? Listener { get => listener; set => listener = value; }
    
    public float Gain { get => gain; set => gain = value; }
    public float Pitch { get => pitch; set => pitch = value; }
    public float Pan { get => pan; set => pan = value; }

    public bool Loop { get => loop; set => loop = value; }
}