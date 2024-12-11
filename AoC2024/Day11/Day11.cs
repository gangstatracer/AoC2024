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
}