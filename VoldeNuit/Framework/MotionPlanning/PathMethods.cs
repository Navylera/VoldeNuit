using Microsoft.Xna.Framework;
using VoldeNuit.Framework.Instances;

namespace VoldeNuit.Framework.MotionPlanning;

using static Configuration;
using static Heart;
using static Grid;

public partial class Path {

    internal static int[, ] _search_priority = { 
        
        {  0, -1 }, {  1,  0 }, {  0,  1 }, { -1,  0 }, 
        {  1, -1 }, {  1,  1 }, { -1, -1 }, { -1,  1 }, // Diagonal
    };

    public static Path path_add() { return new Path(); }

    public static bool mp_grid_path(Grid grid, Path path, int xstart, int ystart, int xdest, int ydest, bool diagonal) {

        bool flag_end = false;

        PriorityQueue<Vector2, int> node_open = new PriorityQueue<Vector2, int>();

        int w = grid.hcells;
        int h = grid.vcells;

        Vector2[, ] vfrom = new Vector2[w, h];
        for (int i=0; i<w; i=i+1) { for (int k=0; k<h; k=k+1) { vfrom[i, k].X = -1; } }
        int[, ] cost      = new int[w, h];
        bool[, ] closed   = new bool[w, h];

        Vector2 node_current = new Vector2(xstart, ystart);

        node_open.Enqueue(node_current, 0);

        // Initialize

        vfrom[xstart, ystart] = node_current;
        cost [xstart, ystart] = 0;

        closed[xstart, ystart] = true;

        while (node_open.Count > 0) {

            if (flag_end) { break; }

            node_current = node_open.Dequeue();

            int xprevious = (int)float.Round(node_current.X, ROUNDING);
            int yprevious = (int)float.Round(node_current.Y, ROUNDING);

            closed[xprevious, yprevious] = true;

            if (xprevious == xdest && yprevious == ydest) { break; }

            for (int i=0; i<4+(diagonal? 4:0); i=i+1) {

                if (flag_end) { break; }

                if (xprevious == xdest && yprevious == ydest) {

                    vfrom[xprevious, yprevious] = node_current;

                    flag_end = true; break;
                }
                
                int xto = _search_priority[i, 0]+xprevious;
                int yto = _search_priority[i, 1]+yprevious;

                if (xto < 0 || yto < 0 || xto >= w || yto >= h) { continue; }

                if (!diagonal ||
                    grid[xto, yto+_search_priority[(i%2)*2 ,1]]       == SOLID &&
                    grid[xto+_search_priority[(i/4)+i>5?2:0, 0], yto] == SOLID) {

                    continue;
                }

                if (!(grid[xto, yto] != SOLID) && !closed[xto, yto]) {

                    vfrom [xto, yto] = node_current;
                    cost  [xto, yto] = cost[xprevious, yprevious]+1;

                    // weight = cost+distance

                    int weight = cost[xto, yto]+int.Abs(xdest-xto)+int.Abs(ydest-yto);

                    node_open.Enqueue(new Vector2(xto, yto), weight);
                }
            }
        }

        Vector2 node_rsearch = vfrom[xdest, ydest];

        if (node_rsearch.X == -1) { return false; }

        path.Clear();

        path.Add(new Vector2(xdest, ydest));

        while (node_rsearch.X != xstart && node_rsearch.Y != ystart) {

            path.Add(node_rsearch);
            
            node_rsearch = vfrom[(int)node_rsearch.X, (int)node_rsearch.Y];
        }

        return true;
    }

    public static Grid pcheck(Instance instance, int precision, int depth) {

        return new Grid((int)float.Round(instance.x-(precision*depth), ROUNDING),
                             (int)float.Round(instance.y-(precision*depth), ROUNDING),
                             2*depth, 2*depth, precision, precision
        );
    }
}