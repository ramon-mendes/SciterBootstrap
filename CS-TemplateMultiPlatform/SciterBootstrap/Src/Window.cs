﻿using System;
using SciterSharp;

namespace SciterBootstrap
{
	public class Window : SciterWindow
	{
		public Window()
		{
			var wnd = this;
			wnd.CreateMainWindow(800, 600);
			wnd.CenterTopLevelWindow();
			wnd.Title = "SciterBootstrap";
			#if WINDOWS
			wnd.Icon = Properties.Resources.IconMain;
			#endif
		}
	}
}