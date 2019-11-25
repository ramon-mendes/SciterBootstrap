using SciterSharp;
using SciterSharp.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace SciterBootstrap
{
	class Host : BaseHost
	{
	}

	class HostEvh : SciterEventHandler
	{
	}

	// This base class overrides OnLoadData and does the resource loading strategy
	// explained at http://misoftware.com.br/Bootstrap/Dev
	//
	// - in DEBUG mode: resources loaded directly from the file system
	// - in RELEASE mode: resources loaded from by a SciterArchive (packed binary data contained as C# code in ArchiveResource.cs)
	class BaseHost : SciterHost
	{
		protected static SciterX.ISciterAPI _api = SciterX.API;
		protected SciterArchive _archive = new SciterArchive();
		protected SciterWindow _wnd;

		public BaseHost()
		{
#if !DEBUG
			_archive.Open(SciterSharpAppResource.ArchiveResource.resources);
#endif
		}

		public void Setup(SciterWindow wnd)
		{
			_wnd = wnd;
			SetupWindow(wnd._hwnd);
		}

		public void SetupPage(string page_from_res_folder)
		{
		#if DEBUG
			string path = Environment.CurrentDirectory + "/../../res/" + page_from_res_folder;
			Debug.Assert(File.Exists(path));
            path = path.Replace('\\', '/');
			path = Path.GetFullPath(path);
			Debug.Assert(File.Exists(path));

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
					_api.SciterDataReady(_wnd._hwnd, sld.uri, data, (uint) data.Length);
			}

			// call base to ensure LibConsole is loaded
			return base.OnLoadData(sld);
		}
	}
}