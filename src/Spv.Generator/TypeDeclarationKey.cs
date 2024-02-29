using System;
using System.Diagnostics.CodeAnalysis;

namespace Spv.Generator
{
    internal readonly struct TypeDeclarationKey : IEquatable<TypeDeclarationKey>
    {
        private readonly Instruction _typeDeclaration;

        public TypeDeclarationKey(Instruction typeDeclaration)
        {
            _typeDeclaration = typeDeclaration;
        }

        public override int GetHashCode()
        {
            return DeterministicHashCode.Combine(_typeDeclaration.Opcode, _typeDeclaration.GetHashCodeContent());
        }

        public bool Equals(TypeDeclarationKey other)
        {
            return _typeDeclaration.Opcode == other._typeDeclaration.Opcode && _typeDeclaration.EqualsContent(other._typeDeclaration);
        }

        public override bool Equals([NotNullWhen(true)] object obj)
        {
            return obj is TypeDeclarationKey key && Equals(key);
        }
    }
}

// 这段代码实现了一个名为 `TypeDeclarationKey` 的只读结构体，其功能包括：

// 1. 存储类型声明的指令。
// 2. 生成类型声明的哈希码。
// 3. 比较两个 `TypeDeclarationKey` 实例是否相等。