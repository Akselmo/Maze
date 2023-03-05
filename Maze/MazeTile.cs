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
    public MazeTile Parent;
    public bool Visited = false;

    public MazeTile(Vector2 position, ElementType type, MazeTile parent)
    {
        Position = position;
        Type = type;
        Parent = parent;
    }
    
    public void SetVisited()
    {
        Visited = true;
        if (Type != ElementType.Start)
        {
            Type = ElementType.Visited;
        }
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
