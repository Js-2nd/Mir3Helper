namespace Mir3Helper
{
	using System.Collections.Generic;

	partial class Game
	{
		string m_NameCache;
		int? m_IdCache;
		Point? m_PosCache;

		public void ClearCache(bool all = false)
		{
			if (all)
			{
				m_NameCache = null;
				m_IdCache = null;
			}

			m_PosCache = null;
		}

		public string Name => m_NameCache ?? (m_NameCache = Memory.ReadString(0x0069DF00, 12));
		public int Id => m_IdCache ?? (m_IdCache = Memory.ReadInt32(0x007A8024)).Value;
		public int Hp => Memory.ReadUInt16(0x007A82A2);
		public int Mp => Memory.ReadUInt16(0x007A82A6);
		public int MaxHp => Memory.ReadInt32(0x007D8054);
		public int MaxMp => Memory.ReadInt32(0x007D8058);
		public Point Pos => m_PosCache ?? (m_PosCache = Memory.ReadPoint(Memory["kingmir3.dll+1141C0"])).Value;
		public bool Moving => Memory.ReadBoolean(0x007A82C9);
		public bool Casting => Memory.ReadBoolean(0x007A8298);
		public bool Hiding => (Memory.ReadByte(0x007A82B2) & 0x80) != 0;
		public MemoryValue<int> AttackTarget => Memory.ValueAddress(0x007AC638);
		public MemoryValue<int> MagicTarget => Memory.ValueAddress(0x007AC63C);
		public MemoryValue<int> MagicTargetAlt => Memory.ValueAddress(0x007AC640);

		public int BuffAtk => Memory.ReadInt32(0x007D53C8);
		public int BuffAtkMagic => Memory.ReadInt32(0x007D53E0);
		public int BuffAtkFire => Memory.ReadInt32(0x007D53E4);
		public int BuffAtkIce => Memory.ReadInt32(0x007D53E8);
		public int BuffAtkThunder => Memory.ReadInt32(0x007D53EC);
		public int BuffAtkWind => Memory.ReadInt32(0x007D53F0);
		public int BuffAtkHoly => Memory.ReadInt32(0x007D53F4);
		public int BuffDef => Memory.ReadInt32(0x007D53CC);
		public int BuffDefMagic => Memory.ReadInt32(0x007D53C4);
		public int BuffDefFire => Memory.ReadInt32(0x007D53D0);
		public int BuffDefIce => Memory.ReadInt32(0x007D53D4);
		public int BuffDefThunder => Memory.ReadInt32(0x007D53D8);
		public int BuffDefWind => Memory.ReadInt32(0x007D53DC);

		public IEnumerable<int> AllBuffAtkMagic()
		{
			yield return BuffAtkMagic;
			yield return BuffAtkFire;
			yield return BuffAtkIce;
			yield return BuffAtkThunder;
			yield return BuffAtkWind;
			yield return BuffAtkHoly;
		}

		public IEnumerable<int> AllBuffDefMagic()
		{
			yield return BuffDefMagic;
			yield return BuffDefFire;
			yield return BuffDefIce;
			yield return BuffDefThunder;
			yield return BuffDefWind;
		}

		public bool StatusOpened => Memory.ReadBoolean(0x0076F92C);
		public Point StatusClose => Memory.ReadPoint(0x0076F9AC);
		public Point StatusLeftRing => StatusClose + (-255, 215);
		public bool InventoryOpened => Memory.ReadBoolean(0x006F3260);
		public Point InventoryClose => Memory.ReadPoint(0x006F5878);
		public Point InventoryAction => InventoryClose + (-25, 10);
		public byte TotalOpened => Memory.ReadByte(0x006EF680);
	}
}