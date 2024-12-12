namespace AoC2024.Day12;

public class Day12
{
    [TestCase("Day12/input.txt", 1465112)]
    [TestCase("Day12/example.txt", 140)]
    public void Task1(string filePath, int expected)
    {
        var map = File
            .ReadAllLines(filePath)
            .Select(l => l.ToCharArray())
            .ToArray();
        var visited = new bool[map.Length, map[0].Length];
        var result = 0;
        for (var i = 0; i < map.Length; i++)
        for (var j = 0; j < map[i].Length; j++)
        {
            if (visited[i, j])
                continue;

            var (area, perimeter) = ExploreRegion(new Coordinate(i, j), map, visited);
            result += area * perimeter;
        }

        result.Should().Be(expected);
    }

    private static (int Area, int Perimeter) ExploreRegion(Coordinate coordinate, char[][] map, bool[,] visited)
    {
        visited[coordinate.X, coordinate.Y] = true;
        var area = 1;
        var perimeter = 0;

        foreach (var (xOffset, yOffset) in Directions)
        {
            var next = new Coordinate(X: coordinate.X + xOffset, Y: coordinate.Y + yOffset);

            if (map.TryGetValue(next, out var value)
                && value == map[coordinate.X][coordinate.Y])
            {
                if (visited.TryGetValue(next, out var alreadyVisited) && alreadyVisited)
                    continue;

                var (nextArea, nextPerimeter) = ExploreRegion(next, map, visited);
                area += nextArea;
                perimeter += nextPerimeter;
            }
            else
            {
                perimeter++;
            }
        }


        return (area, perimeter);
    }

    private static readonly (int, int)[] Directions =
    [
        (-1, 0),
        (0, 1),
        (1, 0),
        (0, -1),
    ];
}