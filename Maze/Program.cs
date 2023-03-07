namespace Maze;

internal class Program
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
        List<List<Tile>> mazeArray = FileParser.ReadFile(filePath);
        if (mazeArray == null)
        {
            return false;
        }
        var mazeSolver = new Solver(mazeArray);

        return mazeSolver.Run(500);
    }
}