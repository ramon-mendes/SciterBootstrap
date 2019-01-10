#include "stdafx.h"
#include "HostWindow.h"

LRESULT SC_CALLBACK ProcessWindowMessage(HWINDOW hwnd, UINT msg, WPARAM wParam, LPARAM lParam, LPVOID pParam, BOOL* pHandled)
{
	return 0;
}

HostWindow::HostWindow()
{
}

void HostWindow::Setup()
{
	// Calculate window size
	const float factorx = .72f;
	const float factory = .87f;
	const int minx = 800;
	const int miny = 600;
	float basex = GetSystemMetrics(SM_CXSCREEN)*factorx; if (basex < minx) basex = minx;
	float basey = GetSystemMetrics(SM_CYSCREEN)*factory; if (basey < miny) basey = miny;
	RECT rc = { 0, 0, (long)basex, (long)basey };

	// Create window
	m_wnd = ::SciterCreateWindow(SW_TITLEBAR | SW_CONTROLS | SW_MAIN | SW_ENABLE_DEBUG, &rc, &ProcessWindowMessage, this, NULL);

	// Title and icon
	SetWindowText(m_wnd, L"Sciter Bootstrap");
	SendMessage(m_wnd, WM_SETICON, ICON_SMALL, (LPARAM) LoadIcon(GetModuleHandle(NULL), MAKEINTRESOURCE(100)));
}

void HostWindow::CenterTopLevelWindow(HWND parent)
{
	if(parent == NULL)
		parent = GetDesktopWindow();

	HWND hwndParent = parent;
	RECT rectWindow, rectParent;

	GetWindowRect(m_wnd, &rectWindow);
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

	MoveWindow(m_wnd, nX, nY, nWidth, nHeight, FALSE);
}