﻿/*
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
using net.r_eg.MoConTool.Log;
using net.r_eg.MoConTool.WinAPI;

namespace net.r_eg.MoConTool.Filters
{
    using LPARAM = IntPtr;
    using WPARAM = UIntPtr;

    public class DoubleClicksFilter: FilterAbstract, IMouseListener
    {
        private LMRContainer lmr;

        internal sealed class LMR: LMRAbstract, ILMR
        {
            private volatile bool isPrevCodeDown = false;
            private object sync = new object();

            public override FilterResult process(int nCode, WPARAM wParam, LPARAM lParam)
            {
                lock(sync)
                {
                    var d = Delta;
                    //LSender.Send(this, $"{wParam} - delta {d}ms", Message.Level.Trace);

                    if(isPrevCodeDown && SysMessages.Eq(wParam, CodeUp))
                    {
                        isPrevCodeDown = false;
                        LSender.Send(this, $"Prevent '{wParam}' because of previous {CodeDown}", Message.Level.Debug);
                        return FilterResult.Abort;
                    }

                    if(isPrevCodeDown && SysMessages.Eq(wParam, CodeDown))
                    {
                        isPrevCodeDown = false;
                        LSender.Send(this, $"Expected '{CodeUp}' was not found. Reset flag.", Message.Level.Debug);
                    }

                    if(SysMessages.Eq(wParam, CodeDown) && d < parent.Value)
                    {
                        LSender.Send(this, $"Found double-click bug of '{wParam}' because of delta {d}", Message.Level.Info);
                        isPrevCodeDown = true;

                        parent.trigger();
                        updateStamp();
                        return FilterResult.Abort;
                    }

                    if(SysMessages.Eq(wParam, CodeDown)) {
                        updateStamp();
                    }

                    return FilterResult.Continue;
                }
            }
        }

        /// <param name="nCode">A code that uses to determine how to process the message.</param>
        /// <param name="wParam">The identifier of the mouse message.</param>
        /// <param name="lParam">A pointer to an MSLLHOOKSTRUCT structure.</param>
        /// <returns></returns>
        public override FilterResult msg(int nCode, WPARAM wParam, LPARAM lParam)
        {
            if(!isLMR(wParam)) {
                return FilterResult.Continue;
            }

            return lmr.process(nCode, wParam, lParam);
        }

        public DoubleClicksFilter()
            : base("DoubleClicks")
        {
            Value   = 118;
            lmr     = new LMRContainer(this, typeof(LMR));
        }
    }
}
