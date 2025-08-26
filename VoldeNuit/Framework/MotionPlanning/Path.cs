using Microsoft.Xna.Framework;

namespace VoldeNuit.Framework.MotionPlanning;

public partial class Path {

    private readonly List<Vector2> _pathlist = [];

    public int Count { get => _pathlist.Count; }

    public Vector2 this[int i] {

        get => _pathlist[i];

        set => _pathlist.Insert(i, value);
    }

    public void Add(Vector2 v) { _pathlist[_pathlist.Count] = v; return; }

    public void Clear() { _pathlist.Clear(); return; }
}