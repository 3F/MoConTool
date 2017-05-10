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
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using net.r_eg.Conari.Log;
using net.r_eg.MoConTool.Filters;
using net.r_eg.MoConTool.WinAPI;

namespace net.r_eg.MoConTool.UI
{
    public partial class TrayForm: Form
    {
        internal const int MAX_DBG_RECORDS = 4000;

        private IBootloader loader;
        private IMokona svc;

        private IMouseListener fic;
        private IMouseListener fmc;
        private IMouseListener fdc;
        private IMouseListener fhs;

        private bool closing = false;
        private Size origin;

        private bool suspendedLog;
        private List<object> logmsg = new List<object>();

        private object sync = new object();

        public TrayForm(IBootloader loader)
        {
            if(loader == null) {
                throw new ArgumentNullException("Bootloader cannot be null.");
            }
            this.loader = loader;
            svc         = new Mokona(loader);

            fic = listener(typeof(InterruptedClickFilter));
            fmc = listener(typeof(MixedClicksFilter));
            fdc = listener(typeof(DoubleClicksFilter));
            fhs = listener(typeof(HyperactiveScrollFilter));

            InitializeComponent();

            origin = ClientSize;
            try {
                notifyIconMain.Icon = Icon 
                                    = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            }
            catch(ArgumentException ex) {
                notifyIconMain.Icon = Icon;
                LSender.Send(this, ex.Message, Conari.Log.Message.Level.Debug);
            }

            LSender.SReceived += LSender_SReceived;

            fic.Triggering += (object sender, DataArgs<ulong> e) => {
                uiAction(() => labelInterruptedClick.Text = e.Data.ToString());
            };

            fmc.Triggering += (object sender, DataArgs<ulong> e) => {
                uiAction(() => labelMixedClicks.Text = e.Data.ToString());
            };

            fdc.Triggering += (object sender, DataArgs<ulong> e) => {
                uiAction(() => labelDoubleClick.Text = e.Data.ToString());
            };

            fhs.Triggering += (object sender, DataArgs<ulong> e) => {
                uiAction(() => labelHyperactiveScroll.Text = e.Data.ToString());
            };
        }

        /// <param name="m"></param>
        private void uiAction(MethodInvoker m)
        {
            if(InvokeRequired) {
                BeginInvoke(m);
                return;
            }
            m();
        }

        /// <summary>
        /// Open url in default web-browser
        /// </summary>
        /// <param name="url"></param>
        private void openUrl(string url)
        {
            System.Diagnostics.Process.Start(url);
        }

        private IMouseListener listener(Type type)
        {
            return svc.Loader.Filters[type.GUID];
        }

        private void setFlags(bool state, IMouseListener filter, MouseState.Flags val)
        {
            if(state) {
                filter.Handler |= val;
            }
            else {
                filter.Handler &= ~val;
            }
        }

        private void reset(IMouseListener h, Label label)
        {
            h.resetTriggerCount();
            label.Text = "0";
        }

        private void render()
        {
            chkInterruptedClick.Checked = fic.Activated;
            chkInterruptedBtnL.Checked = (fic.Handler & MouseState.Flags.Left) != 0;
            chkInterruptedBtnM.Checked = (fic.Handler & MouseState.Flags.Middle) != 0;
            chkInterruptedBtnR.Checked = (fic.Handler & MouseState.Flags.Right) != 0;

            chkMixedClicks.Checked = fmc.Activated;
            chkMixedBtnL.Checked = (fmc.Handler & MouseState.Flags.Left) != 0;
            chkMixedBtnM.Checked = (fmc.Handler & MouseState.Flags.Middle) != 0;
            chkMixedBtnR.Checked = (fmc.Handler & MouseState.Flags.Right) != 0;

            chkDoubleClick.Checked = fdc.Activated;
            chkDoubleBtnL.Checked = (fdc.Handler & MouseState.Flags.Left) != 0;
            chkDoubleBtnR.Checked = (fdc.Handler & MouseState.Flags.Middle) != 0;
            chkDoubleBtnM.Checked = (fdc.Handler & MouseState.Flags.Right) != 0;

            chkHyperactiveScroll.Checked = fhs.Activated;

            numInterruptedClick.Value   = (decimal)fic.Value;
            numDoubleClick.Value        = (decimal)fdc.Value;
            numHyperactiveScroll.Value  = (decimal)fhs.Value;
        }

