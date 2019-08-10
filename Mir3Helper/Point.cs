namespace Mir3Helper
{
	using PInvoke;

	public readonly struct Point
	{
		public readonly int X;
		public readonly int Y;

		public Point(int x, int y)
		{
			X = x;
			Y = y;
		}

		public static Point operator +(in Point p, in Point q) => (p.X + q.X, p.Y + q.Y);
		public static Point operator -(in Point p, in Point q) => (p.X - q.X, p.Y - q.Y);
		public static Point operator -(in Point p) => (-p.X, -p.Y);

		public static implicit operator Point(in (int x, int y) p) => new Point(p.x, p.y);
		public static implicit operator POINT(in Point p) => new POINT {x = p.X, y = p.Y};
	}
}