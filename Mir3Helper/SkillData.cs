namespace Mir3Helper
{
	using Tuple = System.ValueTuple<Memory, int>;

	public readonly struct SkillData
	{
		public readonly Memory Memory;
		public readonly int Index;
		public readonly Address Address;

		SkillData(in Tuple t)
		{
			(Memory, Index) = t;
			Address = Memory?[0x7D83E0, Index * 0x110] ?? default;
		}

		public bool IsValid => Memory != null && Address.Value != 0;
		public string Name => Memory.ReadString(Address, 12);
		public Skill Id => Memory.Read<Skill>(Address + 0x104);
		public MemoryValue<SkillTargetLock> TargetLock => Memory.Value(Address + 0x108);
		public MemoryValue<SkillPoison> Poison => Memory.Value(Address + 0x10A);
		public MemoryValue<SkillAmulet> Amulet => Memory.Value(Address + 0x10B);
		public SkillKey Key => Memory.Read<SkillKey>(Address + 0x10C);
		public MemoryValue<SkillEscKey> EscKey => Memory.Value(Address + 0x10D);

		public void SetKey(SkillKey key)
		{
			Memory.Write(Address + 0x10C, key);
			Memory.Write(Memory[0x796B80, Index * 0x67 + 0x4], key);
		}

		public static implicit operator SkillData(in Tuple t) => new SkillData(t);
	}

	public enum Skill : byte
	{
		None = 0,
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

	public enum SkillTargetLock : byte
	{
		None = 0,
		Any = 1,
		PlayerOnly = 2,
	}

	public enum SkillPoison : byte
	{
		None = 0,
		Alternately = 1,
		Red = 2,
		Green = 3,
	}

	public enum SkillAmulet : byte
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

	public enum SkillKey : byte
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

	public enum SkillEscKey : byte
	{
		None = 0,
		F1 = 1,
		F2 = 2,
		F3 = 3,
		F4 = 4,
		F5 = 5,
		F6 = 6,
		F7 = 7,
		F8 = 8,
		F9 = 9,
		F10 = 10,
		F11 = 11,
		F12 = 12,
	}
}