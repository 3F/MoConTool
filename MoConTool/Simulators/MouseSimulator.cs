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
using System.Threading;
using net.r_eg.MoConTool.WinAPI;

namespace net.r_eg.MoConTool.Simulators
{
    internal static class MouseSimulator
    {
        public static bool LeftDown()
        {
            return Push(LPINPUT.MouseFlags.MOUSEEVENTF_LEFTDOWN);
        }

        public static bool LeftUp()
        {
            return Push(LPINPUT.MouseFlags.MOUSEEVENTF_LEFTUP);
        }

        public static bool RightDown()
        {
            return Push(LPINPUT.MouseFlags.MOUSEEVENTF_RIGHTDOWN);
        }

        public static bool RightUp()
        {
            return Push(LPINPUT.MouseFlags.MOUSEEVENTF_RIGHTUP);
        }

        public static bool MiddleDown()
        {
            return Push(LPINPUT.MouseFlags.MOUSEEVENTF_MIDDLEDOWN);
        }

        public static bool MiddleUp()
        {
            return Push(LPINPUT.MouseFlags.MOUSEEVENTF_MIDDLEUP);
        }

        public static void Delay()
        {
            Thread.Sleep(20); //TODO: user option
        }

        public static bool Down(uint code)
        {
            switch(code) {
                case SysMessages.WM_LBUTTONDOWN: {
                    return LeftDown();
                }
                case SysMessages.WM_MBUTTONDOWN: {
                    return MiddleDown();
                }
                case SysMessages.WM_RBUTTONDOWN: {
                    return RightDown();
                }
            }

            throw new NotSupportedException($"Code {code} is not supported as a Down-code.");
        }

        public static bool Up(uint code)
        {
            switch(code) {
                case SysMessages.WM_LBUTTONUP: {
                    return LeftUp();
                }
                case SysMessages.WM_MBUTTONUP: {
                    return MiddleUp();
                }
                case SysMessages.WM_RBUTTONUP: {
                    return RightUp();
                }
            }

            throw new NotSupportedException($"Code {code} is not supported as a Up-code.");
        }

        private static bool Push(LPINPUT.MouseFlags flags)
        {
            var pInputs = new LPINPUT() {
                type = LPINPUT.InputType.InputMouse
            };

            pInputs.mikihi.mi.dwFlags = flags;
            var ret = NativeMethods.SendInput(1, new[] { pInputs }, Marshal.SizeOf(pInputs));

            return ret > 0;
        }
    }
}
