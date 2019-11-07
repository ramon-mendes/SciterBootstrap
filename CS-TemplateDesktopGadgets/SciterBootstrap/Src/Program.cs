using SciterSharp;
using SciterSharp.Interop;
using System;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SciterBootstrap
{
	class Program
	{
		public static Window WndGlobal;
		public static Hooker HookerInstance = new Hooker();

		[STAThread]
		static void Main(string[] args)
		{
			// Sciter needs this for drag'n'drop support; STAThread is required for OleInitialize succeess
			int oleres = PInvokeWindows.OleInitialize(IntPtr.Zero);
			Debug.Assert(oleres == 0);
			
			// Create the window
			var wnd = WndGlobal = new Window();
			wnd.CreateMainWindow(500, 320,
				SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_MAIN |
				SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_ALPHA |
				SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_POPUP |
				SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_ENABLE_DEBUG);
			wnd.CenterTopLevelWindow();
			wnd.HideTaskbarIcon();
			wnd.Title = Window.WND_TITLE;
			wnd.Icon = Properties.Resources.IconMain;

			// Prepares SciterHost and then load the page
			var host = new Host();
			host.Setup(wnd);
			host.AttachEvh(new HostEvh());
			host.SetupPage("index.html");

			HookerInstance.SetMessageHook();

			// Show window and Run message loop
			wnd.Show();
			PInvokeUtils.RunMsgLoop();

			HookerInstance.ClearHook();

			FinalizeApp();
		}

		public static void FinalizeApp()
		{
			GC.Collect();
			GC.WaitForPendingFinalizers();
		}
	}
}