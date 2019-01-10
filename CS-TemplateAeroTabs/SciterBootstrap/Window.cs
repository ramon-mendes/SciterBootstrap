using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using SciterSharp;

namespace SciterBootstrap
{
	class Window : SciterWindow
	{
		public void SetupAeroArea()
		{
			bool bDwmEnabled;
			DwmIsCompositionEnabled(out bDwmEnabled);

			if(bDwmEnabled)
			{
				MARGINS aeromargins = new MARGINS();
				aeromargins.topHeight = 74;
				DwmExtendFrameIntoClientArea(_hwnd, ref aeromargins);

				_api.SciterSetOption(_hwnd, SciterSharp.Interop.SciterXDef.SCITER_RT_OPTIONS.SCITER_TRANSPARENT_WINDOW, new IntPtr(1));
			} else {
				Debug.Assert(false, "Composition is not enabled.");
			}
		}

		#region PInvoke stuff
		[StructLayout(LayoutKind.Sequential)]
		struct MARGINS
		{
			public int leftWidth;
			public int rightWidth;
			public int topHeight;
			public int bottomHeight;
		}

		[DllImport("dwmapi.dll")]
		static extern int DwmIsCompositionEnabled(out bool enabled);

		[DllImport("dwmapi.dll")]
		static extern int DwmExtendFrameIntoClientArea(IntPtr hwnd, ref MARGINS margins);
		#endregion
	}
}
