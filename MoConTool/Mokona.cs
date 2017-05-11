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
using System.Collections.Concurrent;
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
    using ReCodesDict = ConcurrentDictionary<MouseState.Flags, bool>;

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
            int recode = findReCode(wParam);

            int idx = 0;
            foreach(IMouseListener listener in Loader.ActivatedFilters)
            {
                if(recode != -1 && idx++ <= recode) {
                    continue;
                }

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

        private int findReCode(WPARAM wParam)
        {
            Func<ReCodesDict, WPARAM, MouseState.Flags, bool> _re = (ReCodesDict _dict, WPARAM _wParam, MouseState.Flags _flags) =>
            {
                if(wParam != _wParam || !_dict.ContainsKey(_flags)) {
                    return false;
                }

                if(_dict[_flags]) {
                    _dict[_flags] = false;
                    return true;
                }
                return false;
            };

            int idx = -1;
            foreach(IMouseListener listener in Loader.ActivatedFilters)
            {
                ++idx;
                if(listener.ReCodes.Count < 1) {
                    continue;
                }

                if(_re(listener.ReCodes, wParam, MouseState.Flags.LeftDown)
                    || _re(listener.ReCodes, wParam, MouseState.Flags.LeftUp)
                    || _re(listener.ReCodes, wParam, MouseState.Flags.MiddleDown)
                    || _re(listener.ReCodes, wParam, MouseState.Flags.MiddleUp)
                    || _re(listener.ReCodes, wParam, MouseState.Flags.RightDown)
                    || _re(listener.ReCodes, wParam, MouseState.Flags.RightUp))
                {
                    return idx;
                }
            }

            return -1;
        }
    }
}
