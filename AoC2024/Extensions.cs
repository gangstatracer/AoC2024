using System.Drawing;

namespace AoC2024;

public static class Extensions
{
    public static void Print<T, K>(this IEnumerable<T> enumerable, TextWriter writer)
    where T : IEnumerable<K> where K : notnull
    {
        foreach (var line in enumerable)
        {
            foreach (var element in line)
            {
                writer.Write(element.ToString());
            }
            writer.WriteLine();
        }
    }
    public static bool TryGetValue<T>(this List<T> list, int index, out T? value)
    {
        value = default;
        if (index < 0)
            return false;
        if (index >= list.Count)
            return false;
        value = list[index];
        return true;
    }

    public static bool TrySetValue<T>(this T[,] array, int i, int j, T value)
    {
        return TrySetValue(array, new Point(i, j), value);
    }

    public static bool TrySetValue<T>(this T[,] array, Coordinate c, T value)
    {
        return TrySetValue(array, new Point(c.X, c.Y), value);
    }

    public static bool TrySetValue<T>(this T[,] array, Point point, T value)
    {
        if (point.X < 0 || point.X >= array.GetLength(0))
            return false;

        if (point.Y < 0 || point.Y >= array.GetLength(1))
            return false;

        array[point.X, point.Y] = value;
        return true;
    }

    public static bool TryGetValue<T>(this T[,] array, Coordinate c, out T? value)
    {
        value = default;
        var (x, y) = c;
        if (x < 0 || x >= array.GetLength(0) || y < 0 || y >= array.GetLength(1))
            return false;
        value = array[x, y];
        return true;
    }

    public static bool TryGetValue<T>(this T[][] array, int i, int j, out T? value)
    {
        return array.TryGetValue(new Point(i, j), out value);
    }

    public static bool TryGetValue<T>(this T[][] array, Coordinate c, out T? value)
    {
        return array.TryGetValue(new Point(c.X, c.Y), out value);
    }

    public static bool TryGetValue<T>(this T[][] array, Point point, out T? value)
    {
        value = default;

        if (point.X < 0 || point.X >= array.Length)
            return false;

        if (point.Y < 0 || point.Y >= array[point.X].Length)
            return false;

        value = array[point.X][point.Y];
        return true;
    }

    public static Point MoveInDirection(this Point coordinate, Point direction)
    {
        return new Point(coordinate.X + direction.X, coordinate.Y + direction.Y);
    }
}