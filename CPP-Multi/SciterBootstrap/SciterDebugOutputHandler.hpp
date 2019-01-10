//
//  SciterDebugOutputHandler.hpp
//  SciterBootstrap
//
//  Created by Ramon Mendes on 09/04/17.
//  Copyright © 2017 MI Software. All rights reserved.
//

#pragma once

#include "sciter-x.h"


class SciterDebugOutputHandler
{
protected:
	SciterDebugOutputHandler(HWINDOW hwnd = 0)
	{
		::SciterSetupDebugOutput(hwnd, this, _output_debug);
	}

	virtual void OnOutput(OUTPUT_SUBSYTEMS subsystem, OUTPUT_SEVERITY severity, LPCWSTR text, UINT text_length) = 0;

private:
	static VOID SC_CALLBACK _output_debug(LPVOID param, UINT subsystem, UINT severity, LPCWSTR text, UINT text_length)
	{
		static_cast<SciterDebugOutputHandler*>(param)->OnOutput((OUTPUT_SUBSYTEMS)subsystem, (OUTPUT_SEVERITY)severity, (const WCHAR*)text, text_length);
	}
};