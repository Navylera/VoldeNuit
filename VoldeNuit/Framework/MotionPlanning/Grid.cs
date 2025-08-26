using Microsoft.Xna.Framework;

namespace VoldeNuit.Framework.MotionPlanning;

using VoldeNuit.Framework.Instances;
using VoldeNuit.Framework.Drawing;

public partial class Grid {

    public void Dispose() { _ci?.sprite_index?.Dispose(); _ci?.Dispose(); return; }

    public int x;
    public int y;

    public int hcells { get; init; }
    public int vcells { get; init; }

    public int cellwidth { get; init; }
    public int cellheight { get; init; }

    internal int[, ] _grid;

    public const int SOLID = -1;
    public const int UNDEFINED = -2;

    internal Instance? _ci = null;

    public int this[int h, int v] {

        get => _grid[h, v];

        set => _grid[h, v] = value;
    }

    internal class _Collider: Instance {

        internal _Collider() {}
    }
    
    public Grid(int x, int y, int hcells, int vcells, int cellwidth, int cellheight) {

        this.x = x;
        this.y = y;

        this.hcells = hcells;
        this.vcells = vcells;

        this.cellwidth = cellwidth;
        this.cellheight = cellheight;

        _grid = new int[hcells, vcells];

        for (int i=0; i<hcells*vcells; i=i+1) { _grid[i%hcells, i/hcells] = UNDEFINED; }

        _ci = new _Collider() { sprite_index = new Sprite() };

        _ci.sprite_index.bbox = new Rectangle(0, 0, cellwidth, cellheight);
    }
}