namespace Maze;

public static class FileParser
{
    public static Maze ReadFile(string filePath)
    {
        if (!Path.Exists(filePath))
        {
            return null;
        }
        
        var lines = File.ReadAllLines(filePath);
        
        int x = 0;
        int y = 0;
        
        Maze maze = new Maze();
        maze.Path = filePath;
        
        List<List<Tile>> grid = new List<List<Tile>>();

        foreach (var li in lines)
        {
            List<Tile> line = new List<Tile>();
            
            foreach (char ch in li)
            {
                var tile = new Tile(new Point(x, y), null, 0)
                {
                    Type = (TileType)ch
                };
                
                line.Add(tile);
                x++;
            }
            
            x = 0;
            y++;
            
            grid.Add(line);
        }

        maze.Grid = grid;
        
        return maze;
    }
}