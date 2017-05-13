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
using net.r_eg.MoConTool.Log;
using net.r_eg.MoConTool.WinAPI;

namespace net.r_eg.MoConTool.Filters
{
    using LPARAM = IntPtr;
    using WPARAM = UIntPtr;

    public class MixedClicksFilter: FilterAbstract, IMouseListener
    {
        private LMRContainer lmr;

        public sealed class TData
        {
            public bool onlyDownCodes = true;
        }

        internal sealed class LMR: LMRAbstract, ILMR
        {
            private volatile bool isBtnDown     = false;
            private volatile bool isBtnUp       = false;
            private volatile bool toAbortNext   = false;

            private object sync = new object();

            public override FilterResult process(int nCode, WPARAM wParam, LPARAM lParam)
            {
                if(parent.Data == null) {
                    parent.Data = new TData();
                }
                var v = (TData)parent.Data;

                lock(sync)
                {
                    if(toAbortNext) {
                        toAbortNext = false;
                        LSender.Send(this, $"Prevent '{wParam}' because of previous mixed code.", Message.Level.Info);
                        return FilterResult.Abort;
                    }

                    if(isBtnDown && SysMessages.Eq(wParam, CodeDown)) {
                        toAbortNext = true;
                        LSender.Send(this, $"Found mixed {wParam}", Message.Level.Info);
                        parent.trigger();
                        return FilterResult.Abort;
                    }

                    if(!v.onlyDownCodes && (isBtnUp && SysMessages.Eq(wParam, CodeUp))) {
                        LSender.Send(this, $"Found mixed {wParam}", Message.Level.Info);
                        parent.trigger();
                        return FilterResult.Abort;
                    }

                    if(SysMessages.Eq(wParam, CodeDown)) {
                        isBtnDown = true;
                        isBtnUp = false;
                    }
                    else if(SysMessages.Eq(wParam, CodeUp)) {
                        isBtnDown = false;
                        isBtnUp = true;
                    }

                    LSender.Send(this, $"Continue {wParam}", Message.Level.Trace);
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

        public MixedClicksFilter()
            : base("MixedClicks")
        {
            Data    = new TData();
            lmr     = new LMRContainer(this, typeof(LMR));
        }
    }
}
