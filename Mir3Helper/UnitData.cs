namespace Mir3Helper
{
	using Tuple = System.ValueTuple<Memory, uint>;

	public readonly struct UnitData
	{
		public readonly Memory Memory;
		public readonly uint Address;
		public UnitData(in Tuple t) => (Memory, Address) = t;

		public UnitType Type => Memory.Read<UnitType>(Address);
		public int Id => Memory.Read<int>(Address + 0x4);
		public string Name => Memory.ReadString(Address + 0x8, 32);
		public Point Pos => Memory.Read<Int16Pair>(Address + 0x1A0);
		public bool Casting => Memory.Read<bool>(Address + 0x278);
		public bool Attacking => Memory.Read<bool>(Address + 0x279);
		public Skill LastSkill => Memory.Read<Skill>(Address + 0x27C);
		public int MaxHp => Memory.Read<ushort>(Address + 0x280);
		public int Hp => Memory.Read<ushort>(Address + 0x282);
		public int Mp => Memory.Read<ushort>(Address + 0x286);
		public bool Hiding => Memory.Read<byte>(Address + 0x292).Bit(7);
		public bool GreenPoison => Memory.Read<byte>(Address + 0x293).Bit(7);
		public bool RedPoison => Memory.Read<byte>(Address + 0x293).Bit(6);
		public bool Moving => Memory.Read<bool>(Address + 0x2A9);

		public static implicit operator UnitData(in Tuple t) => new UnitData(t);
	}

	public enum UnitType
	{
		Monster = 0x57AC74,
		Npc = 0x57B34C,
		Player = 0x57D644,
		Self = 0x58069C,
	}
}