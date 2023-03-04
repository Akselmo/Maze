using System.Numerics;

namespace Maze;

public enum ElementType
{
    Block = '#', 
    Path = ' ',
    Exit = 'E',
    Start = '^',
    Visited = 'X'

}

public class MazeTile
{
    public Vector2 Position;
    public ElementType Type;
    public bool Visited = false;

    public MazeTile(Vector2 position, ElementType type)
    {
        Position = position;
        Type = type;
    }

    public static MazeTile GetElement(char ch, Vector2 pos)
    {
        var tiles = Enum.GetValues(typeof(ElementType));
        foreach (ElementType tile in tiles)
        {
            if (ch == (char)tile)
            {
                return new MazeTile(pos, tile);
            }
        }
        return null;
    }
}
