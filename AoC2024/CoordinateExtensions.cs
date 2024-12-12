namespace AoC2024;

public static class CoordinateExtensions
{
    public static Coordinate MoveTo(this Coordinate c, Coordinate direction) =>
        new(c.X + direction.X, c.Y + direction.Y);

    public static Coordinate PreviousDirection(this Coordinate d) => new(d.Y * -1, d.X);
}