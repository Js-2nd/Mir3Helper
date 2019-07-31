namespace Mir3Helper
{
	using static PInvoke.User32;

	public static class CustomWindowMessage
	{
		public const WindowMessage StopMessageLoop = WindowMessage.WM_USER;
	}
}