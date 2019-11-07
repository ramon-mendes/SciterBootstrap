// DynamicHook.cpp : Defines the exported functions for the DLL application.

#include "stdafx.h"
#include "DynamicHook.h"
#include <string>
#include <algorithm>


bool shown = false;
bool is_explorer = false;

int HookProc(int nCode, WPARAM wParam, LPARAM lParam)
{
	// Per docs, if nCode < 0, the hook procedure must pass the 
	// message to CallNextHookEx function without further processing
	// and should return the value returned by CallNextHookEx. 
	if (!is_explorer || nCode < 0)
	{
		return CallNextHookEx(0, nCode, wParam, lParam);
	}
	else
	{
		// Only process the message if it has not already been 
		// removed from the queue.
		//if (wParam == PM_NOREMOVE)
		{
			// The lparam that Windows passes us is a pointer to a 
			// MSG struct.
			MSG* msg = (MSG *)lParam;

			if (msg->message == WM_USER + 83)
			{
				WCHAR fn[1024];
				GetWindowText(msg->hwnd, fn, 1024);

				std::wstring str;
				str += L"explorer.exe: YEAHH ";
				str += fn;
				str += L" - ";
				str += std::to_wstring(msg->message - WM_USER);
				str += L" - ";
				str += std::to_wstring(msg->lParam);
				str += L" - ";
				str += std::to_wstring(msg->wParam);
				OutputDebugString(str.c_str());

				auto wnd = FindWindow(NULL, L"Sciter-based desktop TemplateDesktopGadgets");
				if (wnd)
					SendMessage(wnd, WM_APP + 99, msg->lParam, msg->wParam);
			}
		}

		// Ensure that we don't break the hook chain.
		return CallNextHookEx(0, nCode, wParam, lParam);
	}

	return 0;
}