namespace AoC2024;

public static class CoordinateExtensions
{
    public static Coordinate MoveTo(this Coordinate c, Coordinate direction) =>
        new(c.X + direction.X, c.Y + direction.Y);

    public static Coordinate Multiply(this Coordinate c, int k) => new(c.X * k, c.Y * k);
    public static Coordinate Add(this Coordinate c, Coordinate j) => c.MoveTo(j);
    public static Coordinate Subtract(this Coordinate c, Coordinate j) => new (c.X - j.X, c.Y - j.Y);

    public static Coordinate PreviousDirection(this Coordinate d) => new(d.Y * -1, d.X);

    public static readonly Coordinate[] Directions =
    [
        new(-1, 0),
        new(0, 1),
        new(1, 0),
        new(0, -1),
    ];
}