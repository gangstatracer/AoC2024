namespace AoC2024.Day17.Instructions;

public class Jnz(Machine machine, int operand) : Instruction(machine)
{
    private int Operand { get; } = operand;

    public override bool Execute()
    {
        if (Machine.A == 0)
            return true;

        Machine.InstructionPointer = Operand;
        return false;
    }
}