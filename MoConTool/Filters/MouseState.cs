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
using net.r_eg.MoConTool.WinAPI;

namespace net.r_eg.MoConTool.Filters
{
    public static class MouseState
    {
        public enum Flags: uint
        {
            None    = 0x00,
            Left    = 0x01,
            Middle  = 0x02,
            Right   = 0x04,
            Scroll  = 0x08,
            Up      = 0x10,
            Down    = 0x20,

            LMR = Flags.Left | Flags.Middle | Flags.Right,

            LeftDown    = Flags.Left | Flags.Down,
            LeftUp      = Flags.Left | Flags.Up,

            MiddleDown  = Flags.Middle | Flags.Down,
            MiddleUp    = Flags.Middle | Flags.Up,

            RightDown   = Flags.Right | Flags.Down,
            RightUp     = Flags.Right | Flags.Up,
        }

        public static Flags Extract(uint type)
        {
            switch(type) {
                case SysMessages.WM_LBUTTONDOWN: {
                    return Flags.LeftDown;
                }
                case SysMessages.WM_MBUTTONDOWN: {
                    return Flags.MiddleDown;
                }
                case SysMessages.WM_RBUTTONDOWN: {
                    return Flags.RightDown;
                }
                case SysMessages.WM_LBUTTONUP: {
                    return Flags.LeftUp;
                }
                case SysMessages.WM_MBUTTONUP: {
                    return Flags.MiddleUp;
                }
                case SysMessages.WM_RBUTTONUP: {
                    return Flags.RightUp;
                }
            }

            return Flags.None;
        }

        public static Flags Extract(string type)
        {
            Flags ret = Flags.None;

            if(String.IsNullOrWhiteSpace(type)) {
                return ret;
            }

            foreach(char c in type) {
                switch(char.ToUpperInvariant(c)) {
                    case 'L': {
                        ret |= Flags.Left;
                        break;
                    }
                    case 'M': {
                        ret |= Flags.Middle;
                        break;
                    }
                    case 'R': {
                        ret |= Flags.Right;
                        break;
                    }
                    case 'S': {
                        ret |= Flags.Scroll;
                        break;
                    }
                    case 'U': {
                        ret |= Flags.Up;
                        break;
                    }
                    case 'D': {
                        ret |= Flags.Down;
                        break;
                    }
                }
            }

            return ret;
        }
    }
}
