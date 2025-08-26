namespace VoldeNuit.Framework.MotionPlanning;

public partial class Grid {

    public int X { get => x; set => x = value; }
    public int Y { get => y; set => y = value; }

    public int HCells { get => hcells; init => hcells = value; }
    public int VCells { get => vcells; init => vcells = value; }

    public int CellWidth { get => cellwidth; init => cellwidth = value; }
    public int CellHeight { get => cellheight; init => cellheight = value; }
}