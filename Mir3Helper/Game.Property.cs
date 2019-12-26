namespace Mir3Helper
{
	partial class Game
	{
		int m_Id;

		public UnitData Self => Memory.Value(0x7A8020);
		public int Id => m_Id != 0 ? m_Id : m_Id = Self.Id;
		public string Name => Self.Name;
		public Point Pos => Self.Pos;
		public bool Casting => Self.Casting;
		public bool Attacking => Self.Attacking;
		public Skill LastSkill => Self.LastSkill;
		public int MaxHp => Self.MaxHp;
		public int Hp => Self.Hp;
		public int MaxMp => Memory.Read<ushort>(0x7D8058);
		public int Mp => Self.Mp;
		public bool Hiding => Self.Hiding;
		public bool GreenPoison => Self.GreenPoison;
		public bool RedPoison => Self.RedPoison;
		public bool Moving => Self.Moving;
		public bool Mounting => Memory.Read<bool>(0x7AAF68);

		public string Map => Memory.ReadString(0x6AD96D, 32);
		public int SkillCount => Memory.Read<byte>(0x796EB0);
		public string CoupleName => Memory.ReadString(0x7AAF00, 12);
		public int Level => Memory.Read<ushort>(0x7AC451);
		public UInt64Pair Exp => Memory.Read<UInt64Pair>(0x7AC438);
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
		public PlayerClass Class => Memory.Read<PlayerClass>(0x7AC4B3);
		public int Gold => Memory.Read<int>(0x7AC4B4);
		public MemoryValue<int> EscKeyTime => Memory.Value(0x66AB14);
		public MemoryValue<AttackMode> AttackMode => Memory.Value(0x7AC5D5);
		public MemoryValue<int> AttackTarget => Memory.Value(0x7AC638);
		public MemoryValue<int> SkillTarget => Memory.Value(0x7AC63C);
		public MemoryValue<int> SkillTargetPlayerOnly => Memory.Value(0x7AC640);
		public MemoryValue<Int32Pair> MousePos => Memory.Value(0x7ADA44);
		public BagData Bag => new BagData(Memory);
		public BuffData Buff => new BuffData(Memory);

		public int AttackElement(Element element)
		{
			if (element < Element.Fire || element > Element.Phantom) return 0;
			return Memory.Read<ushort>(0x7AC48D + (int) element * sizeof(ushort));
		}

		public int ResistElement(Element element)
		{
			if (element < Element.Fire || element > Element.Phantom) return 0;
			int resist = Memory.Read<byte>(0x7AC49C + (int) element);
			if (resist == 0) resist = -Memory.Read<byte>(0x7AC4A3 + (int) element);
			return resist;
		}

		public Address UnitAddress(int x, int y) => Memory[0x6BACEC + x * 0x78 + y * 0xD98, 0];
		public Point MapToScreen(Point mapPos) => (mapPos - Pos).Scale((48, 32)) + SelfScreenPos;
		public ItemData PickUpItem => (Memory, 0x6EEF52);
		public ItemData WeaponItem => (Memory, 0x76FF12);

		public bool FullScreen => Memory.Read<bool>(0x6658D6);
		public Point ScreenSize => Memory.Read<Int32Pair>(0x6658D8);
		public Point SelfScreenPos => ScreenSize.X == 800 ? (400, 240) : (496, 338);
		public int TotalOpened => Memory.Read<byte>(0x6EF680);
		public bool StatusOpened => Memory.Read<bool>(0x76F92C);
		Point StatusCloseButtonTopLeft => Memory.Read<Int32Pair>(0x76F9AC);
		public Point StatusLeftRingPos => StatusCloseButtonTopLeft + (-255, 215);
		public Point StatusWeaponPos => StatusCloseButtonTopLeft + (-215, 160);
		public bool SkillOpened => Memory.Read<bool>(0x7956A0);
		public bool MiniMapOpened => Memory.Read<bool>(0x776AA4);
		public bool MiniMapDoubled => Memory.Read<bool>(0x776B38);
		public bool MiniMapAllShown => Memory.Read<bool>(0x776B39);
		public bool ItemShortcutOpened => Memory.Read<bool>(0x78FE40);
		public bool MailOpened => Memory.Read<bool>(0x792414);
		public Point YesButton => ScreenSize / 2 - (70, 10);
	}
}