using System.Numerics;

namespace Maze;

public static class FileParser
{
    public static List<MazeTile> ReadFile(string filePath)
    {
        if (!Path.Exists(filePath))
        {
            return null;
        }
        
        var lines = File.ReadAllLines(filePath);

        List<MazeTile> maze = new List<MazeTile>();
        
        int x = 0;
        int y = 0;

        foreach (var line in lines)
        {
            foreach (char ch in line)
            {
                maze.Add(MazeTile.GetElement(ch, new Vector2(x,y)));
                x++;
            }
            x = 0;
            y++;
        }

        return maze;
    }
}