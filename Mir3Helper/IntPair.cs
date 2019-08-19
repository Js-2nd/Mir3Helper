namespace Mir3Helper
{
	using System.Runtime.InteropServices;

	[StructLayout(LayoutKind.Explicit)]
	public readonly struct Int8Pair
	{
		[FieldOffset(0)] public readonly sbyte First;
		[FieldOffset(sizeof(sbyte))] public readonly sbyte Second;
		public Int8Pair(sbyte first, sbyte second) => (First, Second) = (first, second);
	}

	[StructLayout(LayoutKind.Explicit)]
	public readonly struct UInt8Pair
	{
		[FieldOffset(0)] public readonly byte First;
		[FieldOffset(sizeof(byte))] public readonly byte Second;
		public UInt8Pair(byte first, byte second) => (First, Second) = (first, second);
	}

	[StructLayout(LayoutKind.Explicit)]
	public readonly struct Int16Pair
	{
		[FieldOffset(0)] public readonly short First;
		[FieldOffset(sizeof(short))] public readonly short Second;
		public Int16Pair(short first, short second) => (First, Second) = (first, second);
	}

	[StructLayout(LayoutKind.Explicit)]
	public readonly struct UInt16Pair
	{
		[FieldOffset(0)] public readonly ushort First;
		[FieldOffset(sizeof(ushort))] public readonly ushort Second;
		public UInt16Pair(ushort first, ushort second) => (First, Second) = (first, second);
	}

	[StructLayout(LayoutKind.Explicit)]
	public readonly struct Int32Pair
	{
		[FieldOffset(0)] public readonly int First;
		[FieldOffset(sizeof(int))] public readonly int Second;
		public Int32Pair(int first, int second) => (First, Second) = (first, second);
	}

	[StructLayout(LayoutKind.Explicit)]
	public readonly struct UInt32Pair
	{
		[FieldOffset(0)] public readonly uint First;
		[FieldOffset(sizeof(uint))] public readonly uint Second;
		public UInt32Pair(uint first, uint second) => (First, Second) = (first, second);
	}

	[StructLayout(LayoutKind.Explicit)]
	public readonly struct Int64Pair
	{
		[FieldOffset(0)] public readonly long First;
		[FieldOffset(sizeof(long))] public readonly long Second;
		public Int64Pair(long first, long second) => (First, Second) = (first, second);
	}

	[StructLayout(LayoutKind.Explicit)]
	public readonly struct UInt64Pair
	{
		[FieldOffset(0)] public readonly ulong First;
		[FieldOffset(sizeof(ulong))] public readonly ulong Second;
		public UInt64Pair(ulong first, ulong second) => (First, Second) = (first, second);
	}
}