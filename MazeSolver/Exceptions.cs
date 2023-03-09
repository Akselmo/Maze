namespace Maze;

[Serializable]
public class FileIsEmptyException : Exception
{
    public FileIsEmptyException() { }

    public FileIsEmptyException(string message) : base(message)
    {
        Console.WriteLine("File " + message + " is empty!");
    }
    
}

[Serializable]
public class NoTileTypeFound : Exception
{
    public NoTileTypeFound() { }

    public NoTileTypeFound(string tileType) : base(tileType)
    {
        Console.WriteLine("Could not find tile type: " + tileType);
    }
    
}