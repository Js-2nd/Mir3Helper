namespace Mir3Helper
{
	using System;
	using System.Diagnostics;
	using System.IO;
	using System.Linq;
	using System.Threading.Tasks;
	using static PInvoke.User32;

	class Program
	{
		const string Version = "0.1.0";
		const double DefaultActionDelay = 1;

		static async Task Main()
		{
			Console.SetError(new StreamWriter("error.txt", true) {AutoFlush = true});
			AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
				Console.Error.WriteLine(args.ExceptionObject);
			await new Program().Start();
		}

		InputSystem m_Input;
		Game m_User;
		Game m_Assist;
		bool m_Valid;
		bool m_Same;
		bool m_Running;
		int m_Distance;
		DateTime m_TeleportTime;

		async Task Start()
		{
			Console.WriteLine($@"传奇3助手 v{Version}

[PageUp]设置当前窗口为玩家角色（法师）
[PageDown]设置当前窗口为辅助角色（道士）
[End] 运行/暂停
[`] 手动夫妻传送

辅助角色（道士）需按如下设置
F1治愈术 F2隐身术 F7幽灵盾 F8神圣战甲术 F9强震魔法
以上魔法需设置为锁人不锁怪
");
			Task.Run(HandleInput).Catch();
			while (true)
			{
				try
				{
					if (m_Valid && m_Running) await Task.Delay(TimeSpan.FromSeconds(await Update()));
					else await Task.Delay(100);
				}
				catch (Exception ex)
				{
					Console.Error.WriteLine(ex);
				}
			}
		}

		async Task HandleInput()
		{
			m_Input = new InputSystem();
			while (true)
			{
				try
				{
					var key = await m_Input.GetKeyDown();
					if (key == VirtualKey.VK_PRIOR)
					{
						TrySetToForeground(ref m_User);
						if (m_User != null) Console.WriteLine($"玩家角色 => {m_User.Name}");
					}
					else if (key == VirtualKey.VK_NEXT)
					{
						TrySetToForeground(ref m_Assist);
						if (m_Assist != null) Console.WriteLine($"辅助角色 => {m_Assist.Name}");
					}
					else if (key == VirtualKey.VK_END)
					{
						m_Running = !m_Running;
						Console.WriteLine(m_Running ? "运行" : "暂停");
					}
					else if (key == VirtualKey.VK_OEM_3)
					{
						if (m_Assist != null) await m_Assist.CoupleTeleport();
					}
					else if (key == VirtualKey.VK_RSHIFT)
					{
						Game game = null;
						if (TrySetToForeground(ref game)) game.ClickItemAndInventoryAction();
					}
					else if (key == VirtualKey.VK_RCONTROL)
					{
						m_Assist?.Foo();
					}
				}
				catch (Exception ex)
				{
					Console.Error.WriteLine(ex);
				}
			}
		}

		bool TrySetToForeground(ref Game game)
		{
			var process = GetForegroundProcess();
			if (process.ProcessName.ToLowerInvariant() != "mir3" || game?.Process.Id == process.Id) return false;
			game = new Game(process);
			m_Valid = m_User != null && m_Assist != null;
			m_Same = m_User?.Process.Id == m_Assist?.Process.Id;
			return true;
		}

		Process GetForegroundProcess()
		{
			var window = GetForegroundWindow();
			GetWindowThreadProcessId(window, out int processId);
			return Process.GetProcessById(processId);
		}

		async Task<double> Update()
		{
			if (m_Assist.Moving || m_Assist.Casting) return 0.1;
			m_Distance = -1;
			var now = DateTime.UtcNow;
			if (now >= m_TeleportTime && Distance >= 5)
			{
				Console.WriteLine("夫妻传送");
				await m_Assist.CoupleTeleport();
				m_TeleportTime = now + TimeSpan.FromSeconds(3.5);
				return 2.5;
			}

			if (!m_Same && !m_Assist.Hiding)
			{
				Console.WriteLine("隐身术");
				m_Assist.CastAssistMagic(2, m_Assist.Id);
				return DefaultActionDelay;
			}

			if (Distance <= 9 && m_User.Hp > 0)
			{
				if (m_User.AllBuffAtkMagic().All(buff => buff < 3))
				{
					Console.WriteLine("强震魔法");
					m_Assist.CastAssistMagic(9, m_User.Id);
					return DefaultActionDelay;
				}
			}

			if (m_Assist.Hp < m_Assist.MaxHp - 20)
			{
				Console.WriteLine($"治愈术");
				m_Assist.CastAssistMagic(1, m_Assist.Id);
				return DefaultActionDelay;
			}

			if (Distance <= 9 && m_User.Hp > 0)
			{
				if (m_User.BuffDef < 5)
				{
					Console.WriteLine("神圣战甲术");
					m_Assist.CastAssistMagic(8, m_User.Id);
					return DefaultActionDelay;
				}

				if (m_User.AllBuffDefMagic().All(t => t < 5))
				{
					Console.WriteLine("幽灵盾");
					m_Assist.CastAssistMagic(7, m_User.Id);
					return DefaultActionDelay;
				}

				if (!m_Same && m_User.Hp < m_User.MaxHp - 30)
				{
					Console.WriteLine($"治愈术");
					m_Assist.CastAssistMagic(1, m_User.Id);
					return DefaultActionDelay;
				}
			}

			return 0.5;
		}

		int Distance => m_Same ? 0 :
			m_Distance >= 0 ? m_Distance :
			m_Distance = Point.ManhattanDistance(m_User.Pos, m_Assist.Pos);
	}
}