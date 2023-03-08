namespace Maze;
using Raylib_cs;

public static class Graphics
{
    private static int WindowWidth = 800;
    private static int WindowHeight = 600;
    
    public static void Render(Maze maze, int totalMoves, int maxMoves)
    {
        Raylib.InitWindow(WindowWidth, WindowHeight, "Maze Solver");
        
        int DrawingStartX = WindowWidth/maze.Width; 
        int DrawingStartY = 10;
        
        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.BLACK);

            foreach (var line in maze.Grid)
            {
                foreach (var tile in line)
                {
                    var color = Color.GRAY;
                    if (tile.Type == TileType.Start)
                    {
                        color = Color.BLUE;
                    }

                    if (tile.Type == TileType.Exit)
                    {
                        color = Color.GREEN;
                    }

                    if (tile.Type == TileType.Visited)
                    {
                        color = Color.RED;
                    }
                    Raylib.DrawText(""+(char)tile.Type, DrawingStartX+tile.Position.X*20, DrawingStartY+tile.Position.Y*20, 20, color);
                }
            }
            
            Raylib.DrawText("Maze: " + Path.GetFileName(maze.Path), DrawingStartX, Raylib.GetScreenHeight() - 100, 20, Color.WHITE);
            Raylib.DrawText("Moves: " + totalMoves + "/" + maxMoves, DrawingStartX, Raylib.GetScreenHeight() - 50, 20, Color.WHITE);
            
            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}