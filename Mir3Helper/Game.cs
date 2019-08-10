namespace Mir3Helper
{
	using System;
	using System.Diagnostics;
	using System.Threading.Tasks;
	using static PInvoke.User32;

	public sealed class Game : IDisposable
	{
		public Process Process { get; }
		public Memory Memory { get; }
		public Window Window { get; }

		public Game(Process process)
		{
			Process = process;
			Memory = new Memory(process.Handle);
			Window = new Window(process.MainWindowHandle);
		}

		public void Dispose() => Process.Dispose();

		public async ValueTask CoupleTeleport()
		{
			bool sendQ = !StatusOpened;
			if (sendQ)
			{
				Window.SendKey(VirtualKey.VK_Q);
				await Task.Delay(10);
			}

			Window.SendDoubleClick(StatusLeftRing);
			if (sendQ) Window.SendKey(VirtualKey.VK_Q);
		}

		public MemoryString PlayerName => Memory.StringAddress(0x0069DF00, 12);
		public MemoryValue<int> PlayerId => Memory.ValueAddress(0x007A8024);
		public MemoryValue<ushort> Hp => Memory.ValueAddress(0x007A82A2);
		public MemoryValue<ushort> Mp => Memory.ValueAddress(0x007A82A6);
		public MemoryValue<int> MaxHp => Memory.ValueAddress(0x007D8054);
		public MemoryValue<int> MaxMp => Memory.ValueAddress(0x007D8058);
		public MemoryPoint Pos => Memory.PointAddress(0x030341C0);
		public WritableMemoryValue<int> AttackTarget => Memory.ValueAddress(0x007AC638);
		public WritableMemoryValue<int> MagicTarget => Memory.ValueAddress(0x007AC63C);
		public WritableMemoryValue<int> MagicTargetAlt => Memory.ValueAddress(0x007AC640);

		public MemoryValue<int> BuffDefMagic => Memory.ValueAddress(0x007D53C4);
		public MemoryValue<int> BuffAtk => Memory.ValueAddress(0x007D53C8);
		public MemoryValue<int> BuffDef => Memory.ValueAddress(0x007D53CC);
		public MemoryValue<int> BuffDefFire => Memory.ValueAddress(0x007D53D0);
		public MemoryValue<int> BuffDefIce => Memory.ValueAddress(0x007D53D4);
		public MemoryValue<int> BuffDefThunder => Memory.ValueAddress(0x007D53D8);
		public MemoryValue<int> BuffDefWind => Memory.ValueAddress(0x007D53DC);
		public MemoryValue<int> BuffAtkMagic => Memory.ValueAddress(0x007D53E0);
		public MemoryValue<int> BuffAtkFire => Memory.ValueAddress(0x007D53E4);
		public MemoryValue<int> BuffAtkIce => Memory.ValueAddress(0x007D53E8);
		public MemoryValue<int> BuffAtkThunder => Memory.ValueAddress(0x007D53EC);
		public MemoryValue<int> BuffAtkWind => Memory.ValueAddress(0x007D53F0);
		public MemoryValue<int> BuffAtkHoly => Memory.ValueAddress(0x007D53F4);

		public MemoryValue<bool> StatusOpened => Memory.ValueAddress(0x0076F92C);
		public MemoryPoint StatusClose => Memory.PointAddress(0x0076F9AC);
		public Point StatusLeftRing => StatusClose.Value + (-256, 215);
		public MemoryValue<bool> InventoryOpened => Memory.ValueAddress(0x006F3260);
		public MemoryPoint InventoryClose => Memory.PointAddress(0x006F5878);
		public Point InventoryAction => InventoryClose.Value + (-25, 10);
		public MemoryValue<byte> TotalOpened => Memory.ValueAddress(0x006EF680);
	}
}