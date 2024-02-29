using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Spv.Generator
{
    public struct InstructionOperands
    {
        private const int InternalCount = 5;

        public int Count;
        public IOperand Operand1;
        public IOperand Operand2;
        public IOperand Operand3;
        public IOperand Operand4;
        public IOperand Operand5;
        public IOperand[] Overflow;

        public Span<IOperand> AsSpan()
        {
            if (Count > InternalCount)
            {
                return MemoryMarshal.CreateSpan(ref this.Overflow[0], Count);
            }
            else
            {
                return MemoryMarshal.CreateSpan(ref this.Operand1, Count);
            }
        }

        public void Add(IOperand operand)
        {
            if (Count < InternalCount)
            {
                MemoryMarshal.CreateSpan(ref this.Operand1, Count + 1)[Count] = operand;
                Count++;
            }
            else
            {
                if (Overflow == null)
                {
                    Overflow = new IOperand[InternalCount * 2];
                    MemoryMarshal.CreateSpan(ref this.Operand1, InternalCount).CopyTo(Overflow.AsSpan());
                }
                else if (Count == Overflow.Length)
                {
                    Array.Resize(ref Overflow, Overflow.Length * 2);
                }

                Overflow[Count++] = operand;
            }
        }

        private readonly IEnumerable<IOperand> AllOperands => new[] { Operand1, Operand2, Operand3, Operand4, Operand5 }
            .Concat(Overflow ?? Array.Empty<IOperand>())
            .Take(Count);

        public readonly override string ToString()
        {
            return $"({string.Join(", ", AllOperands)})";
        }

        public readonly string ToString(string[] labels)
        {
            var labeledParams = AllOperands.Zip(labels, (op, label) => $"{label}: {op}");
            var unlabeledParams = AllOperands.Skip(labels.Length).Select(op => op.ToString());
            var paramsToPrint = labeledParams.Concat(unlabeledParams);
            return $"({string.Join(", ", paramsToPrint)})";
        }
    }
}

// 这段代码的简要功能如下：

// - 定义了名为 `InstructionOperands` 的结构体，用于管理指令的操作数。
// - 包含公共字段 `Count` 和操作数 `Operand1` 到 `Operand5`，以及溢出操作数数组 `Overflow`。
// - 提供了方法用于添加操作数到集合中，并根据情况存储在固定大小的字段或溢出数组中。
// - 提供了方法用于返回操作数集合的字符串表示形式，包括带标签的操作数。
