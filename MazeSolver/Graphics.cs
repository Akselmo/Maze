namespace Maze;
using Raylib_cs;

// Draw graphics using Raylib_cs
public static class Graphics
{
    private static int WindowWidth = 800;
    private static int WindowHeight = 600;

    private static Color BackgroundColor = Color.BLACK;
    private static Color TextColor = Color.WHITE;
    
    public static void Render(MazeObject mazeObject, int totalMoves, int maxMoves)
    {
        Raylib.InitWindow(WindowWidth, WindowHeight, "MazeSolver");
        
        int DrawingStartX = WindowWidth/mazeObject.Width; 
        int DrawingStartY = 10;
        
        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(BackgroundColor);
            
            // Draw the maze
            foreach (var line in mazeObject.Grid)
            {
                foreach (var tile in line)
                {
                    Point tilePosition = new Point(
                        DrawingStartX + tile.Position.X * 20, 
                        DrawingStartY + tile.Position.Y * 20
                        );
                    
                    Raylib.DrawText(""+(char)tile.Type, tilePosition.X, tilePosition.Y, 20, GetColorByType(tile.Type));
                }
            }
            
            // Draw UI text
            Raylib.DrawText("MazeSolver: " + Path.GetFileName(mazeObject.Path), DrawingStartX, Raylib.GetScreenHeight() - 100, 20, TextColor);
            Raylib.DrawText("Moves: " + totalMoves + "/" + maxMoves, DrawingStartX, Raylib.GetScreenHeight() - 50, 20, TextColor);
            if (!mazeObject.Solved)
            {
                Raylib.DrawText("Failed to solve the maze!", DrawingStartX, Raylib.GetScreenHeight() - 150, 20, Color.RED);
            }
            else
            {
                Raylib.DrawText("Maze solved!", DrawingStartX, Raylib.GetScreenHeight() - 150, 20, Color.GREEN);
            }
            
            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
    static Color GetColorByType(TileType type)
    {
        switch (type)
        {
            case TileType.Start:
                return Color.BLUE;
            case TileType.Exit:
                return Color.GREEN;
            case TileType.Visited:
                return Color.RED;
            default:
                return Color.GRAY;
        }
    }
}