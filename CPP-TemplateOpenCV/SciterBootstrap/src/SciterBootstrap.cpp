#include "stdafx.h"
#include "Ole2.h"
#include "SciterBootstrap.h"


HostWindow g_window;
HostSciter g_host;

void BoilerplateInit()
{
	OleInitialize(0);

	// Common controls (manifest must be present)
	{
		// Include the v6 common controls in the manifest
		#pragma comment(lib,"comctl32.lib")
		#pragma comment(linker,"/manifestdependency:\"type='win32' name='Microsoft.Windows.Common-Controls' version='6.0.0.0' processorArchitecture='*' publicKeyToken='6595b64144ccf1df' language='*'\"")

		INITCOMMONCONTROLSEX InitCtrls;
		InitCtrls.dwSize = sizeof(INITCOMMONCONTROLSEX);
		InitCtrls.dwICC = ICC_WIN95_CLASSES | ICC_STANDARD_CLASSES;
		BOOL res = InitCommonControlsEx(&InitCtrls); _ASSERT(res);
	}
}

int APIENTRY wWinMain(_In_ HINSTANCE hInstance,
	_In_opt_ HINSTANCE hPrevInstance,
	_In_ LPWSTR    lpCmdLine,
	_In_ int       nCmdShow)
{
	UNREFERENCED_PARAMETER(hPrevInstance);
	UNREFERENCED_PARAMETER(lpCmdLine);

	BoilerplateInit();

	g_window.Setup();
	g_host.Setup();

	// center and show window
	g_window.CenterTopLevelWindow();
	ShowWindow(g_window.m_wnd, TRUE);

	MSG msg;
	while(GetMessage(&msg, nullptr, 0, 0))
	{
		TranslateMessage(&msg);
		DispatchMessage(&msg);
	}

	return msg.wParam;
}