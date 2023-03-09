namespace Maze;

// A simple class to hold all MazeSolver related items
public class MazeObject
{
    public List<List<Tile>> Grid = new List<List<Tile>>();
    public string Path = "";
    
    public int Width => Grid[0].Count - 1;
    public int Height => Grid.Count - 1;
    
}