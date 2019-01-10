using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SciterSharp;

namespace SciterBootstrap
{
	class Host : SciterHost
	{
		public Host(SciterWindow wnd)
		{
			SetupWindow(wnd);
			AttachEvh(new HostEvh());
		}
	}

	class HostEvh : SciterEventHandler
	{
		protected override bool OnScriptCall(SciterElement se, string name, SciterValue[] args, out SciterValue result)
		{
			if(name == "Host_Msg")
			{
				result = new SciterValue("Hello World!! from native side");
				return true;
			}
			return base.OnScriptCall(se, name, args, out result);
		}

		// TODO: override more methods of SciterEventHandler base class
	}
}