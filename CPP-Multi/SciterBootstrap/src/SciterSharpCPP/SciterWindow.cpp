#ifdef _WIN32// Window only

#include "..\stdafx.h"
#include "SciterWindow.hpp"

void SciterWindow::SetTitle(LPCSTR title)
{
	SetWindowTextA(_hwnd, title);
}

void SciterWindow::Show(bool show)
{
	ShowWindow(_hwnd, TRUE);
}

void SciterWindow::CenterTopLevelWindow()
{
	//if(parent == NULL)
	HWND parent = GetDesktopWindow();

	HWND hwndParent = parent;
	RECT rectWindow, rectParent;

	GetWindowRect(_hwnd, &rectWindow);
	GetWindowRect(hwndParent, &rectParent);

	int nWidth = rectWindow.right - rectWindow.left;
	int nHeight = rectWindow.bottom - rectWindow.top;

	int nX = ((rectParent.right - rectParent.left) - nWidth) / 2 + rectParent.left;
	int nY = ((rectParent.bottom - rectParent.top) - nHeight) / 2 + rectParent.top;

	int nScreenWidth = GetSystemMetrics(SM_CXSCREEN);
	int nScreenHeight = GetSystemMetrics(SM_CYSCREEN);

	if (nX < 0) nX = 0;
	if (nY < 0) nY = 0;
	if (nX + nWidth > nScreenWidth) nX = nScreenWidth - nWidth;
	if (nY + nHeight > nScreenHeight) nY = nScreenHeight - nHeight;

	MoveWindow(_hwnd, nX, nY, nWidth, nHeight, FALSE);
}
#endif