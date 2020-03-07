namespace Mir3Helper
{
	using System;
	using System.IO;
	using System.Threading;
	using System.Threading.Tasks;
	using static PInvoke.User32;

	public sealed partial class Program
	{
		public const string Version = "0.4.0";
		public static bool DebugOutput;
		public static Random Random => s_Random.Value;
		static readonly ThreadLocal<Random> s_Random = new ThreadLocal<Random>(() => new Random());

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
		bool m_AutoWarp = true;
		bool m_HasUser;
		AssistMode m_AssistMode;
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
			Console.WriteLine("[Insert] Change Assist Mode");
			Console.WriteLine("[`] Assist Warp");
			Console.WriteLine("[Shift+`] Toggle Auto Assist Warp");
			Console.WriteLine("[RightControl] Bag Action With Mouse Item (Repair/Save/Sell)");
			Console.WriteLine("[Delete] Drop Mouse Item");
			Console.WriteLine("[Shift+S] Send Mail With Mouse Item");
			StartTasks();
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

					double delay = 0.2;
					if (m_Running && m_Assist != null)
					{
						var action = await Update();
						if (action == UpdateAction.Warp) delay = 1.7;
						else if (action == UpdateAction.Skill) delay = 1.3;
						else if (action == UpdateAction.LongSkill) delay = 2.1;
					}

					await Task.Delay(TimeSpan.FromSeconds(delay));
					GC.Collect();
				}
				catch (Exception ex)
				{
					Console.Error.WriteLine(ex);
				}
			}
		}

		void StartTasks()
		{
			Task.Run(DropOres);
		}

		void HandleInput(VirtualKey key)
		{
			if (key == VirtualKey.VK_PRIOR)
			{
				if (Game.GetForeground(ref m_User) >= 0)
				{
					OnGameChange(ref m_User);
					Console.WriteLine($"User => {m_User.Name}");
				}
			}
			else if (key == VirtualKey.VK_NEXT)
			{
				if (Game.GetForeground(ref m_Assist) >= 0)
				{
					OnGameChange(ref m_Assist);
					Console.WriteLine($"Assist => {m_Assist.Name}");
				}
			}
			else if (key == VirtualKey.VK_END)
			{
				if (!m_Running && m_Assist == null) Console.WriteLine("Assist not set");
				else
				{
					m_Running = !m_Running;
					Console.WriteLine(m_Running ? "Run" : "Pause");
				}
			}
			else if (key == VirtualKey.VK_INSERT)
			{
				m_AssistMode = (AssistMode) (((int) m_AssistMode + 1) % 3);
				Console.WriteLine($"Assist Mode => {(int) m_AssistMode} ({m_AssistMode.ToString()})");
			}
			else if (key == VirtualKey.VK_OEM_3)
			{
				if (m_Input.IsShiftDown())
				{
					m_AutoWarp = !m_AutoWarp;
					Console.WriteLine($"Auto Assist Warp => {m_AutoWarp.ToString()}");
				}
				else m_Assist?.CoupleWarp();
			}
			else if (key == VirtualKey.VK_RCONTROL)
			{
				if (Game.GetForeground(ref m_Temp) >= 0) m_Temp.BagActionWithMouseItem();
			}
			else if (key == VirtualKey.VK_S && m_Input.IsShiftDown())
			{
				if (Game.GetForeground(ref m_Temp) >= 0) m_Temp.SendMailWithMouseItem();
			}
			else if (key == VirtualKey.VK_DELETE)
			{
				if (Game.GetForeground(ref m_Temp) >= 0) m_Temp.DropMouseItem();
			}
			else if (key == VirtualKey.VK_PAUSE)
			{
				DebugOutput = !DebugOutput;
				Console.WriteLine($"Debug Output => {DebugOutput.ToString()}");
			}
		}

		bool CheckExit(ref Game game)
		{
			if (game == null || !game.Process.HasExited) return false;
			game = null;
			OnGameChange(ref game);
			return true;
		}

		void OnGameChange(ref Game game)
		{
			game?.Init();
			m_HasUser = m_User != null && m_Assist != null && m_User.Process.Id != m_Assist.Process.Id;
		}
	}
}