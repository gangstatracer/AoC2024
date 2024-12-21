namespace AoC2024.Day20;

public class Day20
{
    [TestCase("Day20/example.txt", 40, 2)]
    [TestCase("Day20/input.txt", 100, 0)]
    public void Task1(string filePath, int minimumAdvantage, int expected)
    {
        var map = File.ReadAllLines(filePath).ToArray();

        var distances = new int[map.Length, map[0].Length];
        for (var i = 0; i < map.Length; i++)
            for (var j = 0; j < map[i].Length; j++)
                distances[i, j] = int.MaxValue;

        var visited = new bool[map.Length, map[0].Length];

        var packed = map.SelectMany((l, i) => l.Select((c, j) => (new Coordinate(i, j), c))).ToArray();
        var start = packed.Where(t => t.c == 'S').Single().Item1;
        var end = packed.Where(t => t.c == 'E').Single().Item1;

        var queue = new Queue<Coordinate>();
        queue.Enqueue(start);
        distances[start.X, start.Y] = 0;
        while (queue.TryDequeue(out var c))
        {
            foreach (var n in CoordinateExtensions.Directions.Select(d => c.MoveTo(d)))
            {
                if (!visited.TryGetValue(n, out var v) || v)
                    continue;

                if (map[n.X][n.Y] == '#')
                    continue;

                visited[n.X, n.Y] = true;
                distances[n.X, n.Y] = distances[c.X, c.Y] + 1;
                queue.Enqueue(n);
            }
        }

        var result = 0;
        for (var i = 0; i < map.Length; i++)
            for (var j = 0; j < map[i].Length; j++)
            {

                var c = new Coordinate(i, j);

                if (map[c.X][c.Y] == '#')
                    continue;

                var neighbours = CoordinateExtensions
                    .Directions
                    .Concat(CoordinateExtensions
                        .Diagonals)
                    .Concat(CoordinateExtensions
                        .Directions
                        .Select(d => d.Multiply(2)))
                    .Select(c.MoveTo);
                foreach (var n in neighbours)
                {
                    if (distances.TryGetValue(n, out var d))
                    {
                        var length = Math.Abs(c.X - n.X) + Math.Abs(c.Y - n.Y);
                        if (distances[c.X, c.Y] - d + length >= minimumAdvantage)
                            result++;
                    }
                }
            }

        result.Should().Be(expected);
    }
}
