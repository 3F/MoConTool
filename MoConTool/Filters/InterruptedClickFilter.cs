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

        internal sealed class LMR: LMRAbstract, ILMR
        {
            private volatile bool shadowClick = false;
            private volatile bool lockedThread = false;

            private object sync = new object();

            private static ConcurrentQueue<TCode> buffer = new ConcurrentQueue<TCode>();

            private struct TCode
            {
                public int nCode;
                public WPARAM wParam;
                public LPARAM lParam;
            }

            public override FilterResult process(int nCode, WPARAM wParam, LPARAM lParam)
            {
                if(shadowClick) {
                    LSender.Send(this, $"send {wParam} :: {(DateTime.Now - stamp).TotalMilliseconds}", Message.Level.Debug);
                    return FilterResult.Continue;
                }

                buffer.Enqueue(new TCode() {
                    nCode   = nCode,
                    lParam  = lParam,
                    wParam  = wParam
                });
                LSender.Send(this, $"'{wParam}' has been buffered ({buffer.Count})", Message.Level.Trace);

                lock(sync)
                {
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
                                    Thread.Sleep(1);
                                    continue;
                                }

                                if(SysMessages.Eq(cmd1.wParam, CodeUp))
                                {
                                    // now we should look possible bug: down ... [up, down] ... up
                                    Thread.Sleep((int)parent.Value); // to wait new codes
                                    break;
                                }

                                if(SysMessages.Eq(cmd1.wParam, CodeDown))
                                {
                                    shadowClick = true;
                                    sendCodeDown();
                                    shadowClick = false;

                                    lockedThread = false;
                                    return;
                                }
                            }

                            if(SysMessages.Eq(cmd1.wParam, 0)) {
                                lockedThread = false;
                                return;
                            }

                            TCode cmd2;
                            if(buffer.Count > 0)
                            {
                                while(!buffer.TryDequeue(out cmd2)) {
                                    Thread.Sleep(1);
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

                            lockedThread = false;
                        }
                    });
                }

                stamp = DateTime.Now;
                return FilterResult.Abort;
            }

            private void resend(TCode cmd1, TCode cmd2)
            {
                sendCode(cmd1);
                sendCode(cmd2);
            }

            private void sendCode(TCode cmd)
            {
                if(SysMessages.Eq(cmd.wParam, CodeDown)) {
                    shadowClick = true;
                    sendCodeDown();
                    shadowClick = false;
                }
                else if(SysMessages.Eq(cmd.wParam, CodeUp)) {
                    shadowClick = true;
                    sendCodeUp();
                    shadowClick = false;
                }
            }

            private void sendCodeDown()
            {
                MouseSimulator.Down(CodeDown);

                MouseSimulator.Delay();
                stamp = DateTime.Now;
            }

            private void sendCodeUp()
            {
                MouseSimulator.Up(CodeUp);

                MouseSimulator.Delay();
                stamp = DateTime.Now;
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
            Value = 30;
            lmr = new LMRContainer(this, typeof(LMR));
        }
    }
}
