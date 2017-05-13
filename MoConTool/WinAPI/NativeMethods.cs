/*
 * The MIT License (MIT)
 * 
 * Copyright (c) 2017  Denis Kuzmin < entry.reg@gmail.com > :: github.com/3F
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
*/

using System;
using System.Runtime.InteropServices;

namespace net.r_eg.MoConTool.WinAPI
{
    using BOOL = Int32;
    using BYTE = Byte;
    using DWORD = UInt32;
    using HHOOK = IntPtr;
    using HINSTANCE = IntPtr;
    using LONG = Int32;
    using LPARAM = IntPtr;
    using LRESULT = IntPtr;
    using UINT = UInt32;
    using ULONG_PTR = UIntPtr;
    using WORD = UInt16;
    using WPARAM = UIntPtr;

    internal static class NativeMethods
    {
        /// <summary>
        /// The system assigns a slightly higher priority to the thread that created the foreground window. 
        /// https://msdn.microsoft.com/en-us/library/windows/desktop/ms633539(v=vs.85).aspx
        /// </summary>
        /// <param name="hWnd">A handle to the window that should be activated and brought to the foreground.</param>
        /// <returns>If the window was brought to the foreground, the return value is nonzero. </returns>
        [DllImport("User32", CharSet = CharSet.Auto)]
        public static extern BOOL SetForegroundWindow(HandleRef hWnd);

        /// <summary>
        /// Synthesizes keystrokes, mouse motions, and button clicks.
        /// https://msdn.microsoft.com/en-us/library/windows/desktop/ms646310(v=vs.85).aspx
        /// </summary>
        /// <param name="nInputs">The number of structures in the pInputs array.</param>
        /// <param name="pInputs">An array of INPUT structures. Each structure represents an event to be inserted into the keyboard or mouse input stream.</param>
        /// <param name="cbSize">The size, in bytes, of an INPUT structure.</param>
        /// <returns>
        /// The function returns the number of events that it successfully inserted into the keyboard or mouse input stream. If the function returns zero, the input was already blocked by another thread.
        /// </returns>
        [DllImport("User32", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern UINT SendInput(UINT nInputs, [MarshalAs(UnmanagedType.LPArray), In] LPINPUT[] pInputs, int cbSize);

        /// <summary>
        /// nstalls an application-defined hook procedure into a hook chain.
        /// https://msdn.microsoft.com/en-us/library/windows/desktop/ms644990(v=vs.85).aspx
        /// </summary>
        /// <param name="idHook">The type of hook procedure to be installed.</param>
        /// <param name="lpfn">A pointer to the hook procedure.</param>
        /// <param name="hMod">A handle to the DLL containing the hook procedure pointed to by the lpfn parameter. Use GetModuleHandle() or Marshal.GetHINSTANCE() or NULL</param>
        /// <param name="dwThreadId">The identifier of the thread with which the hook procedure is to be associated. 0 for global.</param>
        /// <returns>
        /// If the function succeeds, the return value is the handle to the hook procedure.
        /// If the function fails, the return value is NULL. To get extended error information, call GetLastError.
        /// </returns>
        [DllImport("user32", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern HHOOK SetWindowsHookEx(int idHook, LowLevelMouseProc lpfn, HINSTANCE hMod, int dwThreadId);

        /// <summary>
        /// Callback function used with the SetWindowsHookEx function. 
        /// The system calls this function every time a new mouse input event is about to be posted into a thread input queue. 
        /// https://msdn.microsoft.com/en-us/library/windows/desktop/ms644986(v=vs.85).aspx
        /// </summary>
        /// <param name="nCode">A code the hook procedure uses to determine how to process the message.</param>
        /// <param name="wParam">The identifier of the mouse message. Can be: WM_LBUTTONDOWN, WM_LBUTTONUP, WM_MOUSEMOVE, WM_MOUSEWHEEL, WM_MOUSEHWHEEL, WM_RBUTTONDOWN, WM_RBUTTONUP, WM_MBUTTONDOWN, WM_MBUTTONUP.</param>
        /// <param name="lParam">A pointer to an MSLLHOOKSTRUCT structure.</param>
        /// <returns> 
        /// It may return a nonzero value to prevent the system from passing the message to the rest of the hook chain.
        /// 
        /// If nCode is less than zero, the hook procedure must return the value returned by CallNextHookEx.
        /// 
        /// If nCode is greater than or equal to zero, and the hook procedure did not process the message, 
        /// it is highly recommended that you call CallNextHookEx and return the value it returns; otherwise, 
        /// other applications that have installed WH_MOUSE_LL hooks will not receive hook notifications and may behave incorrectly as a result.
        /// </returns>
        public delegate LRESULT LowLevelMouseProc(int nCode, WPARAM wParam, LPARAM lParam);

        /// <summary>
        /// Removes a hook procedure installed in a hook chain by the SetWindowsHookEx function. 
        /// https://msdn.microsoft.com/en-us/library/windows/desktop/ms644993(v=vs.85).aspx
        /// </summary>
        /// <param name="hhk">A handle to the hook to be removed. This parameter is a hook handle obtained by a previous call to SetWindowsHookEx.</param>
        /// <returns>
        /// If the function succeeds, the return value is nonzero.
        /// If the function fails, the return value is zero. To get extended error information, call GetLastError.
        /// </returns>
        [DllImport("user32", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern BOOL UnhookWindowsHookEx(HHOOK hhk);

        /// <summary>
        /// Passes the hook information to the next hook procedure in the current hook chain. 
        /// A hook procedure can call this function either before or after processing the hook information. 
        /// https://msdn.microsoft.com/en-us/library/windows/desktop/ms644974(v=vs.85).aspx
        /// </summary>
        /// <param name="hhk">This parameter is ignored.</param>
        /// <param name="nCode">The next hook procedure uses this code to determine how to process the hook information.</param>
        /// <param name="wParam">The wParam value passed to the current hook procedure.</param>
        /// <param name="lParam">The lParam value passed to the current hook procedure.</param>
        /// <returns>This value is returned by the next hook procedure in the chain. The current hook procedure must also return this value.</returns>
        [DllImport("user32", CharSet = CharSet.Auto)]
        public static extern LRESULT CallNextHookEx(HHOOK hhk, int nCode, WPARAM wParam, LPARAM lParam);

        /// <summary>
        /// Defines a system-wide hot key.
        /// https://msdn.microsoft.com/en-us/library/windows/desktop/ms646309(v=vs.85).aspx
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="id"></param>
        /// <param name="fsModifiers"></param>
        /// <param name="vk"></param>
        /// <returns></returns>
        [DllImport("user32", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        /// <summary>
        /// Frees a hot key previously registered by the calling thread.
        /// https://msdn.microsoft.com/en-us/library/windows/desktop/ms646327%28v=vs.85%29.aspx
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [DllImport("user32", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        /// <summary>
        /// Retrieves the status of the specified virtual key.
        /// https://msdn.microsoft.com/en-us/library/windows/desktop/ms646301(v=vs.85).aspx
        /// https://msdn.microsoft.com/en-us/library/windows/desktop/dd375731%28v=vs.85%29.aspx
        /// </summary>
        /// <param name="nVirtKey"></param>
        /// <returns></returns>
        [DllImport("user32", CharSet = CharSet.Auto)]
        public static extern short GetKeyState(int nVirtKey);
    }
}
