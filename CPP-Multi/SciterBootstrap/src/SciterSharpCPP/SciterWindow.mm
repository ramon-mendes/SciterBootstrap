#import <Cocoa/Cocoa.h>

#include "SciterWindow.hpp"

void SciterWindow::SetTitle(LPCSTR title)
{
	NSView* nsview = (__bridge NSView*) _hwnd;
	[[nsview window] setTitle:[NSString stringWithUTF8String:title]];
}

void SciterWindow::CenterTopLevelWindow()
{
	NSView* nsview = (__bridge NSView*) _hwnd;
	[[nsview window] center];
}

void SciterWindow::Show(bool show)
{
	NSView* nsview = (__bridge NSView*) _hwnd;
	if(show)
	{
		[[nsview window] makeMainWindow];
		[[nsview window] makeKeyAndOrderFront:nil];
	} else {
		[[nsview window] orderOut:[nsview window]];
	}
}
