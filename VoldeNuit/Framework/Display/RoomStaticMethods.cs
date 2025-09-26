using VoldeNuit.Framework.Audio;
using VoldeNuit.Framework.Instances;

namespace VoldeNuit.Framework.Display;

using static Heart;

public partial class Room {

    public static void room_restart() {

        room_goto(room_current.GetType());
    }

    public static void room_goto(Type name_room) {

        Type? ptype = name_room.BaseType;

        while (ptype != typeof(Room)) {

            if (ptype == null || ptype == typeof(Room)) { return; }

            ptype = ptype.BaseType;
        }

        Room? dst = null;

        // foreach (Room r in _room) { if (r.GetType() == name_room) { dst = r; break; } }

        foreach (Instance o in _instance_id) { o.Dispose(); }
        foreach (Instance o in _instance_id_deactivated) { o.Dispose(); }

        foreach (SoundInstance si in _soundinstance) { si.Dispose(); }

        _beat_copy.Clear();

        _instance_id.Clear();
        _instance_id_deactivated.Clear();
        _draw.Clear();
        
        dst = dst?? (Room?)Activator.CreateInstance(name_room);

        if (dst == null) { return; }

        _room_current = dst;

        _room_current._Create();

        return;
    }
}