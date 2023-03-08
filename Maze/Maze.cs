namespace Maze;

public class Maze
{
    public List<List<Tile>> Grid = null;
    
    public int Width => Grid[0].Count - 1;
    public int Height => Grid.Count - 1;
    
}