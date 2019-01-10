#include "stdafx.h"
#include "sciter-x.h"
#include "sciter-x-behavior.h"
#include "sciter-x-graphics.hpp"


using namespace sciter;

class SampleBehavior : sciter::event_handler, sciter::behavior_factory
{
public:
	SampleBehavior()
		: sciter::behavior_factory("SampleBehavior") // NAME of the behavior to refer in CSS
	{
	}

	// The only behavior_factory method:
	event_handler* create(HELEMENT he) override
	{
		return this;
	}

	
	// ---------------------------------------------------------------------------------
	// Override here the methods of sciter::event_handler to define your custom behavior
	// Example of overriding on_draw() and drawing a line:
	bool on_draw(HELEMENT he, UINT draw_type, HGFX hgfx, const RECT& rc) override
	{
		sciter::graphics gfx(hgfx);

		gfx.state_save();
		gfx.translate(rc.left, rc.top);

		sciter::POS points[8] = { 100, 0, 150, 100, 50, 100, 100, 0 };
		gfx.line_color(gcolor(255, 0, 0));
		gfx.line_width(3);
		gfx.polyline(points, 4 /*number of points*/);

		gfx.state_restore();
		return false;
	}
};


// You need a global instance of your behavior so engine can find it
// (sciter::behavior_factory base class registers this behavior in a global list)
SampleBehavior global_behavior_for_factory;
