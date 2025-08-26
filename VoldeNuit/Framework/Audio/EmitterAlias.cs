using VoldeNuit.Framework.Instances;

namespace VoldeNuit.Framework.Audio;

public partial class Emitter {

    public float X { get => x; set => x = value; }

    public float Y { get => y; set => y = value; }

    public float HSpeed { get => _speed.X; set { _speed.X = value; } }

    public float VSpeed { get => _speed.Y; set { _speed.Y = value; } }

    public float Speed { get => speed; set => speed = value; }

    public float Direction { get => direction; set => direction = value; }

    public Falloff FallOff { get => falloff; set => falloff = value; }

    public float FallOffReference { get => falloff_reference; set => falloff_reference = value; }
    public float FallOffMax { get => falloff_max; set => falloff_max = value; }
    public float FallOffFactor { get => falloff_factor; set => falloff_factor = value; }

    public float SoundSpeed { get => speed_sound; set => speed_sound = value; }

    public void SyncTo(Instance ID) { sync_to(ID); }
}