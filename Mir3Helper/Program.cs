namespace Mir3Helper
{
	using PInvoke;
	using System;
	using System.Threading.Tasks;

	public static class Program
	{
		public static InputSystem Input { get; private set; }

		public static async Task Main()
		{
			using (Input = new InputSystem())
			{
				while (true)
				{
					var key = await Input.GetKeyDown();
					if (key == User32.VirtualKey.VK_ESCAPE) break;
					var window = User32.GetForegroundWindow();
					string title = User32.GetWindowText(window);
					Console.WriteLine(title);
				}
			}
		}
	}
}