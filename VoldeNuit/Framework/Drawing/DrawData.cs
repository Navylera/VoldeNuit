using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VoldeNuit.Framework.Drawing;

using static Configuration;
using static Heart;

internal partial class DrawData {

    internal Texture2D texture;

    internal readonly int x;
    internal readonly int y;

    internal int vx;
    internal int vy;

    internal readonly float image_xscale = 1f;
    internal readonly float image_yscale = 1f;

    internal float angle = 0f;
    
    internal uint color = 0xffffffff;
    
    internal Rectangle region;

    internal bool gui = _progress == Progress.GUI_DRAW;

    internal bool flag_dispose = false;

    internal DrawData(Texture2D texture, float x, float y, float image_xscale, float image_yscale) {

        this.texture = texture;
        
        this.x = (int)float.Round(x, ROUNDING);
        this.y = (int)float.Round(y, ROUNDING);

        this.image_xscale = image_xscale;
        this.image_yscale = image_yscale;

        this.region = new Rectangle(0, 0, texture.Width, texture.Height);
    }

    internal DrawData(Texture2D texture, float x, float y, float image_xscale, float image_yscale, Rectangle region) {

        this.texture = texture;
        
        this.x = (int)float.Round(x, ROUNDING);
        this.y = (int)float.Round(y, ROUNDING);

        this.image_xscale = image_xscale;
        this.image_yscale = image_yscale;

        this.region = region;

        if (_graphicsDeviceManager.GraphicsDevice.GetRenderTargets().Length == 0) {

            _draw.Add(this);
        }

        return;
    }

    internal void Draw() {

        float radian = angle%(2*float.Pi);

        switch (ANGLE_FORMAT) {

            case AngleFormat.RADIAN: { 
                
                radian = angle%(2*float.Pi); break;
            }

            case AngleFormat.DEGREE: {

                radian = (angle/180f*float.Pi)%(2*float.Pi); break;
            }

            case AngleFormat.LEGACY: {

                radian = (-angle/180f*float.Pi)%(2*float.Pi);

                if (radian < 0) { radian = radian+(2*float.Pi); } break;
            }
        }

        SpriteEffects spe = SpriteEffects.None;

        if (image_xscale < 0) { spe = spe|SpriteEffects.FlipHorizontally; }

        if (image_yscale < 0) { spe = spe|SpriteEffects.FlipVertically; }

        _spritebatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp);

        _spritebatch.Draw(texture,
                          new Vector2(x, y), 
                          region, 
                          Color.color_to_xna(color), 
                          -radian, 
                          new Vector2(vx, vy), 
                          new Vector2(float.Abs(image_xscale), 
                                      float.Abs(image_yscale)
                          ), spe, 0f
        );

        _spritebatch.End();
    }
}