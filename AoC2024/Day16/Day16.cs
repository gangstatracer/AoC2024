
using NUnit.Framework.Interfaces;

namespace AoC2024.Day16;

public class Day16
{
    private const int infinity = int.MaxValue;

    [TestCase("Day16/example.txt", 7036)]
    [TestCase("Day16/input.txt", 98520)]
    public void Task1(string filePath, int expected)
    {
        var map = File.ReadAllLines(filePath).Select(l => l.ToCharArray()).ToArray();
        var start = new Coordinate(map.Length - 2, 1);

        var visited = new int[map.Length, map[0].Length];
        for (var i = 0; i < visited.GetLength(0); i++)
            for (var j = 0; j < visited.GetLength(1); j++)
                visited[i, j] = infinity;

        Search(map, visited, start, new Coordinate(0, 1), 0);
        var result = visited[1, visited.GetLength(1) - 2];
        result.Should().Be(expected);
    }

    private static void Search(char[][] map, int[,] visited, Coordinate c, Coordinate direction, int score)
    {
        if (!map.TryGetValue(c, out var mapValue) || mapValue == '#')
            return;

        if (visited.TryGetValue(c, out var oldScore) && oldScore < score)
            return;

        if (oldScore > score)
            visited[c.X, c.Y] = score;

        if (mapValue == 'E')
            return;

        Search(map, visited, c.MoveTo(direction), direction, score + 1);
        var left = new Coordinate(-1 * direction.Y, direction.X);
        Search(map, visited, c.MoveTo(left), left, score + 1001);
        var right = new Coordinate(direction.Y, -1 * direction.X);
        Search(map, visited, c.MoveTo(right), right, score + 1001);
    }

    [TestCase("Day16/example.txt", 45)]
    [TestCase("Day16/input.txt", 98520)]
    public void Task2(string filePath, int expected)
    {
        var map = File.ReadAllLines(filePath).Select(l => l.ToCharArray()).ToArray();
        var start = new Coordinate(map.Length - 2, 1);

        var visited = new int[map.Length, map[0].Length];
        for (var i = 0; i < visited.GetLength(0); i++)
            for (var j = 0; j < visited.GetLength(1); j++)
                visited[i, j] = infinity;
        Search(map, visited, start, new Coordinate(0, 1), 0);

        var pathsMap = new bool[map.Length, map[0].Length];
        visited[map.Length - 2, 0] = 0;
        Restore(pathsMap, visited, new Coordinate(map.Length - 2, 0), new Coordinate(0, 1), 0);
        var result = 0;
        for (var i = 0; i < pathsMap.GetLength(0); i++)
            for (var j = 0; j < pathsMap.GetLength(1); j++)
                if (pathsMap[i, j])
                    result++;

        pathsMap.Print(TestContext.Out);

        result.Should().Be(expected);
    }

    private static void Restore(bool[,] pathsMap, int[,] visited, Coordinate previous, Coordinate direction, int cost)
    {
        var c = previous.MoveTo(direction);
        if (visited[c.X, c.Y] - visited[previous.X, previous.Y] != cost)
            return;
        pathsMap[c.X, c.Y] = true;
        Restore(pathsMap, visited, c, direction, 1);
        Restore(pathsMap, visited, c, new Coordinate(-1 * direction.Y, direction.X), 1001);
        Restore(pathsMap, visited, c, new Coordinate(direction.Y, -1 * direction.X), 1001);
    }

    private static bool SearchWithBudget(char[][] map, bool[,] pathsMap, Coordinate previous, Coordinate direction, int score, int budget)
    {
        if (score > budget)
            return false;

        var c = previous.MoveTo(direction);
        if (!map.TryGetValue(c, out var mapValue) || mapValue == '#')
            return false;

        var result = false;
        if (mapValue == 'S')
        {
            if (score <= budget)
                result = true;
        }
        else
        {
            var forward = SearchWithBudget(map, pathsMap, c, direction, score + 1, budget);
            var left = SearchWithBudget(map, pathsMap, c, new Coordinate(-1 * direction.Y, direction.X), score + 1001, budget);
            var right = SearchWithBudget(map, pathsMap, c, new Coordinate(direction.Y, -1 * direction.X), score + 1001, budget);
            if (forward || left || right)
                result = true;
        }
        if (result)
            pathsMap[c.X, c.Y] = true;
        return result;
    }
}