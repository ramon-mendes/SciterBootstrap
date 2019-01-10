#include "stdafx.h"
#include "App.hpp"
#include <assert.h>

#if _M_IX86
	#error For compiling for x86/32bits in Visual Studio, make sure to replace sciter.dll with the 32-bits version from Sciter SDK
#endif



void BoilerplateInit()
{
	HRESULT hr = OleInitialize(0);
	assert(hr == S_OK);

	// Common controls (manifest must be present)
	{
		// Include the v6 common controls in the manifest
		#pragma comment(lib,"comctl32.lib")
		#pragma comment(linker,"/manifestdependency:\"type='win32' name='Microsoft.Windows.Common-Controls' version='6.0.0.0' processorArchitecture='*' publicKeyToken='6595b64144ccf1df' language='*'\"")

		INITCOMMONCONTROLSEX InitCtrls;
		InitCtrls.dwSize = sizeof(INITCOMMONCONTROLSEX);
		InitCtrls.dwICC = ICC_WIN95_CLASSES | ICC_STANDARD_CLASSES;
		BOOL res = InitCommonControlsEx(&InitCtrls);
		assert(res);
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

	App::Init();

	MSG msg;
	while(GetMessage(&msg, nullptr, 0, 0))
	{
		TranslateMessage(&msg);
		DispatchMessage(&msg);
	}

	App::Dispose();

	return msg.wParam;
}