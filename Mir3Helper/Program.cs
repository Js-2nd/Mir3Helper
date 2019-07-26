namespace Mir3Helper
{
	using PInvoke;
	using System;

	static class Program
	{
		static void Main(string[] args)
		{
			var input = new InputSystem();
			input.KeyDown += key => Console.WriteLine(key);
			MessageLoop();
			input.Dispose();
			input.EventLoopTask.Wait();
		}

		static unsafe void MessageLoop()
		{
			User32.MSG msg;
			while (User32.GetMessage(&msg, IntPtr.Zero, 0, 0) > 0)
			{
				User32.TranslateMessage(&msg);
				User32.DispatchMessage(&msg);
			}
		}
	}
}