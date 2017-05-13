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
/*
 * origin: net.r_eg.TmVTweaks.HotKeys :: Copyright (c) 2016  Denis Kuzmin <entry.reg@gmail.com>
*/

using net.r_eg.MoConTool.WinAPI;

namespace net.r_eg.MoConTool.HotKeys
{
    public enum Modifiers: uint
    {
        /// <summary>
        /// Either ALT key must be held down.
        /// </summary>
        AltKey = ModifierKeys.MOD_ALT,

        /// <summary>
        /// Either CTRL key must be held down.
        /// </summary>
        ControlKey = ModifierKeys.MOD_CONTROL,

        /// <summary>
        /// Either SHIFT key must be held down.
        /// </summary>
        ShiftKey = ModifierKeys.MOD_SHIFT,

        /// <summary>
        /// Either WINDOWS key was held down. 
        /// These keys are labeled with the Windows logo. 
        /// Keyboard shortcuts that involve the WINDOWS key are reserved for use by the operating system.
        /// </summary>
        WinKey = ModifierKeys.MOD_WIN,

        /// <summary>
        /// Changes the hotkey behavior so that the keyboard auto-repeat does not yield multiple hotkey notifications.
        /// Windows Vista: This flag is not supported.
        /// </summary>
        NoRepeat = ModifierKeys.MOD_NOREPEAT,
    }
}
