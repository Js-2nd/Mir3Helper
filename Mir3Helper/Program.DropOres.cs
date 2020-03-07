namespace Mir3Helper
{
	using System;
	using System.Diagnostics;
	using System.Threading.Tasks;

	partial class Program
	{
		async Task DropOres()
		{
			await Task.Delay(TimeSpan.FromSeconds(5));
			while (true)
			{
				Console.WriteLine($"{DateTime.Now} [DropOres] Start");
				foreach (var process in Process.GetProcessesByName(Game.ProcessName))
				{
					var game = new Game(process);
					int count = await game.DropOres();
					if (count >= 0) Console.WriteLine($"{DateTime.Now} [DropOres] {game.Name} => {count}");
				}

				var delay = TimeSpan.FromHours(1 + Random.NextDouble());
				Console.WriteLine($"Next DropOres Time: {DateTime.Now + delay}");
				await Task.Delay(delay);
			}
		}
	}
}