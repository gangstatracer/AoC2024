using System.Runtime.CompilerServices;

namespace AoC2024.Day07;

public class Day07
{
    [TestCase("Day07/input.txt", 14711933466277)]
    [TestCase("Day07/example.txt", 3749)]
    public void Task1(string filePath, long expected)
    {
        File
            .ReadAllLines(filePath)
            .Select(Parse)
            .Where(t => IsPossible(t.Result, t.Numbers))
            .Sum(t => t.Result)
            .Should()
            .Be(expected);
    }

    private static (long Result, int[] Numbers) Parse(string line)
    {
        var parts = line.Split(": ");
        return (long.Parse(parts[0]), parts[1].Split(' ').Select(int.Parse).ToArray());
    }

    private static bool IsPossible(long expectedResult, int[] numbers)
    {
        int maskLength = numbers.Length - 1;

        for (var i = 0; i < Math.Pow(2, maskLength); i++)
        {
            long result = numbers[0];
            for (var j = 0; j < maskLength; j++)
            {
                Func<long, int, long> @operator = (i & 1 << (maskLength - j - 1)) == 0
                    ? (x, y) => x + y
                    : (x, y) => x * y;

                result = @operator(result, numbers[j + 1]);

                if (result > expectedResult)
                    break;
            }

            if (result == expectedResult)
                return true;
        }

        return false;
    }

    [TestCase("Day07/input.txt", 286580387663654L)]
    [TestCase("Day07/example.txt", 11387)]
    public void Task2(string filePath, long expected)
    {
        File
            .ReadAllLines(filePath)
            .Select(Parse)
            .Where(t => IsPossible2(t.Result, t.Numbers))
            .Sum(t => t.Result)
            .Should()
            .Be(expected);
    }

    private static bool IsPossible2(long expectedResult, int[] numbers)
    {
        int maskLength = numbers.Length - 1;
        Func<long, int, long>[] operators =
        [
            (x, y) => x + y,
            (x, y) => x * y,
            (x, y) => x * (int)Math.Pow(10, y.ToString().Length) + y,
        ];

        for (var i = 0; i < Math.Pow(3, maskLength); i++)
        {
            long result = numbers[0];
            for (var j = 0; j < maskLength; j++)
            {
                var @operator = operators[i / (int)Math.Pow(3, maskLength - j - 1) % 3];

                result = @operator(result, numbers[j + 1]);

                if (result > expectedResult)
                    break;
            }

            if (result == expectedResult)
                return true;
        }

        return false;
    }
}
