using System.Text.RegularExpressions;

namespace AoC2024.Day13;

public class Day13
{
    [TestCase("Day13/input.txt", 28059)]
    [TestCase("Day13/example.txt", 480)]
    public void Task1(string filePath, int expected)
    {
        var regex = new Regex(@"X.(\d+), Y.(\d+)");
        var lines = File.ReadAllLines(filePath);
        var machines = new List<(Coordinate A, Coordinate B, Coordinate Prize)>();
        for (var i = 0; i < lines.Length / 4 + 1; i++)
        {
            var a = regex.Match(lines[i * 4]);
            var b = regex.Match(lines[i * 4 + 1]);
            var prize = regex.Match(lines[i * 4 + 2]);
            machines.Add((
                new Coordinate(int.Parse(a.Groups[1].Value), int.Parse(a.Groups[2].Value)),
                new Coordinate(int.Parse(b.Groups[1].Value), int.Parse(b.Groups[2].Value)),
                new Coordinate(int.Parse(prize.Groups[1].Value), int.Parse(prize.Groups[2].Value))
            ));
        }

        var totalTokens = 0;
        foreach (var (a, b, prize) in machines)
        {
            var solutions = new List<int>();
            for (var i = 0; i <= 100; i++)
            for (var j = 0; j <= 100; j++)
                if (a.Multiply(i).Add(b.Multiply(j)) == prize)
                    solutions.Add(i * 3 + j);

            if (solutions.Count != 0)
                totalTokens += solutions.Min();
        }

        totalTokens.Should().Be(expected);
    }

    [TestCase("Day13/input.txt", 102255878088512L)]
    public void Task2(string filePath, long expected)
    {
        var regex = new Regex(@"X.(\d+), Y.(\d+)");
        var lines = File.ReadAllLines(filePath);
        var machines = new List<(Coordinate A, Coordinate B, (long X, long Y) Prize)>();
        for (var i = 0; i < lines.Length / 4 + 1; i++)
        {
            var a = regex.Match(lines[i * 4]);
            var b = regex.Match(lines[i * 4 + 1]);
            var prize = regex.Match(lines[i * 4 + 2]);
            machines.Add((
                new Coordinate(int.Parse(a.Groups[1].Value), int.Parse(a.Groups[2].Value)),
                new Coordinate(int.Parse(b.Groups[1].Value), int.Parse(b.Groups[2].Value)),
                (
                    10000000000000L + int.Parse(prize.Groups[1].Value),
                    10000000000000L + int.Parse(prize.Groups[2].Value))
            ));
        }

        var totalTokens = 0L;
        foreach (var (a, b, prize) in machines)
        {
            var m = a.Y * b.X - b.Y * a.X;
            if (m == 0)
                continue;
            var x = (prize.Y * b.X - prize.X * b.Y) / m;
            var y = (prize.X - a.X * x) / b.X;
            if (a.X * x + b.X * y == prize.X && a.Y * x + b.Y * y == prize.Y)
                totalTokens += x * 3 + y;
        }

        totalTokens.Should().Be(expected);
    }
}