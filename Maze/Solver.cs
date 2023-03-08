namespace Maze;

public class Solver
{
    public Maze Maze;
    public int TotalMoves = 0;

    public Solver(Maze maze)
    {
        Maze = maze;
    }

    public bool SolveMazePath(Tile startTile, Tile endTile, int maximumMoves)
    {
        startTile.SetDistance(endTile.Position);

        var activeTiles = new List<Tile>();
        activeTiles.Add(startTile);

        while(activeTiles.Any() || TotalMoves > maximumMoves)
        {
            var checkTile = activeTiles.OrderBy(x => x.CostDistance).First();

            if(checkTile.Position == endTile.Position)
            {
                var tile = checkTile;
                Console.WriteLine("Retracing steps backwards...");
                while(true)
                {
                    Console.WriteLine($"{tile.Position.X} : {tile.Position.Y}");
                    if(Maze.Grid[tile.Position.Y][tile.Position.X].Type == TileType.Path)
                    {
                        Maze.Grid[tile.Position.Y][tile.Position.X].Type = TileType.Visited;
                    }
                    tile = tile.Parent;
                    if(tile == null)
                    {
                        Console.WriteLine("Done!");
                        Console.WriteLine("Moves: " + TotalMoves + " / " + maximumMoves);
                        return true;
                    }
                }
            }

            checkTile.Visited = true;
            activeTiles.Remove(checkTile);

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

            TotalMoves++;
        }

        Console.WriteLine("No Path Found!");
        Console.WriteLine("Moves: " + TotalMoves + " / " + maximumMoves);
        return false;
    }
    
    private List<Tile> GetWalkableTiles(Tile currentTile, Tile targetTile)
    {
        Point north = new Point(currentTile.Position.X, currentTile.Position.Y + 1);
        Point south = new Point(currentTile.Position.X, currentTile.Position.Y - 1);
        Point east = new Point(currentTile.Position.X + 1, currentTile.Position.Y);
        Point west = new Point(currentTile.Position.X - 1, currentTile.Position.Y);
            
        var possibleTiles = new List<Tile>()
        {
            new Tile( south, currentTile, currentTile.Cost + 1 ),
            new Tile( north, currentTile, currentTile.Cost + 1 ),
            new Tile( west, currentTile, currentTile.Cost + 1 ),
            new Tile( east, currentTile, currentTile.Cost + 1 ),
        };

        possibleTiles.ForEach(tile => tile.SetDistance(targetTile.Position));

        return possibleTiles
            .Where(tile => tile.Position.X >= 0 && tile.Position.X <= Maze.Width)
            .Where(tile => tile.Position.Y >= 0 && tile.Position.Y <= Maze.Height)
            .Where(tile => Maze.Grid[tile.Position.Y][tile.Position.X].Type == TileType.Path || 
                           Maze.Grid[tile.Position.Y][tile.Position.X].Type == TileType.Exit)
            .ToList();
    }
    
    public Tile GetTileFromMaze(TileType type)
    {
        var position = (from tiles in Maze.Grid
                            from tile in tiles
                            where tile.Type == type
                            select tile.Position).FirstOrDefault();

        var newTile = new Tile(position, null, 0);
        newTile.Type = type;
        return newTile;
    }
    
    public bool Run(int maximumMoves)
    {
        var start = GetTileFromMaze(TileType.Start);
        var exit = GetTileFromMaze(TileType.Exit);
        start.SetDistance(exit.Position);
        var success = SolveMazePath(start, exit, maximumMoves);
        Graphics.Render(Maze);
        return success;
        
    }
    
}