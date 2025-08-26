using System.Runtime.CompilerServices;

using Microsoft.Xna.Framework.Graphics;

namespace VoldeNuit.Framework.Drawing;

public static partial class Draw {

    public static void SetDrawingColor(uint rgb) { 
        
        draw_set_color(rgb);
    }

    public static void SetDrawingAlpha(float a) { 
        
        draw_set_alpha(a);
    }

    public static void DrawLine(float x1, float y1, float x2, float y2, int width = 1) {

        draw_line(x1, y1, x2, y2, width);
    }

    public static void DrawRectangle(float x, float y, int width, int height, bool outLine = false) {

        draw_rectangle(x, y, width, height, outLine);
    }

    public static void DrawSprite(Sprite spriteIndex, float imageIndex, float x, float y) {

        draw_sprite(spriteIndex, imageIndex, x, y);
    }

    public static void DrawSprite(Sprite spriteIndex, float imageIndex, float x, float y, 
                                  float xScale, float yScale, float angle, uint color, float alpha) {

        draw_sprite_ext(spriteIndex, imageIndex, x, y, xScale, yScale, angle, color, alpha); return;
    }

    public static void DrawSpritePart(Sprite spriteIndex, float imageIndex, 
                                      int left, int top, int width, int height, float x, float y) {

        draw_sprite_part(spriteIndex, imageIndex, left, top, width, height, x, y);
    }

    public static void DrawSpritePart(Sprite spriteIndex, float imageIndex, 
                                      int left, int top, int width, int height, 
                                      float x, float y, float xScale, float yScale, 
                                      uint color, float alpha) {

        draw_sprite_part_ext(spriteIndex, imageIndex, left, top, width, height,
                             x, y, xScale, yScale, color, alpha
        );
    }

    public static void SetDrawingFont(Font font) { 
        
        draw_set_font(font);
    }

    public static void SetHorizontalAlign(int horizontalAlign) {

        draw_set_halign(horizontalAlign);
    }

    public static void SetVerticalAlign(int verticalAlign) {

        draw_set_valign(verticalAlign);
    }

    public static void DrawText(float x, float y, string text, 
                                float xScale = 1f, float yScale = 1f, float angle = 0f) {

        draw_text(x, y, text, xScale, yScale, angle);
    }

    public static void DrawText(float x, float y, string text, int lineBreakSpace, int maxWidth, 
                                     float xScale = 1f, float yScale = 1f, float angle = 0f) {

        draw_text_ext(x, y, text, lineBreakSpace, maxWidth, xScale, yScale, angle);
    }

    public static int GetStringWidth(string text) {

        return string_width(text);
    }

    public static void DrawTexture(Texture2D texture, float x, float y) {

        draw_texture(texture, x, y);
    }

    public static void DrawTexture(Texture2D texture, float x, float y,
                                   float xScale, float yScale, int vx, int vy, float angle, uint color, float alpha) {

        draw_texture_ext(texture, x, y, xScale, yScale, vx, vy, angle, color, alpha);
    }

    public static void DrawTexturePart(Texture2D texture, 
                                       int left, int top, int width, int height, float x, float y) {

        draw_texture_part(texture, left, top, width, height, x, y);
    }

    public static void DrawTexturePart(Texture2D texture,
                                       int left, int top, int width, int height, 
                                       float x, float y, float xscale, float yscale, 
                                       uint color, float alpha) {

        draw_texture_part_ext(texture, left, top, width, height, x, y, xscale, yscale, color, alpha);
    }
}