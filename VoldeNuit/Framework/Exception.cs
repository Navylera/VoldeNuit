namespace VoldeNuit.Framework;

using static Configuration;

internal static class Exception {

    internal static void _stacktrace(ExConstants excode) {

        if (!PRINT_STACKTRACE) { Environment.Exit((int)excode+32); return; }

        System.Exception e = new System.Exception(string_exc[(int)LANGUAGE.EN_US, (int)ExConstants.ARGUMENT_NOT_ALLOWED]);

        Console.WriteLine($"{e.Message}\n{e.StackTrace}"); Environment.Exit((int)excode+32); return;
    }

    internal enum LANGUAGE {

        EN_US,
        KO_KR,
        LANG_COUNT
    }

    internal enum ExConstants {

        ARGUMENT_NOT_ALLOWED,
        SPRITE_ALPHA_NOT_MATCHED,
        SPRITE_TEXTURE_NOT_MATCHED,
        ALPHA_IS_NOT_GRAYSCALE,
        VIEWPORT_SIZE_LESS_THEN_ONE,
        ROOM_SIZE_LESS_THEN_ONE,
        FONT_METADATA_NOT_ACCESSABLE,
        FONT_DATA_NOT_ACCESSABLE,
        FONT_TEXTURE_NOT_ACCESSABLE,
        SOUND_NOT_ACCESSABLE,

        EXC_COUNT
    }

    public static string[, ] string_exc = new string[
        (int)LANGUAGE.LANG_COUNT-1, 
        (int)ExConstants.EXC_COUNT-1
    ];

    static Exception() {

        string_exc[(int)LANGUAGE.EN_US, (int)ExConstants.ARGUMENT_NOT_ALLOWED] = 
        "The method contains argument that not allowed.";
        string_exc[(int)LANGUAGE.EN_US, (int)ExConstants.SPRITE_ALPHA_NOT_MATCHED] = 
        "The size of the sprite texture and the transparency texture does not match.";
        string_exc[(int)LANGUAGE.EN_US, (int)ExConstants.SPRITE_TEXTURE_NOT_MATCHED] = 
        "The size of the existing sprite texture and added texture does not match.";
        string_exc[(int)LANGUAGE.EN_US, (int)ExConstants.ALPHA_IS_NOT_GRAYSCALE] = 
        "The transparency texture is not a grayscale image.";
        string_exc[(int)LANGUAGE.EN_US, (int)ExConstants.VIEWPORT_SIZE_LESS_THEN_ONE] = 
        "Viewport has a width or height of 0 or less.";
        string_exc[(int)LANGUAGE.EN_US, (int)ExConstants.ROOM_SIZE_LESS_THEN_ONE] = 
        "Room has a width or height of 0 or less.";
        string_exc[(int)LANGUAGE.EN_US, (int)ExConstants.FONT_METADATA_NOT_ACCESSABLE] = 
        "Unable to access Font metadata file.";
        string_exc[(int)LANGUAGE.EN_US, (int)ExConstants.FONT_DATA_NOT_ACCESSABLE] = 
        "Unable to access Font data file.";
        string_exc[(int)LANGUAGE.EN_US, (int)ExConstants.FONT_TEXTURE_NOT_ACCESSABLE] = 
        "Unable to access Font texture file.";
        string_exc[(int)LANGUAGE.EN_US, (int)ExConstants.SOUND_NOT_ACCESSABLE] = 
        "Unable to access Sound file.";
    }
}

