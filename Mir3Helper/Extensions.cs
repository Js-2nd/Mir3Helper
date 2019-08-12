namespace Mir3Helper
{
	using PInvoke;
	using System;
	using System.Threading.Tasks;

	public static class Extensions
	{
		public static Point AsPoint(this in POINT p) => p;

		public static void Catch(this Task task) =>
			task.ContinueWith(t => Console.Error.WriteLine(t.Exception), TaskContinuationOptions.OnlyOnFaulted);
	}
}