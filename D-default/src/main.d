// Linker libs
pragma(lib, r"vendor\sciter-dport\sciter32-import-lib.lib");

import winkit.WinAPI;
import HostSciter;
import HostWindow;


void Int3Handler(string file, size_t line, string msg) nothrow
{
	asm nothrow { int 3; }
}

void main(string[] args)
{
	debug
	{
		import core.exception;
		assertHandler = &Int3Handler;
	}

	BoilerplateInit();

	HostWindow.Setup();// Create HWND
	HostSciter.Setup();// Load Sciter page
	
	HostWindow.g_window.wnd.CenterTopLevelWindow();
	HostWindow.g_window.wnd.ShowWindow(SW_SHOWNORMAL);


	MSG msg;
	while( .IsWindow(HostWindow.g_window.wnd) && .GetMessageW(&msg, null, 0, 0) )
	{
		if( HostWindow.PreProcessMessage(&msg) )
			continue;

		.TranslateMessage(&msg);
		.DispatchMessageW(&msg);
	}
}

void BoilerplateInit()
{
	/*
	Initialization for COM, GDI+, and other things
	-call it before WindowingLoop()
	-note that assert(false) survives in the 'release' version (as said in D docs)
	*/

	// COM
	OleInitialize(NULL);

	// Common controls (manifest must be present)
	INITCOMMONCONTROLSEX InitCtrls;
	InitCtrls.dwICC = ICC_WIN95_CLASSES | ICC_STANDARD_CLASSES;	// ICC_STANDARD_CLASSES forces Common Controls version 6, else it fails, so make sure to add the manifest declaration
	// see: https://msdn.microsoft.com/en-us/library/windows/desktop/bb773175%28v=vs.85%29.aspx or http://stackoverflow.com/questions/13977583/how-to-enable-common-controls-in-a-windows-app
	InitCommonControlsEx(&InitCtrls) || assert(false);
}