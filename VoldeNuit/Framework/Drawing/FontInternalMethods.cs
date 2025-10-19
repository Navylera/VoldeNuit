using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

using ImageMagick;
using ImageMagick.Drawing;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VoldeNuit.Framework.Drawing;

using static Heart;
using static Exception;
using static Configuration;
using static Draw;

public partial class Font {

    internal static bool _is_sqr (char c) {

        // 0xac00  ~ 0xd7a3     Hangul Syllables
        // 0x2e80  ~ 0x2eff     CJK Radicals Supplement
        // 0x3040  ~ 0x309f     Hiragana
        // 0x30a0  ~ 0x30ff     Katakana
        // 0x31f0  ~ 0x31ff     Katakana Phonetic Extensions
        // 0x3400  ~ 0x4dbf     CJK Unified Ideographs Extension A
        // 0x4e00  ~ 0x9fff     CJK Unified Ideographs
        // 0xf900  ~ 0xfaff     CJK Compatibility Ideographs
        // 0x20000 ~ 0x2a6df    CJK Unified Ideographs Extension B
        // 0x2a700 ~ 0x2b73f    CJK Unified Ideographs Extension C
        // 0x2b740 ~ 0x2b81f    CJK Unified Ideographs Extension D
        // 0x2b820 ~ 0x2ceaf    CJK Unified Ideographs Extension E
        // 0x2ceb0 ~ 0x2ebef    CJK Unified Ideographs Extension F
        // 0x2f800 ~ 0x2fa1f    CJK Compatibility Ideographs Supplement

        if ((c >= 0xac00  && c <= 0xd7a3)  ||
            (c >= 0x2e80  && c <= 0x2ef3)  ||
            (c >= 0x3040  && c <= 0x309f)  ||
            (c >= 0x30a0  && c <= 0x30ff)  ||
            (c >= 0x31f0  && c <= 0x31ff)  ||
            (c >= 0x3400  && c <= 0x4dbf)  ||
            (c >= 0x4e00  && c <= 0x9fff)  ||
            (c >= 0xf900  && c <= 0xfaff)  ||
            (c >= 0x20000 && c <= 0x2a6df) ||
            (c >= 0x2a700 && c <= 0x2b73f) ||
            (c >= 0x2b740 && c <= 0x2b81f) ||
            (c >= 0x2b820 && c <= 0x2ceaf) ||
            (c >= 0x2ceb0 && c <= 0x2ebef) ||
            (c >= 0x2f800 && c <= 0x2fa1f)) {
                
            return true;
        }

        return false;
    }

    private IDrawables<byte> _assign_drawables(string path) {

        return new Drawables().Font(path).
                               FontPointSize(size_font).
                               DisableTextAntialias().
                               DisableStrokeAntialias().
                               // StrokeColor(new MagickColor("#00000000")).
                               FillColor(new MagickColor("#FFFFFFFF")).
                               TextAlignment(TextAlignment.Left);
    }

    private void _flush(MagickImage[] array_texture_image) {

        int width_texture = _texture_font[0].Width;

        using FileStream fstream = new FileStream($"{_path}{name}_font_texture_{_page}.png", FileMode.Create);

        using MagickImage image = new MagickImage(_byte_texture[_page]!, 
                                  new MagickReadSettings() {
                                      Width = (uint)width_texture, Height = (uint)width_texture,
                                      Format = MagickFormat.Rgba
        });

        image.Write(fstream, MagickFormat.Png);

        fstream.Close();
        fstream.Dispose();

        _page = _page+1;

        if (_page < 4) {

            array_texture_image[_page] = new MagickImage(MagickColor.FromRgba(0, 0, 0, 0),
                                                         (uint)width_texture, (uint)width_texture
            );

            array_texture_image[_page] = new MagickImage(
            );

            _texture_font[_page] = new Texture2D(_graphicsDeviceManager.GraphicsDevice,
                                                 width_texture, width_texture
            );
        }

        return;
    }

    internal void _update_texture() {

        for (int i=0; i<4; i=i+1) {

            if (_byte_texture[i] == null) { break; }

            _texture_font[i]?.SetData(_byte_texture[i]);
        }
    }

