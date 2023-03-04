namespace Maze;

internal class Maze
{
    static void Main(string[] args)
    {
        foreach (var file in args)
        {
            if (!Run(file))
            {
                Console.WriteLine("Something went wrong with solving file: " + file);
            }
        }
    }

    public static bool Run(string filePath)
    {
        Console.WriteLine("Solving maze in file: " + filePath);
        var mazeList = FileParser.ReadFile(filePath);
        if (mazeList == null)
        {
            return false;
        }
        var mazeSolver = new MazeSolver(mazeList);

        return mazeSolver.Run(200);
    }
}