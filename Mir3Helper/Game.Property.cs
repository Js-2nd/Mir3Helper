namespace Mir3Helper
{
	partial class Game
	{
		int m_Id;

		public UnitData Self => Memory.Value(0x76CA70);
		public int Id => m_Id != 0 ? m_Id : m_Id = Self.Id;
		public string Name => Self.Name;
		public Point Pos => Self.Pos;
		public bool Casting => Self.Casting;
		public bool Attacking => Self.Attacking;
		public Skill LastSkill => Self.LastSkill;
		public int MaxHp => Self.MaxHp;
		public int Hp => Self.Hp;
		public int MaxMp => Memory.Read<ushort>(0x7A2758);
		public int Mp => Self.Mp;
		public bool Hiding => Self.Hiding;
		public bool GreenPoison => Self.GreenPoison;
		public bool RedPoison => Self.RedPoison;
		public bool Moving => Self.Moving;
		public int Mounted => Memory.Read<byte>(0x771DB0); // TODO enum

		public string Map => Memory.ReadString(0x67A950, 32);
//		public int SkillCount => Memory.Read<byte>(0x796EB0);
		public string CoupleName => Memory.ReadString(0x771D48, 12);
		public int Level => Memory.Read<ushort>(0x773601);
		public UInt64Pair Exp => Memory.Read<UInt64Pair>(0x7735E8);
		public Int16Pair Attack => Memory.Read<Int16Pair>(0x77360B);
		public Int16Pair Magic => Memory.Read<Int16Pair>(0x77360F);
		public Int16Pair Soul => Memory.Read<Int16Pair>(0x773613);
		public Int16Pair Defense => Memory.Read<Int16Pair>(0x773603);
		public Int16Pair Resist => Memory.Read<Int16Pair>(0x773607);
		public Int32Pair BagWeight => Memory.Read<Int32Pair>(0x77362F);
		public Int16Pair EquipWeight => Memory.Read<Int16Pair>(0x773637);
		public Int16Pair HandWeight => Memory.Read<Int16Pair>(0x77363B);
		public int Accuracy => Memory.Read<byte>(0x77365D);
		public int Dexterity => Memory.Read<byte>(0x77365E);
		public PlayerClass Class => Memory.Read<PlayerClass>(0x7AC4B3);
		public int Gold => Memory.Read<int>(0x773664);
		public MemoryValue<int> EscKeyTime => Memory.Value(0x637B94);
		public MemoryValue<AttackMode> AttackMode => Memory.Value(0x773785);
		public MemoryValue<int> AttackTarget => Memory.Value(0x7737E8);
		public MemoryValue<int> SkillTarget => Memory.Value(0x7737EC);
		public MemoryValue<int> SkillTargetPlayerOnly => Memory.Value(0x7737F0);
		public MemoryValue<Int32Pair> MouseScreenPos => Memory.Value(0x775C2C);
		public MemoryValue<Int32Pair> MouseWorldPos => Memory.Value(0x775C34);
		public BagData Bag => new BagData(this);
		public BuffData Buff => new BuffData(Memory);

		public int AttackElement(Element element)
		{
			if (element < Element.Fire || element > Element.Phantom) return 0;
			return Memory.Read<ushort>(0x77363F + (int) element * sizeof(ushort));
		}

		public int ResistElement(Element element)
		{
			if (element < Element.Fire || element > Element.Phantom) return 0;
			int resist = Memory.Read<byte>(0x77364D + (int) element);
			if (resist == 0) resist = -Memory.Read<byte>(0x773654 + (int) element);
			return resist;
		}

		public Address UnitAddress(int x, int y) => Memory[0x687CCC + x * 0x78 + y * 0xD98, 0];
		public Point MapToScreen(Point mapPos) => (mapPos - Pos).Scale((48, 32)) + SelfScreenPos;
		public ItemData PickUpItem => (Memory, 0x6B0B86);
		public ItemData WeaponItem => (Memory, 0x731B4A);

//		public bool FullScreen => Memory.Read<bool>(0x6658D6);
		public Point ScreenSize => Memory.Read<Int32Pair>(0x6345A0);
		public Point SelfScreenPos => ScreenSize.X == 800 ? (400, 240) : (496, 338);
//		public int TotalOpened => Memory.Read<byte>(0x6EF680);
		public bool StatusOpened => Memory.Read<bool>(0x731564);
		Point StatusAnchorPos => Memory.Read<Int32Pair>(0x73154C);
		public Point StatusLeftRingPos => StatusAnchorPos + (125, 250);
		public Point StatusWeaponPos => StatusAnchorPos + (165, 195);
		public bool SkillOpened => Memory.Read<bool>(0x75A0E0);
//		public bool MiniMapOpened => Memory.Read<bool>(0x776AA4);
//		public bool MiniMapDoubled => Memory.Read<bool>(0x776B38);
//		public bool MiniMapAllShown => Memory.Read<bool>(0x776B39);
//		public bool ItemShortcutOpened => Memory.Read<bool>(0x78FE40);
		public bool MailOpened => Memory.Read<bool>(0x756C40);
		public Point YesButton => ScreenSize / 2 - (70, 10);
	}
}