namespace Mir3Helper
{
	using System;
	using System.Diagnostics;
	using System.Threading.Tasks;
	using static PInvoke.User32;

	class Program
	{
		static readonly TimeSpan s_DefaultActionDelay = TimeSpan.FromSeconds(2);

		static async Task Main() => await new Program().Start();

		InputSystem m_Input;
		Game m_User;
		Game m_Assist;
		bool m_Valid;
		bool m_Running;
		int m_Distance;
		DateTime m_TeleportTime;
		DateTime m_HideTime;

		async ValueTask Start()
		{
			Task.Run(HandleInput).Forget();
			while (true)
			{
				if (m_Valid && m_Running) await Task.Delay(await Update());
				else await Task.Delay(100);
			}
		}

		async ValueTask HandleInput()
		{
			m_Input = new InputSystem();
			while (true)
			{
				var key = await m_Input.GetKeyDown();
				if (key == VirtualKey.VK_PRIOR)
				{
					if (TrySetToForeground(ref m_User)) Console.WriteLine($"User => {m_User.PlayerName}");
				}
				else if (key == VirtualKey.VK_NEXT)
				{
					if (TrySetToForeground(ref m_Assist)) Console.WriteLine($"Assist => {m_Assist.PlayerName}");
				}
				else if (key == VirtualKey.VK_END)
				{
					m_Running = !m_Running;
					Console.WriteLine(m_Running ? "Running" : "Paused");
				}
				else if (key == VirtualKey.VK_OEM_3)
				{
					await m_Assist.CoupleTeleport();
				}
			}
		}

		bool TrySetToForeground(ref Game game)
		{
			var process = GetForegroundProcess();
			if (process.ProcessName.ToLowerInvariant() != "mir3" || game?.Process.Id == process.Id) return false;
			game = new Game(process);
			m_Valid = m_User != null && m_Assist != null && m_User.Process.Id != m_Assist.Process.Id;
			return true;
		}

		Process GetForegroundProcess()
		{
			var window = GetForegroundWindow();
			GetWindowThreadProcessId(window, out int processId);
			return Process.GetProcessById(processId);
		}

		async ValueTask<TimeSpan> Update()
		{
			m_Distance = -1;
			var now = DateTime.UtcNow;
			if (now >= m_TeleportTime && Distance >= 7)
			{
				await m_Assist.CoupleTeleport();
				m_TeleportTime = now + TimeSpan.FromSeconds(3.5);
				return s_DefaultActionDelay;
			}

			if (now >= m_HideTime)
			{
				m_Assist.Cast(3, m_Assist.PlayerId);
				m_HideTime = now + TimeSpan.FromSeconds(10);
				return s_DefaultActionDelay;
			}

			if (Distance <= 9)
			{
				if (m_User.BuffAtkIce + m_User.BuffAtkWind < 5)
				{
					m_Assist.Cast(9, m_User.PlayerId);
					return s_DefaultActionDelay;
				}

				if (m_User.BuffDef < 5)
				{
					m_Assist.Cast(8, m_User.PlayerId);
					return s_DefaultActionDelay;
				}

				if (m_User.BuffDefMagic < 5)
				{
					m_Assist.Cast(7, m_User.PlayerId);
					return s_DefaultActionDelay;
				}
			}

			if (m_Assist.Hp < m_Assist.MaxHp)
			{
				m_Assist.Cast(1, m_Assist.PlayerId);
				return s_DefaultActionDelay;
			}

			if (Distance <= 9 && m_User.Hp < m_User.MaxHp)
			{
				m_Assist.Cast(1, m_User.PlayerId);
				return s_DefaultActionDelay;
			}

			return TimeSpan.FromSeconds(0.5);
		}

		int Distance => m_Distance < 0 ? m_Distance = Point.ManhattanDistance(m_User.Pos, m_Assist.Pos) : m_Distance;
	}
}