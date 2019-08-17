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

		uint UnitAddress(int x, int y) => Memory.Read<uint>((uint) (0x6BACEC + x * 0x78 + y * 0xD98));

		public void Foo(int range = 9)
		{
			for (int x = -range; x <= range; x++)
			{
				for (int y = -range; y <= range; y++)
				{
					uint addr = UnitAddress(x, y);
					if (addr != 0)
					{
						var type = Memory.Read<UnitType>(addr);
						string name = Memory.ReadString(addr + 8, 32);
						Point pos = Memory.Read<Int16Pair>(addr + 0x1A0);
						Console.WriteLine($"{pos} => {type} {name}");
					}
				}
			}
		}
	}
}