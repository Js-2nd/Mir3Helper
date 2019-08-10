namespace Mir3Helper
{
	using System;

	[Flags]
	public enum MK
	{
		LBUTTON = 0x1,
		RBUTTON = 0x2,
		SHIFT = 0x4,
		CONTROL = 0x8,
		MBUTTON = 0x10,
		XBUTTON1 = 0x20,
		XBUTTON2 = 0x40,
	}
}