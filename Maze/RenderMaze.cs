namespace Maze;
using Raylib_cs;

public static class RenderMaze
{
    public static void Render(MazeTile[,] maze, List<MazeTile> path)
    {
        Raylib.InitWindow(800, 480, "Close window with ESC");

        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.WHITE);

            foreach (var tile in maze)
            {
                var color = Color.BLACK;
                if (tile.Type == ElementType.Start)
                {
                    color = Color.BLUE;
                }

                if (tile.Type == ElementType.Exit)
                {
                    color = Color.GREEN;
                }
                Raylib.DrawText(""+(char)tile.Type, (int)tile.Position.X*20, (int)tile.Position.Y*20, 20, color);
            }

            foreach (var tile in path)
            {
                Raylib.DrawText("X", (int)tile.Position.X*20, (int)tile.Position.Y*20, 20, Color.RED);
            }

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}