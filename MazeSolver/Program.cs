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
        
        MazeObject mazeObjectArray = FileParser.ReadFile(filePath);
        
        var mazeSolver = new Solver(mazeObjectArray);

        return mazeSolver.Run(200);
    }
}