namespace Mir3Helper
{
	using PInvoke;
	using System;
	using System.Diagnostics;
	using static PInvoke.User32;

	public sealed class Window
	{
		readonly IntPtr m_Handle;
		public Window(Process process) => m_Handle = process.MainWindowHandle;

		public void SendMessage(WindowMessage message, IntPtr wParam = default, IntPtr lParam = default)
		{
			User32.SendMessage(m_Handle, message, wParam, lParam);
		}

		public void SendKey(VirtualKey key)
		{
			SendMessage(WindowMessage.WM_IME_KEYDOWN, (IntPtr) key);
		}

		public void SendClick(in Point point)
		{
			var lParam = point.ToLParam();
			SendMessage(WindowMessage.WM_LBUTTONDOWN, (IntPtr) MK.LBUTTON, lParam);
			SendMessage(WindowMessage.WM_LBUTTONUP, IntPtr.Zero, lParam);
		}

		public void SendDoubleClick(in Point point)
		{
			SendMessage(WindowMessage.WM_LBUTTONDBLCLK, (IntPtr) MK.LBUTTON, point.ToLParam());
		}

		public void SendRightClick(in Point point)
		{
			var lParam = point.ToLParam();
			SendMessage(WindowMessage.WM_RBUTTONDOWN, (IntPtr) MK.RBUTTON, lParam);
			SendMessage(WindowMessage.WM_RBUTTONUP, IntPtr.Zero, lParam);
		}
	}
}