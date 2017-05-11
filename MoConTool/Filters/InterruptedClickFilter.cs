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
using System.Threading;
using System.Threading.Tasks;
using net.r_eg.Conari.Log;
using net.r_eg.MoConTool.Simulators;
using net.r_eg.MoConTool.WinAPI;

namespace net.r_eg.MoConTool.Filters
{
    using LPARAM = IntPtr;
    using WPARAM = UIntPtr;

    public class InterruptedClickFilter: FilterAbstract, IMouseListener
    {
        private LMRContainer lmr;

        public sealed class TData
        {
            public uint deltaMin = 10;
            public uint deltaMax = 251;
        }

        internal sealed class LMR: LMRAbstract, ILMR
        {
            private volatile bool lockedThread  = false;
            private volatile bool pressedCodeUp = false;
            private DateTime stampCodeDown      = DateTime.Now;

            private object sync = new object();

            private ConcurrentQueue<TCode> buffer = new ConcurrentQueue<TCode>();

            private struct TCode
            {
                public int nCode;
                public WPARAM wParam;
                public LPARAM lParam;
            }

            private TData Data
            {
                get
                {
                    if(parent.Data == null) {
                        parent.Data = new TData();
                    }
                    return (TData)parent.Data;
                }
            }

            public override FilterResult process(int nCode, WPARAM wParam, LPARAM lParam)
            {
                //if(shadowClick) { // now it will be processed via ReCodes + lowLevelMouseProc
                //    return FilterResult.Continue;
                //}

                if(!pressedCodeUp && SysMessages.Eq(wParam, CodeDown)) {
                    stampCodeDown = DateTime.Now;
                    return FilterResult.Continue;
                }

                if(pressedCodeUp && SysMessages.Eq(wParam, CodeUp)) {
                    return FilterResult.Continue;
                }

                var delta   = (DateTime.Now - stampCodeDown).TotalMilliseconds;
                var v       = Data;

                if((delta > v.deltaMin && delta <= v.deltaMax) && SysMessages.Eq(wParam, CodeUp)) {
                    LSender.Send(this, $"{CodeDown} <--> {CodeUp}-{CodeDown} :: consider as a user click", Message.Level.Debug);
                    return FilterResult.Continue;
                }

                if(SysMessages.Eq(wParam, CodeUp)) {
                    pressedCodeUp = true;
                }

                buffer.Enqueue(new TCode() {
                    nCode   = nCode,
                    lParam  = lParam,
                    wParam  = wParam
                });
                LSender.Send(this, $"'{wParam}' has been buffered ({buffer.Count})", Message.Level.Trace);

                if(lockedThread) {
                    return FilterResult.Abort;
                }

                Task.Factory.StartNew(() => 
                {
                    lock(sync)
                    {
                        lockedThread = true;

                        var cmd1 = new TCode();
                        while(buffer.Count > 0)
                        {
                            if(!buffer.TryDequeue(out cmd1)) {
                                shortSleep();
                                continue;
                            }

                            if(SysMessages.Eq(cmd1.wParam, CodeUp)) {
                                // now we should look possible bug: down ... [up, down] ... up
                                waitNewCodes();
                                break;
                            }

                            if(SysMessages.Eq(cmd1.wParam, CodeDown)) {
                                sendCodeDown();
                                lockedThread = false;
                                return;
                            }
                        }

                        if(SysMessages.Eq(cmd1.wParam, 0)) {
                            pressedCodeUp = false;
                            lockedThread = false;
                            return;
                        }

                        TCode cmd2;
                        if(buffer.Count > 0)
                        {
                            while(!buffer.TryDequeue(out cmd2)) {
                                shortSleep();
                            }
                            LSender.Send(this, $"Buffer > 0 :: {cmd2.wParam}", Message.Level.Trace);
                        }
                        else {
                            cmd2 = new TCode();
                        }

                        if(SysMessages.Eq(cmd1.wParam, CodeUp)
                            && SysMessages.Eq(cmd2.wParam, CodeDown))
                        {
                            LSender.Send(this, $"Found bug with recovering of codes {CodeUp} -> {CodeDown}", Message.Level.Info);
                            parent.trigger();
                        }
                        else {
                            resend(cmd1, cmd2);
                        }

                        pressedCodeUp = false;
                        lockedThread = false;
                    }
                });

                stamp = DateTime.Now;
                return FilterResult.Abort;
            }

            private void waitNewCodes()
            {
                // ~ while(buffer.Count < 1) Thread.Sleep(1); - gives ~15ms for each iteration because of ConcurrentQueue and others.
                Thread.Sleep((int)parent.Value);
                return;
            }

            private void resend(TCode cmd1, TCode cmd2)
            {
                sendCode(cmd1);
                sendCode(cmd2);
            }

            private void sendCode(TCode cmd)
            {
                if(SysMessages.Eq(cmd.wParam, CodeDown)) {
                    sendCodeDown();
                }
                else if(SysMessages.Eq(cmd.wParam, CodeUp)) {
                    sendCodeUp();
                }
            }

            private void sendCodeDown()
            {
                parent.ReCodes[MouseState.Extract(CodeDown)] = true;
                MouseSimulator.Down(CodeDown);

                MouseSimulator.Delay();
                stamp = DateTime.Now;
            }

            private void sendCodeUp()
            {
                parent.ReCodes[MouseState.Extract(CodeUp)] = true;
                MouseSimulator.Up(CodeUp);

                MouseSimulator.Delay();
                stamp = DateTime.Now;
            }

            private void shortSleep()
            {
                //Thread.Sleep(1);
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

        public InterruptedClickFilter()
            : base("InterruptedClick")
        {
            Value   = 110;
            Data    = new TData();
            lmr     = new LMRContainer(this, typeof(LMR));
        }
    }
}
