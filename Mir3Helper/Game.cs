namespace Mir3Helper
{
	using System;
	using System.Diagnostics;
	using System.Threading.Tasks;
	using static PInvoke.User32;

	public sealed partial class Game : IDisposable
	{
		public Process Process { get; }
		public Memory Memory { get; }
		public Window Window { get; }

		public Game(Process process)
		{
			Process = process;
			Memory = new Memory(process);
			Window = new Window(process);
		}

		public void Dispose() => Process.Dispose();

		public async Task CoupleTeleport(bool send = false)
		{
			bool opened = StatusOpened;
			if (opened)
			{
				Window.Key(VirtualKey.VK_Q, send);
				await Task.Delay(20);
			}

			Window.Key(VirtualKey.VK_Q, send);
			await Task.Delay(20);
			Window.DoubleClick(StatusLeftRing, send);
			if (!opened) Window.Key(VirtualKey.VK_Q, send);
		}

		public void CastAssistMagic(ushort magic, int target = 0)
		{
			if (magic >= 1 && magic <= 12)
			{
				if (target != 0) MagicTarget.Set(target);
				Window.Key(VirtualKey.VK_F1 - 1 + magic);
			}
		}

		public void ClickItemAndInventoryAction(bool send = false)
		{
			if (!InventoryOpened) return;
			GetCursorPos(out var pos);
			ScreenToClient(Process.MainWindowHandle, ref pos);
			Window.Click(pos, send);
			Window.Click(InventoryAction, send);
		}
	}
}