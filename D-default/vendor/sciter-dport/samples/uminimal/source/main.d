import sciter.interop.sciter_x_types;
import sciter.api;

void main()
{
	RECT frame;
	frame.right = 800;
	frame.bottom = 600;

	HWINDOW wnd = SciterCreateWindow(
		SCITER_CREATE_WINDOW_FLAGS.SW_TITLEBAR |
		SCITER_CREATE_WINDOW_FLAGS.SW_RESIZEABLE |
		SCITER_CREATE_WINDOW_FLAGS.SW_MAIN |
		SCITER_CREATE_WINDOW_FLAGS.SW_CONTROLS,
		&frame, null, null, null);
		
	SciterLoadFile(wnd, "minimal.html");
	
	
	version(Windows)
	{
		import winkit.WinAPI;
		
		ShowWindow(wnd, 1);
		
		MSG msg;
		while( GetMessageW(&msg, null, 0, 0) )
		{
			TranslateMessage(&msg);
			DispatchMessageW(&msg);
		}
	}
}