    internal void _init_font(string name, uint size_font, string range) {

        int isize_font = (int)size_font;

        StringBuilder sbuilder = new StringBuilder();     

        sbuilder.Clear().Append(CONTENT_PATH);

        if (CONTENT_PATH[^1] != separator) { sbuilder.Append(separator); }
        
        sbuilder.Append($"Font{separator}");

        _path = sbuilder.ToString();

        string file_name_metadata = $"{_path}{separator}{name}_metadata";
        string file_name_data     = $"{_path}{separator}{name}_data";

        bool flag_modify_needed = false;

        string string_hash;

        sbuilder.Clear();

        foreach (byte b in SHA256.HashData(Encoding.UTF8.GetBytes(range))) {

            sbuilder.Append(b.ToString("X2"));
        }

        string_hash = sbuilder.ToString();

        sbuilder.Clear();

        FileStream fstream;

        // Check modified

        while (!flag_modify_needed) {

            if (!File.Exists(file_name_metadata) ||
                !File.Exists(file_name_data)) {

                flag_modify_needed = true; break;
            }

            fstream = File.Open(file_name_metadata, FileMode.Open, FileAccess.Read);

            fstream.Seek(0, SeekOrigin.Begin);

            byte[] array_byte = new byte[1024];

            while (fstream.Read(array_byte, 0, array_byte.Length) > 0) {

                sbuilder.Append(new UTF8Encoding().GetString(array_byte));
            }

            fstream.Close();
            fstream.Dispose();

            string[] array_metadata = sbuilder.ToString().Split(" ");

            // [0]font_name
            // [1]font_size
            // [2]texture_page_count
            // [3]version
            // [4]string_hash

            if (array_metadata.Length < 5) { flag_modify_needed = true; break; }

            if (array_metadata[0] != name ||
                array_metadata[1] != size_font.ToString() ||
                array_metadata[3] != version ||
                array_metadata[4] != string_hash) {

                flag_modify_needed = true; break;
            }

            for (int i=0; i<Convert.ToInt32(array_metadata[2]); i=i+1) {

                if (File.Exists($"{_path}{name}_font_texture_{i}.png")) { continue; }

                flag_modify_needed = true; break;
            }

            if (!flag_modify_needed) { break; }
        }

        // Not Modified

        if (!flag_modify_needed) {

            for (int i=0; i<4; i=i+1) {

                if (!File.Exists($"{_path}{name}_font_texture_{i}.png")) { break; }

                using MagickImage image = new MagickImage($"{_path}{name}_font_texture_{i}.png", MagickFormat.Png);

                using IPixelCollection<byte> pdata = image.GetPixels();

                _texture_font[i] = new Texture2D(_graphicsDeviceManager.GraphicsDevice,
                                                 (int)image.Width, (int)image.Height
                );

                _byte_texture[i] = pdata.ToByteArray(PixelMapping.RGBA);
            }

            // Load metadata to Dictionary

            fstream = File.Open(file_name_data, FileMode.Open, FileAccess.Read);

            fstream.Seek(0, SeekOrigin.Begin);

            using StreamReader sreader = new StreamReader(fstream);

            foreach (char c in range) { _dict_char_data.Add(c, uint.Parse(sreader.ReadLine()!)); }

            fstream.Close();
            fstream.Dispose();

            return;
        }

        if (File.Exists(file_name_metadata)) {

            try { File.Delete(file_name_metadata); }

            catch { _stacktrace(ExConstants.FONT_METADATA_NOT_ACCESSABLE); return; }
        }

        if (File.Exists(file_name_data)) {

            try { File.Delete(file_name_data); }

            catch { _stacktrace(ExConstants.FONT_DATA_NOT_ACCESSABLE); return; }
        }

        for (int i=0; i<4; i=i+1) {

            if (File.Exists($"{_path}{name}_font_texture_{i}.png")) {

                try { File.Delete($"{_path}{name}_font_texture_{i}.png"); }

                catch { _stacktrace(ExConstants.FONT_TEXTURE_NOT_ACCESSABLE); }
            }
        }

        int size_texture = range.Length*isize_font*(isize_font+1);

        int width_texture = 1<<(int)float.Ceiling(float.Log2(float.Ceiling(float.Sqrt(size_texture))));

        // Less then .75 -> Length less then sqrt(.75)≃.866f -> Split texture

        MagickImage?[] array_texture_image = [null, null, null, null];

        if ((float)float.Sqrt(size_texture)/width_texture < .866f) {

            width_texture = width_texture>>1;
        }

        using MagickImage texture = new MagickImage(MagickColor.FromRgba(0, 0, 0, 0),
                                                    (uint)width_texture, (uint)width_texture
        );

        array_texture_image[0] = texture;

        _texture_font[0] = new Texture2D(_graphicsDeviceManager.GraphicsDevice,
                                         width_texture, width_texture
        );

        _byte_texture[0] = new byte[4*width_texture*width_texture];

        IDrawables<byte> drawables = _assign_drawables(_path+name);

        uint xpos = 0;
        uint ypos = size_font;

        uint width_char = size_font;

        int asize = range.Length;

        int ci = 0;

        string pstring = "";

        uint data;

        uint pre_space = 0;

        _vmod = isize_font+1+(int)(size_font/10);

        while (ci < asize) {

            while (_is_sqr(range[ci])) {

                // Square -> Skip calculation & stack

                // width_margin  = r1?
                // height_margin = t1?
                // lbreak_margin = b2?

                bool flag_flush = false;

                FLUSH_REMAINING:

                if (xpos > width_texture-size_font || flag_flush) {

                    int xpos_pstring = (int)xpos-(isize_font*pstring.Length);

                    drawables.Text(xpos_pstring, ypos, pstring).Draw(array_texture_image[_page]!);

                    Rectangle region = new Rectangle(xpos_pstring, (int)(ypos-size_font), 
                                                     width_texture, _vmod
                    );

                    using IPixelCollection<byte> pdata = array_texture_image[_page]!.GetPixels();

                    // Assign

                    _byte_texture[_page] = _byte_texture[_page]?? new byte[4*width_texture*width_texture];

                    _set_byte_data_region(_byte_texture[_page]!, width_texture, region,
                                          pdata.ToByteArray(PixelMapping.RGBA)!, width_texture, region
                    );

                    drawables = _assign_drawables(_path+name);

                    pstring = "";

                    if (!flag_flush) {

                        xpos = 0;
                        ypos = ypos+size_font;
                    }

                    if (ypos > width_texture) {

                        xpos = 0;
                        ypos = size_font;

                        _flush(array_texture_image!);
                    }

                    pre_space = 0;

                    if (flag_flush) { flag_flush = false; break; }
                }

                pstring = pstring+range[ci];

                // [2]page|[8]width_char|[11]xpos|[11]ypos

                data = ((uint)_page<<30)|(width_char<<22)|(xpos<<11)|(ypos-size_font);

                _dict_char_data.Add(range[ci], data);

                ci = ci+1;

                if (!_is_sqr(range[ci])) { flag_flush = true; goto FLUSH_REMAINING; }

                xpos = xpos+size_font;

                pre_space = size_font;
            }

            using MagickImage char_insp = new MagickImage(MagickColor.FromRgba(0, 0, 0, 0),
                                                          (uint)isize_font, (uint)_vmod
            );

            IDrawables<byte> draw_insp = _assign_drawables(_path+name);

            draw_insp.Text(0, size_font, range[ci].ToString()).Draw(char_insp);

            using IPixelCollection<byte> chr_ipc = char_insp.GetPixels();

            byte[] chr_pdata = chr_ipc.ToByteArray(PixelMapping.RGBA)!;

            int l = 0;
            int r = isize_font-1;

            for (int w=0; w<size_font; w=w+1) {

                for (int h=0; h<_vmod; h=h+1) {

                    if (chr_pdata[4*((size_font*h)+w)] > 0) {

                        l = w; goto CHECK_RIGHT;
                    }
                }
            }

            CHECK_RIGHT:

            for (int w=isize_font-1; w>-1; w=w-1) {

                for (int h=0; h<_vmod; h=h+1) {

                    if (chr_pdata[4*((size_font*h)+w)] > 0) {

                        r = w; goto CHECK_DONE;
                    }
                }
            }

            CHECK_DONE:

            width_char = (uint)(r-l)+1;

            xpos = xpos+pre_space+1;

            if (xpos > width_texture-width_char) {

                xpos = 0;
                ypos = ypos+(uint)_vmod;
            }

            pre_space = width_char;

            if (ypos > width_texture) {

                _flush(array_texture_image!);

                ypos = size_font;
            }

            // [2]page|[8]width_char|[11]xpos|[11]ypos
            data = ((uint)_page<<30)|((width_char+1)<<22)|(xpos<<11)|(ypos-size_font);

            _dict_char_data.Add(range[ci], data);

            // Assign

            _byte_texture[_page] = _byte_texture[_page]?? new byte[4*width_texture*width_texture];

            _set_byte_data_region(_byte_texture[_page]!, 
                                  width_texture,
                                  new Rectangle((int)xpos, (int)ypos-isize_font, 
                                                (int)width_char, _vmod), 
                                  chr_pdata,
                                  isize_font,
                                  new Rectangle(l, 0, 
                                                (int)width_char, _vmod
            ));

            ci = ci+1;
        }

        _flush(array_texture_image!);

        // Write metadata

        fstream = File.Open(file_name_metadata, FileMode.CreateNew, FileAccess.ReadWrite);

        fstream.Seek(0, SeekOrigin.Begin);

        sbuilder = new StringBuilder();

        byte[] arr_byte = new byte[1024];

        // [0]font_name
        // [1]font_size
        // [2]texture_page_count
        // [3]version
        // [4]string_hash

        sbuilder.Append($"{name} {size_font} {_page} {version} {string_hash}");

        string metadata = sbuilder.ToString();

        fstream.Write(new ReadOnlySpan<byte>(arr_byte, 0, Encoding.UTF8.GetBytes(metadata.AsSpan(), new Span<byte>(arr_byte, 0, 4*metadata.Length))));

        sbuilder.Clear();

        fstream.Close();
        fstream.Dispose();

        //Write data

        fstream = File.Open(file_name_data, FileMode.CreateNew, FileAccess.ReadWrite);

        fstream.Seek(0, SeekOrigin.Begin);

        for (int i=0; i<_dict_char_data.Count; i=i+1) {

            fstream.Write(new ReadOnlySpan<byte>(arr_byte, 0, Encoding.UTF8.GetBytes($"{_dict_char_data[range[i]]}\n".AsSpan(), new Span<byte>(arr_byte, 0, 4*metadata.Length))));
        }

        fstream.Close();
        fstream.Dispose();
    }
}