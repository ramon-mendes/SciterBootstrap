#pragma once

#include "SciterHost.hpp"
#include "SciterArchive.hpp"

#include "aux-slice.h"
#include <string>


class BaseHost : public SciterHost
{
public:
	SciterArchive _archive;

	BaseHost(SciterWindow& wnd);

	void SetupPage(LPCWSTR page_from_res_folder)
	{
		std::ustring path;
		path = WSTR("archive://app/");
		path += page_from_res_folder;
		_wnd.LoadPage(path.c_str());
	}

	virtual LRESULT OnLoadData(LPSCN_LOAD_DATA pnmld) override
	{
		aux::wchars wu = aux::chars_of(pnmld->uri);

		// Load resource from SciterArchive
		if (wu.like(WSTR("archive://app/*")))
		{
			auto data = _archive.Get(wu.start + 14);
			if (data.length != 0)
				::SciterDataReady(_wnd.GetHWND(), pnmld->uri, data.start, data.length);
		}

#if false
		// LibConsole handling
		else if (wu.like(WSTR("sciter:debug-peer.tis")) ||
			wu.like(WSTR("sciter:console.tis")) ||
			wu.like(WSTR("sciter:utils.tis")) ||
			wu.like(WSTR("sciter:tracewnd.html")))
		{
			wu.prune(7);
			auto newurl = WSTR("LibConsole/") + aux::make_string(wu);
			auto data = _archive.Get(newurl.c_str());
			::SciterDataReady(_wnd.GetHWND(), pnmld->uri, data.start, data.length);
		}
#endif
		return LOAD_OK;
	}
};