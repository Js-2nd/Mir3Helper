namespace Mir3Helper
{
	public readonly struct BagData
	{
		readonly Memory m_Memory;
		public BagData(Memory memory) => m_Memory = memory;

		public bool IsOpened => m_Memory.Read<bool>(0x6F3260);
		public int Scroll => m_Memory.Read<ushort>(0x6F32E0);
		public ItemData Item(int index, int scroll = 0) => (m_Memory, 0x6F5E99 + 0x339 * (index + 6 * scroll));
		public Point ItemPos(int index) => CloseButtonTopLeft + (-200, -325) + 38 * (Point) (index % 6, index / 6);
		public Point ActionButtonPos => CloseButtonTopLeft + (-25, 10);
		public Point ScrollUpPos => CloseButtonTopLeft + (30, -358);
		public Point ScrollDownPos => CloseButtonTopLeft + (30, -38);
		Point CloseButtonTopLeft => m_Memory.Read<Int32Pair>(0x6F5878);
	}
}