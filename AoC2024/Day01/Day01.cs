namespace AoC2024.Day01;

public class Day01
{
    [Test]
    public void Task1()
    {
        var first = new List<int>();
        var second = new List<int>();
        var lines = File.ReadLines("Day01/input.txt");
        foreach (var line in lines)
        {
            var ids = line.Split("   ").Select(int.Parse).ToArray();
            first.Add(ids[0]);
            second.Add(ids[1]);
        }
        first
        .Order()
        .Zip(second.Order())
        .Select(t => Math.Abs(t.First - t.Second))
        .Sum()
        .Should()
        .Be(1660292);
    }

    [Test]
    public void Task2()
    {
        var first = new List<int>();
        var second = new List<int>();
        var lines = File.ReadLines("Day01/input.txt");
        foreach (var line in lines)
        {
            var ids = line.Split("   ").Select(int.Parse).ToArray();
            first.Add(ids[0]);
            second.Add(ids[1]);
        }

        var secondStatistics = new Dictionary<int, int>();
        foreach (var number in second)
        {
            if (secondStatistics.ContainsKey(number))
                secondStatistics[number]++;
            else
                secondStatistics[number] = 1;
        }

        first
        .Select(i => i * (secondStatistics.TryGetValue(i, out var count) ? count : 0))
        .Sum()
        .Should()
        .Be(22776016);
    }
}
