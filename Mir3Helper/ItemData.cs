namespace Mir3Helper
{
	using Tuple = System.ValueTuple<Memory, Address>;

	public readonly struct ItemData
	{
		public readonly Memory Memory;
		public readonly Address Address;

		public ItemData(in Tuple t) => (Memory, Address) = t;

		public bool IsValid => Memory != null && MaxDurability > 0;
		public string Name => Memory.ReadString(Address, 16);
		public int Weight => Memory.Read<ushort>(Address + 0x20);
		public int Durability => Memory.Read<ushort>(Address + 0x46);
		public int MaxDurability => Memory.Read<ushort>(Address + 0x48);
		public int StackCount => Memory.Read<ushort>(Address + 0x4D);

		public static implicit operator ItemData(in Tuple t) => new ItemData(t);
	}
}