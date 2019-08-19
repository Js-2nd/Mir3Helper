namespace Mir3Helper
{
	using static PInvoke.User32;

	public static class Extensions
	{
		public static bool Bit(this byte value, byte bit) => (value & (1 << bit)) != 0;

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