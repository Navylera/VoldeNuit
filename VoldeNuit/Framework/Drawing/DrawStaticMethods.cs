using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VoldeNuit.Framework.Drawing;

using static Configuration;
using static Color;
using static Heart;

public static partial class Draw {

    public static void draw_set_color(uint rgb) {

        color = _color&0xff000000u|rgb&0xff0000u|rgb&0x00ff00u|rgb&0x0000ffu;
    }

    public static void draw_set_alpha(float a) { A = (byte)float.Floor(a*255); return; }

    public static void draw_line(float x1, float y1, float x2, float y2, int width = 1) {

        if (((uint)_progress &120) == 0) { return; }

        float angle = float.Atan2(y2-y1, x2-x1);
        float distance = float.Sqrt(float.Pow(x2-x1, 2)+float.Pow(y2-y1, 2));

        DrawData drawdata = new DrawData(_primitive, x1, y1, distance, width, 
                                         new Rectangle(0, 0, 1, 1)) { 
                                vx = 0, vy = width/2, angle = angle, color = _color 
        }; 
        
        if (_graphicsDeviceManager.GraphicsDevice.GetRenderTargets().Length != 0) {

            drawdata.Draw();
        }

        return;
    }

    public static void draw_rectangle(float x, float y, int width, int height, bool outline = false, 
                                      int width_line = 1) {

        if (((uint)_progress &120) == 0) { return; }

        if (width < 1 || height < 1) { return; }

        DrawData drawdata;

        if (outline) {

            // lt -> rt
            drawdata = new DrawData(_primitive, x, y, width, width_line, 
                                    new Rectangle(0, 0, 1, 1)) { 
                           vx = 0, vy = 0, color = _color
            };

            if (_graphicsDeviceManager.GraphicsDevice.GetRenderTargets().Length != 0) {

                drawdata.Draw();
            }

            // lb -> rb
            drawdata = new DrawData(_primitive, x, y+height-width_line, width, width_line, 
                                    new Rectangle(0, 0, 1, 1)) { 
                           vx = 0, vy = 0, color = _color
            };

            if (_graphicsDeviceManager.GraphicsDevice.GetRenderTargets().Length != 0) {

                drawdata.Draw();
            }

            // lt -> lb
            drawdata = new DrawData(_primitive, x, y+width_line, width_line, height-(2*width_line), 
                                    new Rectangle(0, 0, 1, 1)) {
                           vx = 0, vy = 0, color = _color
            };

            if (_graphicsDeviceManager.GraphicsDevice.GetRenderTargets().Length != 0) {

                drawdata.Draw();
            }

            // rt -> rb
            drawdata = new DrawData(_primitive, x+width-width_line, y+width_line, width_line, height-(2*width_line), 
                                    new Rectangle(0, 0, 1, 1)) { 
                           vx = 0, vy = 0, color = _color
            };

            if (_graphicsDeviceManager.GraphicsDevice.GetRenderTargets().Length != 0) {

                drawdata.Draw();
            }
            
            return;
        }

        drawdata = new DrawData(_primitive, x, y, width, height, 
                                new Rectangle(0, 0, 1, 1)) { 
                       vx = 0, vy = 0, color = _color 
        }; 
        
        if (_graphicsDeviceManager.GraphicsDevice.GetRenderTargets().Length != 0) {

            drawdata.Draw();
        }

        return;
    }

    public static void draw_sprite(Sprite sprite_index, float image_index, float x, float y) {

        if (((uint)_progress &120) == 0) { return; }

        if (sprite_index == null || sprite_index.texture == null) { return; }

        int index = (int)float.Floor(image_index)%sprite_index.image_number;

        DrawData drawdata = new DrawData(sprite_index.texture, x, y, 1, 1,
                                         new Rectangle(index*sprite_index.sprite_width, 0,
                                                       sprite_index.sprite_width,
                                                       sprite_index.sprite_height)) { 
                                vx = sprite_index.x, vy = sprite_index.y
        }; 
        
        if (_graphicsDeviceManager.GraphicsDevice.GetRenderTargets().Length != 0) {

            drawdata.Draw();
        }
        
        return;
    }

