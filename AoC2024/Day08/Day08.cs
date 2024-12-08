using System.Drawing;

namespace AoC2024.Day08;

public class Day08
{
    [TestCase("Day08/input.txt", 247)]
    [TestCase("Day08/example.txt", 14)]
    public void Task1(string filePath, int expected)
    {
        var map = File
            .ReadAllLines(filePath)
            .Select(l => l.ToCharArray())
            .ToArray();
        var index = new Dictionary<char, List<Point>>();
        for (var i = 0; i < map.Length; i++)
            for (var j = 0; j < map[i].Length; j++)
            {
                char c = map[i][j];
                if (IsAntenna(c))
                {
                    if (!index.ContainsKey(c))
                        index[c] = [];
                    index[c].Add(new Point(i, j));
                }
            }

        var locations = new bool[map.Length, map[0].Length];

        foreach (var kvp in index)
            for (var i = 0; i < kvp.Value.Count; i++)
                for (var j = 0; j <= i; j++)
                    if (i != j)
                    {
                        var a = kvp.Value[i];
                        var b = kvp.Value[j];
                        var xDistance = b.X - a.X;
                        var yDistance = b.Y - a.Y;
                        var antinode1 = new Point(a.X - xDistance, a.Y - yDistance);
                        locations.TrySetValue(antinode1, true);
                        var antinode2 = new Point(b.X + xDistance, b.Y + yDistance);
                        locations.TrySetValue(antinode2, true);
                    }

        for (var i = 0; i < locations.GetLength(0); i++)
        {
            for (var j = 0; j < locations.GetLength(1); j++)
            {
                Console.Write(locations[i, j] ? '#' : '.');
            }
            Console.WriteLine();
        }

        locations
            .Cast<bool>()
            .Count(l => l)
            .Should()
            .Be(expected);
    }

    [TestCase("Day08/input.txt", 861)]
    [TestCase("Day08/example.txt", 34)]
    public void Task2(string filePath, int expected)
    {
        var map = File
            .ReadAllLines(filePath)
            .Select(l => l.ToCharArray())
            .ToArray();

        var index = new Dictionary<char, List<Point>>();
        for (var i = 0; i < map.Length; i++)
            for (var j = 0; j < map[i].Length; j++)
            {
                char c = map[i][j];
                if (IsAntenna(c))
                {
                    if (!index.ContainsKey(c))
                        index[c] = [];
                    index[c].Add(new Point(i, j));
                }
            }

        var locations = new bool[map.Length, map[0].Length];

        foreach (var kvp in index)
            for (var i = 0; i < kvp.Value.Count; i++)
                for (var j = 0; j <= i; j++)
                    if (i != j)
                    {
                        var a = kvp.Value[i];
                        var b = kvp.Value[j];

                        var xDistance = b.X - a.X;
                        var yDistance = b.Y - a.Y;
                        while (locations.TrySetValue(a, true))
                        {
                            a = new Point(a.X - xDistance, a.Y - yDistance);
                        }
                        while (locations.TrySetValue(b, true))
                        {
                            b = new Point(b.X + xDistance, b.Y + yDistance);
                        }
                    }

        for (var i = 0; i < locations.GetLength(0); i++)
        {
            for (var j = 0; j < locations.GetLength(1); j++)
            {
                Console.Write(locations[i, j] ? '#' : '.');
            }
            Console.WriteLine();
        }

        locations
            .Cast<bool>()
            .Count(l => l)
            .Should()
            .Be(expected);
    }

    private static bool IsAntenna(char c) => c is >= '0' and <= '9' or >= 'a' and <= 'z' or >= 'A' and <= 'Z';
}
