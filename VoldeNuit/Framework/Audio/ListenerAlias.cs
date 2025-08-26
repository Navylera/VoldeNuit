using Microsoft.Xna.Framework;

using VoldeNuit.Framework.Instances;

namespace VoldeNuit.Framework.Audio;

public partial class Listener {

    public float X { get => x; set => x = value; }
    public float Y { get => y; set => y = value; }

    public float HSpeed { get => _speed.X; set { _speed.X = value; } }

    public float VSpeed { get => _speed.Y; set { _speed.Y = value; } }

    public float Speed { get => speed; set => speed = value; }

    public float Direction { get => direction; set => direction = value; }

    public float PanAmount { get => pan_amount; set => pan_amount = value; }

    public Vector2 LookAt { get => _lookat; set => _lookat = Vector2.Normalize(value); }

    public void SyncTo(Instance ID) { sync_to(ID); }
}