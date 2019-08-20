namespace Mir3Helper
{
	partial class Game
	{
		int? m_Id;
		string m_Name;

		public UnitData Self => Memory.Value(0x7A8020);
		public int Id => m_Id ?? (m_Id = Self.Id).Value;
		public string Name => m_Name ?? (m_Name = Self.Name);
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
		public MemoryValue<int> AttackTarget => Memory.Value(0x7AC638);
		public MemoryValue<int> SkillTarget => Memory.Value(0x7AC63C);
		public MemoryValue<int> SkillTargetPlayerOnly => Memory.Value(0x7AC640);
		public MemoryValue<Int32Pair> MousePos => Memory.Value(0x7ADA44);
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

		public Point MapPosToScreenPos(Point pos)
		{
			var delta = pos - Pos;
			return SelfScreenPos + (delta.X * 48, delta.Y * 32);
		}

		public Point ScreenSize => Memory.Read<Int32Pair>(0x6658D0);
		public Point SelfScreenPos => ScreenSize.X == 800 ? (400, 240) : (496, 338);
		public int TotalOpened => Memory.Read<byte>(0x6EF680);
		public bool StatusOpened => Memory.Read<bool>(0x76F92C);
		public Point StatusCloseButton => Memory.Read<Int32Pair>(0x76F9AC);
		public Point StatusLeftRing => StatusCloseButton + (-255, 215);
		public bool BagOpened => Memory.Read<bool>(0x6F3260);
		public int BagScroll => Memory.Read<int>(0x6F32E0);
		public Point BagCloseButton => Memory.Read<Int32Pair>(0x6F5878);
		public Point BagAction => BagCloseButton + (-25, 10);
		public bool SkillOpened => Memory.Read<bool>(0x7956A0);
		public bool MiniMapOpened => Memory.Read<bool>(0x776AA4);
		public bool MiniMapDouble => Memory.Read<bool>(0x776B38);
		public bool MiniMapShowAll => Memory.Read<bool>(0x776B39);
		public bool ItemShortcutOpened => Memory.Read<bool>(0x78FE40);
		public bool MailOpened => Memory.Read<bool>(0x792414);
	}
}