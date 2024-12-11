namespace AoC2024.Day11;

public class Day11
{
    [TestCase("Day11/input.txt", 25, 198089)]
    [TestCase("Day11/example.txt", 25, 55312)]
    public void Task1(string filePath, int blinkCount, int expected)
    {
        var stones = File
            .ReadAllText(filePath)
            .Split(" ")
            .Select(long.Parse)
            .ToList();

        var result = 0L;

        foreach (var stone in stones)
        {
            var ancestors = new List<long> { stone };
            for (var blink = 0; blink < blinkCount; blink++)
            {
                var initialCount = ancestors.Count;
                for (var j = 0; j < initialCount; j++)
                {
                    if (ancestors[j] == 0)
                    {
                        ancestors[j] = 1;
                        continue;
                    }

                    var stringRepresentation = ancestors[j].ToString();
                    if (stringRepresentation.Length % 2 == 0)
                    {
                        var left = int.Parse(stringRepresentation[..(stringRepresentation.Length / 2)]);
                        ancestors[j] = left;
                        var right = int.Parse(stringRepresentation[(stringRepresentation.Length / 2)..]);
                        ancestors.Add(right);
                        continue;
                    }

                    ancestors[j] *= 2024;
                }
            }

            result += ancestors.Count;
        }

        result.Should().Be(expected);
    }

    [TestCase("Day11/input.txt", 75, 236302670835517L)]
    [TestCase("Day11/example.txt", 25, 55312)]
    public void Task2(string filePath, int blinkCount, long expected)
    {
        var stones = File
            .ReadAllText(filePath)
            .Split(" ")
            .Select(long.Parse)
            .ToList();

        var result = 0L;

        foreach (var stone in stones)
        {
            result += CountStones(stone, blinkCount);
        }

        result.Should().Be(expected);
    }

    private static long CountStones(long stone, int blinkCount)
    {
        if (resultsCache.TryGetValue((stone, blinkCount), out var result))
            return result;

        if (blinkCount == 0)
            return 1;

        if (stone == 0)
        {
            result = CountStones(1, blinkCount - 1);
        }
        else
        {
            var stringRepresentation = stone.ToString();
            if (stringRepresentation.Length % 2 == 0)
            {
                var left = int.Parse(stringRepresentation[..(stringRepresentation.Length / 2)]);
                var right = int.Parse(stringRepresentation[(stringRepresentation.Length / 2)..]);
                result = CountStones(left, blinkCount - 1) + CountStones(right, blinkCount - 1);
            }
            else
            {
                result = CountStones(stone * 2024, blinkCount - 1);
            }
        }
        resultsCache.Add((stone, blinkCount), result);
        return result;
    }

    private static Dictionary<(long, int), long> resultsCache = [];
}