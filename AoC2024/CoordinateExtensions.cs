namespace AoC2024;

public static class CoordinateExtensions
{
    public static Coordinate MoveTo(this Coordinate c, Coordinate direction) =>
        new(c.X + direction.X, c.Y + direction.Y);

    public static Coordinate Multiply(this Coordinate c, int k) => new(c.X * k, c.Y * k);
    public static Coordinate Add(this Coordinate c, Coordinate j) => c.MoveTo(j);

    public static Coordinate PreviousDirection(this Coordinate d) => new(d.Y * -1, d.X);
}