#pragma once

#include "sciter-x.h"
#include "SciterWindow.hpp"


class Window : public SciterWindow
{
public:
	Window()
	{
		CreateMainWindow(800, 600);
		SetTitle("SciterBootstrap");
	}
};
