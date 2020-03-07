namespace Mir3Helper
{
	using System.Threading.Tasks;

	public readonly struct BagData
	{
		readonly Game m_Game;
		public BagData(Game game) => m_Game = game;

		public bool IsOpened => m_Game.Memory.Read<bool>(0x6B4E98);
		public int Scroll => m_Game.Memory.Read<byte>(0x6B4F18);

		public async Task EnsureItemVisible(int index)
		{
			int diff = index / 6 - Scroll;
			if (diff < 0) await RepeatClick(ScrollUpPos, -diff);
			else if (diff > 7) await RepeatClick(ScrollDownPos, diff - 7);
		}

		async Task RepeatClick(Point pos, int count)
		{
			for (int i = 0; i < count; i++)
			{
				m_Game.Window.Click(pos);
				await Task.Delay(700);
			}
		}

		public int ItemCount
		{
			get
			{
				int count = 0;
				for (int i = 0; i < 600; i++)
					if (Item(i).IsValid)
						count++;
				return count;
			}
		}

		public ItemData Item(int index) => (m_Game.Memory, 0x6B7AD1 + 0x339 * index);

		public Point? ItemPosByIndex(int index)
		{
			int slot = index - Scroll * 6;
			if (slot >= 0 && slot < 48) return ItemPosBySlot(slot);
			return null;
		}

		public Point ItemPosBySlot(int slot) => AnchorPos + (150, 110) + 38 * (Point) (slot % 6, slot / 6);
		public Point ActionButtonPos => AnchorPos + (324, 448);
		public Point ScrollUpPos => AnchorPos + (380, 82);
		public Point ScrollDownPos => AnchorPos + (380, 402);

		Point AnchorPos => m_Game.Memory.Read<Int32Pair>(0x6B4E80);
	}
}