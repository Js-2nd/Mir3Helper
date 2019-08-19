namespace Mir3Helper
{
	using System;
	using System.IO;
	using System.Threading.Tasks;
	using static PInvoke.User32;

	public sealed partial class Program
	{
		public const string Version = "0.2.0";

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
		Game m_Temp;

		async Task Start()
		{
			m_Input = new InputSystem();
			m_Input.KeyDown += HandleInput;
			while (true)
			{
				try
				{
					if (!m_Running || m_Assist == null) await Task.Delay(100);
					else await Task.Delay(TimeSpan.FromSeconds(await Update()));
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
				if (Game.GetForeground(ref m_User) >= 0) Console.WriteLine($"User => {m_User.Name}");
			}
			else if (key == VirtualKey.VK_NEXT)
			{
				if (Game.GetForeground(ref m_Assist) >= 0) Console.WriteLine($"Assist => {m_Assist.Name}");
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
			else if (key == VirtualKey.VK_OEM_3) m_Assist?.CoupleWarp();
			else if (key == VirtualKey.VK_RCONTROL)
			{
				if (Game.GetForeground(ref m_Temp) >= 0) m_Temp.ClickWithBagAction();
			}
		}
	}
}