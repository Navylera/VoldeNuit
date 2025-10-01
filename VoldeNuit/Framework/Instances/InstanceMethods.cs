using VoldeNuit.Framework.Drawing;

namespace VoldeNuit.Framework.Instances;

using static Quadtree;
using static Heart;
using static Draw;

public abstract partial class Instance {

    public void draw_self() {

        if (sprite_index != null) {
            
            draw_sprite_ext(sprite_index, image_index, x, y, image_xscale, image_yscale, image_angle, 0xffffffff, image_alpha);
        }
    }

    public Instance? instance_place(float x, float y, Type object_name) {

        if (!_is_child_of(object_name, typeof(Instance)) ||
            !_instance_id.Contains(this)) {
            
            return null;
        }

        HashSet<Instance> list_instance = [
            this, 
            .._instance_id.Where(i => !i._disposed && 
                                 (i.GetType() == object_name || i._is_child_of(object_name))
        )];

        if (list_instance.Count == 1) { return null; }

        float px = this.x;
        float py = this.y;

        this.x = x;
        this.y = y;

        if (_qtree_collapsed) { _qtree_collision = new Quadtree(); }

        _qtree_collision._sort(list_instance);

        Instance? ret = _qtree_collision._check_prec(this, list_instance);

        this.x = px;
        this.y = py;

        return ret;
    }

    public Instance? instance_place(float x, float y, Instance id) {

        if (!_is_child_of(typeof(Instance)) ||
            !_instance_id.Contains(id) ||
            id._disposed) {
            
            return null;
        }

        float px = this.x;
        float py = this.y;

        this.x = x;
        this.y = y;

        if (_qtree_collapsed) { _qtree_collision = new Quadtree(); }

        _qtree_collision._sort([this, id]);

        Instance? ret = _qtree_collision._check_prec(this, [id]);

        this.x = px;
        this.y = py;

        return ret;
    }

    public List<Instance> instance_place_list(float x, float y, Type object_name) {

        if (!_is_child_of(object_name, typeof(Instance)) ||
            !_instance_id.Contains(this)) {
            
            return [];
        }

        HashSet<Instance> list_instance = [
            this, 
            .._instance_id.Where(i => !i._disposed && 
                                 (i.GetType() == object_name || i._is_child_of(object_name))
        )];

        if (list_instance.Count == 1) { return []; }

        float px = this.x;
        float py = this.y;

        this.x = x;
        this.y = y;

        if (_qtree_collapsed) { _qtree_collision = new Quadtree(); }

        _qtree_collision._sort(list_instance);

        _qtree_collision._check_prec_list(this, list_instance, out HashSet<Instance> ret);

        this.x = px;
        this.y = py;

        return [..ret];
    }

    public void activate() {

        if (_instance_id_deactivated.Remove(this)) { _instance_id.Add(this); } return;
    }

    public void deactivate() {

        if (_instance_id.Remove(this)) { _instance_id_deactivated.Add(this); } return;
    }
}