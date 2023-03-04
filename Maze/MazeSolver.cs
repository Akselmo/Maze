using System.Numerics;

namespace Maze;

// TODO: astar pathfinder https://scribe.rip/a-star-a-search-for-solving-a-maze-using-python-with-visualization-b0cae1c3ba92

public class MazeSolver
{
    public List<MazeTile> MazeTiles;
    public List<MazeTile> StartPoints = new List<MazeTile>();
    public List<MazeTile> ExitPoints = new List<MazeTile>();
    public int Moves = 0;
    public MazeSolver(List<MazeTile> mazeTiles)
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

    public bool Run(int maximumMoves)
    {
        foreach (var startPoint in StartPoints)
        {
            Solve(startPoint);
        }

        RenderMaze.Render(MazeTiles);
        return true;
    }

    public void Solve(MazeTile startPosition)
    {
        MazeTile currentTile = startPosition;
        foreach (var exit in ExitPoints)
        {
            while (!exit.Visited)
            {
                if (currentTile.Position == exit.Position)
                {
                    exit.Visited = true;
                    break;
                }
            }

        }
    }

    MazeTile? GetTile(Vector2 position)
    {
        foreach (var tile in MazeTiles)
        {
            if (tile.Position == position)
            {
                return tile;
            }
        }
        return null;
    }


}