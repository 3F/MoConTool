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
    [Serializable]
    public class HotKeyEventArgs: EventArgs
    {
        public Modifiers Modifier
        {
            get;
            protected set;
        }

        public Keys Key
        {
            get;
            protected set;
        }

        public Message Msg
        {
            get;
            protected set;
        }

        public HotKeyEventArgs(Modifiers modifier, Keys key, Message m)
        {
            Modifier = modifier;
            Key = key;
            Msg = m;
        }
    }
}
