import std.stdio;
import std.conv;
import std.traits;

import winkit.WinAPI;
import winkit.Window;
import winkit.WindowImplMixin;
import sciter.api;


void Setup()
{
	// Calculate window size
	enum factorx = .72f;
	enum factory = .87f;
	enum minx = 800;
	enum miny = 600;
	float basex = .GetSystemMetrics(SM_CXSCREEN)*factorx; if(basex < minx) basex = minx;
	float basey = .GetSystemMetrics(SM_CYSCREEN)*factory; if(basey < miny) basey = miny;

	// Create window
	g_window.Create(SIZE(cast(int)basex, cast(int)basey), "Sciter Bootstrap");
}


// Window interface
public
{
	mixin WindowImplMixin!true g_window;
}

private
{
	bool ProcessWindowMessage(UINT message, WPARAM wParam, LPARAM lParam, ref LRESULT lResult)
	{
		switch(message)
		{
		default:
			return false;
		}
	}

	public bool PreProcessMessage(MSG* pMsg)
	{
		return false;
	}
}