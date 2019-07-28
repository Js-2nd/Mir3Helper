namespace Mir3Helper
{
	using System;
	using System.Linq;
	using System.Reactive.Linq;
	using System.Reactive.Threading.Tasks;
	using System.Runtime.InteropServices;
	using System.Threading.Channels;
	using System.Threading.Tasks;
	using static PInvoke.User32;

	public sealed class InputSystem : IDisposable
	{
		public event Action<VirtualKey> KeyDown;
		public event Action<VirtualKey> KeyHold;
		public event Action<VirtualKey> KeyUp;
		readonly int[] m_KeyState;
		readonly Channel<int> m_Channel;
		SafeHookHandle m_Hook;

		public InputSystem()
		{
			m_KeyState = new int[256];
			m_Channel = Channel.CreateUnbounded<int>(new UnboundedChannelOptions {SingleReader = true});
			Task.Factory.StartNew(HookKeyboard, TaskCreationOptions.LongRunning);
			Task.Run(EventLoop);
		}

		unsafe void HookKeyboard()
		{
			m_Hook = SetWindowsHookEx(WindowsHookType.WH_KEYBOARD_LL, LowLevelKeyboardProc, IntPtr.Zero, 0);
			Console.WriteLine($"[InputSystem] HookKeyboard {m_Hook.DangerousGetHandle() != IntPtr.Zero}");
			MSG msg;
			while (GetMessage(&msg, IntPtr.Zero, 0, 0) > 0)
			{
				TranslateMessage(&msg);
				DispatchMessage(&msg);
			}

			Dispose();
		}

		int LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam)
		{
			if (nCode >= 0)
			{
				var type = (WindowMessage) wParam;
				int key = Marshal.ReadInt32(lParam);
				if (type == WindowMessage.WM_KEYUP || type == WindowMessage.WM_SYSKEYUP) key = ~key;
				m_Channel.Writer.WriteAsync(key);
			}

			return CallNextHookEx(m_Hook.DangerousGetHandle(), nCode, wParam, lParam);
		}

		async Task EventLoop()
		{
			while (await m_Channel.Reader.WaitToReadAsync())
			while (m_Channel.Reader.TryRead(out int key))
			{
				if (key < 0)
				{
					key = ~key;
					m_KeyState[key] = 0;
					KeyUp?.Invoke((VirtualKey) key);
				}
				else
				{
					if (++m_KeyState[key] == 1) KeyDown?.Invoke((VirtualKey) key);
					KeyHold?.Invoke((VirtualKey) key);
				}
			}
		}

		public void Dispose()
		{
			m_Hook.Dispose();
			m_Channel.Writer.TryComplete();
		}

		public bool IsKeyDown(VirtualKey key) => m_KeyState[(int) key] > 0;

		public bool IsCtrlDown() =>
			IsKeyDown(VirtualKey.VK_CONTROL) || IsKeyDown(VirtualKey.VK_LCONTROL) || IsKeyDown(VirtualKey.VK_RCONTROL);

		public bool IsAltDown() =>
			IsKeyDown(VirtualKey.VK_MENU) || IsKeyDown(VirtualKey.VK_LMENU) || IsKeyDown(VirtualKey.VK_RMENU);

		public bool IsShiftDown() =>
			IsKeyDown(VirtualKey.VK_SHIFT) || IsKeyDown(VirtualKey.VK_LSHIFT) || IsKeyDown(VirtualKey.VK_RSHIFT);

		public IObservable<VirtualKey> ObserveKeyDown() =>
			Observable.FromEvent<VirtualKey>(h => KeyDown += h, h => KeyDown -= h);

		public IObservable<VirtualKey> ObserveKeyHold() =>
			Observable.FromEvent<VirtualKey>(h => KeyHold += h, h => KeyHold -= h);

		public IObservable<VirtualKey> ObserveKeyUp() =>
			Observable.FromEvent<VirtualKey>(h => KeyUp += h, h => KeyUp -= h);

		public Task<VirtualKey> GetKeyDown() => ObserveKeyDown().FirstOrDefaultAsync().ToTask();

		public Task<VirtualKey> GetKeyDown(VirtualKey key) =>
			ObserveKeyDown().FirstOrDefaultAsync(input => input == key).ToTask();

		public Task<VirtualKey> GetKeyDown(params VirtualKey[] keys) =>
			ObserveKeyDown().FirstOrDefaultAsync(keys.Contains).ToTask();
	}
}