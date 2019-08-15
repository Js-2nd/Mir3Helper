namespace Mir3Helper
{
	using PInvoke;
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Runtime.CompilerServices;
	using System.Text;

	public sealed class Memory
	{
		static readonly Encoding s_Encoding = Encoding.GetEncoding("gb2312");

		readonly IntPtr m_Handle;
		readonly Dictionary<string, uint> m_Modules = new Dictionary<string, uint>();
		byte[] m_Buffer = new byte[16];

		public Memory(Process process)
		{
			m_Handle = process.Handle;
			foreach (ProcessModule module in process.Modules)
				m_Modules[module.ModuleName.ToLowerInvariant()] = (uint) module.BaseAddress;
		}

		uint Address(string module, uint offset) => module == Module.None ? offset :
			m_Modules.TryGetValue(module, out uint address) ? address + offset : 0;

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

		public T Read<T>(string module, uint offset) where T : struct =>
			Read<T>(Address(module, offset));

		public string ReadString(uint address, int length, bool trimEnd = true)
		{
			int count = ReadBuffer(address, length);
			if (trimEnd)
				while (count > 0 && m_Buffer[count - 1] == 0)
					count--;
			return count > 0 ? s_Encoding.GetString(m_Buffer, 0, count) : string.Empty;
		}

		public string ReadString(string module, uint offset, int length, bool trimEnd = true) =>
			ReadString(Address(module, offset), length, trimEnd);

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

		public bool Write<T>(string module, uint offset, T value) where T : struct =>
			Write(Address(module, offset), value);

		public bool WriteString(uint address, string value)
		{
			int length = value.Length;
			int count = s_Encoding.GetMaxByteCount(length);
			EnsureBufferSize(count);
			count = s_Encoding.GetBytes(value, 0, length, m_Buffer, 0);
			return WriteBuffer(address, count) == count;
		}

		public bool WriteString(string module, uint offset, string value) =>
			WriteString(Address(module, offset), value);

		public (Memory, uint) Value(uint address) => (this, address);
		public (Memory, uint) Value(string module, uint offset) => (this, Address(module, offset));
	}
}