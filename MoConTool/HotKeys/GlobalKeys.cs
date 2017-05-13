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
using net.r_eg.MoConTool.Exceptions;
using net.r_eg.MoConTool.WinAPI;

namespace net.r_eg.MoConTool.HotKeys
{
    /// <summary>
    /// As variant, we may avoid the handle & WndProc() and use the message queue with GetMessage() 
    /// https://msdn.microsoft.com/en-us/library/windows/desktop/ms644936%28v=vs.85%29.aspx
    /// Example on C++ here: https://msdn.microsoft.com/en-us/library/windows/desktop/ms646309%28v=vs.85%29.aspx
    /// </summary>
    public class GlobalKeys: NativeWindow, IGlobalHotKey, IDisposable
    {
        /// <summary>
        /// When the registered hot key has been pressed.
        /// </summary>
        public event EventHandler<HotKeyEventArgs> KeyPress = delegate(object sender, HotKeyEventArgs e) { };

        internal Log.ISender log = Log.LSender._;

        private IdentHotKey uid = new IdentHotKey();
        private object _lock    = new object();

        /// <summary>
        /// To register hot key combination.
        /// </summary>
        /// <param name="mod"></param>
        /// <param name="key"></param>
        /// <returns>Identifier of registered combination.</returns>
        public int register(Modifiers mod, Keys key)
        {
            lock(_lock)
            {
                if(Handle == IntPtr.Zero) {
                    createHandle();
                }

                try {
                    bool success = NativeMethods.RegisterHotKey(Handle, uid.Current, (uint)mod, (uint)key);
                    if(success) {
                        log.send(this, $"The Hotkey {(uint)mod}, {(uint)key} has been registered #{uid.Current}.", Log.Message.Level.Info);
                        return uid.Current;
                    }
                }
                finally {
                    uid.Next();
                }
            }

            throw new WinFuncFailException("the RegisterHotKey returns false.");
        }

        /// <summary>
        /// Frees a hot key by identifier from the register() method.
        /// </summary>
        /// <param name="ident"></param>
        /// <returns></returns>
        public bool unregister(int ident)
        {
            if(Handle == IntPtr.Zero) {
                return false;
            }

            log.send(this, $"Free a Hotkey #{ident}", Log.Message.Level.Info);
            return NativeMethods.UnregisterHotKey(Handle, ident);
        }

        /// <summary>
        /// Checks the high-order bit for present key.
        /// </summary>
        /// <param name="key"></param>
        /// <returns>true if bit is 1</returns>
        public bool highOrderBitIsOne(Keys key)
        {
            return (NativeMethods.GetKeyState((int)key) & 0x8000) != 0;
        }

        protected void createHandle()
        {
            CreateHandle(new CreateParams()); // to WndProc
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if(m.Msg != SysMessages.WM_HOTKEY) {
                return;
            }

            int lParam          = (int)m.LParam;
            Keys key            = (Keys)(lParam >> 16);
            Modifiers modifier  = (Modifiers)(lParam & 0xFFFF);

            KeyPress(this, new HotKeyEventArgs(modifier, key, m));
        }

        #region IDisposable

        /// <summary>
        /// To detect redundant calls
        /// </summary>
        private bool disposed = false;

        /// <summary>
        /// To correctly implement the disposable pattern.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if(disposed) {
                return;
            }
            disposed = true;

            foreach(var i in uid.Iter) {
                unregister(i);
            }
            DestroyHandle();
        }

        #endregion
    }
}
