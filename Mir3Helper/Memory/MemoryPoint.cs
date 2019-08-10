namespace Mir3Helper
{
	using Tuple = System.ValueTuple<Memory, uint, uint>;

	public readonly struct MemoryPoint
	{
		public readonly Memory Memory;
		public readonly uint AddressX;
		public readonly uint AddressY;

		MemoryPoint(in Tuple t) => (Memory, AddressX, AddressY) = t;

		public Point Value => this;
		public override string ToString() => Value.ToString();

		public static implicit operator MemoryPoint(in Tuple t) => new MemoryPoint(t);

		public static implicit operator Point(in MemoryPoint v) =>
			(v.Memory.ReadInt32(v.AddressX), v.Memory.ReadInt32(v.AddressY));
	}
}