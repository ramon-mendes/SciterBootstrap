module SciterBaseHost;

import std.conv;
import std.file;

import sciter.interop.sciter_x_types;
import sciter.api;
import sciter.host;
import Utils;

private
{
	SciterArchive s_archive;
}

static this()
{
	// Setup archives
	debug {}
	else
	{
		import archive;
		s_archive = new SciterArchive;
		s_archive.open(archive.resources);
	}
}

class SciterBaseHost : SciterWindowHost
{
	void SetupWindow(HWINDOW hwnd)
	{
		setup_callback(hwnd);

		debug
		{
			SciterSetOption(hwnd, SCITER_RT_OPTIONS.SCITER_SET_DEBUG_MODE, true) || assert(false);
		}
	}
	
	void SetupPage(wstring path)
	{
		assert(m_hwnd, "Call SetupWindow first");

		// choose page to load
		wstring url;
		debug
		{
			string cwd = getcwd();
			url = "file:///" ~ to!wstring(cwd) ~ "\\res\\" ~ path;
		} else {
			url = "archive://app/" ~ path;
		}

		// load the page
		SciterLoadFile(m_hwnd, url.ptr) || assert(false);
	}
	
	override uint on_load_data(LPSCN_LOAD_DATA pnmld)
	{
		import std.algorithm.searching;
		wstring url = pnmld.uri.to_wstring();
		
		if( url.startsWith("archive://app/") )
		{
			auto data = s_archive.get( url[14..$] );
			if(data)
				SciterDataReady(m_hwnd, url.ptr, data.ptr, data.length) || assert(false);
		}
		return 0;
	}
}