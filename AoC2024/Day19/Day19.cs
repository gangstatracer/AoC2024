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

    [TestCase("Day19/example.txt", 16)]
    [TestCase("Day19/input.txt", 723524534506343L)]
    public void Task2(string filePath, long expected)
    {
        var lines = File.ReadAllLines(filePath).ToArray();
        var available = lines[0].Split(", ");
        var index = available
        .GroupBy(x => x[0])
        .ToDictionary(x => x.Key, x => x.ToHashSet());

        var desired = lines.Skip(2).ToArray();
        var possible = 0L;
        var cache = new Dictionary<string, long>();
        foreach (var d in desired)
        {
            possible += Search2(d, index, cache);
        }
        possible.Should().Be(expected);
    }

    private static long Search2(string rest, Dictionary<char, HashSet<string>> available, Dictionary<string, long> cache)
    {
        if (!available.TryGetValue(rest[0], out var towels))
            return 0;

        if (cache.TryGetValue(rest, out var result))
            return result;

        foreach (var towel in towels)
        {
            if (0 + towel.Length > rest.Length)
                continue;
            var v = rest.AsSpan(0, towel.Length);
            if (v.SequenceEqual(towel))
                if (0 + towel.Length == rest.Length)
                    result++;
                else
                    result += Search2(rest[towel.Length..], available, cache);
        }
        cache.Add(rest, result);
        return result;
    }

}
