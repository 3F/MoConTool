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
using System.Reflection;
using System.Runtime.InteropServices;
using net.r_eg.Conari.Log;
using net.r_eg.MoConTool.Exceptions;
using net.r_eg.MoConTool.Filters;
using net.r_eg.MoConTool.WinAPI;

namespace net.r_eg.MoConTool
{
    using HHOOK = IntPtr;
    using LPARAM = IntPtr;
    using LRESULT = IntPtr;
    using WPARAM = UIntPtr;

    public sealed class Mokona: IMokona
    {
        private IntPtr stopChainCode = new IntPtr(1);

        private HHOOK mhook = IntPtr.Zero;

        private NativeMethods.LowLevelMouseProc llMouseProc;
        
        /// <summary>
        /// Access to main loader.
        /// </summary>
        public IBootloader Loader
        {
            get;
            private set;
        }

        /// <summary>
        /// Activates tool in system.
        /// </summary>
        /// <returns>false value if it was already activated before, otherwise true.</returns>
        public bool plug()
        {
            if(mhook != IntPtr.Zero) {
                return false;
            }

            llMouseProc = new NativeMethods.LowLevelMouseProc(lowLevelMouseProc); // to protect from GC
            mhook = NativeMethods.SetWindowsHookEx
            (
                WindowsHookId.WH_MOUSE_LL,
                llMouseProc, 
                Marshal.GetHINSTANCE(Assembly.GetExecutingAssembly().ManifestModule), 
                0
            );

            if(mhook == IntPtr.Zero) {
                throw new WinFuncFailException();
            }

            LSender.Send(this, $"mhook + {mhook}", Message.Level.Debug);
            return true;
        }

        /// <summary>
        /// Deactivates tool from system.
        /// </summary>
        /// <returns>false value if it was not found or already deactivated before, otherwise true.</returns>
        public bool unplug()
        {
            if(mhook == IntPtr.Zero) {
                return false;
            }
            LSender.Send(this, $"mhook - {mhook}", Message.Level.Debug);

            var ret = NativeMethods.UnhookWindowsHookEx(mhook);
            mhook   = IntPtr.Zero;

            // the system can also automatically deactivate our hook because of big delay from filters.
            if(ret == 0) {
                //throw new WinFuncFailException();
                LSender.Send(
                    this, 
                    $"whoops, plugin was already deactivated by system or something went wrong - error {Marshal.GetLastWin32Error()}", 
                    Message.Level.Error
                );
            }
            return true;
        }

        public Mokona(IBootloader loader)
        {
            if(loader == null) {
                throw new ArgumentNullException("Bootloader cannot be null.");
            }

            Loader = loader;
        }

        /// <summary>
        /// Callback function used with the SetWindowsHookEx function. 
        /// The system calls this function every time a new mouse input event is about to be posted into a thread input queue.
        /// </summary>
        /// <param name="nCode">A code the hook procedure uses to determine how to process the message.</param>
        /// <param name="wParam">The identifier of the mouse message.</param>
        /// <param name="lParam">A pointer to an MSLLHOOKSTRUCT structure.</param>
        /// <returns></returns>
        private LRESULT lowLevelMouseProc(int nCode, WPARAM wParam, LPARAM lParam)
        {
            foreach(IMouseListener listener in Loader.ActivatedFilters)
            {
                FilterResult act = listener.msg(nCode, wParam, lParam);

                if(act == FilterResult.IgnoreFilters) {
                    break;
                }
                else if(act == FilterResult.Continue) {
                    continue;
                }

#if DEBUG
                LSender.Send(this, $"prevent msg({nCode}, {wParam}, {lParam}) by filter {listener.Id}:'{listener.Name}'", Message.Level.Trace);
#endif
                return stopChainCode;
            }

            return NativeMethods.CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }
    }
}
