using VoldeNuit.Framework.Instances;
using VoldeNuit.Framework.Drawing;
using VoldeNuit.Framework.Display;
using VoldeNuit.Framework.Audio;

namespace VoldeNuit.Framework;

public static partial class Heart {

    public static ReadOnlySpan<Instance> GetInstancesList() { return instance_id; }

    public static ReadOnlySpan<Instance> GetInstancesDeactivatedList() { return instance_id_deactivated; }

    public static Room CurrentRoom { get => _room_current; }
    
    public static int RoomSpeed { get => room_speed; set => room_speed = value; }

    public static Font CurrentFont { get => font_current; set => value = font_current; }
}