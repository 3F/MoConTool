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
using System.Threading;
using System.Windows.Forms;

namespace net.r_eg.MoConTool
{
    internal static class Program
    {
#if DEBUG
        internal const string ARGS_DEFAULT = "-InterruptedClick L 110;58;217 -MixedClicks L 1 -DoubleClicks L 112 -HyperactiveScroll 14;250";
#else
        internal const string ARGS_DEFAULT = "-InterruptedClick L 110;58;217 -MixedClicks L 1 -DoubleClicks L 112";
#endif

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        internal static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.Automatic);
            Application.ThreadException += onThreadException;

            FormWindowState state;
            try
            {
                if(args == null || args.Length < 1) {
                    args = ARGS_DEFAULT.Split(' ');
                    state = FormWindowState.Normal;
                }
                else {
                    state = FormWindowState.Minimized;
                }

                IBootloader loader = new Bootloader();
                loader.register();
                loader.configure(args);

                Application.Run(new UI.TrayForm(loader, state));
            }
            catch(Exception ex) {
                mfail(ex, false);
            }
        }

        private static void mfail(Exception ex, bool threadEx)
        {
            string msg = $"{ex.Message}{(threadEx ? "[TH]" : "[M]")}\n---\n{ex.ToString()}";

            Console.WriteLine(msg);
            MessageBox.Show(msg, "Something went wrong -_-", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private static void onThreadException(object sender, ThreadExceptionEventArgs e)
        {
            mfail(e.Exception, true);
        }
    }
}
