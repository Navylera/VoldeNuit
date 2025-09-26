using Microsoft.Xna.Framework.Graphics;

namespace VoldeNuit.Framework.Display;

using static Heart;

public partial class Camera(int xview, int yview, int wview, int hview, int xport, int yport, int wport, int hport) {

    public bool visible = true;

    public readonly View view = new View(xview, yview, wview, hview);
    public readonly Viewport viewport = new Viewport(xport, yport, wport, hport);

    internal static void _update_window_size() {

        int bwidth  = 0;
        int bheight = 0;

        foreach (Camera c in room_current.camera) {

            if (c.view._updated) {

                c.view._render_target.Dispose();
                c.view._render_target = new RenderTarget2D(
                    _graphicsDeviceManager.GraphicsDevice, c.view.width, c.view.height
                );
            }

            if (!c.visible) { continue; }

            bwidth  = int.Max(bwidth, c.viewport.x+c.viewport.width);
            bheight = int.Max(bheight, c.viewport.y+c.viewport.height);
        }

        if (_graphicsDeviceManager.PreferredBackBufferWidth  == bwidth &&
            _graphicsDeviceManager.PreferredBackBufferHeight == bheight) {

            return;
        }

        _graphicsDeviceManager.PreferredBackBufferWidth  = bwidth;
        _graphicsDeviceManager.PreferredBackBufferHeight = bheight;

        _graphicsDeviceManager.ApplyChanges(); return;
    }

    public uint colorfilter = 0xffffffffu;
}