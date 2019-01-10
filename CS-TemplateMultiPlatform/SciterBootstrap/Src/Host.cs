using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using SciterSharp;
using SciterSharp.Interop;

namespace SciterBootstrap
{
	class Host : BaseHost
	{
		public Host(SciterWindow wnd)
		{
			var host = this;
			host.Setup(wnd);
			host.AttachEvh(new HostEvh());
			host.SetupPage("index.html");
			wnd.Show();
		}

		// Things to do here:
		// -override OnLoadData() to customize or track resource loading
		// -override OnPostedNotification() to handle notifications generated with SciterHost.PostNotification()
	}

	class HostEvh : SciterEventHandler
	{
		// A dynamic script call handler. Any call in TIScript to function 'view.Host_HelloWorld()' with invoke this method
		// Notice that signature of these handlers is always the same
		// (Hint: install OmniCode snippets which adds the 'ssh' snippet to C# editor so you can easily declare 'Siter Handler' methods)
		// (see: https://github.com/MISoftware/OmniCode-Snippets)
		public bool Host_HelloWorld(SciterElement el, SciterValue[] args, out SciterValue result)
		{
			result = new SciterValue("Hello World! (from native side)");
			return true;
		}

		// (Hint: to overload C# methods of SciterEventHandler base class, type 'override', press space, and Visual Studio will suggest the methods you can override)
	}

	// This base class overrides OnLoadData and does the resource loading strategy
	// explained at http://misoftware.com.br/Bootstrap/Dev
	//
	// - in DEBUG mode: resources loaded directly from the file system
	// - in RELEASE mode: resources loaded from by a SciterArchive (packed binary data contained as C# code in ArchiveResource.cs)
	class BaseHost : SciterHost
	{
		protected static SciterArchive _archive = new SciterArchive();
		protected SciterWindow _wnd;
		private static string _rescwd;

		static BaseHost()
		{
#if DEBUG
			_rescwd = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location).Replace('\\', '/');
	#if OSX
			_rescwd += "/../../../../../res/";
	#else
			_rescwd += "/../../res/";
	#endif

			_rescwd = Path.GetFullPath(_rescwd).Replace('\\', '/');
			Debug.Assert(Directory.Exists(_rescwd));
#else
			_archive.Open(SciterAppResource.ArchiveResource.resources);
#endif
		}

		public void Setup(SciterWindow wnd)
		{
			_wnd = wnd;
			SetupWindow(wnd);
		}

		public void SetupPage(string page_from_res_folder)
		{
			string path = _rescwd + page_from_res_folder;
			Debug.Assert(File.Exists(path));

#if DEBUG
			string url = "file://" + path;
#else
			string url = "archive://app/" + page_from_res_folder;
#endif

			bool res = _wnd.LoadPage(url);
			Debug.Assert(res);
		}

		protected override SciterXDef.LoadResult OnLoadData(SciterXDef.SCN_LOAD_DATA sld)
		{
			if(sld.uri.StartsWith("archive://app/"))
			{
				// load resource from SciterArchive
				string path = sld.uri.Substring(14);
				byte[] data = _archive.Get(path);
				if(data!=null)
					SciterX.API.SciterDataReady(sld.hwnd, sld.uri, data, (uint) data.Length);
			}

			// call base to ensure LibConsole is loaded
			return base.OnLoadData(sld);
		}
	}
}