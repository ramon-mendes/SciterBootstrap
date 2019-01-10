using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SciterSharp;
using SciterSharp.Interop;

namespace SciterBootstrap
{
	class SciterMessages : SciterDebugOutputHandler
	{
		protected override void OnOutput(SciterSharp.Interop.SciterXDef.OUTPUT_SUBSYTEM subsystem, SciterSharp.Interop.SciterXDef.OUTPUT_SEVERITY severity, string text)
		{
			Console.WriteLine(text);// prints to console Sciter's debug output (works even if 'native debugging' is off)
		}
	}

	static class App
	{
		private static SciterMessages sm = new SciterMessages();
		public static Window AppWnd { get; private set; }
		public static Host AppHost { get; private set; }

		public static void Run()
		{
			// Create the window
			AppWnd = new Window();

			// Prepares SciterHost and then load the page
			AppHost = new Host(AppWnd);

#if !OSX
			PInvokeUtils.RunMsgLoop();
#endif
		}
	}
}