        private void LSender_SReceived(object sender, Conari.Log.Message e)
        {
            if(!chkDebug.Checked) {
                return;
            }

            string msg = $"{e.stamp.ToString("HH:mm:ss.fffff")}] {e.content} :: {sender.ToString()}";
            if(suspendedLog) {
                logmsg.Add(msg);
                return;
            }

            uiAction(() =>
            {
                lock(sync)
                {
                    if(listBoxDebug.Items.Count < MAX_DBG_RECORDS)
                    {
                        if(logmsg.Count > 0) {
                            listBoxDebug.Items.AddRange(logmsg.ToArray());
                            logmsg.Clear();
                        }
                        listBoxDebug.Items.Add(msg);
                        listBoxDebug.TopIndex = listBoxDebug.Items.Count - 1;
                        return;
                    }

                    // the RemoveAt() is much more expensive than [Copy + Clear + AddRange] because of logic for moving elements to removed index.
                    // But unfortunately we can't access to array of ListBox elements, 
                    // and we can't copy starting with specific index like for Array.Copy ...seriously, who designed this component >(
                    // well, like this:
                    object[] origin = new object[MAX_DBG_RECORDS];
                    listBoxDebug.Items.CopyTo(origin, Math.Max(0, listBoxDebug.Items.Count - MAX_DBG_RECORDS));

                    // then finally:
                    int maxdel      = (int)(MAX_DBG_RECORDS * 0.75);
                    object[] dest   = new object[MAX_DBG_RECORDS - maxdel];
                    Array.Copy(origin, maxdel, dest, 0, dest.Length);

                    listBoxDebug.SuspendLayout();
                    listBoxDebug.Items.Clear();
                    listBoxDebug.Items.AddRange(dest);
                    listBoxDebug.ResumeLayout();
                }
            });
        }

        private void TrayForm_Load(object sender, EventArgs e)
        {
            chkDebug_CheckedChanged(this, EventArgs.Empty);
            render();

            notifyIconMain.Text = menuCaption.Text
                                = $"{Text} v{Application.ProductVersion}";

            chkPlug.Checked = true;
        }

