using AForge.Video.DirectShow;
using SciterSharp;
using SciterSharp.Interop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SciterBootstrap
{
	class CameraEvh : SciterEventHandler
	{
		private IntPtr _vd_ptr;
		private SciterXVideoAPI.video_destination_vtable? _vd;
		private VideoCaptureDevice _videoSource;
		private bool _firstFrame = true;

		protected override void Subscription(SciterElement se, out SciterXBehaviors.EVENT_GROUPS event_groups)
		{
			event_groups = SciterXBehaviors.EVENT_GROUPS.HANDLE_BEHAVIOR_EVENT;
		}

		protected override void Detached(SciterElement se)
		{
			if(_vd!=null)
				_vd.Value.release(_vd_ptr);
			if(_videoSource!=null && _videoSource.IsRunning)
				_videoSource.Stop();
		}

		protected override bool OnEvent(SciterElement se, SciterElement target, SciterXBehaviors.BEHAVIOR_EVENTS type, IntPtr reason, SciterValue data)
		{
			if(type==SciterXBehaviors.BEHAVIOR_EVENTS.VIDEO_BIND_RQ)
			{
				if(reason==IntPtr.Zero)
					return true;

				IntPtr ptr_vtable = Marshal.ReadIntPtr(reason);
				_vd_ptr = reason;
				_vd = (SciterXVideoAPI.video_destination_vtable) Marshal.PtrToStructure(ptr_vtable, typeof(SciterXVideoAPI.video_destination_vtable));
				_vd.Value.add_ref(_vd_ptr);
				return true;
			}
			return false;
		}

		protected override bool OnScriptCall(SciterElement se, string name, SciterValue[] args, out SciterValue result)
		{
			result = null;

			if(name=="start_device")
			{
				var videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
				if(videoDevices.Count==0)
				{
					result = new SciterValue(false);
				}
				else
				{
					_videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
					_videoSource.VideoResolution = _videoSource.VideoCapabilities.FirstOrDefault(vc => vc.FrameSize.Width==640);
					_videoSource.NewFrame += NewFrameEventHandler;
					_videoSource.Start();

					result = new SciterValue(true);
				}

				return true;
			}

			return false;
		}

		private void NewFrameEventHandler(object sender, AForge.Video.NewFrameEventArgs eventArgs)
		{
			Bitmap bitmap = eventArgs.Frame;

			if(_firstFrame)
			{
				Debug.Assert(bitmap.PixelFormat == PixelFormat.Format24bppRgb);
				_vd.Value.start_streaming(_vd_ptr, bitmap.Width, bitmap.Height, SciterXVideoAPI.COLOR_SPACE.COLOR_SPACE_RGB24, IntPtr.Zero);
				_firstFrame = false;
			}

			if(_vd.Value.is_alive(_vd_ptr))
			{
				BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, bitmap.PixelFormat);
				int bytes = Math.Abs(data.Stride) * data.Height;
				_vd.Value.render_frame(_vd_ptr, data.Scan0, (uint) bytes);
				bitmap.UnlockBits(data);
			}
		}
	}
}
