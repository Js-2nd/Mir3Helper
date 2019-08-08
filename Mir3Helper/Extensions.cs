namespace Mir3Helper
{
	using PInvoke;
	using System;

	public static class Extensions
	{
		public static IntPtr ToLParam(this in POINT p) => (IntPtr) ((p.x & 0xFFFF) | (p.y << 16));
	}
}