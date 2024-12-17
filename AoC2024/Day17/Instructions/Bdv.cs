﻿using AoC2024.Day17.Operands;

namespace AoC2024.Day17.Instructions;

public class Bdv(Machine machine, int operand) : Instruction(machine)
{
    private ComboOperand Operand { get; } = ComboOperand.Parse(operand, machine);

    public override bool Execute()
    {
        var numerator = Machine.A;
        var denominator = (int)Math.Pow(2, Operand.GetValue());
        Machine.B = numerator / denominator;
        return true;
    }
}