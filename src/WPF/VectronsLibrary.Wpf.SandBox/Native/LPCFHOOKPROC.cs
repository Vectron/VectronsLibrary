using System;
using System.Runtime.InteropServices;

namespace Windows.Win32.UI.Controls.Dialogs;

/// <summary>
/// A callback function, which you define in your application,
/// that processes messages sent to a window.
/// The WNDPROC type defines a pointer to this callback function.
/// The WndProc name is a placeholder for the name of the function that you define in your application.
/// </summary>
/// <param name="hWnd">A handle to the window. This parameter is typically named hWnd.</param>
/// <param name="msg">The message. This parameter is typically named uMsg.</param>
/// <param name="wParam">Additional message information. This parameter is typically named wParam.
/// The contents of the wParam parameter depend on the value of the uMsg parameter.</param>
/// <param name="lParam">Additional message information. This parameter is typically named lParam.
/// The contents of the lParam parameter depend on the value of the uMsg parameter.</param>
/// <returns>The return value is the result of the message processing, and depends on the message sent.</returns>
[UnmanagedFunctionPointer(CallingConvention.Winapi)]
internal unsafe delegate IntPtr LPCFHOOKPROC(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam);