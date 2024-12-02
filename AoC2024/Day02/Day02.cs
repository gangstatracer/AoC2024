using System;

namespace AoC2024.Day02;

public class Day02
{
    [Test]
    public void Task1()
    {
        var lines = File.ReadLines("Day02/input.txt");
        lines.Count(IsSafe1).Should().Be(314);
    }

    private bool IsSafe1(string line)
    {
        var reports = line
        .Split(" ")
        .Select(int.Parse)
        .ToArray();

        var direction = Math.Sign(reports[1] - reports[0]);

        if (direction == 0)
            return false;

        for (var i = 1; i < reports.Length; i++)
        {
            var distance = Math.Abs(reports[i] - reports[i - 1]);

            if (distance < 1 || distance > 3)
                return false;

            if (Math.Sign(reports[i] - reports[i - 1]) != direction)
                return false;
        }
        return true;
    }

    [Test]
    public void Task2()
    {
        var lines = File.ReadLines("Day02/input.txt");
        lines.Count(l =>
        {
            var reports = l
                .Split(" ")
                .Select(int.Parse)
                .ToArray();

            if (IsSafe2(reports, null))
                return true;
            else
                return Enumerable.Range(0, reports.Length).Any(i => IsSafe2(reports, i));

        })
        .Should()
        .Be(373);
    }
    private bool IsSafe2(int[] line, int? removeIndex)
    {
        var reports = line.ToList();

        if (removeIndex != null)
            reports.RemoveAt(removeIndex.Value);


        var direction = Math.Sign(reports[1] - reports[0]);

        if (direction == 0)
            return false;

        for (var i = 1; i < reports.Count; i++)
        {
            var distance = Math.Abs(reports[i] - reports[i - 1]);

            if (distance < 1 || distance > 3)
                return false;

            if (Math.Sign(reports[i] - reports[i - 1]) != direction)
                return false;
        }
        return true;
    }
}
