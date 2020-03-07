namespace Mir3Helper
{
	using Tuple = System.ValueTuple<Memory, Address>;

	public readonly struct UnitData
	{
		public readonly Memory Memory;
		public readonly Address Address;
		public UnitData(in Tuple t) => (Memory, Address) = t;

		public bool IsValid => Memory != null && Address.Value != 0;
		public bool IsDead => Hp == 0 && MaxHp > 0;
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
		public bool MonsterIsUndead => Memory.Read<bool>(Address + 0x26EE);
		public bool MonsterIsTamable => Memory.Read<bool>(Address + 0x26EF);
		public int MonsterSpeed => Memory.Read<byte>(Address + 0x26F1);
		public Element MonsterAttackElement => Memory.Read<Element>(Address + 0x26F3);
		public int MonsterResist(Element element) => Memory.Read<sbyte>(Address + 0x26F3 + (int) element);

		public static implicit operator UnitData(in Tuple t) => new UnitData(t);
	}

	public enum UnitType
	{
		None = 0,
		Monster = 0x55FC3C,
		Npc = 0x55FDCC,
		Player = 0x5608AC,
		Self = 0x561BE4,
	}
}