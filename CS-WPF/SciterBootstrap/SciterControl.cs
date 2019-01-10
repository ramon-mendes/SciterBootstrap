using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Interop;
using SciterSharp;

namespace SciterBootstrap
{
	class SciterControl : HwndHost
	{
		private SciterWindow _wnd;
		private Host _host;

		protected override HandleRef BuildWindowCore(HandleRef hwndParent)
		{
			_wnd = new SciterWindow();
			_wnd.CreateChildWindow(hwndParent.Handle);

			// Create the SciterHost to handle window-level events
			_host = new Host(_wnd);

			// Load the HTML
			_wnd.LoadHtml(@"
<script type='text/tiscript'>
	$(h1).text = view.Host_Msg();
</script>

<h1 style='color: blue' />
");

			return new HandleRef(this, _wnd._hwnd);
		}

		protected override void DestroyWindowCore(HandleRef hwnd)
		{
			_wnd.Destroy();
		}
	}
}