namespace Mir3Helper
{
	using Tuple = System.ValueTuple<Memory, uint>;

	public readonly struct WritableMemoryValue<T> where T : struct
	{
		public readonly Memory Memory;
		public readonly uint Address;

		WritableMemoryValue(in Tuple t) => (Memory, Address) = t;

		public T Value => this;
		public void Set(T value) => Memory.Write(Address, value);
		public override string ToString() => Value.ToString();

		public static implicit operator WritableMemoryValue<T>(in Tuple t) => new WritableMemoryValue<T>(t);
		public static implicit operator T(in WritableMemoryValue<T> v) => v.Memory.Read<T>(v.Address);
	}
}