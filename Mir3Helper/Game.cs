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
			Memory = new Memory(process);
			Window = new Window(process);
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

		public void Cast(ushort magic, int target = 0)
		{
			if (magic >= 1 && magic <= 12)
			{
				if (target != 0)
				{
					MagicTarget.Set(target);
					MagicTargetAlt.Set(target);
				}

				Window.SendKey(VirtualKey.VK_F1 + magic - 1);
			}
		}

		public string PlayerName => Memory.ReadString(0x0069DF00, 12);
		public int PlayerId => Memory.ReadInt32(0x007A8024);
		public ushort Hp => Memory.ReadUInt16(0x007A82A2);
		public ushort Mp => Memory.ReadUInt16(0x007A82A6);
		public int MaxHp => Memory.ReadInt32(0x007D8054);
		public int MaxMp => Memory.ReadInt32(0x007D8058);
		public Point Pos => Memory.ReadPoint(Memory["kingmir3.dll+1141C0"]);
		public MemoryValue<int> AttackTarget => Memory.ValueAddress(0x007AC638);
		public MemoryValue<int> MagicTarget => Memory.ValueAddress(0x007AC63C);
		public MemoryValue<int> MagicTargetAlt => Memory.ValueAddress(0x007AC640);

		public int BuffDefMagic => Memory.ReadInt32(0x007D53C4);
		public int BuffAtk => Memory.ReadInt32(0x007D53C8);
		public int BuffDef => Memory.ReadInt32(0x007D53CC);
		public int BuffDefFire => Memory.ReadInt32(0x007D53D0);
		public int BuffDefIce => Memory.ReadInt32(0x007D53D4);
		public int BuffDefThunder => Memory.ReadInt32(0x007D53D8);
		public int BuffDefWind => Memory.ReadInt32(0x007D53DC);
		public int BuffAtkMagic => Memory.ReadInt32(0x007D53E0);
		public int BuffAtkFire => Memory.ReadInt32(0x007D53E4);
		public int BuffAtkIce => Memory.ReadInt32(0x007D53E8);
		public int BuffAtkThunder => Memory.ReadInt32(0x007D53EC);
		public int BuffAtkWind => Memory.ReadInt32(0x007D53F0);
		public int BuffAtkHoly => Memory.ReadInt32(0x007D53F4);

		public bool StatusOpened => Memory.ReadBoolean(0x0076F92C);
		public Point StatusClose => Memory.ReadPoint(0x0076F9AC);
		public Point StatusLeftRing => StatusClose + (-256, 215);
		public bool InventoryOpened => Memory.ReadBoolean(0x006F3260);
		public Point InventoryClose => Memory.ReadPoint(0x006F5878);
		public Point InventoryAction => InventoryClose + (-25, 10);
		public byte TotalOpened => Memory.ReadByte(0x006EF680);
	}
}