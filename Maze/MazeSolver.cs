using System.Numerics;

namespace Maze;

// TODO: astar pathfinder https://scribe.rip/a-star-a-search-for-solving-a-maze-using-python-with-visualization-b0cae1c3ba92

public class MazeSolver
{
    public MazeTile[,] MazeTiles;
    public Stack<MazeTile> MazeQueue = new Stack<MazeTile>();
    public List<MazeTile> StartPoints = new List<MazeTile>();
    public List<MazeTile> ExitPoints = new List<MazeTile>();
    public int Moves = 0;

    public MazeSolver(MazeTile[,] mazeTiles)
    {
        MazeTiles = mazeTiles;
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

    public bool GetPathDFS(MazeTile mazeTile, int maximumMoves)
    {
        MazeQueue.Push(mazeTile);
        var moves = 0;
        while (MazeQueue.Count > 0 && moves < maximumMoves)
        {
            moves++;
            MazeTile tile = MazeQueue.Pop();

            if (tile.Type == ElementType.Exit)
            {
                Console.WriteLine("Exit reached");
                return true;
            }

            QueueNextTile(tile, new Vector2(tile.Position.X + 1, tile.Position.Y));
            QueueNextTile(tile, new Vector2(tile.Position.X - 1, tile.Position.Y));
            QueueNextTile(tile, new Vector2(tile.Position.X, tile.Position.Y + 1));
            QueueNextTile(tile, new Vector2(tile.Position.X, tile.Position.Y - 1));
            Console.WriteLine("Solving tile... Queue elements: " + MazeQueue.Count);
            
        }

        return false;
    }

    public void QueueNextTile(MazeTile currentTile, Vector2 nextPosition)
    {
        if (CanUseTile(nextPosition))
        {
            currentTile.SetVisited();
            MazeTile nextTile = MazeTiles[(int)nextPosition.X, (int)nextPosition.Y];
            nextTile.Parent = currentTile;
            MazeQueue.Push(nextTile);
        }
    }
    
    public bool CanUseTile(Vector2 position)
    {
        int x = (int)position.X;
        int y = (int)position.Y;
        
        if(x > 0 && x < MazeTiles.GetLength(0) && 
           y > 0 && y < MazeTiles.GetLength(1) && 
           (MazeTiles[x, y].Type == ElementType.Path || 
            MazeTiles[x, y].Type == ElementType.Exit)
          )
        {
            return true;
        }
        return false;
    }
    
    public bool Run(int maximumMoves)
    {
        foreach (var start in StartPoints)
        {
            var success = GetPathDFS(start, maximumMoves);
            RenderMaze.Render(MazeTiles);
            return success;
        }

        return false;
    }
    
}