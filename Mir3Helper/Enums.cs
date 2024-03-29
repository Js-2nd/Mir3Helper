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

	public enum UpdateAction
	{
		Wait,
		Warp,
		Skill,
		LongSkill,
	}

	public enum PlayerClass : byte
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

	public enum AttackMode : byte
	{
		Any = 0,
		Peace = 1,
		Group = 2,
		Guild = 3,
		EvilOnly = 4,
	}

	public enum AssistMode : byte
	{
		Minimal = 0,
		Passive = 1,
		Active = 2,
	}
}