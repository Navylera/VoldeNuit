namespace VoldeNuit.Framework.Display;

public partial class Camera {

    public bool IsVisible { get => visible; set => visible = value; }

    public View View { get => view; }
    public Viewport Viewport { get => viewport; }

    public uint ColorFilter { get => colorfilter; set => colorfilter = value; }
}