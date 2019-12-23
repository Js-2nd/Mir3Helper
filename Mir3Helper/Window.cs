namespace Mir3Helper
{
	using System;
	using System.Diagnostics;
	using static PInvoke.User32;

	public sealed class Window
	{
		public readonly IntPtr Handle;
		public Window(Process process) => Handle = process.MainWindowHandle;

		public void Message(WindowMessage message, IntPtr wParam = default, IntPtr lParam = default, bool send = false)
		{
			if (send) SendMessage(Handle, message, wParam, lParam);
			else PostMessage(Handle, message, wParam, lParam);
		}

		public void KeyDown(VirtualKey key, bool send = false)
		{
			Message(WindowMessage.WM_IME_KEYDOWN, (IntPtr) key, IntPtr.Zero, send);
		}

		public void KeyUp(VirtualKey key, bool send = false)
		{
			Message(WindowMessage.WM_IME_KEYUP, (IntPtr) key, IntPtr.Zero, send);
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
			RightClickDown(point, send);
			RightClickUp(point, send);
		}

		public void RightClickDown(in Point point, bool send = false)
		{
			Message(WindowMessage.WM_RBUTTONDOWN, (IntPtr) MK.RBUTTON, point.ToLParam(), send);
		}

		public void RightClickUp(in Point point, bool send = false)
		{
			Message(WindowMessage.WM_RBUTTONUP, IntPtr.Zero, point.ToLParam(), send);
		}

		public Point MousePos
		{
			get
			{
				GetCursorPos(out var pos);
				ScreenToClient(Handle, ref pos);
				return pos;
			}
			set
			{
				PInvoke.POINT pos = value;
				ClientToScreen(Handle, ref pos);
				SetCursorPos(pos.x, pos.y);
			}
		}
	}
}