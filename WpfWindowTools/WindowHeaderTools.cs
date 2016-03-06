using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace WpfWindowTools
{
    public static class WindowHeaderTools
    {
        private const int GWL_STYLE = -16;
        private const int WS_SYSMENU = 0x80000;

		private const int GWL_EXSTYLE = -20;
		private const int WS_EX_DLGMODALFRAME = 0x0001;
		private const int SWP_NOSIZE = 0x0001;
		private const int SWP_NOMOVE = 0x0002;
		private const int SWP_NOZORDER = 0x0004;
		private const int SWP_FRAMECHANGED = 0x0020;
		const uint WM_SETICON = 0x0080;


		[DllImport("user32.dll", SetLastError = true)]
        private static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

		[DllImport("user32.dll")]
		private static extern int SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll")]
		static extern bool SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter,
				   int x, int y, int width, int height, uint flags);

		public static void HideCloseButton(Window form) {
            // http://stackoverflow.com/questions/743906/how-to-hide-close-button-in-wpf-window
            var hwnd = new WindowInteropHelper(form).Handle;
            SetWindowLong(hwnd, GWL_STYLE, (GetWindowLong(hwnd, GWL_STYLE) & ~WS_SYSMENU));
        }

		public static void HideWindowIcon(Window form) {
			// http://wpftutorial.net/RemoveIcon.html
			var hwnd = new WindowInteropHelper(form).Handle;
			int exs = GetWindowLong(hwnd, GWL_EXSTYLE);
			SetWindowLong(hwnd, GWL_EXSTYLE, exs | WS_EX_DLGMODALFRAME);
			SetWindowPos(hwnd, IntPtr.Zero, 0, 0, 0, 0, SWP_NOMOVE |
				  SWP_NOSIZE | SWP_NOZORDER | SWP_FRAMECHANGED);
		}
	}
}
