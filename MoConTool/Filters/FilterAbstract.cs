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
using net.r_eg.MoConTool.WinAPI;

namespace net.r_eg.MoConTool.Filters
{
    using LPARAM = IntPtr;
    using WPARAM = UIntPtr;

    public abstract class FilterAbstract: IMouseListener, IMouseListenerSvc
    {
        /// <summary>
        /// When result of filter prevents message for any system handlers.
        /// </summary>
        public event EventHandler<DataArgs<ulong>> Triggering = delegate(object sender, DataArgs<ulong> e) { };

        /// <summary>
        /// State of listener.
        /// </summary>
        public bool Activated
        {
            get;
            set;
        }

        /// <summary>
        /// Displays number of triggering.
        /// </summary>
        public ulong TriggerCount
        {
            get;
            protected set;
        }

        /// <summary>
        /// The purpose of the filter.
        /// </summary>
        public MouseState.Flags Handler
        {
            get;
            set;
        } = MouseState.Flags.Left | MouseState.Flags.Down | MouseState.Flags.Up;

        /// <summary>
        /// A common value for control.
        /// </summary>
        public double Value
        {
            get;
            set;
        }

        /// <summary>
        /// Extra data.
        /// </summary>
        public object Data
        {
            get;
            set;
        }

        /// <summary>
        /// Gets unique id of listener.
        /// </summary>
        public Guid Id
        {
            get;
            protected set;
        }

        /// <summary>
        /// The name of listener.
        /// </summary>
        public string Name
        {
            get;
            protected set;
        }

        /// <param name="nCode">A code that uses to determine how to process the message.</param>
        /// <param name="wParam">The identifier of the mouse message.</param>
        /// <param name="lParam">A pointer to an MSLLHOOKSTRUCT structure.</param>
        /// <returns></returns>
        public abstract FilterResult msg(int nCode, WPARAM wParam, LPARAM lParam);

        /// <summary>
        /// To reset number of triggering.
        /// </summary>
        public void resetTriggerCount()
        {
            TriggerCount = 0;
        }

        /// <param name="name">Optional name of listener.</param>
        public FilterAbstract(string name = null)
        {
            Id      = GetType().GUID;
            Name    = name;
        }

        void IMouseListenerSvc.trigger()
        {
            unchecked {
                Triggering(this, new DataArgs<ulong>(++TriggerCount));
            }
        }

        protected bool isLMR(WPARAM wParam)
        {
            if(SysMessages.EqOr(wParam,
                                    SysMessages.WM_LBUTTONDOWN,
                                    SysMessages.WM_LBUTTONUP,
                                    SysMessages.WM_MBUTTONDOWN,
                                    SysMessages.WM_MBUTTONUP,
                                    SysMessages.WM_RBUTTONDOWN,
                                    SysMessages.WM_RBUTTONUP))
            {
                return true;
            }
            return false;
        }

        protected MSLLHOOKSTRUCT getMSLLHOOKSTRUCT(LPARAM lParam)
        {
            return (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
        }
    }
}
