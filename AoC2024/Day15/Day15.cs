namespace AoC2024.Day15;

public class Day15
{
    [TestCase("Day15/example.txt", 2028)]
    [TestCase("Day15/input.txt", 1511865)]
    public void Task1(string filePath, int expected)
    {
        var lines = File.ReadAllLines(filePath);
        var map = new List<char[]>();
        var instructions = new List<char>();
        var readingMap = true;
        var robot = new Coordinate(0, 0);
        for (var i = 0; i < lines.Length; i++)
        {
            if (string.IsNullOrEmpty(lines[i]))
            {
                readingMap = false;
            }
            else
            {
                if (readingMap)
                {
                    var line = lines[i].ToCharArray();
                    var atIndex = lines[i].IndexOf('@');
                    if (atIndex >= 0)
                    {
                        robot = new Coordinate(i, atIndex);
                        line[atIndex] = '.';
                    }
                    map.Add(line);
                }
                else
                {
                    instructions.AddRange(lines[i].ToCharArray());
                }
            }

        }

        foreach (var instruction in instructions)
        {
            var direction = instruction switch
            {
                '^' => new Coordinate(-1, 0),
                '>' => new Coordinate(0, 1),
                'v' => new Coordinate(1, 0),
                '<' => new Coordinate(0, -1),
                _ => throw new ArgumentOutOfRangeException(nameof(instruction))
            };

            if (MoveObject(map, robot, '.', direction))
                robot = robot.MoveTo(direction);
        }

        var result = 0;
        for (var i = 0; i < map.Count; i++)
            for (var j = 0; j < map[i].Length; j++)
                if (map[i][j] == 'O')
                    result += i * 100 + j;

        result.Should().Be(expected);
    }

    private static bool MoveObject(List<char[]> map, Coordinate from, char fromValue, Coordinate direction)
    {
        var nextCoordinate = from.MoveTo(direction);
        var nextValue = map[nextCoordinate.X][nextCoordinate.Y];
        switch (nextValue)
        {
            case '.':
                map[nextCoordinate.X][nextCoordinate.Y] = fromValue;
                return true;
            case 'O':
                if (MoveObject(map, nextCoordinate, 'O', direction))
                {
                    map[nextCoordinate.X][nextCoordinate.Y] = fromValue;
                    return true;
                }
                else
                {
                    return false;
                }
            case '#':
                return false;
            default:
                throw new ArgumentOutOfRangeException(nameof(nextValue));
        }
    }
}