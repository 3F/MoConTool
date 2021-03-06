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

    public class MouseFilter: FilterAbstract, IMouseListener
    {
        /// <param name="nCode">A code that uses to determine how to process the message.</param>
        /// <param name="wParam">The identifier of the mouse message.</param>
        /// <param name="lParam">A pointer to an MSLLHOOKSTRUCT structure.</param>
        /// <returns></returns>
        public override FilterResult msg(int nCode, WPARAM wParam, LPARAM lParam)
        {
            if(SysMessages.Eq(wParam, SysMessages.WM_LBUTTONDOWN)) {
                LSender.Send(this, $"WM_LBUTTONDOWN", Message.Level.Info);
            }
            else if(SysMessages.Eq(wParam, SysMessages.WM_LBUTTONUP)) {
                LSender.Send(this, $"WM_LBUTTONUP", Message.Level.Info);
            }
            else if(SysMessages.Eq(wParam, SysMessages.WM_RBUTTONDOWN)) {
                LSender.Send(this, $"WM_RBUTTONDOWN", Message.Level.Info);
            }
            else if(SysMessages.Eq(wParam, SysMessages.WM_RBUTTONUP)) {
                LSender.Send(this, $"WM_RBUTTONUP", Message.Level.Info);
            }
            else if(SysMessages.Eq(wParam, SysMessages.WM_MBUTTONDOWN)) {
                LSender.Send(this, $"WM_MBUTTONDOWN", Message.Level.Info);
            }
            else if(SysMessages.Eq(wParam, SysMessages.WM_MBUTTONUP)) {
                LSender.Send(this, $"WM_MBUTTONUP", Message.Level.Info);
            }
            else if(SysMessages.Eq(wParam, SysMessages.WM_MOUSEWHEEL)) {
                LSender.Send(this, $"WM_MOUSEWHEEL", Message.Level.Info);
            }
            else if(SysMessages.Eq(wParam, SysMessages.WM_MOUSEHWHEEL)) {
                LSender.Send(this, $"[WM_MOUSEHWHEEL]", Message.Level.Info);
            }

            //unchecked {
            //    ++TriggerCount;
            //}

            return FilterResult.Continue;
        }

        public MouseFilter()
            : base("MouseFilter")
        {

        }
    }
}