        private void menuAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"Version: {Application.ProductVersion}\n\nLicense: MIT\nentry.reg@gmail.com :: github.com/3F", 
                            Text, 
                            MessageBoxButtons.OK, 
                            MessageBoxIcon.Information);
        }

        private void menuExit_Click(object sender, EventArgs e)
        {
            closing = true;
            Application.Exit();
        }

        private void menuSrcCode_Click(object sender, EventArgs e)
        {
            openUrl("https://github.com/3F/MoConTool");
        }

        private void menuTweaks_Click(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void notifyIconMain_MouseUp(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Left) {
                NativeMethods.SetForegroundWindow(new HandleRef(sender, Handle)); // fixes of destroying menu for left click
            }
            menuTray.Show(this, PointToClient(Cursor.Position));
        }

        private void TrayForm_Resize(object sender, EventArgs e)
        {
            if(WindowState == FormWindowState.Minimized) {
                Hide();
            }
            else {
                Show();
            }
        }

        private void TrayForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(!closing) {
                e.Cancel = true;
                Hide();
            }
        }

        private void chkPlug_CheckedChanged(object sender, EventArgs e)
        {
            if(chkPlug.Checked) {
                svc.plug();
            }
            else {
                svc.unplug();
            }
        }

        private void menuCopy_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            foreach(string item in listBoxDebug.SelectedItems) {
                sb.Append(item + Environment.NewLine);
            }

            if(sb.Length > 0) {
                Clipboard.SetText(sb.ToString());
            }
        }

        private void menuSelectAll_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < listBoxDebug.Items.Count; ++i) {
                listBoxDebug.SetSelected(i, true);
            }
        }

        private void menuClear_Click(object sender, EventArgs e)
        {
            listBoxDebug.Items.Clear();
        }

        private void chkDebug_CheckedChanged(object sender, EventArgs e)
        {
            ClientSize = chkDebug.Checked ? origin : new Size(ClientSize.Width, listBoxDebug.Location.Y);
            if(!chkDebug.Checked) {
                uiAction(() => listBoxDebug.Items.Clear());
            }
        }

        private void listBoxDebug_MouseHover(object sender, EventArgs e)
        {
            suspendedLog = true;
        }

        private void listBoxDebug_MouseLeave(object sender, EventArgs e)
        {
            suspendedLog = false;
        }

        private void contextMenuDebug_MouseHover(object sender, EventArgs e)
        {
            suspendedLog = true;
        }

        private void contextMenuDebug_MouseLeave(object sender, EventArgs e)
        {
            suspendedLog = false;
        }

        private void labelInterruptedClick_Click(object sender, EventArgs e)
        {
            reset(fic, labelInterruptedClick);
        }

        private void labelMixedClicks_Click(object sender, EventArgs e)
        {
            reset(fmc, labelMixedClicks);
        }

        private void labelDoubleClick_Click(object sender, EventArgs e)
        {
            reset(fdc, labelDoubleClick);
        }

        private void labelHyperactiveScroll_Click(object sender, EventArgs e)
        {
            reset(fhs, labelHyperactiveScroll);
        }

        private void numInterruptedClick_ValueChanged(object sender, EventArgs e)
        {
            fic.Value = (double)numInterruptedClick.Value;
        }

        private void numDoubleClick_ValueChanged(object sender, EventArgs e)
        {
            fdc.Value = (double)numDoubleClick.Value;
        }

        private void numHyperactiveScroll_ValueChanged(object sender, EventArgs e)
        {
            fhs.Value = (double)numHyperactiveScroll.Value;
        }

        private void chkInterruptedBtnL_CheckedChanged(object sender, EventArgs e)
        {
            setFlags(chkInterruptedBtnL.Checked, fic, MouseState.Flags.Left);
        }

        private void chkInterruptedBtnM_CheckedChanged(object sender, EventArgs e)
        {
            setFlags(chkInterruptedBtnM.Checked, fic, MouseState.Flags.Middle);
        }

        private void chkInterruptedBtnR_CheckedChanged(object sender, EventArgs e)
        {
            setFlags(chkInterruptedBtnR.Checked, fic, MouseState.Flags.Right);
        }

        private void chkMixedBtnL_CheckedChanged(object sender, EventArgs e)
        {
            setFlags(chkMixedBtnL.Checked, fmc, MouseState.Flags.Left);
        }

        private void chkMixedBtnM_CheckedChanged(object sender, EventArgs e)
        {
            setFlags(chkMixedBtnM.Checked, fmc, MouseState.Flags.Middle);
        }

        private void chkMixedBtnR_CheckedChanged(object sender, EventArgs e)
        {
            setFlags(chkMixedBtnR.Checked, fmc, MouseState.Flags.Right);
        }

        private void chkDoubleBtnL_CheckedChanged(object sender, EventArgs e)
        {
            setFlags(chkDoubleBtnL.Checked, fdc, MouseState.Flags.Left);
        }

        private void chkDoubleBtnM_CheckedChanged(object sender, EventArgs e)
        {
            setFlags(chkDoubleBtnM.Checked, fdc, MouseState.Flags.Middle);
        }

        private void chkDoubleBtnR_CheckedChanged(object sender, EventArgs e)
        {
            setFlags(chkDoubleBtnR.Checked, fdc, MouseState.Flags.Right);
        }

        private void chkInterruptedClick_CheckedChanged(object sender, EventArgs e)
        {
            fic.Activated = chkInterruptedClick.Checked;
        }

        private void chkMixedClicks_CheckedChanged(object sender, EventArgs e)
        {
            fmc.Activated = chkMixedClicks.Checked;
        }

        private void chkDoubleClick_CheckedChanged(object sender, EventArgs e)
        {
            fdc.Activated = chkDoubleClick.Checked;
        }

        private void chkHyperactiveScroll_CheckedChanged(object sender, EventArgs e)
        {
            fhs.Activated = chkHyperactiveScroll.Checked;
        }
    }
}
