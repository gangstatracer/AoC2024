namespace AoC2024.Day18;

public class Day18
{
    [TestCase("Day18/example.txt", 12, 7, 22)]
    [TestCase("Day18/input.txt", 1024, 71, 312)]
    public void Task1(string filePath, int bytesFallen, int mapSize, int expected)
    {
        var bytes = File
        .ReadAllLines(filePath)
        .Select(l => 
        {
            var parts = l.Split(',');
            return new Coordinate(
                int.Parse(parts[1]), 
                int.Parse(parts[0]));
        })
        .ToArray();

        var memory = new bool[mapSize, mapSize];
        for(var i = 0; i < bytesFallen; i++)
            memory.TrySetValue(bytes[i], true);

        // memory.Print(TestContext.Out);

        BFS(memory).Should().Be(expected);
    }

    private static int BFS(bool[,] memory)
    {
        var mapSize = memory.GetLength(0);
        var visited = new bool[mapSize, mapSize];
        var distances = new int[mapSize, mapSize];
        var q = new Queue<Coordinate>();
        
        q.Enqueue(new Coordinate(0,0));

        while(q.TryDequeue(out var c))
        {
            foreach(var n in CoordinateExtensions.Directions.Select(d => c.MoveTo(d)))
            {
                if(!visited.TryGetValue(n, out var v) || v)
                    continue;
                
                if(memory[n.X, n.Y])
                    continue;   
                
                visited[n.X, n.Y] = true;
                distances[n.X, n.Y] = distances[c.X, c.Y] + 1;
                q.Enqueue(n);
            }
        }

        return distances[mapSize - 1, mapSize - 1];
    }

    [TestCase("Day18/example.txt", 12, 7, "6,1")]
    [TestCase("Day18/input.txt", 1024, 71, "")]
    public void Task1(string filePath, int bytesFallen, int mapSize, string expected)
    {
        var bytes = File
        .ReadAllLines(filePath)
        .Select(l => 
        {
            var parts = l.Split(',');
            return new Coordinate(
                int.Parse(parts[1]), 
                int.Parse(parts[0]));
        })
        .ToArray();

        var memory = new bool[mapSize, mapSize];
        for(var i = 0; i < bytesFallen; i++)
            memory.TrySetValue(bytes[i], true);

        while(BFS(memory) != 0 && bytesFallen < bytes.Length){
            memory.TrySetValue(bytes[bytesFallen], true);
            bytesFallen++;
        }
        
        var last = bytes[bytesFallen - 1];
        $"{last.X},{last.Y}".Should().Be(expected);
    }
}
