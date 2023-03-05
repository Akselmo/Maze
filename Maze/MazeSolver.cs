using System.Numerics;

namespace Maze;

// TODO: astar pathfinder https://scribe.rip/a-star-a-search-for-solving-a-maze-using-python-with-visualization-b0cae1c3ba92

public class MazeSolver
{
    public MazeTile[,] MazeTiles;
    public List<MazeTile> SolvedMaze = new List<MazeTile>();
    public List<MazeTile> StartPoints = new List<MazeTile>();
    public List<MazeTile> ExitPoints = new List<MazeTile>();
    public int Moves = 0;
    public int Width = 0;
    public int Height = 0;

    public MazeSolver(MazeTile[,] mazeTiles)
    {
        MazeTiles = mazeTiles;
        Width = MazeTiles.GetLength(0)-1;
        Height = MazeTiles.GetLength(1)-1;
        foreach (var tile in MazeTiles)
        {
            switch (tile.Type)
            {
                case ElementType.Start:
                    StartPoints.Add(tile);
                    break;
                case ElementType.Exit:
                    ExitPoints.Add(tile);
                    break;
            }
        }
    }

    public bool SolveMazePath(MazeTile startTile, MazeTile endTile, int maximumMoves)
    {
        var moves = 0;
        // Add start tile to solved maze in the beginning
        SolvedMaze.Add(startTile);
        while (moves < maximumMoves)
        {
            

            var checkTile = SolvedMaze.OrderByDescending(t => t.CostDistance).Last();
            
            if (checkTile.Type == ElementType.Exit)
            {
                var tile = checkTile;
                Console.WriteLine("Retracing steps backwards...");
                while(true)
                {
                    Console.WriteLine($"{tile.Position.X} : {tile.Position.Y}");
                    if(MazeTiles[(int)tile.Position.X, (int)tile.Position.Y].Type == ElementType.Path)
                    {
                        MazeTiles[(int)tile.Position.X, (int)tile.Position.Y].Type = ElementType.Visited;
                    }
                    tile = tile.Parent;
                    if(tile == null)
                    {
                        Console.WriteLine("Map looks like :");
                        Console.WriteLine("Done!");
                        return true;
                    }
                }
            }
            
            var usableTiles = GetUsableTiles(checkTile, endTile);
            Console.WriteLine("Usable tiles " + usableTiles.Count);
            
            foreach (var usableTile in usableTiles)
            {
                if (usableTile.Visited)
                {
                    continue;
                }

                if (SolvedMaze.Any(t => (int)t.Position.X == (int)usableTile.Position.X &&
                                        (int)t.Position.Y == (int)usableTile.Position.Y))
                {
                    var existingTile = SolvedMaze.First(t => (int)t.Position.X == (int)usableTile.Position.X &&
                                                             (int)t.Position.Y == (int)usableTile.Position.Y);
                    
                    if (existingTile.CostDistance > checkTile.CostDistance)
                    {
                        usableTile.SetVisited();
                        SolvedMaze.Remove(existingTile);
                        SolvedMaze.Add(usableTile);
                    }
                }
                else
                {
                    usableTile.SetVisited();
                    SolvedMaze.Add(usableTile);
                }
            }
            moves++;
        }
        Console.WriteLine("Solving tile... Maze elements: " + SolvedMaze.Count);
        return false;
    }
    

    public List<MazeTile> GetUsableTiles(MazeTile currentTile, MazeTile targetTile)
    {
        Vector2 north = new Vector2(currentTile.Position.X, currentTile.Position.Y + 1);
        Vector2 south = new Vector2(currentTile.Position.X, currentTile.Position.Y - 1);
        Vector2 east = new Vector2(currentTile.Position.X + 1, currentTile.Position.Y);
        Vector2 west = new Vector2(currentTile.Position.X - 1, currentTile.Position.Y);

        var possibleTiles = new List<MazeTile>()
        {
            new MazeTile(north, ElementType.None, currentTile, currentTile.Cost + 1),
            new MazeTile(south, ElementType.None, currentTile, currentTile.Cost + 1),
            new MazeTile(east, ElementType.None, currentTile, currentTile.Cost + 1),
            new MazeTile(west, ElementType.None, currentTile, currentTile.Cost + 1)
        };
        
        possibleTiles.ForEach(tile => tile.SetDistance(targetTile.Position));

        return possibleTiles
            .Where(tile => (int)tile.Position.X >= 0 && (int)tile.Position.X <= Width)
            .Where(tile => (int)tile.Position.Y >= 0 && (int)tile.Position.Y <= Height)
            .Where(tile => MazeTiles[(int)tile.Position.X, (int)tile.Position.Y].Type == ElementType.Path ||
                           MazeTiles[(int)tile.Position.X, (int)tile.Position.Y].Type == ElementType.Exit)
            .ToList();
    }

    public bool Run(int maximumMoves)
    {
        foreach (var start in StartPoints)
        {
            foreach (var exit in ExitPoints)
            {
                start.SetDistance(exit.Position);
                var success = SolveMazePath(start, exit, 5000);
                RenderMaze.Render(MazeTiles, SolvedMaze);
                return success;
            }
        }

        return false;
    }
    
}