using System;
using System.IO;
using System.Text;

namespace Spv.Generator
{
    public class LiteralString : IOperand, IEquatable<LiteralString>
    {
        public OperandType Type => OperandType.String;

        private readonly string _value;

        public LiteralString(string value)
        {
            _value = value;
        }

        public ushort WordCount => (ushort)(_value.Length / 4 + 1);

        public void WriteOperand(BinaryWriter writer)
        {
            writer.Write(_value.AsSpan());

            // String must be padded to the word size (which is 4 bytes).
            int paddingSize = 4 - (Encoding.ASCII.GetByteCount(_value) % 4);

            Span<byte> padding = stackalloc byte[paddingSize];

            writer.Write(padding);
        }

        public override bool Equals(object obj)
        {
            return obj is LiteralString literalString && Equals(literalString);
        }

        public bool Equals(LiteralString cmpObj)
        {
            return Type == cmpObj.Type && _value.Equals(cmpObj._value);
        }

        public override int GetHashCode()
        {
            return DeterministicHashCode.Combine(Type, DeterministicHashCode.GetHashCode(_value));
        }

        public bool Equals(IOperand obj)
        {
            return obj is LiteralString literalString && Equals(literalString);
        }

        public override string ToString() => _value;
    }
}

// 这段代码实现了一个名为 `LiteralString` 的类，用于表示字符串类型的操作数。主要功能包括：
// 1. 操作数类型表示：通过 `Type` 属性指定操作数类型为字符串。
// 2. 字符串值存储：使用私有字段 `_value` 存储字符串值。
// 3. 构造函数：提供了一个构造函数，接受字符串参数，并将其赋值给 `_value` 字段。
// 4. 字数计算：通过 `WordCount` 属性计算字符串占用的字数，以便在序列化时正确计算操作数的长度。
// 5. 序列化：通过 `WriteOperand()` 方法将字符串操作数写入二进制写入器。在写入字符串内容后，根据 ASCII 编码计算字符串的字节长度，并根据需要填充字节，确保字符串长度是 4 的倍数。
// 6. 相等性比较：重写了 `Equals(object obj)` 方法和 `GetHashCode()` 方法，以及 `Equals(LiteralString cmpObj)` 和 `Equals(IOperand obj)` 方法，用于比较两个 `LiteralString` 实例的相等性。
// 7. 字符串表示：重写了 `ToString()` 方法，返回对象的字符串表示形式。
// 总的来说，这段代码提供了一个用于处理字符串类型操作数的类，实现了操作数的序列化、相等性比较和字符串表示等功能。
