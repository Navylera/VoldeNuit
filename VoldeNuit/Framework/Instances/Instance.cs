using Microsoft.Xna.Framework;

using VoldeNuit.Framework.Drawing;

namespace VoldeNuit.Framework.Instances;

using static Configuration;
using static Quadtree;
using static Heart;

public abstract partial class Instance {
    
    internal bool _disposed = false;

    internal bool _execute_event_flag = true;
    

    internal virtual void Dispose() {

        if (_disposed) { return; }
        
        if (_execute_event_flag) { Destroy(); }

        _list_outdated.Add(this);
        
        _instance_id.Remove(this);
        instance_id_solid.Remove(this);

        _disposed = true; return;
    }

    public Instance id { get => this; }


    private float _x = 0f;

    public float x { get => _x; set { _x = value; _list_outdated.Add(this); return; } }

    private float _y = 0f;

    public float y { get => _y; set { _y = value; _list_outdated.Add(this); return; } }

    public float xprevious = 0;
    public float yprevious = 0;

    private Vector2 _speed = new Vector2(0, 0);

    public float hspeed { get => _speed.X; set { _speed.X = value; } }

    public float vspeed { get => _speed.Y; set { _speed.Y = value; } }

    private int _sign_speed = 0;

    public float speed {

        get => _sign_speed*_speed.Length();

        set {

            _speed.X = float.Cos(_direction);
            _speed.Y = float.Sin(_direction);

            if (_sign_speed != float.Sign(value)) {

                _sign_speed = float.Sign(value);

                _speed = -1*_speed;
                value  = -1*value;
            }

            _speed = value*_speed;
        }
    }

    private float _direction = 0;
    
    public float direction {

        get {

            switch (ANGLE_FORMAT) {

                case AngleFormat.RADIAN: {

                    return _direction;
                }

                case AngleFormat.DEGREE: {

                    return _direction*180f/float.Pi;
                }

                case AngleFormat.LEGACY: {

                    if (-_direction < 0) { return ((2*float.Pi)-_direction)*180f/float.Pi; }

                    return -_direction*180f/float.Pi;
                }
            }

            return _direction;
        }

        set {

            // Maintain previous speed

            float sprevious = speed;

            switch (ANGLE_FORMAT) {

                case AngleFormat.RADIAN: { 
                    
                    _direction = value%(2*float.Pi); break;
                }

                case AngleFormat.DEGREE: {

                    _direction = (value/180f*float.Pi)%(2*float.Pi); break;
                }

                case AngleFormat.LEGACY: {

                    _direction = (-value/180f*float.Pi)%(2*float.Pi);

                    if (_direction < 0) { _direction = _direction+(2*float.Pi); } break;
                }
            }

            _speed.X = float.Cos(_direction);
            _speed.Y = float.Sin(_direction);

            _speed = sprevious*_speed;
        }
    }

    private float _friction = 0f;
    public float friction {

        get => _friction;

        set => _friction = float.Clamp(value, 0, value);
    }

    private bool _solid = false;
    public bool solid {
        
        get => _solid; set {
            
            if (solid == value) { return; }

            _solid = value;

            if (_solid) { instance_id_solid.Add(this); return; }

            instance_id_solid.Remove(this);
        }
    }

    private bool _collision = true;
    public bool collision {

        get {

            if (solid) { return true; } return _collision;
        }

        set { 

            if (_collision == value) { return; }
            
            _collision = value;

            _list_outdated.Add(this);
        }
    }

    public float depth = 0f;

    private Sprite? _sprite_index = null;
    public Sprite? sprite_index {
        
        get => _sprite_index;

        set { if (value != _sprite_index) { _sprite_index = value; _list_outdated.Add(this); } return; }
    }

    public int sprite_width {

        get { return sprite_index != null? sprite_index.sprite_width: 0; }
    }

    public int sprite_height {

        get { return sprite_index != null? sprite_index.sprite_height: 0; }
    }
    
    public float image_index = 0f;
    public float image_speed = 1f;

