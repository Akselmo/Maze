namespace Maze;
using Raylib_cs;

public static class Graphics
{
    public static void Render(List<List<Tile>> maze)
    {
        Raylib.InitWindow(800, 480, "Close window with ESC");

        while (!Raylib.WindowShouldClose())
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.WHITE);

            foreach (var line in maze)
            {
                foreach (var tile in line)
                {
                    var color = Color.BLACK;
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
                    Raylib.DrawText(""+(char)tile.Type, tile.Position.X*20, tile.Position.Y*20, 20, color);
                }
            }

            Raylib.EndDrawing();
        }

        Raylib.CloseWindow();
    }
}