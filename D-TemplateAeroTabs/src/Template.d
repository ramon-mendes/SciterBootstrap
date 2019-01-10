module Template;

import HostWindow;
import sciter.api;


void Setup()
{
	if(DwmEnabled())
	{
		MARGINS aeromargins = { 0, 0, 74, 0 };
		DwmSetAeroFrame(g_window.wnd, aeromargins);
		.SciterSetOption(g_window.wnd, SCITER_RT_OPTIONS.SCITER_TRANSPARENT_WINDOW, TRUE) || assert(false);
	} else {
		assert(false, "Composition is not enabled.");
	}
}


// DWM
pragma(lib, r"lib\dwmapi.lib");

static
{
private:
	import winkit.WinAPI;

	struct MARGINS {
		int cxLeftWidth;
		int cxRightWidth;
		int cyTopHeight;
		int cyBottomHeight;
	}
	extern(Windows) HRESULT DwmExtendFrameIntoClientArea(HWND, MARGINS*);
	extern(Windows) HRESULT DwmIsCompositionEnabled(BOOL* pfEnabled);

public:
	public bool DwmEnabled()
	{
		BOOL b;
		DwmIsCompositionEnabled(&b) == 0 || assert(false);
		return !!b;
	}
	public void DwmSetAeroFrame(HWND hwnd, MARGINS aeromargins = MARGINS(-1) )
	{
		.DwmExtendFrameIntoClientArea(hwnd, &aeromargins) == 0 || assert(false);
	}
}