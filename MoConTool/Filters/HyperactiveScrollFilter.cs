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
using net.r_eg.Conari.Log;
using net.r_eg.MoConTool.WinAPI;

namespace net.r_eg.MoConTool.Filters
{
    using LPARAM = IntPtr;
    using WPARAM = UIntPtr;

    public class HyperactiveScrollFilter: FilterAbstract, IMouseListener
    {
        public sealed class TData
        {
            public uint limit = 250;
            public int capacity = 14;
            public int acceleration = 16;
        }

        private volatile uint prevStamp = 0;
        private volatile int count = 0;

        /// <param name="nCode">A code that uses to determine how to process the message.</param>
        /// <param name="wParam">The identifier of the mouse message.</param>
        /// <param name="lParam">A pointer to an MSLLHOOKSTRUCT structure.</param>
        /// <returns></returns>
        public override FilterResult msg(int nCode, WPARAM wParam, LPARAM lParam)
        {
            if(Data == null || !SysMessages.EqOr(wParam, SysMessages.WM_MOUSEWHEEL)) {
                //TODO: SysMessages.WM_MOUSEHWHEEL
                return FilterResult.Continue;
            }
            var llhook = getMSLLHOOKSTRUCT(lParam);

            uint delta = llhook.time - prevStamp;
            var v = (TData)Data;

            if(delta > v.limit) {
                count = 0;
            }
            else {
                ++count;
            }

            if(count > v.capacity) {
                LSender.Send(this, $"Scroll has been filtered {wParam}/{count} = {v.capacity}:{v.limit}", Message.Level.Info);
                ((IMouseListenerSvc)this).trigger();
                return FilterResult.Abort;
            }

            //if(delta < v.acceleration && count > v.capacity) {
            //    ++accelerationCount;
            //}

            LSender.Send(this, $"MouseData: count {count} - delta {delta}", Message.Level.Trace);
            prevStamp = llhook.time;

            return FilterResult.Continue;
        }

        public HyperactiveScrollFilter()
            : base("HyperactiveScroll")
        {
            Data = new TData();
        }
    }
}
