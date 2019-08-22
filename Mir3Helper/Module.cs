namespace Mir3Helper
{
	public sealed class Module
	{
		public static readonly Module Mir3 = new Module("mir3.exe");
		public static readonly Module KingMir3 = new Module("KingMir3.dll");

		public string Name { get; }
		public Module(string name) => Name = name.ToLowerInvariant();
		public override string ToString() => Name;
	}
}