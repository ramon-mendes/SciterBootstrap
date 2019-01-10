#import <Cocoa/Cocoa.h>
#import "App.hpp"

#include "sciter-x.h"


int main(int argc, const char* argv[])
{
	// Default GFX in Sciter v4 is Skia, switch to CoreGraphics (seems more stable)
	SciterSetOption(nullptr, SCITER_SET_GFX_LAYER, GFX_LAYER_CG);
	
	App::ParseArgs(argc, argv);
	
	NSApplication* application = [NSApplication sharedApplication];
	
	NSArray* tl;
	[[NSBundle mainBundle] loadNibNamed:@"MainMenu" owner:application topLevelObjects:&tl];
	
	AppDelegate* applicationDelegate = [[AppDelegate alloc] init];
	[application setDelegate:applicationDelegate];
	[application run];
}
