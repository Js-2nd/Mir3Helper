namespace Mir3Helper
{
	using PInvoke;
	using System;
	using System.Threading.Tasks;

	public static class Extensions
	{
		public static Point AsPoint(this in POINT p) => p;

		public static void Forget(this Task task) =>
			task.ContinueWith(t => Console.WriteLine(t.Exception), TaskContinuationOptions.OnlyOnFaulted);
	}
}