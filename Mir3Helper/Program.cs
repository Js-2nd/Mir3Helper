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
					await Input.GetKeyDown(User32.VirtualKey.VK_PRIOR);
					var mir3 = Game.OpenForeground();
					if (mir3 == null) continue;
					using (mir3)
					{
						Console.WriteLine($"{mir3.Hp} {mir3.Mp}");
					}
				}
			}
		}
	}
}