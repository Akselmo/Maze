namespace MazeTests;

public class Tests
{
    // Change this according your setup
    private string WorkingMazeFilePath = "../../../../maze-task-first.txt";
    private string BrokenMazeFilePath = "thisisbrokenfilepath.txt";

    [Test]
    public void WorkingFilePathTest()
    {
        var lines = FileParser.TryReadingLines(WorkingMazeFilePath);
        Assert.Greater(lines.Length, 0);
    }
    
    [Test]
    public void BrokenFilePathTest()
    {
        try
        {
            FileParser.TryReadingLines(BrokenMazeFilePath);
            Assert.Fail();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.ToString());
            Assert.Pass();
        }
    }
    
}