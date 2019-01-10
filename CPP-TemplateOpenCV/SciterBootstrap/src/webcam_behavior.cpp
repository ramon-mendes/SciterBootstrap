#include "stdafx.h"

#include "sciter-x-video-api.h"
#include "sciter-x-threads.h"
#include <opencv2/opencv.hpp>
#include <opencv2/video.hpp>
#include <iostream>
#include "CTimer.h"

// Link OpenCV import lib
#if _DEBUG
	#pragma comment(lib, "vendor/opencv/build/x86/vc12/lib/opencv_world300d.lib")
#else
	#pragma comment(lib, "vendor/opencv/build/x86/vc12/lib/opencv_world300d.lib")
#endif

using namespace cv;
using namespace std;


struct webcam_behavior : public sciter::event_handler
{
	aux::asset_ptr<sciter::video_destination> rendering_site;
	VideoCapture vc;
	CTimer timer;
	float current_fps;

	virtual ~webcam_behavior() {}

	virtual void detached(HELEMENT he) { delete this; }

	virtual bool subscription(HELEMENT he, UINT& event_groups)
	{
		event_groups = HANDLE_BEHAVIOR_EVENT; // we only handle VIDEO_BIND_RQ here
		return true;
	}

	virtual bool on_event(HELEMENT he, HELEMENT target, BEHAVIOR_EVENTS type, UINT_PTR reason)
	{
		if(type != VIDEO_BIND_RQ)
			return false;

		if(!reason)
			return true; // first phase, consume the event to mark as we will provide frames 

		rendering_site = (sciter::video_destination*) reason;
		return true;
	}

	BEGIN_FUNCTION_MAP
		FUNCTION_0("start_device", start_device)
		FUNCTION_0("get_FPS", get_FPS)
	END_FUNCTION_MAP

	sciter::value start_device()
	{
		vc.open(0);
		if(vc.isOpened())
		{
			sciter::thread(ThreadedCapture, this);
			return json::value(true);
		}
		return json::value(false);
	}

	sciter::value get_FPS()
	{
		return json::value(current_fps);
	}

	static void ThreadedCapture(webcam_behavior* webcam)
	{
		sciter::video_destination& vd = *webcam->rendering_site;
		VideoCapture& vc = webcam->vc;

		int width = (int)vc.get(CAP_PROP_FRAME_WIDTH);
		int height = (int)vc.get(CAP_PROP_FRAME_HEIGHT);
		vd.start_streaming(width, height, sciter::COLOR_SPACE_RGB24);

		// FPS calc vars
		webcam->timer.Start();

		// get webcam frames
		while(true)
		{
			if(!vd.is_alive())
				return;

			Mat frame;
			if(vc.read(frame))
			{
				// match sciter::COLOR_SPACE_RGB24
				_ASSERT(frame.elemSize() == 3);
				_ASSERT(frame.type() == CV_8UC3);

				int size = frame.dataend - frame.datastart;
				vd.render_frame(frame.datastart, size);
			}

			webcam->timer.Update();
			webcam->current_fps = webcam->timer.GetFPS();
		}
	}
};

struct webcam_behavior_factory : public sciter::behavior_factory
{
	webcam_behavior_factory() : behavior_factory("camera") {}

	// the only behavior_factory method:
	virtual sciter::event_handler* create(HELEMENT he) { return new webcam_behavior(); }
};

webcam_behavior_factory factory_instance;