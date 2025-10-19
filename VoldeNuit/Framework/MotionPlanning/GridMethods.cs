using Microsoft.Xna.Framework;

using VoldeNuit.Framework.Instances;
using VoldeNuit.Framework.Drawing;

namespace VoldeNuit.Framework.MotionPlanning;

using static Configuration;
using static Quadtree;

public partial class Grid {

    public void InitRegion(int x, int y, int width, int height) {
        
        int ltx = int.Clamp(x/cellwidth,  0, hcells);
        int lty = int.Clamp(y/cellheight, 0, vcells);

        int rbx = int.Clamp((x+width) /cellwidth,  0, hcells);
        int rby = int.Clamp((y+height)/cellheight, 0, vcells);

        for (int i=ltx; ltx<rbx; i=i+1) {

            for (int k=lty; k<rby; k=k+1) {

                this[i, k] = SOLID;
            }
        }

        return;
    }

    public void InitRegion(Rectangle region) {

        InitRegion(region.X, region.Y, region.Width, region.Height); return;
    }

    public void InitInstance(Instance instance, bool precision) {

        Sprite mask = instance.mask_index!;

        if (mask == null) return;
        
        int ltx_mask = (int)float.Round(instance.x-mask.x, ROUNDING);
        int lty_mask = (int)float.Round(instance.y-mask.y, ROUNDING);

        if (!precision) { InitRegion(ltx_mask, lty_mask, mask.sprite_width, mask.sprite_height); return; }

        int ltx = int.Clamp(ltx_mask/cellwidth,  0, hcells);
        int lty = int.Clamp(lty_mask/cellheight, 0, vcells);

        int rbx = int.Clamp((ltx_mask+mask.sprite_width) /cellwidth,  0, hcells);
        int rby = int.Clamp((lty_mask+mask.sprite_height)/cellheight, 0, vcells);

        HashSet<Instance> _li = [instance];

        for (int i=ltx; i<rbx; i=i+1) {

            for (int k=lty; k<rby; k=k+1) {

                _ci.x = i*cellwidth;
                _ci.y = k*cellheight;

                if (_qtree_collision._check_prec(_ci, _li) != null) { this[i, k] = -1; }
            }
        }

        return;
    }

    public void Clear() {

        for (int i=0; i<hcells; i=i+1) {

            for (int k=0; k<vcells; k=k+1) {

                this[i, k] = UNDEFINED;
            }
        }
    }

    public void Destroy() { Dispose(); }
}