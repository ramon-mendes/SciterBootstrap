#pragma once


class HostWindow
{
public:
	HostWindow();
	void Setup();
	void CenterTopLevelWindow(HWND parent = NULL);

public:
	HWINDOW m_wnd;
};