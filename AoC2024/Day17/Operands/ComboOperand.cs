namespace AoC2024.Day17.Operands;

public abstract class ComboOperand
{
    public abstract long GetValue();
    public static ComboOperand Parse(int operand, Machine machine) =>
        operand switch
        {
            >= 0 and < 4 => new Literal(operand),
            >= 4 and < 7 => new RegisterValue(operand, machine),
            _ => throw new ArgumentOutOfRangeException(nameof(operand))
        };
}