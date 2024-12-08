using System.Drawing;

namespace AoC2024.Day06;

public class Day06
{
    [TestCase("Day06/input.txt", 5551)]
    [TestCase("Day06/example.txt", 41)]
    public void Task1(string filePath, int expected)
    {
        var map = File
            .ReadAllLines(filePath)
            .Select(l => l.ToCharArray())
            .ToArray();
        var visited = new bool[map.Length, map[0].Length];
        var guard = new Point(0, 0);
        var direction = new Point(-1, 0);

        for (var i = 0; i < map.Length; i++)
            for (var j = 0; j < map[i].Length; j++)
                if (map[i][j] == '^')
                {
                    guard = new Point(i, j);
                    break;
                }
        while (map.TryGetValue(guard, out var _))
        {
            visited[guard.X, guard.Y] = true;
            var nextPosition = guard.MoveInDirection(direction);
            if (!map.TryGetValue(nextPosition, out var nextValue))
                break;
            if (nextValue == '#')
            {
                direction = new Point(direction.Y, -1 * direction.X);
            }
            else
            {
                guard = nextPosition;
            }
        }

        visited
            .Cast<bool>()
            .Count(p => p)
            .Should()
            .Be(expected);
    }

    [TestCase("Day06/input.txt", 1939)]
    [TestCase("Day06/example.txt", 6)]
    public void Task2(string filePath, int expected)
    {
        var map = File
            .ReadAllLines(filePath)
            .Select(l => l.ToCharArray())
            .ToArray();
        var visited = new bool[map.Length, map[0].Length];
        var guard = new Point(0, 0);
        var direction = new Point(-1, 0);

        for (var i = 0; i < map.Length; i++)
            for (var j = 0; j < map[i].Length; j++)
                if (map[i][j] == '^')
                {
                    guard = new Point(i, j);
                    break;
                }

        var startingPoint = guard;

        while (map.TryGetValue(guard, out var value))
        {
            visited[guard.X, guard.Y] = true;
            var nextPosition = guard.MoveInDirection(direction);
            if (!map.TryGetValue(nextPosition, out var nextValue))
                break;
            if (nextValue == '#')
            {
                direction = new Point(direction.Y, -1 * direction.X);
            }
            else
            {
                guard = nextPosition;
            }
        }

        var result = 0;
        for (var i = 0; i < map.Length; i++)
            for (var j = 0; j < map[i].Length; j++)
                if (visited[i, j] && (startingPoint.X != i || startingPoint.Y != j))
                    result += HasLoop(map, startingPoint, new Point(i, j)) ? 1 : 0;


        result
            .Should()
            .Be(expected);
    }

    private static bool HasLoop(char[][] map, Point guard, Point additionalObstacle)
    {
        var direction = new Point(-1, 0);
        var visited = new HashSet<Point>[map.Length, map[0].Length];
        while (map.TryGetValue(guard, out _))
        {
            if (visited[guard.X, guard.Y] == null)
                visited[guard.X, guard.Y] = [];

            if (visited[guard.X, guard.Y].Contains(direction))
                return true;

            visited[guard.X, guard.Y].Add(direction);

            var nextPosition = guard.MoveInDirection(direction);
            if (!map.TryGetValue(nextPosition, out var nextValue))
                break;
            if (nextValue == '#' || nextPosition == additionalObstacle)
            {
                direction = new Point(direction.Y, -1 * direction.X);
            }
            else
            {
                guard = nextPosition;
            }
        }
        return false;
    }
}