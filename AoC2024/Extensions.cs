using System.Drawing;

namespace AoC2024;

public static class Extensions
{
    public static bool TryGetValue<T>(this T[][] array, int i, int j, out T? value)
    {
        return array.TryGetValue(new Point(i, i), out value);
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
