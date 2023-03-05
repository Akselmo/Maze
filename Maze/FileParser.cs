using System.Numerics;

namespace Maze;

public static class FileParser
{
    public static MazeTile[,] ReadFile(string filePath)
    {
        if (!Path.Exists(filePath))
        {
            return null;
        }
        
        var lines = File.ReadAllLines(filePath);
        
        int x = 0;
        int y = 0;
        MazeTile[,] maze = new MazeTile[lines[1].Length,lines.Length];
        
        foreach (var line in lines)
        {
            foreach (char ch in line)
            {
                var tile = new MazeTile(new Vector2(x, y), MazeTile.GetElement(ch), null, 0);
                maze[x, y] = tile;
                x++;
            }
            x = 0;
            y++;
        }

        return maze;
    }
}