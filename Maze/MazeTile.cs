using System.Numerics;

namespace Maze;

public enum ElementType
{
    Block = '#', 
    Path = ' ',
    Exit = 'E',
    Start = '^',
    Visited = 'X',
    None = '\0'

}

public class MazeTile
{
    public Vector2 Position;
    public ElementType Type;
    public int Cost;
    public int Distance;
    public int CostDistance => Cost + Distance;
    public MazeTile Parent;
    public bool Visited = false;

    public MazeTile(Vector2 position, ElementType type, MazeTile parent, int cost)
    {
        Position = position;
        Type = type;
        Parent = parent;
        Cost = cost;
    }
    
    public void SetVisited()
    {
        Visited = true;
        // Only change Paths to Visited so they're drawn correctly in render
        if (Type == ElementType.Path)
        {
            Type = ElementType.Visited;
        }
    }
    
    public void SetDistance(Vector2 target)
    {
        Distance = (int)(Math.Abs(target.X - Position.X) + Math.Abs(target.Y - Position.Y));
    }
    
    public static ElementType GetElement(char ch)
    {
        var tiles = Enum.GetValues(typeof(ElementType));
        foreach (ElementType tile in tiles)
        {
            if (ch == (char)tile)
            {
                return tile;
            }
        }
        return ElementType.None;
    }


}
