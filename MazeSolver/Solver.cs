namespace Maze;

public class Solver
{
    private MazeObject _mazeObject;
    private int TotalMoves = 0;
    private int MaximumMoves = 0;

    enum BacktrackStatus
    {
        Done,
        Skip,
        OutOfMoves
    }
    
    public Solver(MazeObject mazeObject)
    {
        _mazeObject = mazeObject;
    }
    
    bool SolveMazePath(Tile startTile, Tile endTile)
    {
        startTile.SetDistanceToTarget(endTile.Position);

        // List of tiles to check the adjacent tiles from
        var checkTiles = new List<Tile> { startTile };

        // Loop through tiles until check tiles is empty or we reach the maximum moves
        while(checkTiles.Any())
        {
            //Take the tile with the lowest Distance + Cost
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
    
    void WalkTiles(List<Tile> checkTiles, Tile checkTile, Tile endTile)
    {
        // Get list of tiles that we can walk on
        var walkableTiles = GetWalkableTiles(checkTile, endTile);
        
        foreach(var walkableTile in walkableTiles)
        {
            // Skip a tile we have already visited
            if (walkableTile.Visited)
                continue;

            // Check if any tiles in the active list have better movement value
            if(checkTiles.Any(x => x.Position == walkableTile.Position))
            {
                var existingTile = checkTiles.First(x => x.Position == walkableTile.Position);
                if(existingTile.CostDistance > checkTile.CostDistance)
                {
                    checkTiles.Remove(existingTile);
                    checkTiles.Add(walkableTile);
                }
            }
            else
            {
                // Unseen tile, add it to active tiles
                checkTiles.Add(walkableTile);
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
        possibleTiles.ForEach(tile => tile.SetDistanceToTarget(targetTile.Position));

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
            return _mazeObject.Grid[position.Y][position.X];
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
        return position.X >= 0 && position.X <= _mazeObject.Width && position.Y >= 0 && position.Y <= _mazeObject.Height;
    }
    
    List<Tile> GetTilesByType(TileType type)
    {
        // Get tiles by given type, tried some LINQ here :)
        var tilesByType = (from tiles in _mazeObject.Grid
                                    from tile in tiles
                                    where tile.Type == type
                                    select tile).ToList();
        if (tilesByType.Count == 0)
        {
            throw new NoTileTypeFound(type.ToString());
        }
        
        return tilesByType;
    }

    Tile GetTileWithShortestDistance(Tile start, List<Tile> endTiles)
    {
        Tile nearest = endTiles.First();
        endTiles.Remove(nearest);
        foreach (var end in endTiles)
        {
            if (start.GetDistanceToTarget(end.Position) < nearest.Distance)
            {
                nearest = end;
            }
        }

        return nearest;
    }
    
    public bool Run(int maximumMoves)
    {
        MaximumMoves = maximumMoves;
        
        // Get all start tiles
        var startTiles = GetTilesByType(TileType.Start);
        var exitTiles = GetTilesByType(TileType.Exit);

        // Get the first tile
        var start = startTiles.First();

        // Get the exit with the shortest distance
        Tile exit = GetTileWithShortestDistance(start, exitTiles);

        start.SetDistanceToTarget(exit.Position);
        
        var success = SolveMazePath(start, exit);

        Graphics.Render(_mazeObject, TotalMoves, maximumMoves);

        return success;
    }
    
}