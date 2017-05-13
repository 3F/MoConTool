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

namespace net.r_eg.MoConTool.WinAPI
{
    using DWORD = UInt32;
    using LONG = Int32;
    using ULONG_PTR = UIntPtr;

    /// <summary>
    /// Used by SendInput to store information for synthesizing input events such as keystrokes, mouse movement, and mouse clicks.
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/ms646270(v=vs.85).aspx
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct LPINPUT
    {
        public InputType type;

        // union { MOUSEINPUT mi; KEYBDINPUT ki; HARDWAREINPUT hi; }
        public InputUnion mikihi;

        public enum InputType: DWORD
        {
            InputMouse      = 0,
            InputKeyboard   = 1,
            InputHardware   = 2,
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct InputUnion
        {
            [FieldOffset(0)]
            public MOUSEINPUT mi;

            //[FieldOffset(0)]
            //public KEYBDINPUT ki;

            //[FieldOffset(0)]
            //public HARDWAREINPUT hi;
        }

        // https://msdn.microsoft.com/en-us/library/windows/desktop/ms646273(v=vs.85).aspx
        [StructLayout(LayoutKind.Sequential)]
        public struct MOUSEINPUT
        {
            public LONG dx;
            public LONG dy;
            public DWORD mouseData;
            public MouseFlags dwFlags;
            public DWORD time;
            internal ULONG_PTR dwExtraInfo;

            public UInt64 getDwExtraInfo()
            {
                return (UInt64)dwExtraInfo;
            }
        }

        [Flags]
        public enum MouseFlags: DWORD
        {
            MOUSEEVENTF_MOVE            = 0x0001,
            MOUSEEVENTF_LEFTDOWN        = 0x0002,
            MOUSEEVENTF_LEFTUP          = 0x0004,
            MOUSEEVENTF_RIGHTDOWN       = 0x0008,
            MOUSEEVENTF_RIGHTUP         = 0x0010,
            MOUSEEVENTF_MIDDLEDOWN      = 0x0020,
            MOUSEEVENTF_MIDDLEUP        = 0x0040,
            MOUSEEVENTF_XDOWN           = 0x0080,
            MOUSEEVENTF_XUP             = 0x0100,
            MOUSEEVENTF_WHEEL           = 0x0800,
            MOUSEEVENTF_HWHEEL          = 0x1000,
            MOUSEEVENTF_MOVE_NOCOALESCE = 0x2000,
            MOUSEEVENTF_VIRTUALDESK     = 0x4000,
            MOUSEEVENTF_ABSOLUTE        = 0x8000,
        }
    }
}
