using System.Text.RegularExpressions;

namespace AoC2024.Day03;

public class Day03
{
    [Test]
    public void Task1()
    {
        var regex = new Regex(@"mul\((\d{1,3}),(\d{1,3})\)");
        var text = File.ReadAllText("Day03/input.txt");
        regex
        .Matches(text)
        .Aggregate(0, (sum, match) => sum + int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value))
        .Should()
        .Be(178794710);
    }

    [Test]
    public void Task2()
    {
        var regex = new Regex(@"(mul)\((\d{1,3}),(\d{1,3})\)|(do)\(\)|(don't)\(\)");
        var text = File.ReadAllText("Day03/input.txt");
        var sum = 0;
        var doMultiplication = true;
        foreach (var match in regex.Matches(text).Cast<Match>())
        {
            switch (match.Value.Split('(').First())
            {
                case "mul":
                    if (doMultiplication)
                        sum += int.Parse(match.Groups[2].Value) * int.Parse(match.Groups[3].Value);
                    continue;
                case "do":
                    doMultiplication = true;
                    continue;
                case "don't":
                    doMultiplication = false;
                    continue;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        sum
        .Should()
        .Be(76729637);
    }

}
