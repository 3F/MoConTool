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
using System.Collections.Generic;
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
        protected Dictionary<MouseState.Flags, LMR> data;

        protected sealed class LMR
        {
            private uint codeDown;
            private uint codeUp;
            private InterruptedClickFilter parent;

            private volatile bool shadowClick = false;
            private volatile bool lockedThread = false;
            private DateTime stamp = DateTime.Now;

            private object sync = new object();

            private static ConcurrentQueue<TCode> buffer = new ConcurrentQueue<TCode>();

            private struct TCode
            {
                public int nCode;
                public WPARAM wParam;
                public LPARAM lParam;
            }

            public FilterResult process(int nCode, WPARAM wParam, LPARAM lParam)
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

                                if(SysMessages.Eq(cmd1.wParam, codeUp))
                                {
                                    // now we should look possible bug: down ... [up, down] ... up
                                    Thread.Sleep((int)parent.Value); // to wait new codes
                                    break;
                                }

                                if(SysMessages.Eq(cmd1.wParam, codeDown))
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

                            if(SysMessages.Eq(cmd1.wParam, codeUp)
                                && SysMessages.Eq(cmd2.wParam, codeDown))
                            {
                                LSender.Send(this, $"Found bug with recovering of codes {codeUp} -> {codeDown}", Message.Level.Info);
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

            public LMR(uint codeDown, uint codeUp, InterruptedClickFilter parent)
            {
                this.codeDown   = codeDown;
                this.codeUp     = codeUp;
                this.parent     = parent;
            }

            private void resend(TCode cmd1, TCode cmd2)
            {
                sendCode(cmd1);
                sendCode(cmd2);
            }

            private void sendCode(TCode cmd)
            {
                if(SysMessages.Eq(cmd.wParam, codeDown)) {
                    shadowClick = true;
                    sendCodeDown();
                    shadowClick = false;
                }
                else if(SysMessages.Eq(cmd.wParam, codeUp)) {
                    shadowClick = true;
                    sendCodeUp();
                    shadowClick = false;
                }
            }

            private void sendCodeDown()
            {
                MouseSimulator.Down(codeDown);

                MouseSimulator.Delay();
                stamp = DateTime.Now;
            }

            private void sendCodeUp()
            {
                MouseSimulator.Up(codeUp);

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
            if(!SysMessages.EqOr(wParam,
                                    SysMessages.WM_LBUTTONDOWN,
                                    SysMessages.WM_LBUTTONUP,
                                    SysMessages.WM_MBUTTONDOWN,
                                    SysMessages.WM_MBUTTONUP,
                                    SysMessages.WM_RBUTTONDOWN,
                                    SysMessages.WM_RBUTTONUP))
            {
                return FilterResult.Continue;
            }

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

        public InterruptedClickFilter()
            : base("InterruptedClick")
        {
            Value = 30;

            data = new Dictionary<MouseState.Flags, LMR>();
            data[MouseState.Flags.Left]   = new LMR(SysMessages.WM_LBUTTONDOWN, SysMessages.WM_LBUTTONUP, this);
            data[MouseState.Flags.Middle] = new LMR(SysMessages.WM_MBUTTONDOWN, SysMessages.WM_MBUTTONUP, this);
            data[MouseState.Flags.Right]  = new LMR(SysMessages.WM_RBUTTONDOWN, SysMessages.WM_RBUTTONUP, this);
        }

        private bool isFlag(MouseState.Flags flags, WPARAM wParam, uint code1, uint code2)
        {
            if((Handler & flags) != 0
                && (SysMessages.EqOr(wParam, code1, code2)))
            {
                return true;
            }
            return false;
        }
    }
}
