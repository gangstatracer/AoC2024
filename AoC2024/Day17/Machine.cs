using AoC2024.Day17.Instructions;

namespace AoC2024.Day17;

public class Machine(long a, int b, int c, int[] instructions)
{
    public long A { get; set; } = a;

    public long B { get; set; } = b;

    public long C { get; set; } = c;

    public int InstructionPointer { get; set; }

    private int[] Instructions { get; } = instructions;

    public Action<Machine, long>? OnWriteOutput { get; set; }

    public void WriteOutput(long value)
    {
        OnWriteOutput?.Invoke(this, value);
        _output.Add(value);
    }

    private readonly List<long> _output = [];

    public IReadOnlyList<long> Output => _output;

    public void Execute()
    {
        InstructionPointer = 0;
        while (InstructionPointer < Instructions.Length)
        {
            var opcode = Instructions[InstructionPointer];
            var operand = Instructions[InstructionPointer + 1];
            var instruction = Parse(opcode, operand);
            var movePointer = instruction.Execute();
            if (movePointer)
                InstructionPointer += 2;
        }
    }

    private Instruction Parse(int opcode, int operand)
    {
        return opcode switch
        {
            0 => new Adv(this, operand),
            1 => new Bxl(this, operand),
            2 => new Bst(this, operand),
            3 => new Jnz(this, operand),
            4 => new Bxc(this),
            5 => new Out(this, operand),
            6 => new Bdv(this, operand),
            7 => new Cdv(this, operand),
            _ => throw new ArgumentOutOfRangeException(nameof(opcode)),
        };
    }
}