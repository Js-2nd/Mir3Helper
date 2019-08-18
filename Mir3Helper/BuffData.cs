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

		public int? FromSkill(Skill skill, SkillAmulet amulet = SkillAmulet.None)
		{
			switch (skill)
			{
				case Skill.幽灵盾:
					switch (amulet)
					{
						case SkillAmulet.Normal: return Resist;
						case SkillAmulet.Fire: return ResistFire;
						case SkillAmulet.Ice: return ResistIce;
						case SkillAmulet.Thunder: return ResistThunder;
						case SkillAmulet.Wind: return ResistWind;
						default: return null;
					}
				case Skill.神圣战甲术: return Defense;
				case Skill.强震魔法:
					switch (amulet)
					{
						case SkillAmulet.Normal: return Magic;
						case SkillAmulet.Fire: return AttackFire;
						case SkillAmulet.Ice: return AttackIce;
						case SkillAmulet.Thunder: return AttackThunder;
						case SkillAmulet.Wind: return AttackWind;
						case SkillAmulet.Holy: return AttackHoly;
						default: return null;
					}
				case Skill.猛虎强势: return Attack;
				default: return null;
			}
		}
	}
}