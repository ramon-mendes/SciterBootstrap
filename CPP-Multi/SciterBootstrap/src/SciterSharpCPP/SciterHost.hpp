//
//  SciterHost.hpp
//  SciterBootstrap
//
//  Created by Ramon Mendes on 09/04/17.
//  Copyright © 2017 MI Software. All rights reserved.
//

#pragma once

#include "sciter-x.h"
#include "sciter-x-host-callback.h"
#include "SciterElement.hpp"
#include "SciterWindow.hpp"
#include "SciterEventHandler.hpp"


class SciterHost : public sciter::host<SciterHost>
{
protected:
	SciterWindow& _wnd;
	
	SciterHost(SciterWindow& wnd) : _wnd(wnd)
	{
		setup_callback(wnd.GetHWND());
	}
	
public:
	void AttachEvh(SciterEventHandler& evh)
	{
		::SciterWindowAttachEventHandler(_wnd.GetHWND(), &sciter::event_handler::element_proc, &evh, HANDLE_ALL);
	}
	
protected:
	virtual LRESULT OnLoadData(LPSCN_LOAD_DATA sld) { return LOAD_OK; }
	
public:
	// sciter::host traits
	HWINDOW get_hwnd() const { return _wnd.GetHWND(); }
	HINSTANCE get_resource_instance() const { return ::GetModuleHandle(NULL); }
	
	LRESULT on_load_data(LPSCN_LOAD_DATA pnmld)
	{
		return OnLoadData(pnmld);
	}

/*protected:
	virtual bool OnAttachBehavior (SciterElement el, LPCSTR behaviorName, SciterEventHandler* elementEvh);
	virtual void OnDataLoaded (SCN_DATA_LOADED sdl);
	virtual void OnEngineDestroyed ();
	virtual void OnGraphicsCriticalFailure (HWINDOW hwnd);
	virtual LRESULT OnLoadData (SCN_LOAD_DATA sld);
	virtual LRESULT OnPostedNotification (void* wparam, void* lparam);*/
};
