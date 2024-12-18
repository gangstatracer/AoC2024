namespace AoC2024.Day17.Operands;

public class RegisterValue : ComboOperand
{
    private Machine Machine { get; }

    public RegisterValue(int value, Machine machine)
    {
        if (value is < 4 or > 6)
            throw new ArgumentOutOfRangeException(nameof(value));
        Machine = machine;
        Type = (RegisterType)(value - 4);
    }

    public RegisterType Type { get; init; }

    public override long GetValue()
    {
        return Type switch
        {
            RegisterType.A => Machine.A,
            RegisterType.B => Machine.B,
            RegisterType.C => Machine.C,
            _ => throw new ArgumentOutOfRangeException(nameof(Type)),
        };
    }
}