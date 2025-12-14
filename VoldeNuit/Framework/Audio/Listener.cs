using Microsoft.Xna.Framework;

using VoldeNuit.Framework.Instances;

namespace VoldeNuit.Framework.Audio;

using static Heart;
using static Configuration;

public partial class Listener {

    public float x = 0f;
    public float y = 0f;

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

    public float pan_amount = .15f;

    private Vector2 _lookat = new Vector2(1f, 0f);
    public Vector2 lookat { get => _lookat; set => _lookat = Vector2.Normalize(value); }

    public float lookat_x { get => _lookat.X; }
    public float lookat_y { get => _lookat.Y; }


    public static void audio_listener_position(float x, float y) {

        listener[0].x = x; listener[0].y = y; return;
    }

    public static void audio_listener_set_position(Listener listener, float x, float y) {

        listener.x = x; listener.y = y; return;
    }

    public static void audio_listener_velocity(float vx, float vy) {

        listener[0]._speed.X = vx; listener[0]._speed.Y = vy; return;
    }

    public static void audio_listener_set_velocity(Listener listener, float vx, float vy) {

        listener._speed.X = vx; listener._speed.Y = vy; return;
    }

    public static void audio_listener_orientation(float lookat_x, float lookat_y) {

        listener[0].lookat = new Vector2(lookat_x, lookat_y);

        return;
    }

    public static void audio_listener_set_orientation(Listener listener, float lookat_x, float lookat_y) {

        listener.lookat = new Vector2(lookat_x, lookat_y);

        return;
    }

    public void sync_to(Instance id) {

        x = id.x;
        y = id.y;

        speed = id.speed;

        direction = id.direction;

        return;
    }
}