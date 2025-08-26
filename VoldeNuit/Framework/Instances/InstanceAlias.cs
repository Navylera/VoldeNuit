using VoldeNuit.Framework.Drawing;

namespace VoldeNuit.Framework.Instances;

public abstract partial class Instance {

    public Instance ID { get => this; }    

    public float X { get => _x; set => x = value; }
    public float Y { get => _y; set => y = value; }

    public float XPrevious { get => xprevious; set => xprevious = value; }
    public float YPrevious { get => yprevious; set => yprevious = value; }

    public float HSpeed { get => _speed.X; set => _speed.X = value; }

    public float VSpeed { get => _speed.Y; set => _speed.Y = value; }

    public float Speed { get => speed; set => speed = value; }
    
    public float Direction { get => direction; set => direction = value; }

    public float Friction { get => friction; set => friction = value; }

    public bool isSolid { get => solid; set => solid = value; }

    public bool isCollidable { get => collision; set => collision = value; }

    public float Depth { get => depth; set => depth = value; }

    public Sprite? SpriteIndex { get => _sprite_index; set => sprite_index = value; }
    
    public float ImageIndex { get => image_index; set => image_index = value; }
    public float ImageSpeed { get => image_speed; set => image_speed = value; }
    public float ImageAngle { get => image_angle; set => image_angle = value; }
    public float ImageAlpha { get => image_alpha; set => image_alpha = value; }

    public int ImageNumber { get => image_number; }

    public float ImageXScale { get => image_xscale; set => image_xscale = value; }
    public float ImageYScale { get => image_yscale; set => image_yscale = value; }
    
    public Sprite? Mask { get => mask_index; set => mask_index = value; }
}