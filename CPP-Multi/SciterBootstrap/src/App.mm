#import "App.hpp"

#include "sciter-x.h"


@implementation AppDelegate

- (void)applicationDidFinishLaunching:(NSNotification*)aNotification {
	App::Init();
}


- (void)applicationWillTerminate:(NSNotification*)aNotification {
	App::Dispose();
}

@end
