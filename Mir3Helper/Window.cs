namespace Mir3Helper
{
	using System;
	using System.Diagnostics;
	using static PInvoke.User32;

	public sealed class Window
	{
		readonly IntPtr m_Handle;
		public Window(Process process) => m_Handle = process.MainWindowHandle;

		public void Message(WindowMessage message, IntPtr wParam = default, IntPtr lParam = default, bool send = false)
		{
			if (send) SendMessage(m_Handle, message, wParam, lParam);
			else PostMessage(m_Handle, message, wParam, lParam);
		}

		public void Key(VirtualKey key, bool send = false)
		{
			Message(WindowMessage.WM_IME_KEYDOWN, (IntPtr) key, IntPtr.Zero, send);
		}

		public void Click(in Point point, bool send = false)
		{
			var lParam = point.ToLParam();
			Message(WindowMessage.WM_LBUTTONDOWN, (IntPtr) MK.LBUTTON, lParam, send);
			Message(WindowMessage.WM_LBUTTONUP, IntPtr.Zero, lParam, send);
		}

		public void DoubleClick(in Point point, bool send = false)
		{
			Message(WindowMessage.WM_LBUTTONDBLCLK, (IntPtr) MK.LBUTTON, point.ToLParam(), send);
		}

		public void RightClick(in Point point, bool send = false)
		{
			var lParam = point.ToLParam();
			Message(WindowMessage.WM_RBUTTONDOWN, (IntPtr) MK.RBUTTON, lParam, send);
			Message(WindowMessage.WM_RBUTTONUP, IntPtr.Zero, lParam, send);
		}

		public Point GetMousePos()
		{
			GetCursorPos(out var pos);
			ScreenToClient(m_Handle, ref pos);
			return pos;
		}
	}
}