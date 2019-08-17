namespace Mir3Helper
{
	using System.Runtime.InteropServices;

	[StructLayout(LayoutKind.Explicit)]
	public readonly struct Int8Pair
	{
		[FieldOffset(0)] public readonly sbyte First;
		[FieldOffset(sizeof(sbyte))] public readonly sbyte Second;
	}

	[StructLayout(LayoutKind.Explicit)]
	public readonly struct UInt8Pair
	{
		[FieldOffset(0)] public readonly byte First;
		[FieldOffset(sizeof(byte))] public readonly byte Second;
	}

	[StructLayout(LayoutKind.Explicit)]
	public readonly struct Int16Pair
	{
		[FieldOffset(0)] public readonly short First;
		[FieldOffset(sizeof(short))] public readonly short Second;
	}

	[StructLayout(LayoutKind.Explicit)]
	public readonly struct UInt16Pair
	{
		[FieldOffset(0)] public readonly ushort First;
		[FieldOffset(sizeof(ushort))] public readonly ushort Second;
	}

	[StructLayout(LayoutKind.Explicit)]
	public readonly struct Int32Pair
	{
		[FieldOffset(0)] public readonly int First;
		[FieldOffset(sizeof(int))] public readonly int Second;
	}

	[StructLayout(LayoutKind.Explicit)]
	public readonly struct UInt32Pair
	{
		[FieldOffset(0)] public readonly uint First;
		[FieldOffset(sizeof(uint))] public readonly uint Second;
	}

	[StructLayout(LayoutKind.Explicit)]
	public readonly struct Int64Pair
	{
		[FieldOffset(0)] public readonly long First;
		[FieldOffset(sizeof(long))] public readonly long Second;
	}

	[StructLayout(LayoutKind.Explicit)]
	public readonly struct UInt64Pair
	{
		[FieldOffset(0)] public readonly ulong First;
		[FieldOffset(sizeof(ulong))] public readonly ulong Second;
	}
}