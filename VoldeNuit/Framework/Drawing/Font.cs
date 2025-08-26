using Microsoft.Xna.Framework.Graphics;

namespace VoldeNuit.Framework.Drawing;

public partial class Font {

    public string name { get; init; }

    public uint size_font { get; init; }

    public string range { internal get; set; }

    internal readonly Dictionary<char, uint> _dict_char_data = [];

    internal Texture2D[] _texture_font = new Texture2D[4];

    internal byte[]?[] _byte_texture = new byte[4][];
    
    internal int _page = 0;

    internal int _vmod;

    private string _path;

    internal enum FontType {

        Monogame,
        VoldeNuit
    }

    internal FontType fonttype = FontType.VoldeNuit;
}