namespace Mir3Helper
{
	using System;

	public readonly struct Address
	{
		public readonly int Value;
		public Address(int value) => Value = value;
		public override string ToString() => Value.ToString("X8");

		public static Address operator +(Address a, Address b) => a.Value + b.Value;
		public static implicit operator Address(int value) => new Address(value);
		public static implicit operator Address(IntPtr value) => value.ToInt32();
	}
}