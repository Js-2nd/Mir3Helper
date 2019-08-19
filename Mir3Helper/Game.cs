namespace Mir3Helper
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Threading.Tasks;
	using static PInvoke.User32;

	public sealed partial class Game : IDisposable
	{
		public static int GetForeground(ref Game game)
		{
			var window = GetForegroundWindow();
			GetWindowThreadProcessId(window, out int processId);
			if (game?.Process.Id == processId) return 0;
			var process = Process.GetProcessById(processId);
			if (process.ProcessName.ToLowerInvariant() != "mir3") return -1;
			game = new Game(process);
			return processId;
		}

		public Process Process { get; }
		public Memory Memory { get; }
		public Window Window { get; }

		public Game(Process process)
		{
			Process = process;
			Memory = new Memory(process);
			Window = new Window(process);
		}

		public void Dispose() => Process.Dispose();

		bool m_UnitsCached;
		Dictionary<int, Address> m_Units;
		bool m_SkillsCached;
		Dictionary<Skill, int> m_Skills;
		Dictionary<SkillHotKey, int> m_SkillHotKeys;

		public void Update()
		{
			m_UnitsCached = false;
			m_SkillsCached = false;
			if (m_Id != null)
			{
				int id = Self.Id;
				if (m_Id != id)
				{
					m_Id = id;
					OnIdChange();
				}
			}
		}

		void OnIdChange()
		{
			m_Name = null;
		}

		public UnitData GetUnit(int id)
		{
			UpdateUnits();
			return m_Units.TryGetValue(id, out var address) ? (Memory, address) : default;
		}

		void UpdateUnits(int range = 8)
		{
			if (m_UnitsCached) return;
			m_UnitsCached = true;
			if (m_Units != null) m_Units.Clear();
			else m_Units = new Dictionary<int, Address>();
			for (int y = -range; y <= range; y++)
			for (int x = -range; x <= range; x++)
			{
				UnitData unit = (Memory, UnitAddress(x, y));
				int id = unit.Id;
				if (id != 0) m_Units[id] = unit.Address;
			}
		}

		public SkillData GetSkill(Skill id)
		{
			UpdateSkills();
			return m_Skills.TryGetValue(id, out int index) ? (Memory, index) : default;
		}

		public SkillData GetSkill(SkillHotKey hotKey)
		{
			UpdateSkills();
			return m_SkillHotKeys.TryGetValue(hotKey, out int index) ? (Memory, index) : default;
		}

		void UpdateSkills()
		{
			if (m_SkillsCached) return;
			m_SkillsCached = true;
			if (m_Skills != null) m_Skills.Clear();
			else m_Skills = new Dictionary<Skill, int>();
			if (m_SkillHotKeys != null) m_SkillHotKeys.Clear();
			else m_SkillHotKeys = new Dictionary<SkillHotKey, int>();
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

		public void CastSkill(ushort magic, int target = 0)
		{
			if (magic >= 1 && magic <= 12)
			{
				if (target != 0) SkillTargetPlayerOnly.Set(target);
				Window.Key(VirtualKey.VK_F1 - 1 + magic);
			}
		}

		public bool CoupleWarping { get; private set; }

		public async Task CoupleWarp(bool send = false)
		{
			if (CoupleWarping) return;
			CoupleWarping = true;
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

			CoupleWarping = false;
		}

		public void ClickWithBagAction(bool send = false)
		{
			if (!BagOpened) return;
			GetCursorPos(out var pos);
			ScreenToClient(Process.MainWindowHandle, ref pos);
			Window.Click(pos, send);
			Window.Click(BagAction, send);
		}
	}
}