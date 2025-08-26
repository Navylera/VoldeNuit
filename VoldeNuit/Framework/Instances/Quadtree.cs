using Microsoft.Xna.Framework;

namespace VoldeNuit.Framework.Instances;

using static Configuration;
using static Heart;

internal partial class Quadtree {

    internal static int _size_qtree_collision = 0;
        
    internal static Quadtree _qtree_collision;
    
    internal static HashSet<Instance> _list_outdated = [];

    internal static bool _qtree_collapsed = false;

    private readonly Quadtree[] _qtr = new Quadtree[4];

    private readonly int width;

    private Quadtree lt { set => _qtr[0] = value; get => _qtr[0]; }
    private Quadtree rt { set => _qtr[1] = value; get => _qtr[1]; }
    private Quadtree lb { set => _qtr[2] = value; get => _qtr[2]; }
    private Quadtree rb { set => _qtr[3] = value; get => _qtr[3]; }

    internal Dictionary<Instance, Rectangle> dict_regiondata;

    private Rectangle region;

    internal Quadtree() {

        _qtree_collapsed = false;

        _list_outdated.Clear();

        width = _size_qtree_collision/2;

        dict_regiondata = [];

        region = new Rectangle(0, 0, _size_qtree_collision, _size_qtree_collision);

        HashSet<Instance> list_intersect = [.._instance_id];

        foreach (Instance instance in _instance_id) {

            if (instance.mask_index == null || !instance.collision) { continue; }

            dict_regiondata.Add(instance, _get_instance_region(instance));
        }

        lt = new Quadtree(new Rectangle(0, 0, width, width), dict_regiondata, list_intersect);
        rt = new Quadtree(new Rectangle(width, 0, width, width), dict_regiondata, list_intersect);
        lb = new Quadtree(new Rectangle(0, width, width, width), dict_regiondata, list_intersect);
        rb = new Quadtree(new Rectangle(width, width, width, width), dict_regiondata, list_intersect);
    }

    private Quadtree(Rectangle region, Dictionary<Instance, Rectangle> rdata, HashSet<Instance> list_intersect) {

        this.region = region;

        dict_regiondata = [];

        HashSet<Instance> _list_intersect = [];

        foreach (Instance instance in list_intersect) {

            rdata.TryGetValue(instance, out Rectangle _region);

            if (!instance.collision || !region.Intersects(_region)) { continue; }

            dict_regiondata.Add(instance, _region);

            _list_intersect.Add(instance);
        }

        if (dict_regiondata.Count == 0 || region.Width <= QTREE_MINSIZE) { return; }

        width = region.Width/2;

        lt = new Quadtree(new Rectangle(region.X, region.Y, width, width), dict_regiondata, _list_intersect);
        rt = new Quadtree(new Rectangle(region.X+width, region.Y, width, width), dict_regiondata, _list_intersect);
        lb = new Quadtree(new Rectangle(region.X, region.Y+width, width, width), dict_regiondata, _list_intersect);
        rb = new Quadtree(new Rectangle(region.X+width, region.Y+width, width, width), dict_regiondata, _list_intersect);
    }
}