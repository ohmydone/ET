using System;

namespace ET
{
    public static class TypeInfo<T>
    {
        [StaticField]
        public static readonly Type Type = typeof(T);
        [StaticField]
        public static readonly int HashCode = typeof(T).GetHashCode();
        [StaticField]
        public static readonly string TypeName = typeof(T).Name;
    }
}