namespace Maze;
using Raylib_cs;

public static class RenderMaze
{
    public static void Render(List<MazeTile> maze)
    {
        Raylib.InitWindow(800, 480, "Close window with ESC");

        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.WHITE);

            foreach (var tile in maze)
            {
                var color = Color.BLACK;
                if (tile.Visited)
                {
                    color = Color.RED;
                }
                Raylib.DrawText(""+(char)tile.Type, (int)tile.Position.X*20, (int)tile.Position.Y*20, 20, color);
            }

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}