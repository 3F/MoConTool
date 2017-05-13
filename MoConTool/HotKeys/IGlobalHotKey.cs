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

using System;
using System.Windows.Forms;

namespace net.r_eg.MoConTool.HotKeys
{
    public interface IGlobalHotKey
    {
        /// <summary>
        /// When the registered hot key has been pressed.
        /// </summary>
        event EventHandler<HotKeyEventArgs> KeyPress;

        /// <summary>
        /// To register hot key combination.
        /// </summary>
        /// <param name="mod"></param>
        /// <param name="key"></param>
        /// <returns>Identifier of registered combination.</returns>
        int register(Modifiers mod, Keys key);

        /// <summary>
        /// Frees a hot key by identifier from the register() method.
        /// </summary>
        /// <param name="ident"></param>
        /// <returns></returns>
        bool unregister(int ident);

        /// <summary>
        /// Checks the high-order bit for present key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>true if bit is 1</returns>
        bool highOrderBitIsOne(Keys key);
    }
}
