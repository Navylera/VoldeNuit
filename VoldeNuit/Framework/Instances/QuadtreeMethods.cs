using Microsoft.Xna.Framework;

using VoldeNuit.Framework.Drawing;

namespace VoldeNuit.Framework.Instances;

using static Configuration;
using static Heart;

internal partial class Quadtree {

    internal static byte[] _get_byte_data_region(byte[] data, Rectangle region, bool alpha_only = false) {

        byte[] ret;

        ret = alpha_only? new byte[region.Width*region.Height]: 
                          new byte[4*region.Width*region.Height];

        int count = 0;

        int pos_alpha = 3;

        if (alpha_only) {

            for (int k=0; k<region.Height; k=k+1) {

                for (int i=0; i<region.Width; i=i+1) {

                    int pos = 4*(((region.Y+k)*region.Width)+region.X+i);

                    ret[count] = data[pos+pos_alpha];

                    count = count+1;
                }
            }

            return ret;
        }

        for (int k=0; k<region.Height; k=k+1) {

            for (int i=0; i<region.Width; i=i+1) {

                int pos = 4*(((region.Y+k)*region.Width)+region.X+i);

                ret[count  ] = data[pos  ];
                ret[count+1] = data[pos+1];
                ret[count+2] = data[pos+2];
                ret[count+3] = data[pos+3];

                count = count+4;
            }
        }

        return ret;
    }

    internal static Rectangle _get_instance_region(Instance instance) {

        if (instance.mask_index == null) { return new Rectangle(0, 0, 0, 0); }

        Sprite sprite = instance.mask_index;

        int px = (int)float.Round(instance.x, ROUNDING)-sprite.x;
        int py = (int)float.Round(instance.y, ROUNDING)-sprite.y;

        int pw = sprite.sprite_width;
        int ph = sprite.sprite_height;

        if (sprite.bbox == null) { return new Rectangle(px, py, pw, ph); }

        Rectangle bbox = (Rectangle)sprite.bbox;

        px = (int)instance.x-bbox.X;
        py = (int)instance.y-bbox.Y;

        pw = bbox.Width;
        ph = bbox.Height;

        return new Rectangle(px, py, pw, ph);
    }

    internal Instance? _check_point(float x, float y, HashSet<Instance> list_instance) {

        HashSet<Instance> _list_instance = [];

        Instance? ret = null;

        foreach (Instance instance in list_instance) {

            if (dict_regiondata.ContainsKey(instance)) { _list_instance.Add(instance); }
        }

        if (_list_instance.Count == 0) { return null; }

        for (int i=0; i<4; i=i+1) {

            if (ret != null) { return ret; }

            if (_qtr[i] == null) { continue; }

            if (width > QTREE_MINSIZE) {

                ret = _qtr[i]._check_point(x, y, _list_instance);

                if (ret != null) { return ret; }
            }

            foreach (Instance instance in _list_instance) {

                Sprite? _sprite = instance.mask_index;

                if (_sprite == null) { continue; }

                dict_regiondata.TryGetValue(instance, out Rectangle region_instance);

                Rectangle region_point = new Rectangle((int)float.Round(x, ROUNDING), 
                                                       (int)float.Round(y, ROUNDING), 
                                                       1, 1
                );

                if (!region_instance.Intersects(region_point)) { continue; }

                if (_sprite.bbox != null) { return instance; }

                int rx  = (int)float.Round(x, ROUNDING);
                int rix = (int)float.Round(instance.x, ROUNDING);

                int ry  = (int)float.Round(y, ROUNDING);
                int riy = (int)float.Round(instance.y, ROUNDING);

                int pos = 4*(((ry-riy+_sprite.y)*_sprite.sprite_width)+(rx-rix+_sprite.x));

                if (_sprite[instance.image_index][pos] > 0) { return instance; }
            }
        }

        return null;
    }

    internal void _check_point_list(float x, float y, HashSet<Instance> list_instance,
                                    out HashSet<Instance> list_collision) {

        list_collision = [];

        HashSet<Instance> _list_instance = [];

        foreach (Instance instance in list_instance) {

            if (dict_regiondata.ContainsKey(instance)) { _list_instance.Add(instance); }
        }

        if (_list_instance.Count == 0) { return; }

        for (int i=0; i<4; i=i+1) {

            if (_qtr[i] == null) { continue; }

            if (width > QTREE_MINSIZE) { 
                
                _qtr[i]._check_point_list(x, y, _list_instance, list_collision);
            }
        }

        return;
    }

