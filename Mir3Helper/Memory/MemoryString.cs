namespace Mir3Helper
{
	using Tuple = System.ValueTuple<Memory, uint, int>;

	public readonly struct MemoryString
	{
		public readonly Memory Memory;
		public readonly uint Address;
		public readonly int Length;

		MemoryString(in Tuple t) => (Memory, Address, Length) = t;

		public string Value => this;
		public override string ToString() => Value;

		public static implicit operator MemoryString(in Tuple t) => new MemoryString(t);
		public static implicit operator string(in MemoryString v) => v.Memory.ReadString(v.Address, v.Length);
	}
}