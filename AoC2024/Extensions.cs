namespace AoC2024;

public static class Extensions
{
    public static bool TryGetValue<T>(this T[][] array, int i, int j, out T? value)
    {
        value = default;

        if (i < 0 || i >= array.Length)
            return false;

        if (j < 0 || j >= array[i].Length)
            return false;

        value = array[i][j];
        return true;
    }
}
