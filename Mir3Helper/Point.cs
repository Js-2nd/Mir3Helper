namespace Mir3Helper
{
	using PInvoke;
	using System;

	public readonly struct Point
	{
		public readonly int X;
		public readonly int Y;

		public Point(int x, int y) => (X, Y) = (x, y);

		public int ManhattanLength => Math.Abs(X) + Math.Abs(Y);
		public IntPtr ToLParam() => (IntPtr) ((X & 0xFFFF) | (Y << 16));
		public override string ToString() => $"({X.ToString()}, {Y.ToString()})";

		public static int ManhattanDistance(in Point p, in Point q) => (p - q).ManhattanLength;

		public static Point operator +(in Point p, in Point q) => (p.X + q.X, p.Y + q.Y);
		public static Point operator -(in Point p, in Point q) => (p.X - q.X, p.Y - q.Y);
		public static Point operator -(in Point p) => (-p.X, -p.Y);

		public static implicit operator Point(in (int x, int y) p) => new Point(p.x, p.y);
		public static implicit operator Point(in Int32Pair p) => (p.First, p.Second);
		public static implicit operator Point(in POINT p) => (p.x, p.y);
		public static explicit operator POINT(in Point p) => new POINT {x = p.X, y = p.Y};
	}
}