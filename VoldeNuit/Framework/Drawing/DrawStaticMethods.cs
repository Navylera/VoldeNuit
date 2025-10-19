using System.Text;
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

        DrawData drawdata = new DrawData(_primitive, x1, y1, distance, 1f, 
                                         new Rectangle(0, 0, 1, 1)) { 
                                vx = 0, vy = width/2, angle = angle, color = _color 
        }; 
        
        if (_graphicsDeviceManager.GraphicsDevice.GetRenderTargets().Length != 0) {

            drawdata.Draw();
        }

        return;
    }

    public static void draw_rectangle(float x, float y, int width, int height, bool outline = false) {

        if (((uint)_progress &120) == 0) { return; }

        if (width < 1 || height < 1) { return; }

        DrawData drawdata;

        if (outline) {

            // lt -> rt
            drawdata = new DrawData(_primitive, x, y, width, 1, 
                                    new Rectangle(0, 0, 1, 1)) { 
                           vx = 0, vy = 0, color = _color
            };

            if (_graphicsDeviceManager.GraphicsDevice.GetRenderTargets().Length != 0) {

                drawdata.Draw();
            }

            // lb -> rb
            drawdata = new DrawData(_primitive, x, y+height-1, width, 1, 
                                    new Rectangle(0, 0, 1, 1)) { 
                           vx = 0, vy = 0, color = _color
            };

            if (_graphicsDeviceManager.GraphicsDevice.GetRenderTargets().Length != 0) {

                drawdata.Draw();
            }

            // lt -> lb
            drawdata = new DrawData(_primitive, x, y+1, 1, height-2, 
                                    new Rectangle(0, 0, 1, 1)) {
                           vx = 0, vy = 0, color = _color
            };

            if (_graphicsDeviceManager.GraphicsDevice.GetRenderTargets().Length != 0) {

                drawdata.Draw();
            }

            // rt -> rb
            drawdata = new DrawData(_primitive, x+width-1, y+1, 1, height-2, 
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

        if (((uint)_progress &120) == 0 || text == "") { return; }

        Texture2D[] array_texture = font_current._texture_font;

        float lspace = yscale*1.375f*font_current.size_font;
        float cspace = xscale*font_current.size_font*.25f;

        int lbcount = 0;

        float angle_converted = 0f;

        DrawData drawdata;

        switch (ANGLE_FORMAT) {

            case AngleFormat.RADIAN: { 
                
                angle_converted = angle%(2*float.Pi); break;
            }

            case AngleFormat.DEGREE: {

                angle_converted = (angle/180f*float.Pi)%(2*float.Pi); break;
            }

            case AngleFormat.LEGACY: {

                angle_converted = (-angle/180f*float.Pi)%(2*float.Pi);

                if (angle_converted < 0) { angle_converted = angle+(2*float.Pi); } break;
            }
        }

        float cos = float.Cos(angle_converted);
        float sin = float.Sin(angle_converted);

        float cwidth;

        List<float> ldata = [];

        Queue<uint> metadata = [];

        float lmax = 0f;

        float length = 0f;
        float height = font_current.size_font;

        int lbreak = 0;

        foreach (char c in text) {

            if (c == '\n') { 
                
                ldata.Add(length);

                if (length > lmax) { lmax = length; }
                
                length = 0f;
                
                lbreak = lbreak+1;

                height = height+lspace;
                
                continue;
            }

            if (c == ' ') { length = length+cspace; continue; }

            if (!font_current._dict_char_data.TryGetValue(c, out uint value)) {

                length = length+(xscale*font_current.size_font);

                metadata.Enqueue(0);

                continue;
            }

            length = length+(xscale*(value>>22&0xff));

            metadata.Enqueue(value); continue;
        }

        ldata.Add(length);

        float xoffset = x;

        switch (halign) {

            case fa_left:   { xoffset = x; break; }

            case fa_center: { xoffset = x-(ldata[0]/2); break; }

            case fa_right:  { xoffset = x-ldata[0]; break; }
        }

        float yoffset = y;

        switch (valign) {

            case fa_top:    { yoffset = y; break; }

            case fa_middle: { yoffset = y-(height/2); break; }

            case fa_bottom: { yoffset = y-height; break; }
        }

        float xpos = x+((cos*(xoffset-x))-(sin*(yoffset-y)));
        float ypos = y+((sin*(xoffset-x))+(cos*(yoffset-y)));

        foreach (char c in text) {

            if (c == '\n') {

                lbcount = lbcount+1;

                switch (halign) {

                    case fa_left:   { xoffset = x; break; }

                    case fa_center: { xoffset = x-(ldata[lbcount]/2); break; }

                    case fa_right:  { xoffset = x-ldata[lbcount]; break; }
                }

                switch (valign) {

                    case fa_top:    { yoffset = y+(lbcount*lspace); break; }

                    case fa_middle: { yoffset = y-(height/2)+(lbcount*lspace); break; }

                    case fa_bottom: { yoffset = y-height+(lbcount*lspace); break; }
                }

                xpos = x+((cos*(xoffset-x))-(sin*(yoffset-y)));
                ypos = y+((sin*(xoffset-x))+(cos*(yoffset-y)));

                continue;
            }

            if (c == ' ') { 
                
                xpos = xpos+(xscale*cos*cspace); 
                ypos = ypos+(yscale*sin*cspace);

                continue;
            }

            uint value = metadata.Dequeue();

            if (value == 0) {
                
                xpos = xpos+(xscale*cos*font_current.size_font);
                ypos = ypos+(yscale*sin*font_current.size_font);
                
                continue;
            }

            // [2]page|[8]width_char|[11]xpos|[11]ypos

            cwidth = value>>22&0xff;

            Texture2D texture = array_texture[value>>30&0x3];

            int _height = font_current._vmod;
            
            if (Font._is_sqr(c)) { _height = (int)font_current.size_font; }

            drawdata = new DrawData(texture, xpos, ypos, xscale, yscale,
                                    new Rectangle((int)(value>>11&0x7ff), 
                                                  (int)(value&0x7ff), 
                                                  (int)(value>>22&0xff), 
                                                  _height)) {
                           vx = 0, vy = 0, angle = angle, color = color
            };

            if (_graphicsDeviceManager.GraphicsDevice.GetRenderTargets().Length != 0) {

                drawdata.Draw();
            }

            xpos = xpos+(xscale*cos*cwidth);
            ypos = ypos+(yscale*sin*cwidth);
        }

        return;
    }

    public static void draw_text_ext(float x, float y, string text, float sep, float w, 
                                     float xscale = 1f, float yscale = 1f, float angle = 0f) {

        if (((uint)_progress &120) == 0 || text == "") { return; }

        Texture2D[] array_texture = font_current._texture_font;

        float lspace = sep;
        float cspace = font_current.size_font*.25f;

        int lbcount = 0;

        float angle_converted = 0f;

        DrawData drawdata;

        switch (ANGLE_FORMAT) {

            case AngleFormat.RADIAN: { 
                
                angle_converted = angle%(2*float.Pi); break;
            }

            case AngleFormat.DEGREE: {

                angle_converted = (angle/180f*float.Pi)%(2*float.Pi); break;
            }

            case AngleFormat.LEGACY: {

                angle_converted = (-angle/180f*float.Pi)%(2*float.Pi);

                if (angle_converted < 0) { angle_converted = angle+(2*float.Pi); } break;
            }
        }

        float cos = float.Cos(angle_converted);
        float sin = float.Sin(angle_converted);

        float cwidth = 0;

        List<float> ldata = [];

        Queue<uint> metadata = [];

        float lmax = 0f;

        float length = 0f;
        float height = font_current.size_font;

        int lbreak = 0;

        StringBuilder sbuilder = new StringBuilder();

        foreach (char c in text) {

            if (c == '\n') { 
                
                ldata.Add(length);

                if (length > lmax) { lmax = length; }
                
                length = 0f;
                
                lbreak = lbreak+1;

                height = height+lspace;
                
                continue;
            }

            if (c == ' ') { length = length+cspace; }

            if (!font_current._dict_char_data.TryGetValue(c, out uint value)) {

                value = 0;

                length = length+(xscale*font_current.size_font);

                cwidth = font_current.size_font;
            }

            if (value != 0) { cwidth = value>>22&0xff; }

            if (length+cwidth > w) {

                sbuilder.Append('\n');

                ldata.Add(length);

                if (length > lmax) { lmax = length; }
                
                length = 0f;
                
                lbreak = lbreak+1;

                height = height+lspace;
            }

            length = length+cwidth;

            sbuilder.Append(c);

            metadata.Enqueue(value);
        }

        text = sbuilder.ToString();

        ldata.Add(length);

        float xoffset = x;

        switch (halign) {

            case fa_left:   { xoffset = x; break; }

            case fa_center: { xoffset = x-(xscale*ldata[0]/2); break; }

            case fa_right:  { xoffset = x-(xscale*ldata[0]); break; }
        }

        float yoffset = y;

        switch (valign) {

            case fa_top:    { yoffset = y; break; }

            case fa_middle: { yoffset = y-(yscale*height/2); break; }

            case fa_bottom: { yoffset = y-(yscale*height); break; }
        }

        float xpos = x+((cos*(xoffset-x))-(sin*(yoffset-y)));
        float ypos = y+((sin*(xoffset-x))+(cos*(yoffset-y)));

        foreach (char c in text) {

            if (c == '\n') {

                lbcount = lbcount+1;

                switch (halign) {

                    case fa_left:   { xoffset = x; break; }

                    case fa_center: { xoffset = x-(xscale*ldata[lbcount]/2); break; }

                    case fa_right:  { xoffset = x-(xscale*ldata[lbcount]); break; }
                }

                switch (valign) {

                    case fa_top:    { yoffset = y+(yscale*lbcount*lspace); break; }

                    case fa_middle: { yoffset = y-(yscale*((height/2)+(lbcount*lspace))); break; }

                    case fa_bottom: { yoffset = y-(yscale*(height+(lbcount*lspace))); break; }
                }

                xpos = x+((cos*(xoffset-x))-(sin*(yoffset-y)));
                ypos = y+((sin*(xoffset-x))+(cos*(yoffset-y)));

                continue;
            }

            if (c == ' ') { 
                
                xpos = xpos+(xscale*cos*cspace);
                ypos = ypos+(yscale*sin*cspace);

                continue;
            }

            uint value = metadata.Dequeue();

            if (value == 0) { 
                
                xpos = xpos+(xscale*cos*font_current.size_font);
                ypos = ypos+(yscale*sin*font_current.size_font);

                continue;
            }

            // [2]page|[8]width_char|[11]xpos|[11]ypos

            cwidth = value>>22&0xff;

            Texture2D texture = array_texture[value>>30&0x3];

            int _height = font_current._vmod;
            
            if (Font._is_sqr(c)) { _height = (int)font_current.size_font; }

            drawdata = new DrawData(texture, xpos, ypos, xscale, yscale,
                                    new Rectangle((int)(value>>11&0x7ff), 
                                                  (int)(value&0x7ff), 
                                                  (int)(value>>22&0xff), 
                                                  _height)) {
                           vx = 0, vy = 0, angle = angle, color = color
            };

            xpos = xpos+(xscale*cos*cwidth);
            ypos = ypos+(yscale*sin*cwidth);
        }

        return;
    }

    public static int string_width(string text) {

        float ret = 0;
        float nwidth = 0;

        float cspace = font_current.size_font*.25f;

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