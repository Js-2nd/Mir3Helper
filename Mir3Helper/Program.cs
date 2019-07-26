namespace Mir3Helper
{
	using PInvoke;
	using System;
	using System.Runtime.InteropServices;

	static class Program
	{
		public static int MainThreadId { get; private set; }
		public static InputSystem Input { get; private set; }

		static void Main(string[] args)
		{
			MainThreadId = Kernel32.GetCurrentThreadId();
			Input = new InputSystem();
			MainAsync(args);
			MessageLoop();
//			Input.Dispose();
//			Input.EventLoopTask.Wait();
		}

		static async void MainAsync(string[] args)
		{
			while (true)
			{
				var key = await Input.WaitKeyDown();
				if (key == User32.VirtualKey.VK_ESCAPE) break;
				var window = User32.GetForegroundWindow();
				string title = User32.GetWindowText(window);
				Console.WriteLine(title);
			}

			User32.PostThreadMessage(MainThreadId, User32.WindowMessage.WM_QUIT, IntPtr.Zero, IntPtr.Zero);
		}

		static unsafe void MessageLoop()
		{
			User32.MSG msg;
			while (User32.GetMessage(&msg, IntPtr.Zero, 0, 0) > 0)
			{
				switch (msg.message)
				{
					case CustomWindowMessage.PostAction:
						Marshal.GetDelegateForFunctionPointer<Action>(msg.wParam).Invoke();
						break;
					default:
						User32.TranslateMessage(&msg);
						User32.DispatchMessage(&msg);
						break;
				}
			}
		}

		public static void PostToMainThread(Action action)
		{
			User32.PostThreadMessage(MainThreadId, CustomWindowMessage.PostAction,
				Marshal.GetFunctionPointerForDelegate(action), IntPtr.Zero);
		}
	}
}