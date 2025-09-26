using VoldeNuit.Framework.Instances;

namespace VoldeNuit.Framework.Display;

using static Quadtree;
using static Heart;
using static Exception;

public partial class Room {

    public readonly List<Camera> camera = [];

    private int _room_width = 1;

    public int room_width {

        set {

            if (value < 1) { _stacktrace(ExConstants.ROOM_SIZE_LESS_THEN_ONE); }

            _room_width = value;

            int qts = _size_qtree_collision;

            _size_qtree_collision = int.Max(_room_width, _room_height);
            _size_qtree_collision = 1<<(int)float.Ceiling(float.Log2(_size_qtree_collision));

            if (qts != _size_qtree_collision) { _qtree_collapsed = true; }
        }

        get => _room_width;
    }

    private int _room_height = 1;

    public int room_height {

        set {

            if (value < 1) { _stacktrace(ExConstants.ROOM_SIZE_LESS_THEN_ONE); }

            _room_height = value;

            int qts = _size_qtree_collision;

            _size_qtree_collision = int.Max(_room_width, _room_height);
            _size_qtree_collision = 1<<(int)float.Ceiling(float.Log2(_size_qtree_collision));

            if (qts != _size_qtree_collision) { _qtree_collapsed = true; }
        }

        get => _room_height;
    }

    private int _room_speed = 60;

    public int room_speed {

        get => _room_speed; set {

            _room_speed = room_speed;

            _main.TargetElapsedTime = TimeSpan.FromSeconds(1d/_room_speed);
        }
    }

    public uint color_background = 0xffffffffu;

    public Room() {

        Create();
    }

    public virtual void Create() { }

    internal void _Create() {

        _size_qtree_collision = int.Max(_room_width, _room_height);
        _size_qtree_collision = 1<<(int)float.Ceiling(float.Log2(_size_qtree_collision));

        _qtree_collapsed = true;

        if (camera.Count == 0) {

            camera.Add(new Camera(0, 0, _room_width, _room_height, 0, 0, _room_width, _room_height));
        }

        return;
    }
}