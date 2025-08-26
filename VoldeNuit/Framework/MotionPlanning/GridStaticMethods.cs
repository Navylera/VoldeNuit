using Microsoft.Xna.Framework;

using VoldeNuit.Framework.Instances;
using VoldeNuit.Framework.Drawing;

namespace VoldeNuit.Framework.MotionPlanning;

using static Configuration;
using static Quadtree;

public partial class Grid {

    public static Grid mp_grid_create(int x, int y, int hcells, int vcells, int cellwidth, int cellheight) {

        return new Grid(x, y, hcells, vcells, cellwidth, cellheight);
    }

    public static void mp_grid_add_cell(Grid grid, int h, int v) { grid[h, v] = -1; return; }

    public static void mp_grid_add_rectangle(Grid grid, int x, int y, int width, int height) {
        
        int ltx = int.Clamp(x/grid.cellwidth,  0, grid.hcells);
        int lty = int.Clamp(y/grid.cellheight, 0, grid.vcells);

        int rbx = int.Clamp((x+width) /grid.cellwidth,  0, grid.hcells);
        int rby = int.Clamp((y+height)/grid.cellheight, 0, grid.vcells);

        for (int i=ltx; ltx<rbx; i=i+1) {

            for (int k=lty; k<rby; k=k+1) {

                grid[i, k] = SOLID;
            }
        }

        return;
    }

    public static void mp_grid_add_region(Grid grid, Rectangle region) {

        mp_grid_add_rectangle(grid, region.X, region.Y, region.Width, region.Height); return;
    }

    public static void mp_grid_add_instances(Grid grid, Instance instance, bool precision) {

        Sprite mask = instance.mask_index;

        if (mask == null) return;
        
        int ltx_mask = (int)float.Round(instance.x-mask.x, ROUNDING);
        int lty_mask = (int)float.Round(instance.y-mask.y, ROUNDING);

        if (!precision) { mp_grid_add_rectangle(grid, ltx_mask, lty_mask, mask.sprite_width, mask.sprite_height); return; }

        int ltx = int.Clamp(ltx_mask/grid.cellwidth,  0, grid.hcells);
        int lty = int.Clamp(lty_mask/grid.cellheight, 0, grid.vcells);

        int rbx = int.Clamp((ltx_mask+mask.sprite_width) /grid.cellwidth,  0, grid.hcells);
        int rby = int.Clamp((lty_mask+mask.sprite_height)/grid.cellheight, 0, grid.vcells);

        HashSet<Instance> _li = [instance];

        for (int i=ltx; i<rbx; i=i+1) {

            for (int k=lty; k<rby; k=k+1) {

                grid._ci.x = i*grid.cellwidth;
                grid._ci.y = k*grid.cellheight;

                if (_qtree_collision._check_prec(grid._ci, _li) != null) { grid[i, k] = -1; }
            }
        }

        return;
    }

    public static void mp_grid_clear_all(Grid grid) {

        for (int i=0; i<grid.hcells; i=i+1) {

            for (int k=0; k<grid.vcells; k=k+1) {

                grid[i, k] = SOLID;
            }
        }
    }

    public static void mp_grid_destroy(Grid grid) { grid.Dispose(); }
}