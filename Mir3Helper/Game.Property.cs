namespace Mir3Helper
{
	using PInvoke;
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

		public string Name => m_NameCache ?? (m_NameCache = Memory.ReadString(0x69DF00, 12));
		public int Id => m_IdCache ?? (m_IdCache = Memory.Read<int>(0x7A8024)).Value;
		public int Hp => Memory.Read<ushort>(0x7A82A2);
		public int Mp => Memory.Read<ushort>(0x7A82A6);
		public int MaxHp => Memory.Read<int>(0x7D8054);
		public int MaxMp => Memory.Read<int>(0x7D8058);
		public Point Pos => m_PosCache ?? (m_PosCache = Memory.Read<POINT>(Module.KingMir3, 0x1141C0)).Value;
		public bool Moving => Memory.Read<bool>(0x7A82C9);
		public bool Casting => Memory.Read<bool>(0x7A8298);
		public bool Hiding => (Memory.Read<byte>(0x7A82B2) & 0x80) != 0;
		public MemoryValue<int> AttackTarget => Memory.Value(0x7AC638);
		public MemoryValue<int> MagicTarget => Memory.Value(0x7AC63C);
		public MemoryValue<int> MagicTargetAlt => Memory.Value(0x7AC640);

		public int BuffAtk => Memory.Read<int>(0x7D53C8);
		public int BuffAtkMagic => Memory.Read<int>(0x7D53E0);
		public int BuffAtkFire => Memory.Read<int>(0x7D53E4);
		public int BuffAtkIce => Memory.Read<int>(0x7D53E8);
		public int BuffAtkThunder => Memory.Read<int>(0x7D53EC);
		public int BuffAtkWind => Memory.Read<int>(0x7D53F0);
		public int BuffAtkHoly => Memory.Read<int>(0x7D53F4);
		public int BuffDef => Memory.Read<int>(0x7D53CC);
		public int BuffDefMagic => Memory.Read<int>(0x7D53C4);
		public int BuffDefFire => Memory.Read<int>(0x7D53D0);
		public int BuffDefIce => Memory.Read<int>(0x7D53D4);
		public int BuffDefThunder => Memory.Read<int>(0x7D53D8);
		public int BuffDefWind => Memory.Read<int>(0x7D53DC);

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

		public bool StatusOpened => Memory.Read<bool>(0x76F92C);
		public Point StatusClose => Memory.Read<POINT>(0x76F9AC);
		public Point StatusLeftRing => StatusClose + (-255, 215);
		public bool InventoryOpened => Memory.Read<bool>(0x6F3260);
		public Point InventoryClose => Memory.Read<POINT>(0x6F5878);
		public Point InventoryAction => InventoryClose + (-25, 10);
		public int TotalOpened => Memory.Read<byte>(0x6EF680);
	}
}