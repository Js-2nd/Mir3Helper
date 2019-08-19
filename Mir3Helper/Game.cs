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
		Dictionary<SkillKey, int> m_SkillKeys;
		Dictionary<SkillEscKey, int> m_SkillEscKeys;

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

		public SkillData GetSkill(SkillKey key)
		{
			UpdateSkills();
			return m_SkillKeys.TryGetValue(key, out int index) ? (Memory, index) : default;
		}

		public SkillData GetSkill(SkillEscKey escKey)
		{
			UpdateSkills();
			return m_SkillEscKeys.TryGetValue(escKey, out int index) ? (Memory, index) : default;
		}

		void UpdateSkills()
		{
			if (m_SkillsCached) return;
			m_SkillsCached = true;
			if (m_Skills != null) m_Skills.Clear();
			else m_Skills = new Dictionary<Skill, int>();
			if (m_SkillKeys != null) m_SkillKeys.Clear();
			else m_SkillKeys = new Dictionary<SkillKey, int>();
			if (m_SkillEscKeys != null) m_SkillEscKeys.Clear();
			else m_SkillEscKeys = new Dictionary<SkillEscKey, int>();
			for (int i = 0, count = SkillCount; i < count; i++)
			{
				SkillData skill = (Memory, i);
				var id = skill.Id;
				if (id != Skill.None)
				{
					m_Skills[id] = i;
					var key = skill.Key;
					if (key != SkillKey.None) m_SkillKeys[key] = i;
					var escKey = skill.EscKey.Value;
					if (escKey != SkillEscKey.None) m_SkillEscKeys[escKey] = i;
				}
			}
		}

		public async ValueTask<bool> TryCastSkill(Skill id, int target = 0)
		{
			var skill = GetSkill(id);
			if (!skill.IsValid) return false;
			var key = skill.Key;
			if (key != SkillKey.None) Window.Key(key.ToVirtualKey());
			else
			{
				var escKey = skill.EscKey.Value;
				if (escKey == SkillEscKey.None) skill.EscKey.Set(escKey = SkillEscKey.F12);
				Window.Key(VirtualKey.VK_ESCAPE);
				await Task.Delay(20);
				Window.Key(escKey.ToVirtualKey());
				await Task.Delay(20);
				if (escKey == SkillEscKey.F12) skill.EscKey.Set(SkillEscKey.None);
			}

			return true;
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