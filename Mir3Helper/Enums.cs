namespace Mir3Helper
{
	using System;

	[Flags]
	public enum MK
	{
		LBUTTON = 0x1,
		RBUTTON = 0x2,
		SHIFT = 0x4,
		CONTROL = 0x8,
		MBUTTON = 0x10,
		XBUTTON1 = 0x20,
		XBUTTON2 = 0x40,
	}

	public enum CharacterClass : byte
	{
		Warrior = 0,
		Mage = 1,
		Taoist = 2,
	}

	public enum Element : byte
	{
		None = 0,
		Fire = 1,
		Ice = 2,
		Thunder = 3,
		Wind = 4,
		Holy = 5,
		Dark = 6,
		Phantom = 7,
	}

	public enum UnitType
	{
		Monster = 0x57AC74,
		Npc = 0x57B34C,
		Player = 0x57D644,
		Self = 0x58069C,
	}

	public enum Skill : byte
	{
		治愈术 = 2,
		施毒术 = 6,
		灵魂火符 = 13,
		幽灵盾 = 14,
		神圣战甲术 = 15,
		困魔咒 = 16,
		召唤骷髅 = 17,
		隐身术 = 18,
		集体隐身术 = 19,
		群体治愈术 = 29,
		召唤神兽 = 30,
		月魂断玉 = 37,
		月魂灵波 = 38,
		强震魔法 = 89,
	}
}