namespace Mir3Helper
{
	public readonly struct BuffData
	{
		const int BaseAddress = 0x79FAC4;

		public readonly Memory Memory;
		public BuffData(Memory memory) => Memory = memory;

		public int Attack => Memory.Read<int>(BaseAddress + 0x4);
		public int Magic => Memory.Read<int>(BaseAddress + 0x1C);
		public int AttackFire => Memory.Read<int>(BaseAddress + 0x20);
		public int AttackIce => Memory.Read<int>(BaseAddress + 0x24);
		public int AttackThunder => Memory.Read<int>(BaseAddress + 0x28);
		public int AttackWind => Memory.Read<int>(BaseAddress + 0x2C);
		public int AttackHoly => Memory.Read<int>(BaseAddress + 0x30);
		public int Defense => Memory.Read<int>(BaseAddress + 0x8);
		public int Resist => Memory.Read<int>(BaseAddress + 0x0);
		public int ResistFire => Memory.Read<int>(BaseAddress + 0xC);
		public int ResistIce => Memory.Read<int>(BaseAddress + 0x10);
		public int ResistThunder => Memory.Read<int>(BaseAddress + 0x14);
		public int ResistWind => Memory.Read<int>(BaseAddress + 0x18);

		public bool CanReceive(Skill skill, SkillAmulet amulet = SkillAmulet.None, int ignoreTime = 5)
		{
			switch (skill)
			{
				case Skill.幽灵盾:
					switch (amulet)
					{
						case SkillAmulet.Normal: return Resist <= ignoreTime;
						case SkillAmulet.Fire:
						case SkillAmulet.Ice:
						case SkillAmulet.Thunder:
						case SkillAmulet.Wind:
							return ResistFire <= ignoreTime && ResistIce <= ignoreTime &&
							       ResistThunder <= ignoreTime && ResistWind <= ignoreTime;
						default: return false;
					}
				case Skill.神圣战甲术: return Defense <= ignoreTime;
				case Skill.强震魔法:
					switch (amulet)
					{
						case SkillAmulet.Normal:
						case SkillAmulet.Fire:
						case SkillAmulet.Ice:
						case SkillAmulet.Thunder:
						case SkillAmulet.Wind:
						case SkillAmulet.Holy:
							return Magic <= ignoreTime && AttackFire <= ignoreTime && AttackIce <= ignoreTime &&
							       AttackThunder <= ignoreTime && AttackWind <= ignoreTime && AttackHoly <= ignoreTime;
						default: return false;
					}
				case Skill.猛虎强势: return Attack <= ignoreTime;
				default: return false;
			}
		}
	}
}