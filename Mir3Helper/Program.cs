namespace Mir3Helper
{
	using PInvoke;
	using System;

	static class Program
	{
		static int MainThreadId;

		static void Main(string[] args)
		{
			MainThreadId = Kernel32.GetCurrentThreadId();
			MainAsync(args);
			MessageLoop();
		}

		static async void MainAsync(string[] args)
		{
			var input = new InputSystem();
			while (true)
			{
				var key = await input.WaitKeyDown();
				if (key == User32.VirtualKey.VK_ESCAPE) break;
				Console.WriteLine(key);
			}

			input.Dispose();
			await input.EventLoopTask;
			User32.PostThreadMessage(MainThreadId, User32.WindowMessage.WM_QUIT, IntPtr.Zero, IntPtr.Zero);
		}

		static unsafe void MessageLoop()
		{
			User32.MSG msg;
			while (User32.GetMessage(&msg, IntPtr.Zero, 0, 0) > 0)
			{
				Console.WriteLine($"[Message] {msg.message} {msg.wParam} {msg.lParam}");
				User32.TranslateMessage(&msg);
				User32.DispatchMessage(&msg);
			}
		}
	}
}