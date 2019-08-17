namespace Mir3Helper
{
	using Tuple = System.ValueTuple<Memory, uint>;

	public readonly struct Unit
	{
		public readonly Memory Memory;
		public readonly uint Address;
		public Unit(in Tuple t) => (Memory, Address) = t;
		public UnitType Type => Memory.Read<UnitType>(Address);
		public int Id => Memory.Read<int>(Address + 0x4);
		public string Name => Memory.ReadString(Address + 0x8, 32);
		public Point Pos => Memory.Read<Int16Pair>(Address + 0x1A0);
		public bool Casting => Memory.Read<bool>(Address + 0x278);
		public bool Attacking => Memory.Read<bool>(Address + 0x279);
		public byte LastSkill => Memory.Read<byte>(Address + 0x27C);
		public int MaxHp => Memory.Read<ushort>(Address + 0x280);
		public int Hp => Memory.Read<ushort>(Address + 0x282);
		public int Mp => Memory.Read<ushort>(Address + 0x286);
		public bool Hiding => (Memory.Read<byte>(Address + 0x292) & 0x80) != 0;
		public bool GreenPoison => (Memory.Read<byte>(Address + 0x293) & 0x80) != 0;
		public bool RedPoison => (Memory.Read<byte>(Address + 0x293) & 0x40) != 0;
		public bool Moving => Memory.Read<bool>(Address + 0x2A9);
	}
}