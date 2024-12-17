namespace AoC2024.Day17.Instructions;

public class Bxl(Machine machine, int operand) : Instruction(machine)
{
    private int Operand { get; } = operand;

    public override bool Execute()
    {
        Machine.B ^= Operand;
        return true;
    }
}