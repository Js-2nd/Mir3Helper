namespace Mir3Helper
{
	using Tuple = System.ValueTuple<Memory, Address>;

	public readonly struct MemoryValue<T> where T : struct
	{
		public readonly Memory Memory;
		public readonly Address Address;
		MemoryValue(in Tuple t) => (Memory, Address) = t;

		public bool IsValid => Memory != null && Address.Value != 0;
		public T Value => IsValid ? Memory.Read<T>(Address) : default;
		public bool Set(in T value) => IsValid && Memory.Write(Address, value);
		public override string ToString() => Value.ToString();

		public static implicit operator MemoryValue<T>(in Tuple t) => new MemoryValue<T>(t);
		public static implicit operator T(in MemoryValue<T> v) => v.Value;
	}
}