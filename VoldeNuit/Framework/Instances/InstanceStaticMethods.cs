using VoldeNuit.Framework.Drawing;
using VoldeNuit.Framework.Audio;

namespace VoldeNuit.Framework.Instances;

using static Quadtree;
using static Heart;
using static Exception;

public abstract partial class Instance {

    public static dynamic? Instantiate(Type object_name) {
        
        switch (object_name) {

            case Type t when t.BaseType == typeof(Sprite): {

                foreach (Sprite s in _sprite) { if (s.GetType() == object_name) { return s; } }

                Sprite? _s = (Sprite?)Convert.ChangeType(Activator.CreateInstance(object_name), object_name);

                if (_s == null) { return null; }

                _ = _s.texture;

                _sprite.Add(_s);
                
                return _s;
            }

            case Type t when t.BaseType == typeof(Font): {

                foreach (Font f in _font) {

                    if (f.GetType() == object_name) { return f; }
                }

                Font? _f = (Font?)Convert.ChangeType(Activator.CreateInstance(object_name), object_name);

                if (_f == null) { return null; }

                _f._init_font(_f.name, _f.size_font, _f.range);

                _font.Add(_f);
                
                return _f;
            }

            case Type t when t.BaseType == typeof(Sound): {

                foreach (Sound s in _sound) {

                    if (s.GetType() == object_name) { return s; }
                }

                Sound? _s = (Sound?)Convert.ChangeType(Activator.CreateInstance(object_name), object_name);

                if (_s == null) { return null; }

                _ = _s.sfx;

                _sound.Add(_s);
                
                return _s;
            }

            case Type t when t.BaseType == typeof(Instance): {

                foreach (Instance i in _instance_id) {

                    if (i.GetType() == object_name) { return i; }
                }

                return null;
            }

            default: { 

                _stacktrace(ExConstants.ARGUMENT_NOT_ALLOWED);
                
                return null;
            }
        }
    }

    public static Instance instance_create(float x, float y, Type object_name, float depth = 0f) {

        Instance? _instance = (Instance?)Convert.ChangeType(Activator.CreateInstance(object_name), object_name);

        if (_instance == null) { _stacktrace(ExConstants.ARGUMENT_NOT_ALLOWED); return null!; }

        _instance.x = x;
        _instance.y = y;
        
        _instance.depth = depth;

        return _instance;
    }

    public static bool instance_exists(Type object_name) {

        foreach (Instance instance in _instance_id.Where(i => !i._disposed)) {

            if (instance.GetType() == object_name || instance._is_child_of(object_name)) {

                return true;
            }
        }

        return false;
    }

    public static bool instance_exists(Instance id) {

        foreach (Instance instance in _instance_id.Where(i => !i._disposed)) { 
            
            if (instance == id) { return true; }
        }

        return false;
    }

    public static bool instance_destroy(Type object_name, bool execute_event_flag = true) {

        bool ret = false;

        foreach (Instance instance in _instance_id.Where(i => !i._disposed)) {

            if (instance.GetType() == object_name || instance._is_child_of(object_name)) {

                instance._execute_event_flag = execute_event_flag;

                instance.Dispose(); ret = true;
            }
        }

        return ret;
    }
    
    public static bool instance_destroy(Instance id, bool execute_event_flag = true) {

        foreach (Instance instance in _instance_id.Where(i => !i._disposed)) {

            if (instance == id) {

                id._execute_event_flag = execute_event_flag;

                id.Dispose(); return true;
            }
        }

        return false;
    }


    public static Instance? instance_position(float x, float y, Type object_name) {

        if (!_is_child_of(object_name, typeof(Instance))) { return null; }

        HashSet<Instance> list_instance = [];

        foreach (Instance instance in _instance_id.Where(i => !i._disposed)) {

            if (instance.GetType() == object_name || instance._is_child_of(object_name)) {

                list_instance.Add(instance);
            }
        }

        if (list_instance.Count == 0) { return null; }

        if (_qtree_collapsed) { _qtree_collision = new Quadtree(); }

        _qtree_collision._sort(list_instance);

        return _qtree_collision._check_point(x, y, list_instance);
    }

    public static Instance? instance_position(float x, float y, Instance id) {

        if (!_is_child_of(id.GetType(), typeof(Instance)) || !_instance_id.Contains(id)) {

            return null;
        }

        if (_qtree_collapsed) { _qtree_collision = new Quadtree(); }

        _qtree_collision._sort([id]);

        return _qtree_collision._check_point(x, y, [id]);
    }

    public static List<Instance> instance_position_list(float x, float y, Type object_name) {

        if (!_is_child_of(object_name, typeof(Instance))) { return []; }

        HashSet<Instance> list_instance = [];

        foreach (Instance instance in _instance_id.Where(i => !i._disposed)) {

            if (instance.GetType() == object_name || instance._is_child_of(object_name)) {

                list_instance.Add(instance);
            }
        }

        if (list_instance.Count == 0) { return []; }

        if (_qtree_collapsed) { _qtree_collision = new Quadtree(); }

        _qtree_collision._sort(list_instance);

        _qtree_collision._check_point_list(x, y, list_instance, out HashSet<Instance> result);

        return [..result];
    }

    public static Instance? instance_find(Type object_name, int index = 0) {

        Instance[] ifound = [
            .._instance_id.Where(i => !i._disposed &&
                                 (i.GetType() == object_name || i._is_child_of(object_name)
        ))];

        if (ifound.Length < index+1) { return null; }

        return ifound[index];
    }

    public static Instance? instance_find(Instance id) {

        foreach (Instance instance in _instance_id.Where(i => !i._disposed)) {

            if (instance == id) { return instance; }
        }

        return null;
    }

    public static List<Instance> instance_find_list(Type object_name) {

        return [.._instance_id.Where(i => !i._disposed &&
                                     (i.GetType() == object_name || i._is_child_of(object_name)
        ))];
    }

    public static int instance_number(Type object_name) {

        int count = 0;

        foreach (Instance instance in _instance_id.Where(i => !i._disposed)) {

            if (instance.GetType() == object_name || instance._is_child_of(object_name)) {
                
                count = count+1;
            }
        }

        return count;
    }
}