using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VoldeNuit.Framework.Drawing;

using static Configuration;
using static Color;
using static Heart;


public static partial class Draw {

    internal static byte[] _get_byte_data_region(byte[] data, Rectangle region) {

        byte[] ret = new byte[4*region.Width*region.Height];

        int count = 0;

        for (int k=0; k<region.Height; k=k+1) {

            for (int i=0; i<region.Width; i=i+1) {

                int pos = 4*(((region.Y+k)*region.Width)+region.X+i);

                ret[count  ] = data[pos  ];
                ret[count+1] = data[pos+1];
                ret[count+2] = data[pos+2];
                ret[count+3] = data[pos+3];

                count = count+4;
            }
        }

        return ret;
    }

    internal static void _set_byte_data_region(byte[] dest, int destwidth, Rectangle destregion, byte[] source, int sourcewidth, Rectangle sourceregion) {

        for (int k=0; k<destregion.Height; k=k+1) {

            for (int i=0; i<destregion.Width; i=i+1) {

                int destpos   = 4*(((destregion.Y+k)*destwidth)+destregion.X+i);
                int sourcepos = 4*(((sourceregion.Y+k)*sourcewidth)+sourceregion.X+i);

                dest[destpos  ] = source[sourcepos  ];
                dest[destpos+1] = source[sourcepos+1];
                dest[destpos+2] = source[sourcepos+2];
                dest[destpos+3] = source[sourcepos+3];
            }
        }
    }

    internal static void _draw_text(float x, float y, string text, int cwidth, 
                                    float xscale = 1f, float yscale = 1f, float angle = 0f) {

        if (((uint)_progress &120) == 0 || text == "" || xscale == 0 || yscale == 0) { return; }

        Texture2D[] array_texture = font_current._texture_font;

        int lspace = (int)float.Floor(yscale*1.375f*font_current.size_font);
        // int cspace = (int)float.Floor(xscale*font_current.size_font*.25f);
        int cspace = cwidth >= 0? (int)float.Floor(cwidth*xscale): 
                                  (int)float.Floor(xscale*font_current.size_font*.25f);
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

        List<float> ldata = [];

        Queue<uint> metadata = [];

        float length = 0f;
        float height = float.Floor(yscale*font_current.size_font);

        int lbreak = 0;

        foreach (char c in text) {

            if (c == '\n') { 
                
                ldata.Add(length);
                
                length = 0f;
                
                lbreak = lbreak+1;

                height = height+lspace;
                
                continue;
            }

            if (c == ' ') { length = length+cspace; continue; }

            if (!font_current._dict_char_data.TryGetValue(c, out uint value)) {

                length = length+float.Floor(xscale*font_current.size_font);

                metadata.Enqueue(0);

                continue;
            }

            int _cwidth = (int)(value>>22&0xff);

            _cwidth = cwidth >= 0? cwidth: _cwidth;

            _cwidth = (cwidth >= 0 && 
                       System.Text.Encoding.UTF8.GetByteCount([c]) > 1)? cwidth*2: _cwidth;

            length = length+float.Floor(xscale*_cwidth);

            // length = length+float.Floor(xscale*(value>>22&0xff));

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
                
                xpos = xpos+float.Floor(cos*cspace); 
                ypos = ypos+float.Floor(sin*cspace);

                continue;
            }

            uint value = metadata.Dequeue();

            if (value == 0) {
                
                xpos = xpos+float.Floor(xscale*cos*font_current.size_font);
                ypos = ypos+float.Floor(xscale*sin*font_current.size_font);
                
                continue;
            }

            // [2]page|[8]width_char|[11]xpos|[11]ypos

            // int _ocwidth = (int)value>>22&0xff;

            // int _cwidth = cwidth >= 0? cwidth: _ocwidth;

            // _cwidth = _ocwidth > _cwidth? _cwidth*2: _cwidth;

            int _cwidth = (int)value>>22&0xff;

            Texture2D texture = array_texture[value>>30&0x3];

            int _height = font_current._vmod;
            
            if (Font._is_sqr(c)) { _height = (int)font_current.size_font; }

            int vx = xscale > 0? 0: _cwidth;
            int vy = yscale > 0? 0: _height;

            int _a = _cwidth < cwidth? (int)float.Floor(xscale*(cwidth-_cwidth)): 0;

            drawdata = new DrawData(texture, xpos+_a, ypos, xscale, yscale,
                                    new Rectangle((int)(value>>11&0x7ff), 
                                                  (int)(value&0x7ff), 
                                                  _cwidth, 
                                                  _height)) {
                               vx = vx, vy = vy, angle = angle, color = color
            };

            if (_graphicsDeviceManager.GraphicsDevice.GetRenderTargets().Length != 0) {

                drawdata.Draw();
            }

            _cwidth = cwidth >= 0? cwidth: _cwidth;

            _cwidth = (cwidth >= 0 && 
                       System.Text.Encoding.UTF8.GetByteCount([c]) > 1)? cwidth*2: _cwidth;

            xpos = xpos+(xscale*cos*_cwidth);
            ypos = ypos+(xscale*sin*_cwidth);
        }

