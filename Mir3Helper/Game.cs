namespace Mir3Helper
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Threading.Tasks;
	using static PInvoke.User32;

	public sealed partial class Game : IDisposable
	{
		public Process Process { get; }
		public Memory Memory { get; }
		public Window Window { get; }
		public Point ScreenCenter { get; }
		readonly Dictionary<int, Address> m_Units = new Dictionary<int, Address>();
		readonly Dictionary<Skill, int> m_Skills = new Dictionary<Skill, int>();
		readonly Dictionary<SkillHotKey, int> m_SkillHotKeys = new Dictionary<SkillHotKey, int>();

		public Game(Process process)
		{
			Process = process;
			Memory = new Memory(process);
			Window = new Window(process);
			ScreenCenter = ScreenSize.X == 800 ? (400, 240) : (496, 338);
		}

		public void Dispose() => Process.Dispose();

		public void Update()
		{
			UpdateUnits();
			UpdateSkills();
		}

		public void UpdateUnits(int range = 10)
		{
			m_Units.Clear();
			for (int y = -range; y <= range; y++)
			for (int x = -range; x <= range; x++)
			{
				UnitData unit = (Memory, UnitAddress(x, y));
				int id = unit.Id;
				if (id != 0) m_Units[id] = unit.Address;
			}
		}

		public UnitData GetUnit(int id) =>
			m_Units.TryGetValue(id, out var address) ? (Memory, address) : default;

		public void UpdateSkills()
		{
			m_Skills.Clear();
			m_SkillHotKeys.Clear();
			for (int i = 0, count = SkillCount; i < count; i++)
			{
				SkillData skill = (Memory, i);
				var id = skill.Id;
				if (id != Skill.None)
				{
					m_Skills[id] = i;
					var hotKey = skill.HotKey;
					if (hotKey != SkillHotKey.None) m_SkillHotKeys[hotKey] = i;
				}
			}
		}

		public SkillData GetSkill(Skill id) =>
			m_Skills.TryGetValue(id, out int index) ? (Memory, index) : default;

		public SkillData GetSkill(SkillHotKey hotKey) =>
			m_SkillHotKeys.TryGetValue(hotKey, out int index) ? (Memory, index) : default;

		public void CastSkill(ushort magic, int target = 0)
		{
			if (magic >= 1 && magic <= 12)
			{
				if (target != 0) SkillTargetPlayerOnly.Set(target);
				Window.Key(VirtualKey.VK_F1 - 1 + magic);
			}
		}

		public async Task CoupleTeleport(bool send = false)
		{
			bool opened = StatusOpened;
			if (opened)
			{
				Window.Key(VirtualKey.VK_Q, send);
				await Task.Delay(20);
			}

			Window.Key(VirtualKey.VK_Q, send);
			await Task.Delay(20);
			Window.DoubleClick(StatusLeftRing, send);
			if (!opened)
			{
				await Task.Delay(20);
				Window.Key(VirtualKey.VK_Q, send);
			}
		}

		public void ClickBagAction(bool send = false)
		{
			if (!BagOpened) return;
			GetCursorPos(out var pos);
			ScreenToClient(Process.MainWindowHandle, ref pos);
			Window.Click(pos, send);
			Window.Click(BagAction, send);
		}
	}
}