namespace VoldeNuit.Framework.Display;

public partial class Room {

    public List<Camera> Cameras { get => camera; }

    public int Width { get => room_width; set => room_width = value; }
    public int Height { get => room_height; set => room_height = value; }

    public int RoomSpeed { get => room_speed; set => room_speed = value; }

    public uint BackgroundColor { get => color_background; set => color_background = value; }
}