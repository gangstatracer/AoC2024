namespace AoC2024.Day19;

public class Day19
{
    [TestCase("Day19/example.txt", 6)]
    [TestCase("Day19/input.txt", 263)]
    public void Task1(string filePath, int expected)
    {
        var lines = File.ReadAllLines(filePath).ToArray();
        var available = lines[0].Split(", ");
        var index = available
        .GroupBy(x => x[0])
        .ToDictionary(x => x.Key, x => x.ToHashSet());

        var desired = lines.Skip(2).ToArray();
        var possible = 0;
        foreach (var d in desired)
        {
            if (Search(d, 0, index))
                possible++;
        }
        possible.Should().Be(expected);
    }

    private static bool Search(string pattern, int index, Dictionary<char, HashSet<string>> available)
    {
        if (!available.TryGetValue(pattern[index], out var towels))
            return false;
        foreach (var towel in towels)
        {
            if (index + towel.Length > pattern.Length)
                continue;
            var v = pattern.AsSpan(index, towel.Length); 
            if (v.SequenceEqual(towel))
                if (index + towel.Length == pattern.Length 
                || Search(pattern, index + towel.Length, available))
                    return true;
        }
        return false;
    }
}
