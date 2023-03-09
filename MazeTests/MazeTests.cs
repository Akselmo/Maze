namespace MazeTests;

public class MazeTests
{
    public Maze.MazeObject MazeObject;
    private string WorkingMazeFilePath = "../../../../maze-task-first.txt";

    [SetUp]
    public void SetUp()
    {
        MazeObject = FileParser.ReadFile(WorkingMazeFilePath);
        Assert.Greater(MazeObject.Grid.Count, 0);
    }

    
    [Test]
    public void MazeLowMoves()
    {
        var solver = new Solver(MazeObject);
        Assert.False(solver.Run(1));
    }
    
    [Test]
    public void MazeEnoughMoves()
    {
        var solver = new Solver(MazeObject);
        Assert.True(solver.Run(200));
    }
    
    [Test]
    public void MazeBroken()
    {
        MazeObject mazeObject = new MazeObject();
        mazeObject.Grid = new List<List<Tile>>();
        var solver = new Solver(mazeObject);
        try
        {
            solver.Run(200);
            Assert.Fail();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            Assert.Pass();
        }
    }
    
    
}