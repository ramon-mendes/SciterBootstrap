#pragma once

#include "SciterBootstrap.h"


class HostSciter : public sciter::host<HostSciter>, public sciter::event_handler
{
public:
	HostSciter();
	void Setup();

public:
	// sciter::host traits:
	HWINDOW   get_hwnd() { return g_window.m_wnd; }
	HINSTANCE get_resource_instance() { _ASSERT(false); return 0; }

private:
	// Things to do here:
	// -override on_load_data() to customize or track resource loading
	// -override on_posted_notification() to handle notifications generated with SciterPostCallback()

private:
	void LoadPage(wchar_t* path);

	BEGIN_FUNCTION_MAP
		// Put here your handlers for function calls from script

		// Note: if you don't wan't to use this macro heavy way, I recommend you to declare 'virtual bool on_script_call(...)' yourself
		// Note: if you use FUNCTION_2 for example, your function handler NEEDS to have two json::value parameters
	END_FUNCTION_MAP
};