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

namespace net.r_eg.MoConTool.Filters
{
    using LPARAM = IntPtr;
    using WPARAM = UIntPtr;

    public interface IMouseListener: IListener
    {
        /// <summary>
        /// When result of filter prevents message for any system handlers.
        /// </summary>
        event EventHandler<DataArgs<ulong>> Triggering;

        /// <summary>
        /// State of listener.
        /// </summary>
        bool Activated { get; set; }

        /// <summary>
        /// Displays number of triggering.
        /// </summary>
        ulong TriggerCount { get; }

        /// <summary>
        /// The purpose of the filter.
        /// </summary>
        MouseState.Flags Handler { get; set; }

        /// <summary>
        /// A common value for control.
        /// </summary>
        double Value { get; set; }

        /// <summary>
        /// Extra data.
        /// </summary>
        object Data { get; set; }

        /// <summary>
        /// The name of listener.
        /// </summary>
        string Name { get; }

        /// <param name="nCode">A code that uses to determine how to process the message.</param>
        /// <param name="wParam">The identifier of the mouse message.</param>
        /// <param name="lParam">A pointer to an MSLLHOOKSTRUCT structure.</param>
        /// <returns></returns>
        FilterResult msg(int nCode, WPARAM wParam, LPARAM lParam);
    }
}
