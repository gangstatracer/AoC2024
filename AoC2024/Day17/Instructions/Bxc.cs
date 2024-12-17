namespace AoC2024.Day17.Instructions;

public class Bxc(Machine machine) : Instruction(machine)
{
    public override bool Execute()
    {
        Machine.B ^= Machine.C;
        return true;
    }
}