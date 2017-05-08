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

namespace net.r_eg.MoConTool.WinAPI
{
    using WPARAM = UIntPtr;

    /// <summary>
    /// System-Defined Messages
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/ms644927%28v=vs.85%29.aspx#system_defined
    /// 
    /// Mouse Input Notifications:
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/ff468877(v=vs.85).aspx
    /// </summary>
    public struct SysMessages
    {
        /// <summary>
        /// When the user presses the left mouse button while the cursor is in the client area of a window. 
        /// https://msdn.microsoft.com/en-us/library/windows/desktop/ms645607%28v=vs.85%29.aspx
        /// </summary>
        public const uint WM_LBUTTONDOWN = 0x0201;

        /// <summary>
        /// When the user releases the left mouse button while the cursor is in the client area of a window.
        /// https://msdn.microsoft.com/en-us/library/windows/desktop/ms645608(v=vs.85).aspx
        /// </summary>
        public const uint WM_LBUTTONUP = 0x0202;

        /// <summary>
        /// When the user presses the right mouse button while the cursor is in the client area of a window. 
        /// https://msdn.microsoft.com/en-us/library/windows/desktop/ms646242(v=vs.85).aspx
        /// </summary>
        public const uint WM_RBUTTONDOWN = 0x0204;

        /// <summary>
        /// When the user releases the right mouse button while the cursor is in the client area of a window.
        /// https://msdn.microsoft.com/en-us/library/windows/desktop/ms646243(v=vs.85).aspx
        /// </summary>
        public const uint WM_RBUTTONUP = 0x0205;

        /// <summary>
        /// When the user presses the middle mouse button while the cursor is in the client area of a window.
        /// https://msdn.microsoft.com/en-us/library/windows/desktop/ms645610(v=vs.85).aspx
        /// </summary>
        public const uint WM_MBUTTONDOWN = 0x0207;

        /// <summary>
        /// When the user releases the middle mouse button while the cursor is in the client area of a window. 
        /// https://msdn.microsoft.com/en-us/library/windows/desktop/ms645611(v=vs.85).aspx
        /// </summary>
        public const uint WM_MBUTTONUP = 0x0208;

        /// <summary>
        ///  When the cursor moves.
        /// https://msdn.microsoft.com/en-us/library/windows/desktop/ms645616(v=vs.85).aspx
        /// </summary>
        public const uint WM_MOUSEMOVE = 0x0200;

        /// <summary>
        /// When the mouse wheel is rotated.
        /// https://msdn.microsoft.com/en-us/library/windows/desktop/ms645617(v=vs.85).aspx
        /// </summary>
        public const uint WM_MOUSEWHEEL = 0x020A;

        /// <summary>
        /// When the mouse's horizontal scroll wheel is tilted or rotated.
        /// https://msdn.microsoft.com/en-us/library/windows/desktop/ms645614(v=vs.85).aspx
        /// </summary>
        public const uint WM_MOUSEHWHEEL = 0x020E;

        /// <summary>
        /// Checks equality.
        /// </summary>
        public static bool Eq(WPARAM a, uint b)
        {
            return a == (UIntPtr)b;
        }

        /// <summary>
        /// Checks equality by OR logic.
        /// </summary>
        /// <returns>true if at least one from barr is equal to a.</returns>
        public static bool EqOr(WPARAM a, params uint[] barr)
        {
            if(barr == null) {
                return false;
            }

            foreach(uint b in barr)
            {
                if(Eq(a, b)) {
                    return true;
                }
            }
            return false;
        }
    }
}
