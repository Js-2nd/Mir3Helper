namespace Mir3Helper
{
	using PInvoke;

	public static class Extensions
	{
		public static Point AsPoint(this in POINT p) => p;
	}
}