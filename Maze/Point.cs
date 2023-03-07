namespace Maze;

// Make own Point for coordinates, since Vector2 uses float. 
// This way there's no need to cast everything to Int
public struct Point
{
    public int X;
    public int Y;

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }
    
    public static bool operator ==(Point a, Point b)
    {
        bool xEqual = a.X == b.X;
        bool yEqual = a.Y == b.Y;

        return xEqual && yEqual;
    }

    public static bool operator !=(Point a, Point b)
    {
        return !(a == b);
    }
}