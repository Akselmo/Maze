namespace Maze;

public static class FileParser
{

    public static string[] TryReadingLines(string filePath)
    {
        try
        {
            string[] lines = File.ReadAllLines(filePath);
            if (lines.Length == 0)
            {
                throw new FileIsEmptyException(filePath);
            }
            return lines;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public static MazeObject ReadFile(string filePath)
    {
        
        var lines = TryReadingLines(filePath);
        
        int x = 0;
        int y = 0;
        
        MazeObject mazeObject = new MazeObject();
        mazeObject.Path = filePath;

        // Turn text file into a grid of tiles
        foreach (var li in lines)
        {
            List<Tile> line = new List<Tile>();
            
            foreach (char ch in li)
            {
                // Create new tile based on the position
                var tile = new Tile(new Point(x, y), null, 0)
                {
                    // Set the type of the tile by looking at the text grid for same character
                    Type = (TileType)ch
                };
                
                line.Add(tile);
                x++;
            }
            
            x = 0;
            y++;
            
            mazeObject.Grid.Add(line);
        }

        return mazeObject;
    }
}