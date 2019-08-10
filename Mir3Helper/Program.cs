namespace Mir3Helper
{
	using System;
	using System.Diagnostics;
	using System.Threading.Tasks;
	using static PInvoke.User32;

	public static class Program
	{
		public static async Task Main()
		{
			using (var input = new InputSystem())
			{
				Game user = null;
				Game assist = null;
				bool ready = false;
				while (true)
				{
					var key = await input.GetKeyDown();
					if (key == VirtualKey.VK_PRIOR)
					{
						if (TryGetForegroundGame(ref user))
						{
							ready = assist != null && user.Process.Id != assist.Process.Id;
							Console.WriteLine($"User={user.PlayerName}");
						}
					}
					else if (key == VirtualKey.VK_NEXT)
					{
						if (TryGetForegroundGame(ref assist))
						{
							ready = user != null && user.Process.Id != assist.Process.Id;
							Console.WriteLine($"Assist={assist.PlayerName}");
						}
					}

					if (!ready) continue;
					if (key == VirtualKey.VK_OEM_3)
					{
//						await assist.CoupleTeleport();
						assist.MagicTargetAlt.Set(user.PlayerId);
						assist.Window.SendKey(VirtualKey.VK_F1);
					}
				}
			}
		}

		static bool TryGetForegroundGame(ref Game game)
		{
			var process = GetForegroundProcess();
			if (process.ProcessName != "mir3" || game?.Process.Id == process.Id) return false;
			game = new Game(process);
			return true;
		}

		static Process GetForegroundProcess()
		{
			var window = GetForegroundWindow();
			GetWindowThreadProcessId(window, out int processId);
			return Process.GetProcessById(processId);
		}
	}
}