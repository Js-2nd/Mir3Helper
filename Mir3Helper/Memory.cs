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
		readonly Dictionary<string, Address> m_Modules = new Dictionary<string, Address>();
		byte[] m_Buffer = new byte[32];

		public Memory(Process process)
		{
			m_Handle = process.Handle;
			foreach (ProcessModule module in process.Modules)
				m_Modules[module.ModuleName.ToLowerInvariant()] = module.BaseAddress;
		}

		public Address this[params Address[] offsets] => this[null, offsets];

		public Address this[Module module, params Address[] offsets]
		{
			get
			{
				Address address = 0;
				if (module != null) m_Modules.TryGetValue(module.Name, out address);
				int count = offsets.Length;
				if (count > 0)
				{
					address += offsets[0];
					for (int i = 1; i < count; i++)
					{
						address = Read<int>(address);
						address += offsets[i];
					}
				}

				return address;
			}
		}

		void EnsureBufferSize(int size)
		{
			if (m_Buffer.Length < size) m_Buffer = new byte[size];
		}

		int ReadBuffer(Address address, int size)
		{
			EnsureBufferSize(size);
			return ReadBuffer(address, size, m_Buffer);
		}

		public unsafe int ReadBuffer(Address address, int size, byte[] buffer, int index = 0)
		{
			var count = IntPtr.Zero;
			fixed (byte* ptr = buffer)
				Kernel32.ReadProcessMemory(m_Handle, (void*) address.Value, ptr + index, (IntPtr) size, &count);
			return count.ToInt32();
		}

		public T Read<T>(Address address) where T : struct
		{
			int size = Unsafe.SizeOf<T>();
			if (ReadBuffer(address, size) != size) return default;
			return Unsafe.As<byte, T>(ref m_Buffer[0]);
		}

		public string ReadString(Address address, int length, bool trim = true)
		{
			int count = ReadBuffer(address, length);
			if (trim)
				for (int i = 0; i < count; i++)
					if (m_Buffer[i] == 0)
						count = i;
			return count > 0 ? s_Encoding.GetString(m_Buffer, 0, count) : string.Empty;
		}

		unsafe int WriteBuffer(Address address, int size)
		{
			if (size > m_Buffer.Length) throw new ArgumentOutOfRangeException(nameof(size));
			var count = IntPtr.Zero;
			fixed (byte* ptr = m_Buffer)
				Kernel32.WriteProcessMemory(m_Handle, (void*) address.Value, ptr, (IntPtr) size, &count);
			return count.ToInt32();
		}

		public bool Write<T>(Address address, in T value) where T : struct
		{
			int size = Unsafe.SizeOf<T>();
			EnsureBufferSize(size);
			Unsafe.As<byte, T>(ref m_Buffer[0]) = value;
			return WriteBuffer(address, size) == size;
		}

		public bool WriteString(Address address, string value)
		{
			int length = value.Length;
			int count = s_Encoding.GetMaxByteCount(length);
			EnsureBufferSize(count);
			count = s_Encoding.GetBytes(value, 0, length, m_Buffer, 0);
			return WriteBuffer(address, count) == count;
		}

		public (Memory, Address) Value(Address address) => (this, address);
	}
}