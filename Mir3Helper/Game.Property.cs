namespace Mir3Helper
{
	using System.Collections.Generic;

	partial class Game
	{
		public string Name => Memory.ReadString(0x69DF00, 12);
		public int Id => Memory.Read<int>(0x7A8024);
		public int Hp => Memory.Read<ushort>(0x7A82A2);
		public int MaxHp => Memory.Read<int>(0x7D8054);
		public int Mp => Memory.Read<ushort>(0x7A82A6);
		public int MaxMp => Memory.Read<int>(0x7D8058);
		public string Map => Memory.ReadString(0x6AD96D, 16);
		public Point Pos => Memory.Read<Int32Pair>(Module.KingMir3, 0x1141C0);
		public bool Hiding => (Memory.Read<byte>(0x7A82B2) & 0x80) != 0;
		public bool Moving => Memory.Read<bool>(0x7A82C9);
		public bool Attacking => Memory.Read<bool>(0x7A8299);
		public bool Casting => Memory.Read<bool>(0x7A8298);
		public int LastSkill => Memory.Read<byte>(0x7A829C);
		public MemoryValue<int> AttackTarget => Memory.Value(0x7AC638);
		public MemoryValue<int> MagicTarget => Memory.Value(0x7AC63C);
		public MemoryValue<int> AssistTarget => Memory.Value(0x7AC640);

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

		public CharacterClass Class => (CharacterClass) Memory.Read<byte>(0x7AC4B3);
		public int Level => Memory.Read<ushort>(0x7AC451);
		public ulong Exp => Memory.Read<ulong>(0x7AC438);
		public ulong MaxExp => Memory.Read<ulong>(0x7AC440);
		public Int16Pair Attack => Memory.Read<Int16Pair>(0x7AC45B);
		public Int16Pair Magic => Memory.Read<Int16Pair>(0x7AC45F);
		public Int16Pair Soul => Memory.Read<Int16Pair>(0x7AC463);
		public Int16Pair Defense => Memory.Read<Int16Pair>(0x7AC453);
		public Int16Pair Resist => Memory.Read<Int16Pair>(0x7AC457);
		public Int32Pair BagWeight => Memory.Read<Int32Pair>(0x7AC47F);
		public Int16Pair EquipWeight => Memory.Read<Int16Pair>(0x7AC487);
		public Int16Pair HandWeight => Memory.Read<Int16Pair>(0x7AC48B);
		public int Accuracy => Memory.Read<byte>(0x7AC4AD);
		public int Dexterity => Memory.Read<byte>(0x7AC4AE);
		public int Gold => Memory.Read<int>(0x7AC4B4);

		public int AttackElement(Element element)
		{
			if (element < Element.Fire || element > Element.Phantom) return 0;
			return Memory.Read<ushort>(0x7AC48D + (uint) element * sizeof(ushort));
		}

		public int ResistElement(Element element)
		{
			if (element < Element.Fire || element > Element.Phantom) return 0;
			int resist = Memory.Read<byte>(0x7AC49C + (uint) element);
			if (resist == 0) resist = -Memory.Read<byte>(0x7AC4A3 + (uint) element);
			return resist;
		}

		public bool StatusOpened => Memory.Read<bool>(0x76F92C);
		public Point StatusClose => Memory.Read<Int32Pair>(0x76F9AC);
		public Point StatusLeftRing => StatusClose + (-255, 215);
		public bool InventoryOpened => Memory.Read<bool>(0x6F3260);
		public Point InventoryClose => Memory.Read<Int32Pair>(0x6F5878);
		public Point InventoryAction => InventoryClose + (-25, 10);
		public int TotalOpened => Memory.Read<byte>(0x6EF680);
	}
}