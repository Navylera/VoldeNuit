namespace VoldeNuit.Framework.Drawing;

public partial class Sprite {

    public static Sprite SpriteCopy(Sprite sprite) {

        return sprite_copy(sprite);
    }

    public static Sprite SetAlphaFromSprite(Sprite sprite, int spriteImageIndex, 
                                            Sprite grayScale, int grayScaleImageIndex) {
                                                
        return sprite_set_alpha_from_sprite(sprite, spriteImageIndex, 
                                            grayScale, grayScaleImageIndex);
    }
}