#pragma once

#include "Window.hpp"
#include "Host.hpp"


#if __OBJC__ // when compiling App.mm only
#import <Cocoa/Cocoa.h>

@interface AppDelegate : NSObject <NSApplicationDelegate>

@end
#endif

namespace App
{
	// Globals
	extern Window* AppWindow;
	extern Host* AppHost;

	// App lifecycle
	void Init();
	void Dispose();

	// Args handling
	bool ParseArgs(int argc, const char * argv[]);
}
