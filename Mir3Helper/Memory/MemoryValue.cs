namespace Mir3Helper
{
	using Tuple = System.ValueTuple<Memory, uint>;

	public readonly struct MemoryValue<T> where T : struct
	{
		public readonly Memory Memory;
		public readonly uint Address;

		MemoryValue(in Tuple t) => (Memory, Address) = t;

		public T Value => this;
		public override string ToString() => Value.ToString();

		public static implicit operator MemoryValue<T>(in Tuple t) => new MemoryValue<T>(t);
		public static implicit operator T(in MemoryValue<T> v) => v.Memory.Read<T>(v.Address);
	}
}