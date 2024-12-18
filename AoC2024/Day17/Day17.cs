using System.Diagnostics;

namespace AoC2024.Day17;

public class Day17
{
    [TestCase("Day17/example.txt", "4,6,3,5,6,3,5,2,1,0")]
    [TestCase("Day17/input.txt", "7,4,2,0,5,0,5,3,7")]
    public void Task1(string filePath, string expected)
    {
        var lines = File
            .ReadAllLines(filePath)
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .Select(l =>
            {
                var parts = l.Split(": ");
                return parts[1];
            })
            .ToArray();

        var machine = new Machine(
            int.Parse(lines[0]),
            int.Parse(lines[1]),
            int.Parse(lines[2]),
            lines[3]
                .Split(',')
                .Select(int.Parse)
                .ToArray());

        machine.Execute();
        string.Join(',', machine.Output).Should().Be(expected);
    }

    [TestCase("Day17/example2.txt", 117440)]
    [TestCase("Day17/input.txt", 0)]
    public void Task2(string filePath, int expected)
    {
        var lines = File
            .ReadAllLines(filePath)
            .Where(l => !string.IsNullOrWhiteSpace(l))
            .Select(l =>
            {
                var parts = l.Split(": ");
                return parts[1];
            })
            .ToArray();

        var a = 2142029392L;
        var b = int.Parse(lines[1]);
        var c = int.Parse(lines[2]);
        var instructions = lines[3]
            .Split(',')
            .Select(int.Parse)
            .ToArray();
        var matched = false;
        var timer = Stopwatch.StartNew();
        do
        {
            a++;

            if (timer.Elapsed > TimeSpan.FromMinutes(1))
            {
                File.WriteAllTextAsync("output.txt", a.ToString());
                timer.Restart();
            }

            var machine = new Machine(a, b, c, instructions);
            try
            {
                machine.OnWriteOutput += (m, i) =>
                {
                    var index = m.Output.Count;
                    if (index >= instructions.Length || instructions[index] != i)
                        throw new WrongOutputException();
                };

                machine.Execute();
                if (machine.Output.Count == instructions.Length)
                    matched = true;
            }
            catch 
            {
            }
        } while (!matched);

        a.Should().Be(expected);
    }

    private class WrongOutputException : Exception;
}