    private float _image_angle = 0f;
    public float image_angle {

        get {

            switch (ANGLE_FORMAT) {

                case AngleFormat.RADIAN: {

                    return _image_angle;
                }

                case AngleFormat.DEGREE: {

                    return _image_angle*180f/float.Pi;
                }

                case AngleFormat.LEGACY: {

                    if (-_image_angle < 0) { return ((2*float.Pi)-_image_angle)*180f/float.Pi; }

                    return -_image_angle*180f/float.Pi;
                }
            }

            return _image_angle;
        }

        set {

            switch (ANGLE_FORMAT) {

                case AngleFormat.RADIAN: { 
                    
                    _image_angle = value%(2*float.Pi); break;
                }

                case AngleFormat.DEGREE: {

                    _image_angle = (value/180f*float.Pi)%(2*float.Pi); break;
                }

                case AngleFormat.LEGACY: {

                    _image_angle = (-value/180f*float.Pi)%(2*float.Pi);

                    if (_image_angle < 0) { _image_angle = _image_angle+(2*float.Pi); } break;
                }
            }
        }
    }

    public float image_xscale = 1f;
    public float image_yscale = 1f;

    private Sprite? _mask_index = null;
    public Sprite? mask_index {
        
        get => _mask_index ?? sprite_index;

        set { if (value != _mask_index) { _mask_index = value; _list_outdated.Add(this); return; } }
    }

    private float _image_alpha = 1f;

    public float image_alpha {

        get => _image_alpha; set {

            _image_alpha = float.Clamp(value, 0f, 1f);
        }
    }

    public int image_number {

        get {

            if (sprite_index?.texture == null) { return 0; }

            return (int)float.Floor(sprite_index.texture.Width/sprite_width)*
                   (int)float.Round(sprite_index.texture.Height/sprite_height);
        }
    }

    public Instance() { 
        
        _instance_id.Add(this); 
        
        Create();
    }

    public virtual void Create() {}

    internal void _Begin_Step() {

        xprevious = x;
        yprevious = y;

        image_index = sprite_index != null? (image_index+image_speed)%sprite_index.image_number : 0;

        if (_sign_speed > 0) { speed = float.Clamp(speed-friction, 0, speed); }
        if (_sign_speed < 0) { speed = float.Clamp(speed+friction, speed, 0); }

        if (!collision || !solid) { x = x+hspeed; y = y+vspeed; return; }

        float distance = _speed.Length();

        Vector2 nv = _speed; nv.Normalize();

        float xpv;
        float ypv;

        float xcd;
        float ycd;

        while (distance > 0) {

            float coef_distance = distance >= 1f? 1f: distance;

            xcd = coef_distance*nv.X;
            ycd = coef_distance*nv.Y;

            distance = distance-coef_distance;

            xpv = x;
            ypv = y;

            x = x+xcd;
            y = y+ycd;

            _qtree_collision._sort(_list_outdated);

            Instance? sol = _qtree_collision._check_prec(this, instance_id_solid);

            if (sol == null) { continue; }

            x = xpv;
            y = ypv;

            break;
        }

        Begin_Step();

        return;
    }

    public virtual void Begin_Step() {}

    internal void _Step() { 

        if (_disposed) { return; }
        
        if (_qtree_collapsed) { _qtree_collision = new Quadtree(); } 
        
        Step();
        
        return;
    }

    public virtual void Step() {}

    internal void _End_Step() { 

        if (_disposed) { return; }
        
        if (_qtree_collapsed) { _qtree_collision = new Quadtree(); } 

        End_Step();
        
        return; 
    }

    public virtual void End_Step() {}

    // internal void _Begin_Draw() {}

    public virtual void Begin_Draw() {}

    // internal void _Draw() { return; }

    public virtual void Draw() { 

        if (_disposed) { return; }
        
        if (sprite_index != null) { draw_self(); } 
    }

    // internal virtual void _End_Draw() { return; }

    public virtual void End_Draw() { if (_disposed) { return; } }

    public virtual void GUI_Draw() { if (_disposed) { return; } }

    public virtual void Destroy() { if (_disposed) { return; } return; }

    internal bool _is_child_of(Type object_name) {

        Type? ptype = GetType();

        while (ptype != object_name) {

            if (ptype == null) { return false; }

            if (ptype == typeof(Instance)) { return false; }

            ptype = ptype.BaseType;
        }

        return true;
    }

    internal static bool _is_child_of(Type object_name, Type ancestor) {

        Type? ptype = object_name;

        while (ptype != ancestor) {

            if (ptype == null) { return false; }

            ptype = ptype.BaseType;
        }

        return true;
    }
}
