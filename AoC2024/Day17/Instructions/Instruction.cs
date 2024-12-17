namespace AoC2024.Day17.Instructions;

public abstract class Instruction(Machine machine)
{
    protected Machine Machine { get; } = machine;
    public abstract bool Execute();
}