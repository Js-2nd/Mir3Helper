namespace Mir3Helper
{
	using PInvoke;
	using System;
	using Tuple = System.ValueTuple<int, int>;

	public readonly struct Point
	{
		public readonly int X;
		public readonly int Y;
		Point(in Tuple t) => (X, Y) = t;

		public IntPtr ToLParam() => (IntPtr) ((X & 0xFFFF) | (Y << 16));
		public override string ToString() => $"({X.ToString()}, {Y.ToString()})";

		public static Point operator -(in Point p) => (-p.X, -p.Y);
		public static Point operator +(in Point p, in Point q) => (p.X + q.X, p.Y + q.Y);
		public static Point operator -(in Point p, in Point q) => (p.X - q.X, p.Y - q.Y);

		public static implicit operator Point(in Tuple t) => new Point(t);
		public static implicit operator Point(in Int16Pair p) => (p.First, p.Second);
		public static implicit operator Point(in Int32Pair p) => (p.First, p.Second);
		public static implicit operator Point(in POINT p) => (p.x, p.y);
	}
}