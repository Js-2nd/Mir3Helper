namespace Mir3Helper
{
	using PInvoke;
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Globalization;
	using System.Runtime.CompilerServices;
	using System.Text;

	public sealed class Memory
	{
		static readonly Encoding s_Encoding = Encoding.GetEncoding("gb2312");

		readonly IntPtr m_Handle;
		readonly Dictionary<string, uint> m_Modules = new Dictionary<string, uint>();
		readonly Dictionary<string, uint> m_Caches = new Dictionary<string, uint>();
		byte[] m_Buffer = new byte[16];

		public Memory(Process process)
		{
			m_Handle = process.Handle;
			foreach (ProcessModule module in process.Modules)
				m_Modules[module.ModuleName.ToLowerInvariant()] = (uint) module.BaseAddress;
		}

		void EnsureBufferSize(int size)
		{
			if (m_Buffer.Length < size) m_Buffer = new byte[size];
		}

		unsafe int ReadBuffer(uint address, int size)
		{
			EnsureBufferSize(size);
			var count = IntPtr.Zero;
			fixed (void* buffer = m_Buffer)
				Kernel32.ReadProcessMemory(m_Handle, (void*) address, buffer, (IntPtr) size, &count);
			return count.ToInt32();
		}

		public T Read<T>(uint address) where T : struct
		{
			int size = Unsafe.SizeOf<T>();
			if (ReadBuffer(address, size) != size) return default;
			return Unsafe.As<byte, T>(ref m_Buffer[0]);
		}

		public bool ReadBoolean(uint address) => Read<bool>(address);
		public byte ReadByte(uint address) => Read<byte>(address);
		public short ReadInt16(uint address) => Read<short>(address);
		public ushort ReadUInt16(uint address) => Read<ushort>(address);
		public int ReadInt32(uint address) => Read<int>(address);
		public uint ReadUInt32(uint address) => Read<uint>(address);

		public string ReadString(uint address, int length, bool trimEnd = true)
		{
			int count = ReadBuffer(address, length);
			if (trimEnd)
				while (count > 0 && m_Buffer[count - 1] == 0)
					count--;
			return count > 0 ? s_Encoding.GetString(m_Buffer, 0, count) : string.Empty;
		}

		public Point ReadPoint(uint addressX, uint? addressY = null) =>
			(ReadInt32(addressX), ReadInt32(addressY ?? addressX + sizeof(int)));

		unsafe int WriteBuffer(uint address, int size)
		{
			if (size > m_Buffer.Length) throw new ArgumentOutOfRangeException(nameof(size));
			var count = IntPtr.Zero;
			fixed (void* buffer = m_Buffer)
				Kernel32.WriteProcessMemory(m_Handle, (void*) address, buffer, (IntPtr) size, &count);
			return count.ToInt32();
		}

		public bool Write<T>(uint address, T value) where T : struct
		{
			int size = Unsafe.SizeOf<T>();
			EnsureBufferSize(size);
			Unsafe.As<byte, T>(ref m_Buffer[0]) = value;
			return WriteBuffer(address, size) == size;
		}

		public bool WriteBoolean(uint address, bool value) => Write(address, value);
		public bool WriteByte(uint address, byte value) => Write(address, value);
		public bool WriteInt16(uint address, short value) => Write(address, value);
		public bool WriteUInt16(uint address, ushort value) => Write(address, value);
		public bool WriteInt32(uint address, int value) => Write(address, value);
		public bool WriteUInt32(uint address, uint value) => Write(address, value);

		public bool WriteString(uint address, string value)
		{
			int length = value.Length;
			int count = s_Encoding.GetMaxByteCount(length);
			EnsureBufferSize(count);
			count = s_Encoding.GetBytes(value, 0, length, m_Buffer, 0);
			return WriteBuffer(address, count) == count;
		}

		public (Memory, uint) ValueAddress(uint address) => (this, address);
		public (Memory, uint, int) StringAddress(uint address, int size) => (this, address, size);
		public (Memory, uint, uint) PointAddress(uint x, uint? y = null) => (this, x, y ?? x + sizeof(int));

		public uint this[string address]
		{
			get
			{
				if (m_Caches.TryGetValue(address, out uint value)) return value;
				m_Caches[address] = value = Resolve(address);
				return value;
			}
		}

		public uint Resolve(string address)
		{
			uint result = 0;
			foreach (string str in address.ToLowerInvariant().Split('+'))
			{
				if (str.EndsWith(".exe") || str.EndsWith(".dll"))
				{
					if (m_Modules.TryGetValue(str, out uint value)) result += value;
					else
					{
						Console.WriteLine($"Unknown module: {str}");
						return 0;
					}
				}
				else
				{
					if (uint.TryParse(str.Replace("0x", string.Empty),
						NumberStyles.HexNumber, NumberFormatInfo.InvariantInfo, out uint value)) result += value;
					else
					{
						Console.WriteLine($"Invalid address: {str}");
						return 0;
					}
				}
			}

			return result;
		}
	}
}