namespace AoC2024.Day10;

public class Day10
{
    [TestCase("Day10/input.txt", 820)]
    [TestCase("Day10/example.txt", 36)]
    public void Task1(string filePath, int expected)
    {
        var map = File
            .ReadAllLines(filePath)
            .Select(l => l.ToCharArray())
            .ToArray();

        var result = 0;
        for (var i = 0; i < map.Length; i++)
        for (var j = 0; j < map[i].Length; j++)
        {
            if (map[i][j] == '0')
            {
                var visited = new bool[map.Length, map[i].Length];
                Dfs(map, new Coordinate(-1, -1), new Coordinate(i, j), visited);
                for (var ii = 0; ii < visited.GetLength(0); ii++)
                for (var jj = 0; jj < visited.GetLength(1); jj++)
                    if (map[ii][jj] == '9' && visited[ii, jj])
                        result++;
            }
        }

        result.Should().Be(expected);
    }

    private static void Dfs(char[][] map, Coordinate previous, Coordinate current, bool[,] visited)
    {
        if (!map.TryGetValue(current, out var value))
            return;

        if (map.TryGetValue(previous, out var previousValue)
            && value - previousValue != 1)
            return;

        visited.TrySetValue(current, true);

        Dfs(map, current, current with { X = current.X - 1 }, visited);
        Dfs(map, current, current with { X = current.X + 1 }, visited);
        Dfs(map, current, current with { Y = current.Y - 1 }, visited);
        Dfs(map, current, current with { Y = current.Y + 1 }, visited);
    }

    [TestCase("Day10/input.txt", 1786)]
    [TestCase("Day10/example.txt", 81)]
    public void Task2(string filePath, int expected)
    {
        var map = File
            .ReadAllLines(filePath)
            .Select(l => l.ToCharArray())
            .ToArray();

        var result = 0;
        for (var i = 0; i < map.Length; i++)
        for (var j = 0; j < map[i].Length; j++)
        {
            if (map[i][j] == '0')
            {
                var visited = new int[map.Length, map[i].Length];
                Dfs2(map, new Coordinate(-1, -1), new Coordinate(i, j), visited);
                for (var ii = 0; ii < visited.GetLength(0); ii++)
                for (var jj = 0; jj < visited.GetLength(1); jj++)
                    if (map[ii][jj] == '9')
                        result += visited[ii, jj];
            }
        }

        result.Should().Be(expected);
    }

    private static void Dfs2(char[][] map, Coordinate previous, Coordinate current, int[,] visited)
    {
        if (!map.TryGetValue(current, out var value))
            return;

        if (map.TryGetValue(previous, out var previousValue)
            && value - previousValue != 1)
            return;

        visited.TrySetValue(current, visited.TryGetValue(current, out var count) ? count + 1 : 0);

        Dfs2(map, current, current with { X = current.X - 1 }, visited);
        Dfs2(map, current, current with { X = current.X + 1 }, visited);
        Dfs2(map, current, current with { Y = current.Y - 1 }, visited);
        Dfs2(map, current, current with { Y = current.Y + 1 }, visited);
    }
}