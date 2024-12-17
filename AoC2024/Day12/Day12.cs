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

        foreach (var (xOffset, yOffset) in CoordinateExtensions.Directions)
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

    [TestCase("Day12/input.txt", 893790)]
    [TestCase("Day12/example.txt", 80)]
    [TestCase("Day12/example2.txt", 436)]
    [TestCase("Day12/example3.txt", 236)]
    [TestCase("Day12/example4.txt", 368)]
    public void Task2(string filePath, int expected)
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

            var (area, sidesCount) = ExploreRegion2(new Coordinate(i, j), map, visited);
            result += area * sidesCount;
        }

        result.Should().Be(expected);
    }

    private static (int Area, int SidesCount) ExploreRegion2(Coordinate coordinate, char[][] map, bool[,] visited)
    {
        visited[coordinate.X, coordinate.Y] = true;
        var area = 1;
        var outbreaks = 0;
        foreach (var direction in CoordinateExtensions.Directions)
        {
            var next = coordinate.MoveTo(direction);

            if (map.TryGetValue(next, out var value)
                && value == map[coordinate.X][coordinate.Y])
            {
                if (!visited.TryGetValue(next, out var alreadyVisited) || !alreadyVisited)
                {
                    var (nextArea, outbreaksCount) = ExploreRegion2(next, map, visited);
                    area += nextArea;
                    outbreaks += outbreaksCount;
                }
            }

            var previousDirection = direction.PreviousDirection();
            var previous = coordinate.MoveTo(previousDirection);
            var diagonalDirection = new Coordinate(
                direction.X + previousDirection.X,
                direction.Y + previousDirection.Y);
            var diagonal = coordinate.MoveTo(diagonalDirection);

            var diagonalValue = map.TryGetValue(diagonal, out var temp) ? (char?)temp : null;
            var nextValue = map.TryGetValue(next, out temp) ? (char?)temp : null;
            var previousValue = map.TryGetValue(previous, out temp) ? (char?)temp : null;
            var currentValue = map[coordinate.X][coordinate.Y];

            if (diagonalValue != currentValue && nextValue == currentValue && previousValue == currentValue)
                outbreaks++;

            if (nextValue != currentValue && previousValue != currentValue)
                outbreaks++;
        }

        return (area, outbreaks);
    }
}