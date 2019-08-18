namespace Mir3Helper
{
	public sealed class Module
	{
		public static readonly Module KingMir3 = new Module("KingMir3.dll");

		public string Name { get; }
		Module(string name) => Name = name.ToLowerInvariant();
		public override string ToString() => Name;
	}
}