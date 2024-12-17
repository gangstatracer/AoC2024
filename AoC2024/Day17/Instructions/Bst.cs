using AoC2024.Day17.Operands;

namespace AoC2024.Day17.Instructions;

public class Bst(Machine machine, int operand) : Instruction(machine)
{
    private ComboOperand Operand { get; } = ComboOperand.Parse(operand, machine);

    public override bool Execute()
    {
        Machine.B = Operand.GetValue() % 8;
        return true;
    }
}