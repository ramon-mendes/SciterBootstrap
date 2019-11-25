#pragma once

#include <string>
#include <algorithm>


extern bool is_explorer;

static bool endsWith(const std::wstring& str, const std::wstring& suffix)
{
	return str.size() >= suffix.size() && 0 == str.compare(str.size() - suffix.size(), suffix.size(), suffix);
}

extern "C" __declspec(dllexport) int __cdecl HookProc(int nCode, WPARAM wParam, LPARAM lParam);