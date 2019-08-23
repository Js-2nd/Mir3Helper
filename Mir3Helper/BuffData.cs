namespace Mir3Helper
{
	public readonly struct BuffData
	{
		public readonly Memory Memory;
		public BuffData(Memory memory) => Memory = memory;

		public int Attack => Memory.Read<int>(0x7D53C8);
		public int Magic => Memory.Read<int>(0x7D53E0);
		public int AttackFire => Memory.Read<int>(0x7D53E4);
		public int AttackIce => Memory.Read<int>(0x7D53E8);
		public int AttackThunder => Memory.Read<int>(0x7D53EC);
		public int AttackWind => Memory.Read<int>(0x7D53F0);
		public int AttackHoly => Memory.Read<int>(0x7D53F4);
		public int Defense => Memory.Read<int>(0x7D53CC);
		public int Resist => Memory.Read<int>(0x7D53C4);
		public int ResistFire => Memory.Read<int>(0x7D53D0);
		public int ResistIce => Memory.Read<int>(0x7D53D4);
		public int ResistThunder => Memory.Read<int>(0x7D53D8);
		public int ResistWind => Memory.Read<int>(0x7D53DC);

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