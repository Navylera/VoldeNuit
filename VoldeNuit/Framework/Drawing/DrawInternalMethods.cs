using Microsoft.Xna.Framework;

namespace VoldeNuit.Framework.Drawing;

public static partial class Draw {

    internal static byte[] _get_byte_data_region(byte[] data, Rectangle region) {

        byte[] ret = new byte[4*region.Width*region.Height];

        int count = 0;

        for (int k=0; k<region.Height; k=k+1) {

            for (int i=0; i<region.Width; i=i+1) {

                int pos = 4*(((region.Y+k)*region.Width)+region.X+i);

                ret[count  ] = data[pos  ];
                ret[count+1] = data[pos+1];
                ret[count+2] = data[pos+2];
                ret[count+3] = data[pos+3];

                count = count+4;
            }
        }

        return ret;
    }

    internal static void _set_byte_data_region(byte[] dest, int destwidth, Rectangle destregion, byte[] source, int sourcewidth, Rectangle sourceregion) {

        for (int k=0; k<destregion.Height; k=k+1) {

            for (int i=0; i<destregion.Width; i=i+1) {

                int destpos   = 4*(((destregion.Y+k)*destwidth)+destregion.X+i);
                int sourcepos = 4*(((sourceregion.Y+k)*sourcewidth)+sourceregion.X+i);

                dest[destpos  ] = source[sourcepos  ];
                dest[destpos+1] = source[sourcepos+1];
                dest[destpos+2] = source[sourcepos+2];
                dest[destpos+3] = source[sourcepos+3];
            }
        }
    }
}