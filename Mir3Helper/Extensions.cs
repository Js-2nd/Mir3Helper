namespace Mir3Helper
{
	using System.Collections.Generic;
	using static PInvoke.User32;

	public static class Extensions
	{
		public static bool Bit(this byte value, byte bit) => (value & (1 << bit)) != 0;

		public static double ResistToMultiplier(this int resist)
		{
			switch (resist)
			{
				case -5: return 2.48832;
				case -4: return 2.0736;
				case -3: return 1.728;
				case -2: return 1.44;
				case -1: return 1.2;
				case 1: return 0.8;
				case 2: return 0.64;
				case 3: return 0.512;
				case 4: return 0.4096;
				case 5: return 0.32768;
				default: return 1;
			}
		}

		public static IEnumerable<Element> GetAttackElements(this PlayerClass playerClass)
		{
			switch (playerClass)
			{
				case PlayerClass.Mage:
					yield return Element.Fire;
					yield return Element.Ice;
					yield return Element.Thunder;
					yield return Element.Wind;
					break;
				case PlayerClass.Taoist:
					yield return Element.Holy;
					yield return Element.Dark;
					break;
			}
		}

		public static IEnumerable<Skill> GetAttackSkills(this PlayerClass playerClass, Element element = Element.None)
		{
			switch (playerClass)
			{
				case PlayerClass.Mage:
					if (element == Element.Fire || element == Element.None)
					{
						yield return Skill.大火球;
						yield return Skill.火球术;
					}

					if (element == Element.Ice || element == Element.None)
					{
						yield return Skill.冰月震天;
						yield return Skill.冰月神掌;
					}

					if (element == Element.Thunder || element == Element.None)
					{
						yield return Skill.雷电术;
						yield return Skill.霹雳掌;
					}

					if (element == Element.Wind || element == Element.None)
					{
						yield return Skill.击风;
						yield return Skill.风掌;
					}

					break;
				case PlayerClass.Taoist:
					if (element == Element.Holy || element == Element.None)
					{
						yield return Skill.月魂灵波;
						yield return Skill.月魂断玉;
					}

					if (element == Element.Dark || element == Element.None)
					{
						yield return Skill.灵魂火符;
					}

					break;
			}
		}

		public static VirtualKey ToVirtualKey(this SkillKey key)
		{
			switch (key)
			{
				case SkillKey.F1: return VirtualKey.VK_F1;
				case SkillKey.F2: return VirtualKey.VK_F2;
				case SkillKey.F3: return VirtualKey.VK_F3;
				case SkillKey.F4: return VirtualKey.VK_F4;
				case SkillKey.F5: return VirtualKey.VK_F5;
				case SkillKey.F6: return VirtualKey.VK_F6;
				case SkillKey.F7: return VirtualKey.VK_F7;
				case SkillKey.F8: return VirtualKey.VK_F8;
				case SkillKey.F9: return VirtualKey.VK_F9;
				case SkillKey.F10: return VirtualKey.VK_F10;
				case SkillKey.F11: return VirtualKey.VK_F11;
				case SkillKey.F12: return VirtualKey.VK_F12;
				default: return VirtualKey.VK_NO_KEY;
			}
		}

		public static VirtualKey ToVirtualKey(this SkillEscKey escKey)
		{
			switch (escKey)
			{
				case SkillEscKey.F1: return VirtualKey.VK_F1;
				case SkillEscKey.F2: return VirtualKey.VK_F2;
				case SkillEscKey.F3: return VirtualKey.VK_F3;
				case SkillEscKey.F4: return VirtualKey.VK_F4;
				case SkillEscKey.F5: return VirtualKey.VK_F5;
				case SkillEscKey.F6: return VirtualKey.VK_F6;
				case SkillEscKey.F7: return VirtualKey.VK_F7;
				case SkillEscKey.F8: return VirtualKey.VK_F8;
				case SkillEscKey.F9: return VirtualKey.VK_F9;
				case SkillEscKey.F10: return VirtualKey.VK_F10;
				case SkillEscKey.F11: return VirtualKey.VK_F11;
				case SkillEscKey.F12: return VirtualKey.VK_F12;
				default: return VirtualKey.VK_NO_KEY;
			}
		}
	}
}