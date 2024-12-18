namespace AoC2024.Day18;

public class Day18
{
    [TestCase("Day18/example.txt", 12, 7, 22)]
    [TestCase("Day18/input.txt", 1024, 71, 0)]
    public void Task1(string filePath, int bytesFallen, int mapSize, int expected)
    {
        var bytes = File
        .ReadAllLines(filePath)
        .Select(l => 
        {
            var parts = l.Split(',');
            return new Coordinate(
                int.Parse(parts[0]), 
                int.Parse(parts[1]));
        })
        .ToArray();

        var memory = new bool[mapSize, mapSize];
        var visited = new bool[mapSize, mapSize];
        var distances = new int[mapSize, mapSize];

        for(var i = 0; i < bytesFallen; i++)
            memory.TrySetValue(bytes[i], true);

        var q = new Queue<Coordinate>{new Coordinate(0,0)};
        while(q.TryDequeue(out var c))
        {
            foreach(var n in CoordinateExtensions.Directions.Select(d => c.MoveTo(d)))
            {
                if(!visited.TryGetValue(n, out var v) || v)
                    continue;
                
                if(memory[n.X, n.Y])
                    continue;   
                
                visited[n.X, n.Y] = true;
                distances[n.X, n.Y] = distances[c.X, c.Y];
                q.Enqueue(n);
            }
        }
        // Search(memory, distances, new Coordinate(0,0), 0);
        distances[mapSize -1, mapSize -1].Should().Be(expected);
    }

    private static void Search(bool[,] memory, int[,] distances, Coordinate c, int distance)
    {
        if(!memory.TryGetValue(c, out var m) || m)
            return;
        
        if(distances[c.X,c.Y] > distance)
            distances[c.X, c.Y] = distance;
        else
            return;
        
        foreach(var direction in CoordinateExtensions.Directions)
            Search(memory, distances, c.MoveTo(direction), distance + 1);
    }
}
