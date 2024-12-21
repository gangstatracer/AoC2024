using System.ComponentModel;

namespace AoC2024.Day21;

public class Day21
{
    [TestCase("Day21/example.txt", 126384)]
    [TestCase("Day21/input.txt", 0)]
    public void Task1(string filePath, int expected)
    {
        var numericKeypad = new char[4, 3]{
            { '7', '8', '9' },
            { '4', '5', '6' },
            { '1', '2', '3' },
            { '#', '0', 'A' },
        };
        var numericPaths = numericKeypad
            .AllCoordinates()
            .ToDictionary(
                c => numericKeypad[c.X, c.Y],
                c => FindPaths(numericKeypad, c));

        var directionalKeypad = new char[2, 3]
        {
            { '#', '^', 'A' },
            { '<', 'v', '>' },
        };
        var directionalPaths = directionalKeypad
            .AllCoordinates()
            .ToDictionary(
                c => directionalKeypad[c.X, c.Y],
                c => FindPaths(directionalKeypad, c));

        var result = 0;
        var numericCurrent = 'A';
        var robot1 = 'A';
        var robot2 = 'A';
        foreach (var code in File.ReadAllLines(filePath))
        {
var numericPath = numericPaths[numericCurrent];
foreach(var )
        }
        result.Should().Be(expected);
    }

    private static Dictionary<char, List<Coordinate>> FindPaths(char[,] keypad, Coordinate start)
    {
        var q = new Queue<Coordinate>();
        var visited = new bool[keypad.GetLength(0), keypad.GetLength(1)];
        var distances = new int[keypad.GetLength(0), keypad.GetLength(1)];
        var predecessors = new Coordinate?[keypad.GetLength(0), keypad.GetLength(1)];

        visited.TrySetValue(start, true);
        q.Enqueue(start);
        while (q.TryDequeue(out var c))
        {
            foreach (var n in CoordinateExtensions.Directions.Select(c.MoveTo))
            {
                if (!keypad.TryGetValue(n, out var value) || value == '#')
                    continue;

                if (visited[n.X, n.Y])
                    continue;

                visited[n.X, n.Y] = true;
                q.Enqueue(n);
                distances[n.X, n.Y] = distances[c.X, c.Y] + 1;
                predecessors[n.X, n.Y] = c;
            }
        }

        IEnumerable<Coordinate> RestorePath(Coordinate end)
        {
            Coordinate? current = end;
            while (current != null)
            {
                yield return current;
                current = predecessors[current.X, current.Y];
            }
        }

        var result = new Dictionary<char, List<Coordinate>>();
        foreach (var c in keypad.AllCoordinates())
        {
            if (distances[c.X, c.Y] != 0)
            {
                result.Add(
                    keypad[c.X, c.Y],
                    RestorePath(c)
                    .Reverse()
                    .ToList());
            }
        }

        return result;
    }
}
