using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using PInvoke;

namespace SciterBootstrap
{
	class Hooker
	{
		#region Fields

		private int hookHandle = 0;
		private HookProc messageHookProcedure;
		private HookProc mouseHookProcedure;
		private HookProc keyboardHookProcedure;
		private const int WH_GETMESSAGE = 3;
		private const int WH_CALLWNDPROC = 4;
		private const int WH_MOUSE = 7;
		private const int WM_USER = 0x0400;
		private const int WM_APP = 0x8000;
		private const int WH_KEYBOARD_LL = 13;
		private const int PM_NOREMOVE = 0;

		#endregion

		#region DllImports

		// Hook procedure callback type.
		private delegate int HookProc(
			int nCode, IntPtr wParam, IntPtr lParam);

		// Managed equivalent of the POINT struct defined in winuser.h.
		[StructLayout(LayoutKind.Sequential)]
		private struct POINT
		{
			public int x;
			public int y;
		}

		// Managed equivalent of the MSG struct defined in winuser.h.
		[StructLayout(LayoutKind.Sequential)]
		private struct MSG
		{
			internal int hwnd;
			internal uint message;
			internal uint wParam;
			internal int lParam;
			internal uint time;
			internal POINT pt;
		}

		// Managed equivalent of the MOUSEHOOKSTRUCT defined in winuser.h.
		[StructLayout(LayoutKind.Sequential)]
		private class MouseHookStruct
		{
			internal POINT pt;
			internal int hwnd;
			internal int wHitTestCode;
			internal int dwExtraInfo;
		}

		// Managed equivalent of the KBDLLHOOKSTRUCT defined in winuser.h.
		[StructLayout(LayoutKind.Sequential)]
		private struct KbDllHookStruct
		{
			internal int vkCode;
			internal int scanCode;
			internal int flags;
			internal int time;
			internal int dwExtraInfo;
		}

		// SetWindowsHookEx is used to install a thread-specific hook.
		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		private static extern int SetWindowsHookExW(
			int idHook, IntPtr lpfn, IntPtr hInstance, int threadId);

		// UnhookWindowsHookEx is used to uninstall the hook.
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern bool UnhookWindowsHookEx(int idHook);

		// CallNextHookEx is used to pass the hook information to the next
		// hook procedure in the chain.
		[DllImport("user32.dll", CharSet = CharSet.Auto,
		 CallingConvention = CallingConvention.StdCall)]
		private static extern int CallNextHookEx(
			int idHook, int nCode, IntPtr wParam, IntPtr lParam);

		// GetModuleHandle is used when calling SetWindowsHookEx for
		// a keyboard hook.
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		private static extern IntPtr GetModuleHandle(string lpModuleName);

		#endregion

		#region ClearHook

		internal void ClearHook()
		{
			if(hookHandle != 0)
			{
				bool ret = UnhookWindowsHookEx(hookHandle);
				if(ret == false)
				{
					Debug.WriteLine("UnhookWindowsHookEx Failed");
					return;
				}
				hookHandle = 0;
			}
		}

		#endregion

		#region Message hook

		internal void SetMessageHook()
		{
			ClearHook();

			// Create an instance of the HookProc delegate.
			var dll = Kernel32.LoadLibrary(@"DynamicHook.dll");
			var proc = Kernel32.GetProcAddress(dll, "HookProc");

			// Note, using System.AppDomain.GetCurrentThreadId produces
			// a compiler warning: "GetCurrentThreadId has been deprecated 
			// because it does not provide a stable Id when managed 
			// threads are running on fibers. To get a stable Id for a
			// managed thread, use the Thread.ManagedThreadId property."
			// However, passing the Thread.ManagedThreadId to 
			// SetWindowsHookEx always fails.
			hookHandle = SetWindowsHookExW(WH_GETMESSAGE, proc, dll.DangerousGetHandle(), 0);

			Debug.Assert(hookHandle != 0);
		}

		#endregion
	}
}