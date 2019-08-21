namespace Mir3Helper
{
	using System;
	using System.IO;
	using System.Threading.Tasks;
	using static PInvoke.User32;

	public sealed partial class Program
	{
		public const string Version = "0.2.1";

		static async Task Main()
		{
			Console.WriteLine($"Mir3Helper v{Version}");
			Console.SetError(new StreamWriter("error.txt", true) {AutoFlush = true});
			AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
				Console.Error.WriteLine(args.ExceptionObject);
			await new Program().Start();
		}

		InputSystem m_Input;
		bool m_Running;
		bool m_HasUser;
		Game m_User;
		Game m_Assist;
		Game m_Temp;

		async Task Start()
		{
			m_Input = new InputSystem();
			m_Input.KeyDown += HandleInput;
			Console.WriteLine("[PageUp] Set User");
			Console.WriteLine("[PageDown] Set Assist");
			Console.WriteLine("[End] Run / Pause");
			Console.WriteLine("[`] Assist CoupleWarp");
			Console.WriteLine("[RightControl] ClickItemWithBagAction");
			Console.WriteLine("[Shift+A] DropItem");
			Console.WriteLine("[Shift+X] ClickItemWithSendMail");
			while (true)
			{
				try
				{
					if (CheckExit(ref m_User)) Console.WriteLine("User exited");
					if (CheckExit(ref m_Assist))
					{
						Console.WriteLine("Assist exited");
						if (m_Running)
						{
							m_Running = false;
							Console.WriteLine("Pause");
						}
					}

					await Task.Delay(TimeSpan.FromSeconds(m_Running && m_Assist != null ? await Update() : 0.1));
				}
				catch (Exception ex)
				{
					Console.Error.WriteLine(ex);
				}
			}
		}

		void HandleInput(VirtualKey key)
		{
			if (key == VirtualKey.VK_PRIOR)
			{
				if (Game.GetForeground(ref m_User) >= 0)
				{
					OnGameChange();
					Console.WriteLine($"User => {m_User.Name}");
				}
			}
			else if (key == VirtualKey.VK_NEXT)
			{
				if (Game.GetForeground(ref m_Assist) >= 0)
				{
					OnGameChange();
					Console.WriteLine($"Assist => {m_Assist.Name}");
				}
			}
			else if (key == VirtualKey.VK_END)
			{
				if (!m_Running && m_Assist == null)
				{
					Console.WriteLine("Assist not set");
				}
				else
				{
					m_Running = !m_Running;
					Console.WriteLine(m_Running ? "Run" : "Pause");
				}
			}
			else if (key == VirtualKey.VK_OEM_3)
			{
				m_Assist?.CoupleWarp();
			}
			else if (key == VirtualKey.VK_RCONTROL)
			{
				if (Game.GetForeground(ref m_Temp) >= 0) m_Temp.ClickItemWithBagAction();
			}
			else if (key == VirtualKey.VK_A && m_Input.IsShiftDown())
			{
				if (Game.GetForeground(ref m_Temp) >= 0) m_Temp.DropItem();
			}
			else if (key == VirtualKey.VK_X && m_Input.IsShiftDown())
			{
				if (Game.GetForeground(ref m_Temp) >= 0) m_Temp.ClickItemWithSendMail();
			}
		}

		bool CheckExit(ref Game game)
		{
			if (game == null || !game.Process.HasExited) return false;
			game = null;
			OnGameChange();
			return true;
		}

		void OnGameChange()
		{
			m_HasUser = m_User != null && m_Assist != null && m_User.Process.Id != m_Assist.Process.Id;
		}
	}
}