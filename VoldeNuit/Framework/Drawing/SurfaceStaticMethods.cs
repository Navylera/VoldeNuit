using Microsoft.Xna.Framework.Graphics;

namespace VoldeNuit.Framework.Drawing;

using static Heart;
using static Draw;

public static class Surface {

    public static RenderTarget2D surface_create(int width, int height) {

        return new RenderTarget2D(_graphicsDeviceManager.GraphicsDevice, width, height);
    }

    public static bool surface_exists(RenderTarget2D surface) {

        return surface != null;
    }

    public static void surface_set_target(RenderTarget2D surface) {

        _graphicsDeviceManager.GraphicsDevice.SetRenderTarget(surface); return;
    }

    public static void surface_reset_target() {

        _graphicsDeviceManager.GraphicsDevice.SetRenderTarget(null); return;
    }

    public static void draw_serface(RenderTarget2D surface, float x, float y) {

        draw_texture(surface, x, y); return;
    }

    public static void surface_free(RenderTarget2D surface) {

        surface.Dispose(); return;
    }
}