        return;
    }

    internal static void _draw_text_ext(float x, float y, string text, int cwidth, float sep, float w, 
                                        float xscale = 1f, float yscale = 1f, float angle = 0f) {

        if (((uint)_progress &120) == 0 || text == "" || xscale == 0 || yscale == 0) { return; }

        Texture2D[] array_texture = font_current._texture_font;

        // int cspace = (int)float.Floor(xscale*font_current.size_font*.25f);
        int cspace = cwidth >= 0? (int)float.Floor(cwidth*xscale): 
                                  (int)float.Floor(xscale*font_current.size_font*.25f);
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

        List<float> ldata = [];

        Queue<uint> metadata = [];

        float length = 0f;
        float height = float.Floor(yscale*font_current.size_font);

        int lbreak = 0;

        uint linebreak = 0xc0000000u;
        uint blank = (uint)(linebreak+array_texture[0].Width);

        foreach (char c in text) {

            if (c == '\n') { 
                
                ldata.Add(length);
                
                length = 0f;
                
                lbreak = lbreak+1;

                height = height+sep;
                
                continue;
            }

            uint vcopy = 0;

            int _cwidth = 0;

            if (!font_current._dict_char_data.TryGetValue(c, out uint value)) {

                // if (c == ' ') { length = length+cspace; continue; }

                if (c == ' ') { _cwidth = cspace; vcopy = blank; goto LINEBREAK; }

                _cwidth = (int)float.Floor(xscale*font_current.size_font);

                // metadata.Enqueue(0);

                vcopy = 0;

                goto LINEBREAK;
            }

            _cwidth = (int)(value>>22&0xff);

            _cwidth = cwidth >= 0? cwidth: _cwidth;

            _cwidth = (cwidth >= 0 && 
                       System.Text.Encoding.UTF8.GetByteCount([c]) > 1)? cwidth*2: _cwidth;

            vcopy = value;

            LINEBREAK:

            if (length+float.Floor(xscale*_cwidth) > w) {

                ldata.Add(length);
                
                length = 0f;
                
                lbreak = lbreak+1;

                height = height+sep;

                metadata.Enqueue(linebreak);
            }

            length = length+float.Floor(xscale*_cwidth);

            metadata.Enqueue(vcopy);
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

            LINEBREAK:

            uint value = metadata.Peek();

            if (value == linebreak || c == '\n') {

                lbcount = lbcount+1;

                switch (halign) {

                    case fa_left:   { xoffset = x; break; }

                    case fa_center: { xoffset = x-(ldata[lbcount]/2); break; }

                    case fa_right:  { xoffset = x-ldata[lbcount]; break; }
                }

                switch (valign) {

                    case fa_top:    { yoffset = y+(lbcount*sep); break; }

                    case fa_middle: { yoffset = y-(height/2)+(lbcount*sep); break; }

                    case fa_bottom: { yoffset = y-height+(lbcount*sep); break; }
                }

                xpos = x+((cos*(xoffset-x))-(sin*(yoffset-y)));
                ypos = y+((sin*(xoffset-x))+(cos*(yoffset-y)));

                if (value == linebreak) { metadata.Dequeue(); goto LINEBREAK; }

                continue;
            }

            if (c == ' ') { 
                
                xpos = xpos+float.Floor(cos*cspace); 
                ypos = ypos+float.Floor(sin*cspace);

                metadata.Dequeue(); continue;
            }

            if (value == 0) {
                
                xpos = xpos+float.Floor(xscale*cos*font_current.size_font);
                ypos = ypos+float.Floor(xscale*sin*font_current.size_font);
                
                metadata.Dequeue(); continue;
            }

            // [2]page|[8]width_char|[11]xpos|[11]ypos

            int _cwidth = (int)value>>22&0xff;

            Texture2D texture = array_texture[value>>30&0x3];

            int _height = font_current._vmod;
            
            if (Font._is_sqr(c)) { _height = (int)font_current.size_font; }

            int vx = xscale > 0? 0: _cwidth;
            int vy = yscale > 0? 0: _height;

            int _a = _cwidth < cwidth? (int)float.Floor(xscale*(cwidth-_cwidth)): 0;

            drawdata = new DrawData(texture, xpos+_a, ypos, xscale, yscale,
                                    new Rectangle((int)(value>>11&0x7ff), 
                                                  (int)(value&0x7ff), 
                                                  _cwidth, 
                                                  _height)) {
                               vx = vx, vy = vy, angle = angle, color = color
            };

            if (_graphicsDeviceManager.GraphicsDevice.GetRenderTargets().Length != 0) {

                drawdata.Draw();
            }

            _cwidth = cwidth >= 0? cwidth: _cwidth;

            _cwidth = (cwidth >= 0 && 
                       System.Text.Encoding.UTF8.GetByteCount([c]) > 1)? cwidth*2: _cwidth;

            xpos = xpos+(xscale*cos*_cwidth);
            ypos = ypos+(xscale*sin*_cwidth);

            metadata.Dequeue();
        }

        return;
    }
}