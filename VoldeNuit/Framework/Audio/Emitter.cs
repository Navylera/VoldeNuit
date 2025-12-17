using Microsoft.Xna.Framework;

using VoldeNuit.Framework.Instances;

namespace VoldeNuit.Framework.Audio;

using static Configuration;

public partial class Emitter {

    public float x = 0;

    public float y = 0;

    internal Vector2 _speed = new Vector2(0, 0);

    public float hspeed { get => _speed.X; set { _speed.X = value; } }

    public float vspeed { get => _speed.Y; set { _speed.Y = value; } }
    
    internal int _sign_speed = 0;

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

    public Falloff falloff = Falloff.NONE;

    public enum Falloff {

        NONE,
        LINEAR_DISTANCE,
        LINEAR_DISTANCE_CLAMPED,
        INVERSE_DISTANCE,
        INVERSE_DISTANCE_CLAMPED,
        INVERSE_DISTANCE_SCALED,
        EXPONENT_DISTANCE,
        EXPONENT_DISTANCE_CLAMPED,
        EXPONENT_DISTANCE_SCALED,
    }

    public float falloff_reference;
    public float falloff_max;
    public float falloff_factor = 1f;

    private float _speed_sound = 0f;
    public float speed_sound { get => _speed_sound; set => _speed_sound = float.Abs(value); }

    public void sync_to(Instance id) {

        x = id.x;
        y = id.y;

        speed = id.speed;

        direction = id.direction;

        return;
    }
}