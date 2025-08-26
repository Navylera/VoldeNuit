using Microsoft.Xna.Framework;

namespace VoldeNuit.Framework.Drawing;

public partial class Sprite {

    public string? TexturePath { get => texture_path; init => texture_path = value; }

    public int X { get => x; init => x = value; }
    public int Y { get => y; init => y = value; }

    public int Width { get => sprite_width; init => sprite_width = value; }
    public int Height { get => sprite_height; init => sprite_height = value; }

    public int ImageNumber { get => image_number; }

    public Rectangle? Boundbox { get => bbox; set => bbox = value; }

    public bool Precise { get => precise; }
}