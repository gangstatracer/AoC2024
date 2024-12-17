namespace AoC2024.Day17.Operands;

public class Literal(int value) : ComboOperand
{
    private int Value { get; } = value is >= 0 and < 4 
        ? value 
        : throw new ArgumentOutOfRangeException(nameof(value));

    public override int GetValue()
    {
        return Value;
    }
}