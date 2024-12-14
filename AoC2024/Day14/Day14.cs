using System.Text;
using System.Text.RegularExpressions;

namespace AoC2024.Day14;

public class Day14
{
    [TestCase("Day14/example.txt", 11, 7, 12)]
    [TestCase("Day14/input.txt", 101, 103, 210587128)]
    public void Task1(string filePath, int maxX, int maxY, int expected)
    {
        var secondsCount = 100;
        var robots = File
        .ReadAllLines(filePath)
        .Select(l =>
        {
            var pv = l.Split(' ');
            var p = pv[0].Split('=', ',');
            var v = pv[1].Split('=', ',');
            return (
                Position: new Coordinate(int.Parse(p[1]), int.Parse(p[2])),
                Velocity: new Coordinate(int.Parse(v[1]), int.Parse(v[2])));
        })
        .Select(r =>
        {
            var distance = r.Velocity.Multiply(secondsCount);
            var position = r.Position.MoveTo(distance);
            var x = position.X % maxX;
            if (x < 0) x += maxX;
            var y = position.Y % maxY;
            if (y < 0) y += maxY;
            if (x == maxX / 2 || y == maxY / 2)
                return (int?)null;
            var quadrantX = x < maxX / 2 ? 0 : 1;
            var quadrantY = y < maxY / 2 ? 0 : 1;
            return quadrantX * 10 + quadrantY;
        })
        .Where(q => q != null)
        .GroupBy(q => q)
        .Aggregate(1, (a, q) => a * q.Count())
        .Should()
        .Be(expected);
    }

    [TestCase("Day14/input.txt", 101, 103, 7286)]
    public void Task2(string filePath, int maxX, int maxY, int expected)
    {
        var robots = File
        .ReadAllLines(filePath)
        .Select(l =>
        {
            var pv = l.Split(' ');
            var p = pv[0].Split('=', ',');
            var v = pv[1].Split('=', ',');
            return (
                Position: new Coordinate(int.Parse(p[1]), int.Parse(p[2])),
                Velocity: new Coordinate(int.Parse(v[1]), int.Parse(v[2])));
        })
        .ToArray();

        var secondsCount = expected;

        var map = new bool[maxX, maxY];
        foreach (var (p, v) in robots)
        {
            var (X, Y) = (v.X * secondsCount, v.Y * secondsCount);
            var position = (X: p.X + X, Y: p.Y + Y);
            var x = position.X % maxX;
            if (x < 0) x += maxX;
            var y = position.Y % maxY;
            if (y < 0) y += maxY;
            map[x, y] = true;
        }

        var sb = new StringBuilder();
        sb.AppendLine($"Seconds: {secondsCount}");
        sb.AppendLine();
        for (var j = 0; j < maxY; j++)
        {
            for (var i = 0; i < maxX; i++)
            {
                sb.Append(map[i, j] ? "*" : ".");
            }
            sb.AppendLine();
        }
        File.WriteAllText("output.txt", sb.ToString());
    }
}