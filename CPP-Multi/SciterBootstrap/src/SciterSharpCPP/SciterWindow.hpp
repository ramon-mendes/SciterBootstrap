//
//  SciterWindow.hpp
//  SciterBootstrap
//
//  Created by Ramon Mendes on 09/04/17.
//  Copyright © 2017 MI Software. All rights reserved.
//

#pragma once

#include "sciter-x.h"


class SciterWindow
{
private:
	static constexpr UINT DefaultCreateFlags =
		SW_MAIN |
		SW_TITLEBAR |
		SW_RESIZEABLE |
		SW_CONTROLS |
		SW_ENABLE_DEBUG;
	
	HWINDOW _hwnd;

public:
	HWINDOW GetHWND() { return _hwnd; }
	
	void CreateMainWindow(int width, int height, UINT flags = DefaultCreateFlags)
	{
		RECT frame = { 0 };
		frame.right = width;
		frame.bottom = height;
		
		_hwnd = ::SciterCreateWindow(flags, &frame, nullptr, nullptr, nullptr);
	}
	
	bool LoadPage(LPCWSTR filename)
	{
		return ::SciterLoadFile(_hwnd, filename);
	}
	
	void SetTitle(LPCSTR title);
	void Show(bool show = true);
	void CenterTopLevelWindow();
};
