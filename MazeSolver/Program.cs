namespace Maze;

internal class Program
{
    static void Main(string[] args)
    {
        foreach (var file in args)
        {
            TrySolveMaze(file, 20);
            TrySolveMaze(file, 150);
            TrySolveMaze(file, 200);
        }
    }

    static void TrySolveMaze(string filePath, int maximumMoves)
    {
        if (!RunSolver(filePath, maximumMoves))
        {
            Console.WriteLine("Failed to solve file: " + filePath + " with" + maximumMoves +" moves.");
        }
        else
        {
            Console.WriteLine("Solved file: " + filePath + " with" + maximumMoves +" moves!");
        }
    }

    static bool RunSolver(string filePath, int maximumMoves)
    {
        Console.WriteLine("Solving maze in file: " + filePath);
        
        MazeObject mazeObjectArray = FileParser.ReadFile(filePath);
        
        var mazeSolver = new Solver(mazeObjectArray);

        return mazeSolver.Run(maximumMoves);
    }
}