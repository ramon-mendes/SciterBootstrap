module HostSciter;

import std.conv;
import sciter.api;
import sciter.behavior;
import sciter.host;
import sciter.sciter_value;
import SciterBaseHost;
import HostWindow;

// global accessible
Host g_host;
HostEvh g_host_evh;


void Setup()
{
	g_host = new Host();
	g_host_evh = new HostEvh();
	
	g_host.SetupWindow(HostWindow.wnd);
	g_host.attach_evh(g_host_evh);
	
	g_host.SetupPage("index.html");
}

public class Host : SciterBaseHost
{
	// Things to do here:
	// -override on_load_data() to customize or track resource loading
	// -override on_posted_notification() to handle notifications generated with SciterPostCallback()
}


class HostEvh : EventHandler
{
	override bool on_script_call(HELEMENT he, SCRIPTING_METHOD_PARAMS* prms)
	{
		switch(to!string(prms.name))
		{
			case "Host_HelloWorld":
				prms.result = json_value("Hello World! (from native side)");
				return true;

			default:
				return false;
		}
	}
}