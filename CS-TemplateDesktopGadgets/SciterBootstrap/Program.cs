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

		[STAThread]
		static void Main(string[] args)
		{
			// Sciter needs this for drag'n'drop support; STAThread is required for OleInitialize succeess
			int oleres = PInvokeWindows.OleInitialize(IntPtr.Zero);
			Debug.Assert(oleres == 0);
			
			RunHooker();
			
			// Create the window
			var wnd = WndGlobal = new Window();
			wnd.CreateMainWindow(500, 320,
				SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_MAIN |
				SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_ALPHA |
				SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_POPUP |
				SciterXDef.SCITER_CREATE_WINDOW_FLAGS.SW_ENABLE_DEBUG);
			wnd.CenterTopLevelWindow();
			wnd.HideTaskbarIcon();
			wnd.Title = "Sciter-based desktop widget RLZ";
			wnd.Icon = Properties.Resources.IconMain;

			// Prepares SciterHost and then load the page
			var host = new Host();
			host.Setup(wnd);
			host.AttachEvh(new HostEvh());
			host.SetupPage("index.html");

			// Show window and Run message loop
			wnd.Show();
			PInvokeUtils.RunMsgLoop();
		}

		public static void RunHooker()
		{
			string hookexe = Environment.Is64BitOperatingSystem ? @"\Hook\64\Hooker.exe" : @"\Hook\32\Hooker.exe";
			hookexe = AppDomain.CurrentDomain.BaseDirectory + hookexe;
			Debug.Assert(System.IO.File.Exists(hookexe));

			var p = Process.Start(new ProcessStartInfo()
			{
				FileName = hookexe,
				WindowStyle = ProcessWindowStyle.Hidden
			});

			Debug.Assert(p.HasExited==false);
		}
	}
}