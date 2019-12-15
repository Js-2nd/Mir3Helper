namespace Mir3Helper
{
	using Tuple = System.ValueTuple<Memory, int>;

	public readonly struct ItemData
	{
		public const int Size = 0x339;

		public readonly Memory Memory;
		public readonly int Index;
		public readonly Address Address;

		public ItemData(in Tuple t)
		{
			(Memory, Index) = t;
			Address = 0x6F5E98 + Index * Size;
		}

		public bool IsValid => Memory != null && Memory.Read<bool>(Address);
		public string Name => Memory.ReadString(Address + 0x1, 16);
		public int Weight => Memory.Read<ushort>(Address + 0x21);
		public int Durability => Memory.Read<ushort>(Address + 0x47);
		public int MaxDurability => Memory.Read<ushort>(Address + 0x49);
		public int Count => Memory.Read<ushort>(Address + 0x4E);

		public static implicit operator ItemData(in Tuple t) => new ItemData(t);
	}
}