namespace Maze;

internal class Program
{
    static void Main(string[] args)
    {
        foreach (var file in args)
        {
            if (!Run(file))
            {
                Console.WriteLine("Failed to solve file: " + file);
            }
        }
    }

    static bool Run(string filePath)
    {
        Console.WriteLine("Solving maze in file: " + filePath);
        
        Maze mazeArray = FileParser.ReadFile(filePath);
        
        var mazeSolver = new Solver(mazeArray);

        return mazeSolver.Run(200);
    }
}