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

		public void Init()
		{
			m_Id = 0;
			m_SkillsCached = false;
			Update();
		}

		public void Update()
		{
			m_UnitsCached = false;
			if (m_SkillsCached && SkillOpened) m_SkillsCached = false;
		}

		public UnitData GetUnit(int id)
		{
			if (id == Id) return Self;
			while (true)
			{
				UpdateUnits();
				if (!m_Units.TryGetValue(id, out var address)) return default;
				UnitData unit = (Memory, address);
				if (unit.Id == id) return unit;
				m_UnitsCached = false;
			}
		}

		public IEnumerable<UnitData> GetOtherUnits()
		{
			UpdateUnits();
			foreach (var address in m_Units.Values)
				yield return (Memory, address);
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
			while (true)
			{
				UpdateSkills();
				if (!m_Skills.TryGetValue(id, out int index)) return default;
				SkillData skill = (Memory, index);
				if (skill.Id == id) return skill;
				m_SkillsCached = false;
			}
		}

		public SkillData GetSkill(SkillKey key)
		{
			while (true)
			{
				UpdateSkills();
				if (!m_SkillKeys.TryGetValue(key, out int index)) return default;
				SkillData skill = (Memory, index);
				if (skill.Key == key) return skill;
				m_SkillsCached = false;
			}
		}

		public SkillData GetSkill(SkillEscKey escKey)
		{
			while (true)
			{
				UpdateSkills();
				if (!m_SkillEscKeys.TryGetValue(escKey, out int index)) return default;
				SkillData skill = (Memory, index);
				if (skill.EscKey == escKey) return skill;
				m_SkillsCached = false;
			}
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

		public bool ChangeSkillKey(Skill id, SkillKey key)
		{
			var skill = GetSkill(id);
			if (!skill.IsValid) return false;
			var oldKey = skill.Key;
			if (oldKey == key) return false;
			if (oldKey != SkillKey.None) m_SkillKeys.Remove(oldKey);
			if (key != SkillKey.None)
			{
				var oldSkill = GetSkill(key);
				if (oldSkill.IsValid) oldSkill.SetKey(SkillKey.None);
				m_SkillKeys[key] = skill.Index;
			}

			skill.SetKey(key);
			return true;
		}

		public bool ChangeSkillEscKey(Skill id, SkillEscKey escKey)
		{
			var skill = GetSkill(id);
			if (!skill.IsValid) return false;
			var oldEscKey = skill.EscKey.Value;
			if (oldEscKey == escKey) return false;
			if (oldEscKey != SkillEscKey.None) m_SkillEscKeys.Remove(oldEscKey);
			if (escKey != SkillEscKey.None)
			{
				var oldSkill = GetSkill(escKey);
				if (oldSkill.IsValid) oldSkill.EscKey.Set(SkillEscKey.None);
				m_SkillEscKeys[escKey] = skill.Index;
			}

			skill.EscKey.Set(escKey);
			return true;
		}

		public bool TryCastSkill(Skill id, int target = 0,
			SkillPoison poison = SkillPoison.None, SkillAmulet amulet = SkillAmulet.None) =>
			TryCastSkill(id, target == 0 ? Self : GetUnit(target), poison, amulet);

		public bool TryCastSkill(Skill id, in UnitData target,
			SkillPoison poison = SkillPoison.None, SkillAmulet amulet = SkillAmulet.None)
		{
			if (!target.IsValid) return false;
			var skill = GetSkill(id);
			if (!skill.IsValid) return false;
			MousePos.Set(MapToScreen(target.Pos).ToInt32Pair());
			switch (skill.TargetLock.Value)
			{
				case SkillTargetLock.Any:
					SkillTarget.Set(target.Id);
					break;
				case SkillTargetLock.PlayerOnly:
					SkillTargetPlayerOnly.Set(target.Id);
					break;
			}

			if (poison != SkillPoison.None) skill.Poison.Set(poison);
			if (amulet != SkillAmulet.None) skill.Amulet.Set(amulet);
			var key = skill.Key;
			if (key != SkillKey.None) Window.KeyDown(key.ToVirtualKey());
			else
			{
				var escKey = skill.EscKey.Value;
				if (escKey == SkillEscKey.None) ChangeSkillEscKey(id, escKey = SkillEscKey.F12);
				EscKeyTime.Set(Environment.TickCount - 500);
				Window.KeyDown(escKey.ToVirtualKey());
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
				Window.KeyDown(VirtualKey.VK_Q, send);
				await Task.Delay(20);
			}

			Window.KeyDown(VirtualKey.VK_Q, send);
			await Task.Delay(20);
			Window.DoubleClick(StatusLeftRing, send);
			if (!opened)
			{
				await Task.Delay(20);
				Window.KeyDown(VirtualKey.VK_Q, send);
			}

			CoupleWarping = false;
		}

		public void SendMailWithMouseItem(bool send = false)
		{
			if (!MailOpened || !BagOpened) return;
			Window.Click(Window.MousePos, send);
			Window.Click((300, 150), send);
			Window.Click((310, 270), send);
		}

		public void BagActionWithMouseItem(bool send = false)
		{
			if (!BagOpened) return;
			Window.Click(Window.MousePos, send);
			Window.Click(BagAction, send);
		}

		public void DropMouseItem(bool send = false)
		{
			if (!BagOpened) return;
			Window.Click(Window.MousePos, send);
			Window.Click(YesButton, send);
		}
	}
}