    internal void _check_point_list(float x, float y, HashSet<Instance> list_instance,
                                    HashSet<Instance> list_collision) {
        
        HashSet<Instance> _list_instance = [];

        foreach (Instance instance in list_instance) {

            if (dict_regiondata.ContainsKey(instance)) { _list_instance.Add(instance); }
        }

        if (_list_instance.Count == 0) { return; }

        for (int i=0; i<4; i=i+1) {

            if (_qtr[i] == null) { continue; }

            if (width > QTREE_MINSIZE) {

                _qtr[i]._check_point(x, y, _list_instance);
            }

            foreach (Instance instance in _list_instance) {

                Instance? result = _check_point_each(x, y, instance);

                if (result != null) { list_collision.Add(result); }
            }
        }

        return;
    }

    internal Instance? _check_point_each(float x, float y, Instance instance) {

        Sprite? _sprite = instance.mask_index;

        if (_sprite == null) { return null; }

        dict_regiondata.TryGetValue(instance, out Rectangle region_instance);

        Rectangle region_point = new Rectangle((int)float.Round(x, ROUNDING), 
                                                (int)float.Round(y, ROUNDING), 
                                                1, 1
        );

        if (!region_instance.Intersects(region_point)) { return null; }

        if (_sprite.bbox != null) { return instance; }

        int rx  = (int)float.Round(x, ROUNDING);
        int rix = (int)float.Round(instance.x, ROUNDING);

        int ry  = (int)float.Round(y, ROUNDING);
        int riy = (int)float.Round(instance.y, ROUNDING);

        int pos = 4*(((ry-riy+_sprite.y)*_sprite.sprite_width)+(rx-rix+_sprite.x));

        if (_sprite[instance.image_index][pos] > 0) { return instance; }

        return null;
    }

    internal Instance? _check_prec(Instance id, HashSet<Instance> list_instance) {

        if (id.mask_index == null || !dict_regiondata.ContainsKey(id)) { return null; }

        Instance? ret = null;

        HashSet<Instance> _list_instance = [];

        foreach (Instance instance in list_instance) {

            if (dict_regiondata.ContainsKey(instance) && instance != id) {

                _list_instance.Add(instance);
            }
        }

        if (_list_instance.Count == 0) { return null; }

        for (int i=0; i<4; i=i+1) {

            if (ret != null) { return ret; }

            if (_qtr[i] == null) { continue; }

            if (width > QTREE_MINSIZE) {

                ret = _qtr[i]._check_prec(id, _list_instance);

                if (ret != null) { return ret; }
            }

            ret = _check_prec_each(id, _list_instance);
        }

        return ret;
    }

    internal void _check_prec_list(Instance id, HashSet<Instance> list_instance,
                                   HashSet<Instance> list_collision) {

        HashSet<Instance> _list_instance = [];

        foreach (Instance instance in list_instance) {

            if (dict_regiondata.ContainsKey(instance) && instance != id) {

                _list_instance.Add(instance);
            }
        }

        if (_list_instance.Count == 0) { return; }

        for (int i=0; i<4; i=i+1) {

            if (_qtr[i] == null) { continue; }

            if (width > QTREE_MINSIZE) {

                _qtr[i]._check_prec_list(id, _list_instance, list_collision);
            }

            Instance? result = _check_prec_each(id, _list_instance);

            if (result != null) { list_collision.Add(result); }
        }
    }

    internal void _check_prec_list(Instance id, HashSet<Instance> list_instance, 
                                   out HashSet<Instance> list_collision) {

        list_collision = [];

        if (id.mask_index == null || !dict_regiondata.ContainsKey(id)) { return; }

        HashSet<Instance> _list_instance = [];

        foreach (Instance instance in list_instance) {

            if (dict_regiondata.ContainsKey(instance) && instance != id) {

                _list_instance.Add(instance);
            }
        }

        for (int i=0; i<4; i=i+1) {

            if (_qtr[i] == null) { continue; }

            if (width > QTREE_MINSIZE) {

                _qtr[i]._check_prec_list(id, _list_instance, list_collision);
            }
        }

        return;
    }

