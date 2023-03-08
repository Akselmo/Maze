namespace Maze;

public class Solver
{
    private Maze Maze;
    private int TotalMoves = 0;

    public Solver(Maze maze)
    {
        Maze = maze;
    }
    
    bool SolveMazePath(Tile startTile, Tile endTile, int maximumMoves)
    {
        startTile.SetDistance(endTile.Position);

        var activeTiles = new List<Tile>();
        activeTiles.Add(startTile);

        while(activeTiles.Any() || TotalMoves > maximumMoves)
        {
            var checkTile = activeTiles.OrderBy(x => x.CostDistance).First();

            if (BackTrack(checkTile, endTile))
            {
                Console.WriteLine("Moves: " + TotalMoves + " / " + maximumMoves);
                return true;
            }

            checkTile.Visited = true;
            activeTiles.Remove(checkTile);

            WalkTiles(activeTiles, checkTile, endTile);

            TotalMoves++;
        }

        Console.WriteLine("No Path Found!");
        Console.WriteLine("Moves: " + TotalMoves + " / " + maximumMoves);
        return false;
    }
    
    bool BackTrack(Tile checkTile, Tile endTile)
    {
        if(checkTile.Position == endTile.Position)
        {
            var tile = checkTile;
            Console.WriteLine("Retracing steps backwards...");
            while(true)
            {
                if(GetTileFromGrid(tile.Position).Type == TileType.Path)
                {
                    GetTileFromGrid(tile.Position).Type = TileType.Visited;
                }
                
                tile = tile.Parent;
                
                if(tile == null)
                {
                    Console.WriteLine("Done!");
                    return true;
                }
            }
        }
        return false;
    }
    
    void WalkTiles(List<Tile> activeTiles, Tile checkTile, Tile endTile)
    {
        var walkableTiles = GetWalkableTiles(checkTile, endTile);
        foreach(var walkableTile in walkableTiles)
        {
            if (walkableTile.Visited)
                continue;

            if(activeTiles.Any(x => x.Position == walkableTile.Position))
            {
                var existingTile = activeTiles.First(x => x.Position == walkableTile.Position);
                if(existingTile.CostDistance > checkTile.CostDistance)
                {
                    activeTiles.Remove(existingTile);
                    activeTiles.Add(walkableTile);
                }
            }
            else
            {
                activeTiles.Add(walkableTile);
            }
        }
    }
    
    private List<Tile> GetWalkableTiles(Tile currentTile, Tile targetTile)
    {
        Point north = new Point(currentTile.Position.X, currentTile.Position.Y + 1);
        Point south = new Point(currentTile.Position.X, currentTile.Position.Y - 1);
        Point east = new Point(currentTile.Position.X + 1, currentTile.Position.Y);
        Point west = new Point(currentTile.Position.X - 1, currentTile.Position.Y);
            
        var possibleTiles = new List<Tile>
        {
            new(south, currentTile, currentTile.Cost + 1),
            new(north, currentTile, currentTile.Cost + 1),
            new(west, currentTile, currentTile.Cost + 1),
            new(east, currentTile, currentTile.Cost + 1)
        };

        possibleTiles.ForEach(tile => tile.SetDistance(targetTile.Position));

        return possibleTiles
            .Where(tile => GetTileFromGrid(tile.Position).Type == TileType.Path || 
                           GetTileFromGrid(tile.Position).Type == TileType.Exit)
            .ToList();
    }
    
    Tile GetTileFromGrid(Point position)
    {
        if (IsPositionWithinGrid(position))
        {
            return Maze.Grid[position.Y][position.X];
        }

        var nullTile = new Tile(position, null, 0)
        {
            Type = TileType.None
        };
        
        return nullTile;
    }
    
    bool IsPositionWithinGrid(Point position)
    {
        return position.X >= 0 && position.X <= Maze.Width && position.Y >= 0 && position.Y <= Maze.Height;
    }
    
    List<Tile> GetTilesByType(TileType type)
    {
        var tilesByType = (from tiles in Maze.Grid
                                    from tile in tiles
                                    where tile.Type == type
                                    select tile).ToList();
        return tilesByType;
    }
    
    public bool Run(int maximumMoves)
    {
        var startTiles = GetTilesByType(TileType.Start);
        var exitTiles = GetTilesByType(TileType.Exit);

        var start = startTiles.First();
        var exit = exitTiles.First();
        start.SetDistance(exit.Position);
        
        var success = SolveMazePath(start, exit, maximumMoves);
        Graphics.Render(Maze, TotalMoves, maximumMoves);
        
        return success;
    }
    
}