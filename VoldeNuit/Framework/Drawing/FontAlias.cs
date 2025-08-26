namespace VoldeNuit.Framework.Drawing;

public partial class Font {

    public string FontName { get => name; init => name = value; }

    public uint FontSize { get => size_font; init => size_font = value; }

    public string FontRange { set => range = value; }
}