    internal Instance? _check_prec_each(Instance id, HashSet<Instance> list_instance) {

        if (id.mask_index == null) { return null; }

        Sprite left = id.mask_index;

        dict_regiondata.TryGetValue(id, out Rectangle region_left);

        foreach (Instance instance in list_instance) {

            Sprite? right = instance.mask_index;

            if (right == null) { continue; }

            dict_regiondata.TryGetValue(instance, out Rectangle region_right);

            if (!region_left.Intersects(region_right)) { continue; }

            if (left.bbox != null && right.bbox != null) { return instance; }

            int w_inter = region_right.Width+region_left.Width-
                          int.Max(region_right.X+region_right.Width, 
                                  region_left.X+region_left.Width)+
                          int.Min(region_right.X, region_left.X)
            ;

            int h_inter = region_right.Height+region_left.Height-
                          int.Max(region_right.Y+region_right.Height, 
                                  region_left.Y+region_left.Height)+
                          int.Min(region_right.Y, region_left.Y)
            ;

            int x_inter_left = int.Max(region_right.X, region_left.X)-
                               (int)float.Round(id.x, ROUNDING)+left.x;

            int y_inter_left = int.Max(region_right.Y, region_left.Y)-
                               (int)float.Round(id.y, ROUNDING)+left.y;

            Rectangle region_inter_left = new Rectangle(x_inter_left, y_inter_left, w_inter, h_inter);

            int x_inter_right = int.Max(region_right.X, region_left.X)-
                                (int)float.Round(instance.x, ROUNDING)+right.x;

            int y_inter_right = int.Max(region_right.Y, region_left.Y)-
                                (int)float.Round(instance.y, ROUNDING)+right.y;

            Rectangle region_inter_right = new Rectangle(x_inter_right, y_inter_right, w_inter, h_inter);

            byte[] array_inter_left;
            byte[] array_inter_right;

            if (right.bbox != null) { 

                array_inter_left = _get_byte_data_region(left[id.image_index], region_inter_left, true);
                
                goto LEFT_ONLY;     
            }

            if (left.bbox != null) {

                array_inter_right = _get_byte_data_region(right[instance.image_index], region_inter_right, true);

                goto RIGHT_ONLY;
            }

            array_inter_left  = _get_byte_data_region(left[id.image_index], region_inter_left, true);
            array_inter_right = _get_byte_data_region(right[instance.image_index], region_inter_right, true);

            for (int v=0; v<array_inter_left.Length; v=v+1) {

                if (array_inter_left[v] > 0 && array_inter_right[v] > 0) { return instance; }
            }

            continue;

            LEFT_ONLY:
            foreach (byte b in array_inter_left) { if (b > 0) { return instance; } } continue;
            
            RIGHT_ONLY:
            foreach (byte b in array_inter_right) { if (b > 0) { return instance; } } continue;
        }

        return null;
    }

    internal void _sort(HashSet<Instance> list_instance) {

        HashSet<Instance> _list_instance = [];

        foreach (Instance instance in list_instance) {

            if (!_list_outdated.Contains(instance)) { continue; }

            if (!_instance_id.Contains(instance)) {

                dict_regiondata.Remove(instance);

                _list_instance.Add(instance);

                continue;
            }

            Rectangle _region = _get_instance_region(instance);

            if (!region.Intersects(_region)) { dict_regiondata.Remove(instance); continue; }

            dict_regiondata[instance] = _region;

            _list_instance.Add(instance);
        }

        if (dict_regiondata.Count == 0 || region.Width <= QTREE_MINSIZE) { return; }

        int width = region.Width/2;

        lt?._sort(_list_instance);
        lt = lt?? new Quadtree(new Rectangle(region.X, region.Y, width, width), dict_regiondata, _list_instance);

        rt?._sort(_list_instance);
        rt = rt?? new Quadtree(new Rectangle(region.X+width, region.Y, width, width), dict_regiondata, _list_instance);

        lb?._sort(_list_instance);
        lb = lb?? new Quadtree(new Rectangle(region.X, region.Y+width, width, width), dict_regiondata, _list_instance);

        rb?._sort(_list_instance);
        rb = rb?? new Quadtree(new Rectangle(region.X+width, region.Y+width, width, width), dict_regiondata, _list_instance);
    }
}