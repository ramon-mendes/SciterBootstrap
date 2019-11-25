// dllmain.cpp : Defines the entry point for the DLL application.
#include "stdafx.h"
#include <string>
#include <algorithm> 
#include "DynamicHook.h"

BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
                     )
{
	HMODULE h = ::GetModuleHandle(NULL);
	WCHAR fn[1024];
	GetModuleFileName(h, fn, 1024);

	std::wstring str(fn);
	std::transform(str.begin(), str.end(), str.begin(), ::towlower);

    switch (ul_reason_for_call)
    {
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
		break;
	case DLL_PROCESS_DETACH:
		OutputDebugString((L"Dettach " + str).c_str());
		break;
	case DLL_PROCESS_ATTACH:
		{
			is_explorer = endsWith(str, std::wstring(L"explorer.exe"));
			OutputDebugString((L"Attach " + str).c_str());
		}
		break;
    }
    return TRUE;
}