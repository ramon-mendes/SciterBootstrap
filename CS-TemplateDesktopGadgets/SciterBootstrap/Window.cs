using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SciterSharp;
using SciterSharp.Interop;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace SciterBootstrap
{
	class Window : SciterWindow
	{
		static	uint WM_TASKBAR_CREATED = RegisterWindowMessage("TaskbarCreated");
		const   uint WM_APP = 0x8000;
		const	uint WM_DESKTOP_CHANGED = WM_APP + 99;
		const	uint WM_ENDSESSION = 22;
		const	uint WM_CLOSE = 16;
		const	uint WM_NCLBUTTONDOWN = 161;
		const	int HTCAPTION = 2;
		
		protected override bool ProcessWindowMessage(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam, ref IntPtr lResult)
		{
			if(msg==WM_TASKBAR_CREATED)
			{
				Program.HookerInstance.SetMessageHook();
				return true;
			}

			switch(msg)
			{
				case WM_DESKTOP_CHANGED:
					Debug.WriteLine($"{wParam.ToInt64()} {lParam.ToInt64()}");

					if(wParam.ToInt32()==0)
					{
						SetWindowPos(_hwnd, new IntPtr((int) SetWindowPosWindow.HWND_BOTTOM), 0, 0, 0, 0,
							SetWindowPosFlags.SWP_NOSIZE | SetWindowPosFlags.SWP_NOMOVE);
						SetWindowPos(_hwnd, new IntPtr((int) SetWindowPosWindow.HWND_TOPMOST), 0, 0, 0, 0,
							SetWindowPosFlags.SWP_NOSIZE | SetWindowPosFlags.SWP_NOMOVE);
						Show();

						Debug.WriteLine("WM_DESKTOP_CHANGED show");
					}
					else
					{
						SetWindowPos(_hwnd, new IntPtr((int) SetWindowPosWindow.HWND_NOTOPMOST), 0, 0, 0, 0,
							SetWindowPosFlags.SWP_NOSIZE | SetWindowPosFlags.SWP_NOMOVE | SetWindowPosFlags.SWP_NOACTIVATE | SetWindowPosFlags.SWP_NOSENDCHANGING);
						SetWindowPos(_hwnd, new IntPtr((int) SetWindowPosWindow.HWND_BOTTOM), 0, 0, 0, 0,
							SetWindowPosFlags.SWP_NOSIZE | SetWindowPosFlags.SWP_NOMOVE | SetWindowPosFlags.SWP_NOACTIVATE | SetWindowPosFlags.SWP_NOSENDCHANGING);
						Show();

						Debug.WriteLine("WM_DESKTOP_CHANGED hide");
					}
					return true;

				case WM_ENDSESSION:
					// system is shuting down, close app
					SendMessageW(_hwnd, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
					PostQuitMessage(0);
					return true;
			}
			return false;
		}

		public void HideTaskbarIcon()
		{
			const int GWL_EXSTYLE = -20;
			const int WS_EX_TOOLWINDOW = 0x00000080;
			const int WS_EX_LAYERED = 0x00080000;

			SetWindowLong(_hwnd, GWL_EXSTYLE, WS_EX_TOOLWINDOW | WS_EX_LAYERED);
		}
		public void EmulateMoveWnd()
		{
			SendMessageW(_hwnd, WM_NCLBUTTONDOWN, new IntPtr(HTCAPTION), IntPtr.Zero);
		}

		#region PInvoke
		[DllImport("user32.dll", SetLastError=true, CharSet=CharSet.Auto)]
		static extern uint RegisterWindowMessage(string lpString);

		[DllImport("user32.dll")]
		static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, SetWindowPosFlags uFlags);

		[DllImport("user32.dll")]
		static extern IntPtr SendMessageW(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll")]
		static extern void PostQuitMessage(int nExitCode);
		
		[DllImport("user32.dll")]
		static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);



		enum SetWindowPosWindow : int
		{
			HWND_BOTTOM    = 1,
			HWND_NOTOPMOST = -2,
			HWND_TOP       = 0,
			HWND_TOPMOST   = -1,
		}

		[Flags]
		enum SetWindowPosFlags : uint
		{
			SWP_NOSIZE          = 0x0001,
			SWP_NOMOVE          = 0x0002,
			SWP_NOZORDER        = 0x0004,
			SWP_NOREDRAW        = 0x0008,
			SWP_NOACTIVATE      = 0x0010,
			SWP_FRAMECHANGED    = 0x0020,  /* The frame changed: send WM_NCCALCSIZE */
			SWP_SHOWWINDOW      = 0x0040,
			SWP_HIDEWINDOW      = 0x0080,
			SWP_NOCOPYBITS      = 0x0100,
			SWP_NOOWNERZORDER   = 0x0200,  /* Don't do owner Z ordering */
			SWP_NOSENDCHANGING  = 0x0400,  /* Don't send WM_WINDOWPOSCHANGING */
			SWP_DRAWFRAME       = SWP_FRAMECHANGED,
			SWP_NOREPOSITION    = SWP_NOOWNERZORDER,
			SWP_DEFERERASE      = 0x2000,
			SWP_ASYNCWINDOWPOS  = 0x4000,
		}
		#endregion
	}
}