    public static void draw_sprite_ext(Sprite sprite_index, float image_index, float x, float y, 
                                       float xscale, float yscale, float angle, uint color, float alpha) {

        if (((uint)_progress &120) == 0) { return; }

        if (sprite_index == null || sprite_index.texture == null) { return; }

        int index = (int)float.Floor(image_index)%sprite_index.image_number;

        DrawData drawdata = new DrawData(sprite_index.texture, x, y, xscale, yscale, 
                                         new Rectangle(index*sprite_index.sprite_width, 0,
                                                       sprite_index.sprite_width, 
                                                       sprite_index.sprite_height)) { 
                                vx = sprite_index.x, vy = sprite_index.y,
                                angle = angle, 
                                color = (uint)float.Round(255f*alpha, ROUNDING)<<24|
                                        (color&0x00ffffffu) 
        }; 
        
        if (_graphicsDeviceManager.GraphicsDevice.GetRenderTargets().Length != 0) {

            drawdata.Draw();
        }

        return;
    }

    public static void draw_sprite_part(Sprite sprite_index, float image_index, 
                                        int left, int top, int width, int height, float x, float y) {

        if (((uint)_progress &120) == 0) { return; }

        if (sprite_index == null || sprite_index.texture == null) { return; }

        int index = (int)float.Floor(image_index)%sprite_index.image_number;

        if (left < 0 || top < 0 || 
            left > sprite_index.sprite_width || top > sprite_index.sprite_height) { 
                
            return;
        }

        int _width = left+width > sprite_index.sprite_width? sprite_index.sprite_width-left : width;

        int _height = top+height > sprite_index.sprite_height? sprite_index.sprite_height-top : height;

        if (_width <= 0 || _height <= 0) { return; }

        int vpx = index*sprite_index.sprite_width;

        DrawData drawdata = new DrawData(sprite_index.texture, x+left, y+top, 1, 1,
                                         new Rectangle(vpx+left, top, _width, _height)) { 
                                vx = sprite_index.x, vy = sprite_index.y 
        }; 
        
        if (_graphicsDeviceManager.GraphicsDevice.GetRenderTargets().Length != 0) {

            drawdata.Draw();
        }

        return;
    }

    public static void draw_sprite_part_ext(Sprite sprite_index, float image_index, 
                                            int left, int top, int width, int height, 
                                            float x, float y, float xscale, float yscale, 
                                            uint color, float alpha) {

        if (((uint)_progress &120) == 0) { return; }

        if (sprite_index == null || sprite_index.texture == null) { return; }

        int index = (int)float.Floor(image_index)%sprite_index.image_number;

        if (left < 0 || top < 0 || 
            left > sprite_index.sprite_width || top > sprite_index.sprite_height) { 
                
            return;
        }

        int _width = left+width > sprite_index.sprite_width? sprite_index.sprite_width-left : width;

        int _height = top+height > sprite_index.sprite_height? sprite_index.sprite_height-top : height;

        if (_width <= 0 || _height <= 0) { return; }

        int vpx = index*sprite_index.sprite_width;

        DrawData drawdata = new DrawData(sprite_index.texture, x+(xscale*left), y+(yscale*top), xscale, yscale,
                                         new Rectangle(vpx+left, top, _width, _height)) { 
                                vx = sprite_index.x, vy = sprite_index.y,
                                color = (uint)float.Round(255f*alpha, ROUNDING)<<24|
                                        (color&0x00ffffffu) 
        }; 
        
        if (_graphicsDeviceManager.GraphicsDevice.GetRenderTargets().Length != 0) {

            drawdata.Draw();
        }
        
        return;
    }

    public static void draw_set_font(Font font) { font_current = font; return; }

    public static void draw_set_halign(int halign) { Draw.halign = halign; }
    
    public static void draw_set_valign(int valign) { Draw.valign = valign; }

    public static void draw_text(float x, float y, string text, 
                                 float xscale = 1f, float yscale = 1f, float angle = 0f) {
        
        _draw_text(x, y, text, -1, xscale, yscale, angle); return;
    }

    public static void draw_text_ext(float x, float y, string text, float sep, float w, 
                                     float xscale = 1f, float yscale = 1f, float angle = 0f) {

        _draw_text_ext(x, y, text, -1, sep, w, xscale, yscale, angle); return;
    }

    public static void draw_text_mono(float x, float y, string text, int cwidth,
                                      float xscale = 1f, float yscale = 1f, float angle = 0f) {

        _draw_text(x, y, text, cwidth, xscale, yscale, angle);
    }

    public static void draw_text_mono_ext(float x, float y, string text, int cwidth, float sep, float w, 
                                          float xscale = 1f, float yscale = 1f, float angle = 0f) {

        _draw_text_ext(x, y, text, cwidth, sep, w, xscale, yscale, angle); return;
    }

