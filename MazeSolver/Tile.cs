namespace Maze;

// Different types the tile can be, with the matching character in the text file
public enum TileType
{
    Block = '#', 
    Path = ' ',
    Exit = 'E',
    Start = '^',
    Visited = 'X',
    None = '\0'
}

public class Tile
{
    public Point Position;
    public int Cost;
    public int Distance;
    public int CostDistance => Cost + Distance;
    public Tile? Parent;
    public TileType Type;
    public bool Visited = false;

    public Tile(Point position, Tile parent, int cost)
    {
        Position = position;
        Parent = parent;
        Cost = cost;
    }

    // Estimated distance of the target and the tile position, ignoring walls.
    public int GetDistanceToTarget(Point target)
    {
        return Math.Abs(target.X - Position.X) + Math.Abs(target.Y - Position.Y);
    }
    
    public void SetDistanceToTarget(Point target)
    {
        Distance = GetDistanceToTarget(target);
    }
}
