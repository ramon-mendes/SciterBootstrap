#include "stdafx.h"
#include "App.hpp"
#include "sciter-x.h"


// Have to #include "../ArchiveResource.cpp" in a .cpp file and so declared BaseHost constructor here
#include "../ArchiveResource.cpp"

BaseHost::BaseHost(SciterWindow& wnd)
	: SciterHost(wnd)
{
	_archive.Open(aux::make_slice(resources));
}


namespace App
{
	Window* AppWindow;
	Host* AppHost;

	void Init()
	{
		::SciterSetOption(NULL, SCITER_SET_SCRIPT_RUNTIME_FEATURES, ALLOW_EVAL |
			ALLOW_FILE_IO |
			ALLOW_SOCKET_IO |
			ALLOW_SYSINFO);
		::SciterSetOption(NULL, SCITER_SET_DEBUG_MODE, TRUE);

		AppWindow = new Window();
		AppHost = new Host(*AppWindow);

		// center and show window
		AppWindow->CenterTopLevelWindow();
		AppWindow->Show();
	}

	void Dispose()
	{
		delete AppHost;
		delete AppWindow;
	}

	bool ParseArgs(int argc, const char * argv[])
	{
		for(int i = 0; i<argc; i++)
		{
			aux::chars wu = aux::chars_of(argv[i]);
		}
		return true;
	}
}
