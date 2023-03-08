namespace Maze;

public class Solver
{
    private Maze Maze;
    private int TotalMoves = 0;
    private int MaximumMoves = 0;

    public enum BacktrackStatus
    {
        Done,
        Skip,
        OutOfMoves
    }
    
    public Solver(Maze maze)
    {
        Maze = maze;
    }
    
    bool SolveMazePath(Tile startTile, Tile endTile)
    {
        startTile.SetDistance(endTile.Position);

        // List of tiles to check the adjacent tiles from
        var checkTiles = new List<Tile> { startTile };

        // Loop through tiles until check tiles is empty or we reach the maximum moves
        while(checkTiles.Any())
        {
            var checkTile = checkTiles.OrderBy(x => x.CostDistance).First();

            var backtrackStatus = BackTrack(checkTile, endTile);
            
            switch (backtrackStatus)
            {
                case BacktrackStatus.Done:
                    Console.WriteLine("Moves: " + TotalMoves + " / " + MaximumMoves);
                    Console.WriteLine("Done!");
                    return true;
                case BacktrackStatus.Skip:
                    // Backtracking is skipped, continue as normal
                    break;
                case BacktrackStatus.OutOfMoves:
                    Console.WriteLine("Maximum move limit " + MaximumMoves + " reached!");
                    return false;
            }
            
            checkTile.Visited = true;
            
            checkTiles.Remove(checkTile);

            // Walk through tiles and select the lowest cost one from possible tiles
            WalkTiles(checkTiles, checkTile, endTile);
        }

        Console.WriteLine("No Path Found!");
        Console.WriteLine("Moves: " + TotalMoves + " / " + MaximumMoves);
        return false;
    }
    
    BacktrackStatus BackTrack(Tile checkTile, Tile endTile)
    {
        // If we have reached end tile, start backtracking, otherwise skip
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
                TotalMoves++;
                
                if (TotalMoves >= MaximumMoves)
                {
                    return BacktrackStatus.OutOfMoves;
                }
                
                if(tile == null)
                {
                    return BacktrackStatus.Done;
                }
            }
        }
        return BacktrackStatus.Skip;
    }
    
    void WalkTiles(List<Tile> activeTiles, Tile checkTile, Tile endTile)
    {
        // Get list of tiles that we can walk on
        var walkableTiles = GetWalkableTiles(checkTile, endTile);
        
        foreach(var walkableTile in walkableTiles)
        {
            // Skip a tile we have already visited
            if (walkableTile.Visited)
                continue;

            // Check if any tiles in the active list have better movement value
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
                // Unseen tile, add it to active tiles
                activeTiles.Add(walkableTile);
            }
        }
    }
    
    private List<Tile> GetWalkableTiles(Tile currentTile, Tile targetTile)
    {
        // Possible directions
        Point north = new Point(currentTile.Position.X, currentTile.Position.Y + 1);
        Point south = new Point(currentTile.Position.X, currentTile.Position.Y - 1);
        Point east = new Point(currentTile.Position.X + 1, currentTile.Position.Y);
        Point west = new Point(currentTile.Position.X - 1, currentTile.Position.Y);
            
        // Add possible tiles with directions to a list
        var possibleTiles = new List<Tile>
        {
            new(south, currentTile, currentTile.Cost + 1),
            new(north, currentTile, currentTile.Cost + 1),
            new(west, currentTile, currentTile.Cost + 1),
            new(east, currentTile, currentTile.Cost + 1)
        };

        // Set the distance for each tile
        possibleTiles.ForEach(tile => tile.SetDistance(targetTile.Position));

        // Return only tiles that are either Path or Exit type
        return possibleTiles
            .Where(tile => GetTileFromGrid(tile.Position).Type == TileType.Path || 
                           GetTileFromGrid(tile.Position).Type == TileType.Exit)
            .ToList();
    }
    
    Tile GetTileFromGrid(Point position)
    {
        // If tile is inside grid, return tile
        if (IsPositionWithinGrid(position))
        {
            return Maze.Grid[position.Y][position.X];
        }

        // Otherwise return a tile with None type, so solver knows to ignore it
        var nullTile = new Tile(position, null, 0)
        {
            Type = TileType.None
        };
        
        return nullTile;
    }
    
    bool IsPositionWithinGrid(Point position)
    {
        // Check that position is within the grid of our maze
        return position.X >= 0 && position.X <= Maze.Width && position.Y >= 0 && position.Y <= Maze.Height;
    }
    
    List<Tile> GetTilesByType(TileType type)
    {
        // Get tiles by given type, tried some LINQ here :)
        var tilesByType = (from tiles in Maze.Grid
                                    from tile in tiles
                                    where tile.Type == type
                                    select tile).ToList();
        return tilesByType;
    }
    
    public bool Run(int maximumMoves)
    {
        MaximumMoves = maximumMoves;
        
        // Get all start tiles
        var startTiles = GetTilesByType(TileType.Start);
        var exitTiles = GetTilesByType(TileType.Exit);

        // Get the first tile
        var start = startTiles.First();
        var exit = exitTiles.First();
        start.SetDistance(exit.Position);
        
        var success = SolveMazePath(start, exit);
        // Only render the graphics if the solve was a success
        if (success)
        {
            Graphics.Render(Maze, TotalMoves, maximumMoves);
        }
        
        return success;
    }
    
}