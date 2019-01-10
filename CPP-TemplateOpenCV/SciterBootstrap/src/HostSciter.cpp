#include "stdafx.h"
#include "HostSciter.h"


HostSciter::HostSciter()
{
}

void HostSciter::Setup()
{
#if !_DEBUG
	// bind resources[] (defined in "archive.cpp") with the archive - RELEASE mode only
	#include "archive.cpp"
	sciter::archive::instance().open(aux::elements_of(resources));
#endif

	// Setup sciter::host
	setup_callback();

	// Attach Window event handler
	sciter::attach_dom_event_handler(get_hwnd(), this);

	// Load HTML
	LoadPage(L"index.html");
}

void HostSciter::LoadPage(wchar_t* path)
{
	std::wstring url;

#if _DEBUG
	wchar_t BUFF_CWD[1024];
	_wgetcwd(BUFF_CWD, 1024);

	url = L"file:///";
	url += BUFF_CWD;
	url += L"\\res\\";
	url += path;
#else
	url = L"this://app/index.html";
#endif

	bool load = load_file(url.c_str());
	_ASSERT(load);
}