using System.Drawing;
using System.Text.RegularExpressions;

namespace AoC2024.Day02;

public class Day04
{
    [Test]
    public void Task1()
    {
        var text = File
            .ReadAllLines("Day04/input.txt")
            .Select(l => l.ToCharArray())
            .ToArray();

        var sum = 0;
        for (var i = 0; i < text.Length; i++)
            for (var j = 0; j < text[i].Length; j++)
            {
                var index = new Point(i, j);
                for (var k = -1; k < 2; k++)
                    for (var l = -1; l < 2; l++)
                        sum += SearchWord(text, index, "XMAS", new Point(k, l)) ? 1 : 0;
            }

        sum.Should().Be(2573);
    }

    private static bool SearchWord(char[][] text, Point index, string query, Point direction)
    {
        for (var i = 0; i < query.Length; i++)
            if (!text.TryGetValue(index.X + direction.X * i, index.Y + direction.Y * i, out var value) || value != query[i])
                return false;
        return true;
    }

    [TestCase("Day04/input.txt", 1850)]
    [TestCase("Day04/example.txt", 9)]
    public void Task2(string filePath, int expectedResult)
    {
        var text = File
            .ReadAllLines(filePath)
            .Select(l => l.ToCharArray())
            .ToArray();

        var sum = 0;
        for (var i = 0; i < text.Length; i++)
            for (var j = 0; j < text[i].Length; j++)
            {
                var index = new Point(i, j);
                if (text[i][j] == 'A' && GetLinesSnowflake(text, index).Count(l => l == "MAS" || l == "SAM") > 1)
                    sum += 1;
            }

        sum.Should().Be(expectedResult);
    }

    private static IEnumerable<string> GetLinesSnowflake(char[][] text, Point index)
    {
        if (index.X == 0 || index.X == text.Length - 1)
            yield break;
        if (index.Y == 0 || index.Y == text[index.X].Length - 1)
            yield break;
        yield return new string([text[index.X - 1][index.Y - 1], text[index.X][index.Y], text[index.X + 1][index.Y + 1]]);
        yield return new string([text[index.X - 1][index.Y + 1], text[index.X][index.Y], text[index.X + 1][index.Y - 1]]);
    }
}
