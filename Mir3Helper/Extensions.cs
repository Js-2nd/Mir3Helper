namespace Mir3Helper
{
	using System;
	using System.Threading.Tasks;
	using static PInvoke.User32;

	public static class Extensions
	{
		public static void Catch(this Task task) =>
			task.ContinueWith(t => Console.Error.WriteLine(t.Exception), TaskContinuationOptions.OnlyOnFaulted);

		public static bool Bit(this byte value, byte bit) => (value & (1 << bit)) != 0;

		public static VirtualKey ToVirtualKey(this SkillHotKey hotKey)
		{
			switch (hotKey)
			{
				case SkillHotKey.F1: return VirtualKey.VK_F1;
				case SkillHotKey.F2: return VirtualKey.VK_F2;
				case SkillHotKey.F3: return VirtualKey.VK_F3;
				case SkillHotKey.F4: return VirtualKey.VK_F4;
				case SkillHotKey.F5: return VirtualKey.VK_F5;
				case SkillHotKey.F6: return VirtualKey.VK_F6;
				case SkillHotKey.F7: return VirtualKey.VK_F7;
				case SkillHotKey.F8: return VirtualKey.VK_F8;
				case SkillHotKey.F9: return VirtualKey.VK_F9;
				case SkillHotKey.F10: return VirtualKey.VK_F10;
				case SkillHotKey.F11: return VirtualKey.VK_F11;
				case SkillHotKey.F12: return VirtualKey.VK_F12;
				default: return VirtualKey.VK_NO_KEY;
			}
		}
	}
}