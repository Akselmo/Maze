using System.Numerics;

namespace Maze;

// TODO: astar pathfinder https://scribe.rip/a-star-a-search-for-solving-a-maze-using-python-with-visualization-b0cae1c3ba92

public class MazeSolver
{
    public MazeTile[,] MazeTiles;
    public Queue<MazeTile> MazeQueue = new Queue<MazeTile>();
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

    public MazeTile GetPathBFS(MazeTile mazeTile)
    {
        MazeQueue.Enqueue(mazeTile);
        while (MazeQueue.Count > 0)
        {
            MazeTile tile = MazeQueue.Dequeue();

            if (tile.Type == ElementType.Exit)
            {
                Console.WriteLine("Exit reached");
                return tile;
            }

            QueueNextTile(tile, new Vector2(tile.Position.X + 1, tile.Position.Y));
            QueueNextTile(tile, new Vector2(tile.Position.X - 1, tile.Position.Y));
            QueueNextTile(tile, new Vector2(tile.Position.X, tile.Position.Y + 1));
            QueueNextTile(tile, new Vector2(tile.Position.X, tile.Position.Y - 1));
            Console.WriteLine("Solving tile... Queue elements: " + MazeQueue.Count);
        }

        return null;
    }

    public void QueueNextTile(MazeTile currentTile, Vector2 nextPosition)
    {
        if (CanUseTile(nextPosition))
        {
            currentTile.SetVisited();
            MazeTile nextTile = MazeTiles[(int)nextPosition.X, (int)nextPosition.Y];
            nextTile.Parent = currentTile;
            MazeQueue.Enqueue(nextTile);
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
            var tile = GetPathBFS(start);
            RenderMaze.Render(MazeTiles);
        }
        return true;
    }
    
}