    public static int string_width(string text) {

        float ret = 0;
        float nwidth = 0;

        float cspace = float.Floor(font_current.size_font*.25f);

        foreach (char c in text) {

            if (c == '\n') {

                if (nwidth > ret) { ret = nwidth; nwidth = 0f; continue; }
            }

            if (c == ' ') { nwidth = nwidth+cspace; continue; }

            if (!font_current._dict_char_data.TryGetValue(c, out uint value)) {

                nwidth = nwidth+font_current.size_font;

                continue;
            }

            nwidth = nwidth+(value>>22&0xff);
        }

        if (nwidth > ret) { ret = nwidth; }

        return (int)float.Ceiling(ret);
    }

    public static float char_width(char c) {

        if (c == '\n') { return 0; }

        if (c == ' ') { return font_current.size_font*.25f; }

        if (!font_current._dict_char_data.TryGetValue(c, out uint value)) { return font_current.size_font; }

        return value>>22&0xff;
    }

    public static void draw_texture(Texture2D texture, float x, float y) {

        if (((uint)_progress &120) == 0) { return; }

        if (texture == null) { return; }

        DrawData drawdata = new DrawData(texture, x, y, 1, 1,
                                         new Rectangle(0, 0, texture.Width, texture.Height)) { 
                                vx = 0, vy = 0
        };
        
        if (_graphicsDeviceManager.GraphicsDevice.GetRenderTargets().Length != 0) {

            drawdata.Draw();
        }
        
        return;
    }

    public static void draw_texture_ext(Texture2D texture, float x, float y,
                                        float xscale, float yscale, int vx, int vy, float angle, uint color, float alpha) {

        if (((uint)_progress &120) == 0) { return; }

        if (texture == null) { return; }

        DrawData drawdata = new DrawData(texture, x, y, xscale, yscale, 
                                         new Rectangle(0, 0, texture.Width, texture.Height)) { 
                                    vx = vx, vy = vy,
                                    angle = angle, 
                                    color = (uint)float.Round(255f*alpha, ROUNDING)<<24|
                                            (color&0x00ffffffu) 
        }; 
        
        if (_graphicsDeviceManager.GraphicsDevice.GetRenderTargets().Length != 0) {

            drawdata.Draw();
        }
        
        return;
    }

    public static void draw_texture_part(Texture2D texture, 
                                         int left, int top, int width, int height, float x, float y) {
                                            
        if (((uint)_progress &120) == 0) { return; }

        if (texture == null) { return; }

        if (left < 0 || top < 0 || 
            left > texture.Width || top > texture.Height) { 
                
            return;
        }

        int _width  = left+width > texture.Width? texture.Width-left : width;
        int _height = top+height > texture.Height? texture.Height-top : height;

        if (_width <= 0 || _height <= 0) { return; }

        DrawData drawdata = new DrawData(texture, x+left, y+top, 1, 1,
                                         new Rectangle(left, top, _width, _height)) { 
                                vx = 0, vy = 0
        };
        
        if (_graphicsDeviceManager.GraphicsDevice.GetRenderTargets().Length != 0) {

            drawdata.Draw();
        }
        
        return;
    }

    public static void draw_texture_part_ext(Texture2D texture,
                                             int left, int top, int width, int height, 
                                             float x, float y, float xscale, float yscale, 
                                             uint color, float alpha) {

        if (((uint)_progress &120) == 0) { return; }

        if (texture == null) { return; }

        if (left > texture.Width || top > texture.Height) { return; }

        if (left < 0 || top < 0 || 
            left > texture.Width || top > texture.Height) { 
                
            return;
        }

        int _width  = left+width > texture.Width? texture.Width-left : width;
        int _height = top+height > texture.Height? texture.Height-top : height;

        if (_width <= 0 || _height <= 0) { return; }

        DrawData drawdata = new DrawData(texture, x+(xscale*left), y+(yscale*top), xscale, yscale,
                                         new Rectangle(left, top, _width, _height)) { 
                                vx = 0, vy = 0,
                                color = (uint)float.Round(255f*alpha, ROUNDING)<<24|
                                        (color&0x00ffffffu) 
        }; 
        
        if (_graphicsDeviceManager.GraphicsDevice.GetRenderTargets().Length != 0) {

            drawdata.Draw();
        }

        return;
    }
}