using Microsoft.Xna.Framework.Audio;

namespace VoldeNuit.Framework.Audio;

using static Emitter;
using static Heart;

public partial class SoundInstance {

    internal bool _disposed = false;

    internal void Dispose() {

        if (_disposed) { return; }

        _sfxi?.Stop();

        _soundinstance.Remove(this);

        _sfxi?.Dispose();

        _disposed = true; return;
    }

    internal SoundEffectInstance? _sfxi;

    internal Sound sound;
    public Emitter? emitter = null;
    public Listener? listener = null;

    internal float _volume;

    private float _gain = 1f;
    public float gain { get => _gain; set => _gain = float.Clamp(value, -10f, 10f); }
    public float pitch  = 0f;
    public float pan    = 0f;

    public bool loop = false;

    internal bool _update = false;

    internal SoundInstance(Sound sound, bool loop) {

        this.sound = sound;

        this.loop = loop;

        _volume = sound.volume;

        _sfxi = sound.sfx.CreateInstance();

        _soundinstance.Add(this);
    }

    internal SoundInstance(Sound sound, Emitter emitter, Listener listener, bool loop) {

        this.sound    = sound;
        this.emitter  = emitter;
        this.listener = listener;

        this.loop = loop;

        _volume = sound.volume;

        gain = _volume;

        _soundinstance.Add(this);

        _sfxi = sound.sfx.CreateInstance();

        _update = _update_sound();
    }

    internal bool _update_sound() {

        if (_sfxi == null || _sfxi.State == SoundState.Stopped) { Dispose(); return true; }

        float doppler = 1f;

        if (emitter == null || listener == null) { goto NOT3D; }

        float distance = float.Sqrt(float.Pow(emitter.x-listener.x, 2)+float.Pow(emitter.y-listener.y, 2));

        // Pan

        float angle_vto      = float.Atan2(emitter.y-listener.y, emitter.x-listener.x);
        float angle_listener = float.Atan2(listener.lookat.Y, listener.lookat.X);

        float angle_diff = angle_vto-angle_listener;

        pan = float.Sin(angle_diff);

        float flfac = emitter.falloff_factor;
        float flref = emitter.falloff_reference;
        float flmax = emitter.falloff_max;

        switch (emitter.falloff) {

            case Falloff.NONE: { gain = 1f; break; }

            case Falloff.EXPONENT_DISTANCE: {

                // gain = (listener_distance / reference_distance) ^ (-falloff_factor)

                gain = _volume*float.Pow(distance, -flfac);

                
                break;
            }

            case Falloff.EXPONENT_DISTANCE_CLAMPED: {

                // distance = clamp(listener_distance, reference_distance, maximum_distance);

                distance = float.Clamp(distance, flref, flmax);

                // gain = (distance / reference_distance) ^ (-falloff_factor)

                gain = _volume*float.Pow(distance/flref, -flfac);
                
                break;
            }

            case Falloff.EXPONENT_DISTANCE_SCALED: {

                // distance = clamp(listener_distance, reference_distance, maximum_distance)

                distance = float.Clamp(distance, flref, flmax);

                // gain = ((distance / reference_distance) ^ (-falloff_factor)) * (((maximum_distance - distance) / (maximum_distance - reference_distance)) ^ (distance / maximum_distance))

                gain = _volume*
                       float.Pow(distance/flref, -flfac)*
                       float.Pow((flmax-distance)/(flmax-flref), distance/flmax);

                break;
            }

            case Falloff.INVERSE_DISTANCE: {

                // gain = reference_distance / (reference_distance + falloff_factor * (listener_distance - reference_distance))

                gain = _volume*flref/(flref+flfac*(distance-flref));
                
                break;
            }

            case Falloff.INVERSE_DISTANCE_CLAMPED: {

                // distance = clamp(listener_distance, reference_distance, maximum_distance)

                distance = float.Clamp(distance, flref, flmax);

                // gain = reference_distance / (reference_distance + falloff_factor * (distance - reference_distance))

                gain = _volume*flref/(flref+flfac*(distance-flref));
                
                break;
            }

            case Falloff.INVERSE_DISTANCE_SCALED: {

                // distance = clamp(listener_distance, reference_distance, maximum_distance)

                distance = float.Clamp(distance, flref, flmax);

                // gain = (reference_distance / (reference_distance + falloff_factor * (distance - reference_distance))) * (((maximum_distance - distance) / (maximum_distance - reference_distance)) ^ (distance / maximum_distance))

                gain = _volume*flref/(flref+(flfac*(distance-flref)))*float.Pow((flmax-distance)/(flmax-flref), distance/flmax);

                break;
            }

            case Falloff.LINEAR_DISTANCE: {

                // distance = min(distance, maximum_distance)

                distance = float.Min(distance, flmax);

                // gain = (1 - falloff_factor * (distance - reference_distance) / (maximum_distance - reference_distance))

                gain = _volume*(1-(flfac*(distance-flref)/(flmax-flref)));

                break;
            }

            case Falloff.LINEAR_DISTANCE_CLAMPED: {

                // distance = clamp(listener_distance, reference_distance, maximum_distance)

                distance = float.Clamp(distance, flref, flmax);

                // gain = (1 - falloff_factor * (distance - reference_distance) / (maximum_distance - reference_distance))

                gain = _volume*(1-(flfac*(distance-flref)/(flmax-flref)));

                break;
            }
        }

        // Doppler

        if (float.Abs(emitter.speed_sound) >= Configuration.EPSILON) {

            doppler = float.Cos(float.Abs(listener.direction-emitter.direction))*
                      (emitter.speed_sound-listener.speed)/
                      (emitter.speed_sound-emitter.speed)
            ;
        }

        _sfxi.Pan    = float.Clamp(listener.pan_amount*pan, -1, 1);

        NOT3D:

        _sfxi.Volume = _volume*_gain;
        _sfxi.Pitch  = doppler*pitch;

        _sfxi.IsLooped = loop;

        return true;
    }
}