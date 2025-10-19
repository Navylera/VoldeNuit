using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VoldeNuit.Framework.Drawing;

using static Heart;
using static Exception;

public partial class Sprite {

    public static Sprite operator +(Sprite sprite, Texture2D texture) {

        if (sprite.GetType().Name != "Sprite") { _stacktrace(ExConstants.ARGUMENT_NOT_ALLOWED); return null!; }

        if (sprite.texture == null || texture == null) { return sprite; }

        if (sprite.texture.Height != sprite.sprite_height ||
            sprite.texture.Height != texture.Height) {

            _stacktrace(ExConstants.SPRITE_TEXTURE_NOT_MATCHED);

            return sprite;
        }

        byte[] data_sprite = new byte[4*sprite.texture.Width*sprite.texture.Height];
        sprite.texture.GetData(data_sprite);

        byte[] data_texture = new byte[4*texture.Width*texture.Height];
        texture.GetData(data_texture);

        Texture2D texture_new = new Texture2D(_graphicsDeviceManager.GraphicsDevice, 
                                              sprite.texture.Width+texture.Width, 
                                              sprite.texture.Height
        );

        texture_new.SetData(0, new Rectangle(0, 0,
                                             sprite.texture.Width, sprite.texture.Height
                            ), data_sprite, 0, data_sprite.Length
        );

        texture_new.SetData(0, new Rectangle(sprite.texture.Width, 0, 
                                             texture.Width, texture.Height
                            ), data_texture, 0, data_texture.Length
        );

        sprite.texture = texture_new;

        return sprite;
    }

    public static Sprite operator +(Sprite sprite, Sprite subsprite) {

        if (subsprite.texture == null ||
            subsprite.texture.Height != subsprite.sprite_height) { return sprite; }

        if (sprite.GetType().Name != "Sprite") { _stacktrace(ExConstants.ARGUMENT_NOT_ALLOWED); return null!; }

        return sprite+subsprite.texture;
    }

    public void Copy(out Sprite destination) { 
        
        destination = sprite_copy(this); return;
    }
}