namespace Mir3Helper
{
	using Memory;
	using PInvoke;
	using System;

	public sealed class Game : IDisposable
	{
		public static Game OpenForeground()
		{
			var window = User32.GetForegroundWindow();
			User32.GetWindowThreadProcessId(window, out int processId);
			return Open(processId);
		}

		public static Game Open(int processId)
		{
			var memory = new Mem();
			if (memory.OpenProcess(processId) && memory.theProc.ProcessName == "mir3")
			{
				Console.WriteLine($"[Game] {memory.theProc.MainWindowTitle}");
				return new Game(memory);
			}

			return null;
		}

		public Mem Memory { get; }

		Game(Mem memory) => Memory = memory;
		public void Dispose() => Memory.closeProcess();

		public int Hp => Memory.read2Byte("mir3.exe+3A82A2");
		public int Mp => Memory.read2Byte("mir3.exe+3A82A6");
	}
}