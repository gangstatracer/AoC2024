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

    [TestCase("Day15/example2.txt", 9021)]
    [TestCase("Day15/example3.txt", 618)]
    [TestCase("Day15/input.txt", 1519991)]
    public void Task2(string filePath, int expected)
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
                    var line = new List<char>();
                    for (var j = 0; j < lines[i].Length; j++)
                    {
                        switch (lines[i][j])
                        {
                            case '#':
                                line.AddRange(['#', '#']);
                                break;
                            case '.':
                                line.AddRange(['.', '.']);
                                break;
                            case 'O':
                                line.AddRange(['[', ']']);
                                break;
                            case '@':
                                robot = new Coordinate(i, j * 2);
                                line.AddRange(['.', '.']);
                                break;
                            default:
                                throw new ArgumentOutOfRangeException();
                        }
                    }
                    map.Add([.. line]);
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

            if (MoveObject2(map, robot, direction, dryRun: false))
                robot = robot.MoveTo(direction);
        }

        var result = 0;
        for (var i = 0; i < map.Count; i++)
            for (var j = 0; j < map[i].Length; j++)
                if (map[i][j] == '[')
                    result += i * 100 + j;

        result.Should().Be(expected);
    }

    private static bool MoveObject2(List<char[]> map, Coordinate from, Coordinate direction, bool dryRun)
    {
        var froms = new List<Coordinate> { from };
        if (direction.X != 0)
        {
            if (map[from.X][from.Y] == '[')
            {
                froms.Add(@from with { Y = from.Y + 1 });
            }
            if (map[from.X][from.Y] == ']')
            {
                froms.Add(@from with { Y = from.Y - 1 });
            }
        }

        var canMove = true;
        foreach (var f in froms)
        {
            var destination = f.MoveTo(direction);
            var nextValue = map[destination.X][destination.Y];
            switch (nextValue)
            {
                case '[':
                case ']':
                    if (!MoveObject2(map, destination, direction, dryRun: true))
                        canMove = false;
                    break;
                case '.':
                    break;
                case '#':
                    canMove = false;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(nextValue));
            }
        }
        if (canMove)
        {
            if (!dryRun)
            {
                foreach (var f in froms)
                {
                    var destination = f.MoveTo(direction);
                    if (map[destination.X][destination.Y] == '[' || map[destination.X][destination.Y] == ']')
                        MoveObject2(map, destination, direction, dryRun: false);
                    map[destination.X][destination.Y] = map[f.X][f.Y];
                    map[f.X][f.Y] = '.';
                }
            }
            return true;
        }
        return false;
    }
}