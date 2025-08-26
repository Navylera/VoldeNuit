namespace VoldeNuit.Framework.Input;

using VoldeNuit.Framework.Display;

public static partial class Mouse {

    public static int XOnCamera(Camera camera) {
        
        return mouse_x_on(camera);
    }

    public static int YOnCamera(Camera camera) {
        
        return mouse_y_on(camera);
    } 

    public static bool HasButtonInput(int mouseButton) {
        
        return mouse_check_button(mouseButton);
    }

    public static bool HasButtonPressed(int mouseButton) {
        
        return mouse_check_button_pressed(mouseButton);
    }

    public static bool HasButtonReleased(int mb) {
        
        return mouse_check_button_released(mb);
    }
}