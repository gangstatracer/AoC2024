using System;

namespace AoC2024.Day05;

public class Day05
{
    [TestCase("Day05/example.txt", 143)]
    [TestCase("Day05/input.txt", 4905)]
    public void Task1(string filePath, int expected)
    {
        var rules = new List<(int, int)>();
        var updates = new List<List<int>>();
        var lines = File.ReadAllLines(filePath).GetEnumerator();

        while (lines.MoveNext() && !string.IsNullOrEmpty((string)lines.Current))
        {
            var parsed = ((string)lines.Current).Split('|').Select(int.Parse).ToArray();
            rules.Add((parsed[0], parsed[1]));
        }

        while (lines.MoveNext())
        {
            updates.Add([.. ((string)lines.Current).Split(',').Select(int.Parse)]);
        }

        updates
        .Where(u => IsValid(rules, u))
        .Select(u => u[u.Count / 2])
        .Sum()
        .Should()
        .Be(expected);
    }

    private static bool IsValid(List<(int, int)> rules, List<int> updates)
    {
        var alreadyMet = new HashSet<int>();
        foreach (var update in updates)
        {
            var mustBeAfter = rules
                .Where(r => r.Item1 == update)
                .Select(r => r.Item2);
            if (mustBeAfter.Any(alreadyMet.Contains))
                return false;
            alreadyMet.Add(update);
        }
        return true;
    }

    [TestCase("Day05/example.txt", 123)]
    [TestCase("Day05/input.txt", 6204)]
    public void Task2(string filePath, int expected)
    {
        var rules = new List<(int, int)>();
        var updates = new List<List<int>>();
        var lines = File.ReadAllLines(filePath).GetEnumerator();

        while (lines.MoveNext() && !string.IsNullOrEmpty((string)lines.Current))
        {
            var parsed = ((string)lines.Current).Split('|').Select(int.Parse).ToArray();
            rules.Add((parsed[0], parsed[1]));
        }

        while (lines.MoveNext())
        {
            updates.Add([.. ((string)lines.Current).Split(',').Select(int.Parse)]);
        }

        updates
        .Where(u => !IsValid(rules, u))
        .Select(u => Sort(rules, u).ToArray())
        .Select(u => u[u.Length / 2])
        .Sum()
        .Should()
        .Be(expected);
    }

    private static IEnumerable<int> Sort(List<(int, int)> rules, List<int> u)
    {
        var updates = u.ToHashSet();
        rules = [.. rules.Where(r => updates.Contains(r.Item1) && updates.Contains(r.Item2))];
        while (rules.Count > 0)
        {
            var right = rules.Select(r => r.Item2).ToHashSet();
            var leftmost = rules.Select(r => r.Item1).First(r => !right.Contains(r));
            if (u.Contains(leftmost))
                yield return leftmost;
            rules = [.. rules.Where(r => r.Item1 != leftmost && r.Item2 != leftmost)];
        }
    }
}
