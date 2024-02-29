using System;
using System.IO;

namespace Spv.Generator
{
    public class LiteralInteger : IOperand, IEquatable<LiteralInteger>
    {
        [ThreadStatic]
        private static GeneratorPool<LiteralInteger> _pool;

        internal static void RegisterPool(GeneratorPool<LiteralInteger> pool)
        {
            _pool = pool;
        }

        internal static void UnregisterPool()
        {
            _pool = null;
        }

        public OperandType Type => OperandType.Number;

        private enum IntegerType
        {
            UInt32,
            Int32,
            UInt64,
            Int64,
            Float32,
            Float64,
        }

        private IntegerType _integerType;
        private ulong _data;

        public ushort WordCount { get; private set; }

        public LiteralInteger() { }

        private static LiteralInteger New()
        {
            return _pool.Allocate();
        }

        private LiteralInteger Set(ulong data, IntegerType integerType, ushort wordCount)
        {
            _data = data;
            _integerType = integerType;

            WordCount = wordCount;

            return this;
        }

        public static implicit operator LiteralInteger(int value) => New().Set((ulong)value, IntegerType.Int32, 1);
        public static implicit operator LiteralInteger(uint value) => New().Set(value, IntegerType.UInt32, 1);
        public static implicit operator LiteralInteger(long value) => New().Set((ulong)value, IntegerType.Int64, 2);
        public static implicit operator LiteralInteger(ulong value) => New().Set(value, IntegerType.UInt64, 2);
        public static implicit operator LiteralInteger(float value) => New().Set(BitConverter.SingleToUInt32Bits(value), IntegerType.Float32, 1);
        public static implicit operator LiteralInteger(double value) => New().Set(BitConverter.DoubleToUInt64Bits(value), IntegerType.Float64, 2);
        public static implicit operator LiteralInteger(Enum value) => New().Set((ulong)(int)(object)value, IntegerType.Int32, 1);

        // NOTE: this is not in the standard, but this is some syntax sugar useful in some instructions (TypeInt ect)
        public static implicit operator LiteralInteger(bool value) => New().Set(Convert.ToUInt64(value), IntegerType.Int32, 1);

        public static LiteralInteger CreateForEnum<T>(T value) where T : Enum
        {
            return value;
        }

        public void WriteOperand(BinaryWriter writer)
        {
            if (WordCount == 1)
            {
                writer.Write((uint)_data);
            }
            else
            {
                writer.Write(_data);
            }
        }

        public override bool Equals(object obj)
        {
            return obj is LiteralInteger literalInteger && Equals(literalInteger);
        }

        public bool Equals(LiteralInteger cmpObj)
        {
            return Type == cmpObj.Type && _integerType == cmpObj._integerType && _data == cmpObj._data;
        }

        public override int GetHashCode()
        {
            return DeterministicHashCode.Combine(Type, _data);
        }

        public bool Equals(IOperand obj)
        {
            return obj is LiteralInteger literalInteger && Equals(literalInteger);
        }

        public override string ToString() => $"{_integerType} {_data}";
    }
}

// 这段代码的作用是实现了一个名为 `LiteralInteger` 的类，用于表示整数类型的操作数。主要功能包括：
// 1. **整数类型表示**：支持多种整数类型，包括无符号和有符号的 32 位和 64 位整数，以及 32 位和 64 位浮点数。
// 2. **对象池管理**：通过 `_pool` 字段和 `RegisterPool()`、`UnregisterPool()` 方法，实现了对象池的注册和取消注册，以提高对象的重用性和性能。
// 3. **隐式转换操作符**：提供了一系列的隐式转换操作符，允许将不同类型的数据（如整数、浮点数、枚举等）转换为 `LiteralInteger` 实例，方便操作数的创建和赋值。
// 4. **二进制写入**：通过 `WriteOperand()` 方法，将操作数写入二进制写入器，用于序列化操作数数据。
// 5. **比较和哈希**：重写了 `Equals()` 方法和 `GetHashCode()` 方法，用于比较两个 `LiteralInteger` 实例的相等性，并生成对象的哈希码。
// 6. **字符串表示**：重写了 `ToString()` 方法，提供了对象的字符串表示形式，包括整数类型和数据值。
// 总体来说，这段代码提供了一个灵活的整数操作数类，可以方便地用于生成和操作整数类型的数据，并且提供了对象池管理以及比较、哈希等常用功能。