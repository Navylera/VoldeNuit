
using Microsoft.Xna.Framework.Graphics;

namespace VoldeNuit.Framework.Drawing;

using static Heart;
using static Exception;

public partial class Sprite {

    public static Sprite sprite_copy(Sprite sprite) {

        Sprite ret = new Sprite() {

            texture = null,
            x = sprite.x, y = sprite.y,
            sprite_width = sprite.sprite_width, sprite_height = sprite.sprite_height,
        };

        if (sprite.texture == null) { return ret; }

        Texture2D texture = new Texture2D(_graphicsDeviceManager.GraphicsDevice, 
                                          sprite.texture.Width, sprite.texture.Height
        );

        byte[] array_data = new byte[4*sprite.texture.Width*sprite.texture.Height];

        sprite.texture.GetData(array_data);
        texture.SetData(array_data);
        
        ret.texture = texture;

        return ret;
    }

    public static Sprite sprite_set_alpha_from_sprite(Sprite sprite, int sindex, Sprite grayscale, int gindex) {

        if (sprite.sprite_width  != grayscale.sprite_width ||
            sprite.sprite_height != grayscale.sprite_height) {

            _stacktrace(ExConstants.SPRITE_ALPHA_NOT_MATCHED); return null;
        }

        Sprite ret = new Sprite() {

            texture = null,
            x = sprite.x, y = sprite.y, 
            sprite_width  = sprite.sprite_width, 
            sprite_height = sprite.sprite_height,
        };

        byte[] array_data_sprite     = sprite[sindex];
        byte[] array_data_grayscale  = grayscale[gindex];

        byte r, g, b;

        for (int i=0; i<array_data_sprite.Length; i=i+4) {

            r = array_data_grayscale[i];
            g = array_data_grayscale[i+1];
            b = array_data_grayscale[i+2];

            if ((r != g) || (g != b) || (b != r)) {

                _stacktrace(ExConstants.ALPHA_IS_NOT_GRAYSCALE); return null;
            }

            float alpha = (float)r/255;

            array_data_sprite[i] = (byte)float.Floor(array_data_sprite[i]*alpha);
        }

        Texture2D _texture = new Texture2D(_graphicsDeviceManager.GraphicsDevice, 
                                           sprite.sprite_width, sprite.sprite_height
        );

        _texture.SetData(array_data_sprite);

        ret.texture = _texture;

        return ret;
    }
}