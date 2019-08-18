namespace Mir3Helper
{
	using Tuple = System.ValueTuple<Memory, uint>;

	public readonly struct SkillData
	{
		public readonly Memory Memory;
		public readonly uint Address;
		public SkillData(in Tuple t) => (Memory, Address) = t;

		public string Name => Memory.ReadString(Address, 12);
		public SkillId Id => Memory.Read<SkillId>(Address + 0x104);
		public MemoryValue<SkillLockType> Lock => Memory.Value(Address + 0x108);
		public MemoryValue<SkillPoisonType> Poison => Memory.Value(Address + 0x10A);
		public MemoryValue<SkillAmuletType> Amulet => Memory.Value(Address + 0x10B);
		public MemoryValue<SkillHotKey> HotKey => Memory.Value(Address + 0x10C);
		public MemoryValue<SkillHotKeyAlt> HotKeyAlt => Memory.Value(Address + 0x10D);
	}

	public enum SkillId : byte
	{
		火球术 = 1,
		治愈术 = 2,
		精神力战法 = 4,
		大火球 = 5,
		施毒术 = 6,
		抗拒火环 = 8,
		地狱火 = 9,
		疾光电影 = 10,
		雷电术 = 11,
		灵魂火符 = 13,
		幽灵盾 = 14,
		神圣战甲术 = 15,
		困魔咒 = 16,
		召唤骷髅 = 17,
		隐身术 = 18,
		集体隐身术 = 19,
		诱惑之光 = 20,
		瞬息移动 = 21,
		火墙 = 22,
		爆裂火焰 = 23,
		地狱雷光 = 24,
		群体治愈术 = 29,
		召唤神兽 = 30,
		魔法盾 = 31,
		圣言术 = 32,
		冰咆哮 = 33,
		空拳刀法 = 36,
		月魂断玉 = 37,
		月魂灵波 = 38,
		冰月神掌 = 39,
		冰月震天 = 40,
		霹雳掌 = 41,
		冰沙掌 = 53,
		风掌 = 67,
		龙卷风 = 72,
		风震天 = 73,
		击风 = 74,
		回生术 = 77,
		强震魔法 = 89,
		猛虎强势 = 94,
		异形换位 = 104,
		超强召唤骷髅 = 105,
	}

	public enum SkillLockType : byte
	{
		None = 0,
		All = 1,
		PlayerOnly = 2,
	}

	public enum SkillPoisonType : byte
	{
		None = 0,
		Alternately = 1,
		Red = 2,
		Green = 3,
	}

	public enum SkillAmuletType : byte
	{
		None = 0,
		Normal = 1,
		Fire = 2,
		Ice = 3,
		Thunder = 4,
		Wind = 5,
		Holy = 6,
		Dark = 7,
		Revival = 8,
		Any = 9,
	}

	public enum SkillHotKey : byte
	{
		None = 0,
		F1 = 0x31,
		F2 = 0x32,
		F3 = 0x33,
		F4 = 0x34,
		F5 = 0x35,
		F6 = 0x36,
		F7 = 0x37,
		F8 = 0x38,
		F9 = 0x39,
		F10 = 0x41,
		F11 = 0x42,
		F12 = 0x43,
	}

	public enum SkillHotKeyAlt : byte
	{
		None = 0,
		EscF1 = 1,
		EscF2 = 2,
		EscF3 = 3,
		EscF4 = 4,
		EscF5 = 5,
		EscF6 = 6,
		EscF7 = 7,
		EscF8 = 8,
		EscF9 = 9,
		EscF10 = 10,
		EscF11 = 11,
		EscF12 = 12,
	}
}