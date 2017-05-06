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

namespace net.r_eg.MoConTool.WinAPI
{
    /// <summary>
    /// The type of hook procedure to be installed. This parameter can be one of the following values.
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/ms644990(v=vs.85).aspx
    /// </summary>
    public struct WindowsHookId
    {
        /// <summary>
        /// Installs a hook procedure that monitors mouse messages. 
        /// For more information, see the MouseProc hook procedure.
        /// </summary>
        public const int WH_MOUSE = 7;

        /// <summary>
        /// Installs a hook procedure that monitors low-level mouse input events. 
        /// For more information, see the LowLevelMouseProc hook procedure.
        /// </summary>
        public const int WH_MOUSE_LL = 14;

        /// <summary>
        /// Installs a hook procedure that monitors keystroke messages. 
        /// For more information, see the KeyboardProc hook procedure.
        /// </summary>
        public const int WH_KEYBOARD = 2;

        /// <summary>
        /// Installs a hook procedure that monitors low-level keyboard input events. 
        /// For more information, see the LowLevelKeyboardProc hook procedure.
        /// </summary>
        public const int WH_KEYBOARD_LL = 13;
    }
}
