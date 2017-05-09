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
using System.Collections.Generic;
using net.r_eg.MoConTool.WinAPI;

namespace net.r_eg.MoConTool.Filters
{
    using LPARAM = IntPtr;
    using WPARAM = UIntPtr;

    internal sealed class LMRContainer
    {
        private Dictionary<MouseState.Flags, ILMR> data;

        private IMouseListenerSvc listener;

        public FilterResult process(int nCode, WPARAM wParam, LPARAM lParam)
        {
            if(isFlag(MouseState.Flags.Left, wParam, SysMessages.WM_LBUTTONDOWN, SysMessages.WM_LBUTTONUP)) {
                return data[MouseState.Flags.Left].process(nCode, wParam, lParam);
            }

            if(isFlag(MouseState.Flags.Middle, wParam, SysMessages.WM_MBUTTONDOWN, SysMessages.WM_MBUTTONUP)) {
                return data[MouseState.Flags.Middle].process(nCode, wParam, lParam);
            }

            if(isFlag(MouseState.Flags.Right, wParam, SysMessages.WM_RBUTTONDOWN, SysMessages.WM_RBUTTONUP)) {
                return data[MouseState.Flags.Right].process(nCode, wParam, lParam);
            }

            return FilterResult.Continue;
        }

        public bool isFlag(MouseState.Flags flags, WPARAM wParam, uint code1, uint code2)
        {
            if((listener.Handler & flags) != 0
                && (SysMessages.EqOr(wParam, code1, code2)))
            {
                return true;
            }
            return false;
        }

        public LMRContainer(IMouseListenerSvc listener, Type t)
        {
            if(listener == null) {
                throw new ArgumentNullException();
            }
            this.listener = listener;

            ILMR l = (ILMR)Activator.CreateInstance(t);
            ILMR m = (ILMR)Activator.CreateInstance(t);
            ILMR r = (ILMR)Activator.CreateInstance(t);

            l.init(SysMessages.WM_LBUTTONDOWN, SysMessages.WM_LBUTTONUP, listener);
            m.init(SysMessages.WM_MBUTTONDOWN, SysMessages.WM_MBUTTONUP, listener);
            r.init(SysMessages.WM_RBUTTONDOWN, SysMessages.WM_RBUTTONUP, listener);

            data = new Dictionary<MouseState.Flags, ILMR>();
            data[MouseState.Flags.Left]   = l;
            data[MouseState.Flags.Middle] = m;
            data[MouseState.Flags.Right]  = r;
        }
    }
}
