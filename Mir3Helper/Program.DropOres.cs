namespace Mir3Helper
{
	using System;
	using System.Diagnostics;
	using System.Threading.Tasks;

	partial class Program
	{
		async Task DropOres()
		{
			while (true)
			{
				foreach (var process in Process.GetProcessesByName(Game.ProcessName))
				{
					await Task.Delay(TimeSpan.FromSeconds(Random.Next(5, 10)));
					var game = new Game(process);
					int count = await game.DropOres();
					Console.WriteLine($"{DateTime.Now} {game.Name} DropOres {count}");
				}

				await Task.Delay(TimeSpan.FromHours(2 + Random.NextDouble()));
			}
		}
	}
}