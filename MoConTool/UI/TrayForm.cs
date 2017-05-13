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
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using net.r_eg.MoConTool.Filters;
using net.r_eg.MoConTool.HotKeys;
using net.r_eg.MoConTool.Log;
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

        private GlobalKeys hotKeys = new GlobalKeys();

        private bool closing = false;
        private Size origin;

        private ISender log = LSender._;
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
                LSender.Send(this, ex.Message, Log.Message.Level.Debug);
            }

            hotKeys.KeyPress    += onHotKeys;
            log.Received        += onLogMsgReceived;

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

            Modifiers mcomb = Modifiers.ControlKey | Modifiers.AltKey | Modifiers.ShiftKey;
            try {
                hotKeys.register(mcomb, Keys.Z);
                hotKeys.register(mcomb, Keys.X);
            }
            catch(Exception ex) {
                log.send(this, ex.Message, Log.Message.Level.Error);
            }
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

        private string to(decimal val)
        {
            return val.ToString(System.Globalization.CultureInfo.InvariantCulture);
        }

        private void dispose()
        {
            log.Received -= onLogMsgReceived;
            hotKeys.Dispose();
            svc.Dispose();
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

            if(fhs.Data == null) {
                fhs.Data = new HyperactiveScrollFilter.TData();
            }
            var vhs = (HyperactiveScrollFilter.TData)fhs.Data;
            numHyperScrollCapacity.Value    = vhs.capacity;
            numHyperScrollLimit.Value       = vhs.limit;

            if(fic.Data == null) {
                fic.Data = new InterruptedClickFilter.TData();
            }
            var vic = (InterruptedClickFilter.TData)fic.Data;
            numIntClickDeltaMin.Value   = vic.deltaMin;
            numIntClickDeltaMax.Value   = vic.deltaMax;

            if(fmc.Data == null) {
                fmc.Data = new MixedClicksFilter.TData();
            }
            var vmc = (MixedClicksFilter.TData)fmc.Data;
            chkMixedClicksOnlyDown.Checked = vmc.onlyDownCodes;
        }

        private void onLogMsgReceived(object sender, Log.Message e)
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

        private void onHotKeys(object sender, HotKeyEventArgs e)
        {
            if(e.Modifier != (Modifiers.ControlKey | Modifiers.AltKey | Modifiers.ShiftKey)) {
                return;
            }

            if(!hotKeys.highOrderBitIsOne(Keys.RShiftKey)) {
                return;
            }

            log.send(this, $"Received [Ctrl + Alt + RShift] + {e.Key.ToString()}", Log.Message.Level.Debug);
            switch(e.Key)
            {
                case Keys.Z: {
                    chkPlug.Checked = false;
                    Show();
                    return;
                }
                case Keys.X: {
                    chkDebug.Checked = !chkDebug.Checked;
                    return;
                }
            }
        }

        private void TrayForm_Load(object sender, EventArgs e)
        {
            chkDebug_CheckedChanged(this, EventArgs.Empty);
            render();

            Text                = $"{Application.ProductName} v{MoConToolVersion.S_INFO}";
            notifyIconMain.Text = Text;
            menuCaption.Text    = $"{Application.ProductName} v{MoConToolVersion.S_NUM_REV}";
#if DEBUG
            Text += " [Debug version]";
#endif

            chkPlug.Checked = true;
        }

        private void menuAbout_Click(object sender, EventArgs e)
        {
            MessageBox.Show($"Version: {MoConToolVersion.S_INFO_FULL}\n\nLicense: MIT\nentry.reg@gmail.com :: github.com/3F", 
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

        private void TrayForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            dispose();
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

        private void listBoxDebug_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Modifiers == Keys.Control && e.KeyCode == Keys.C) {
                menuCopy_Click(sender, e);
            }
            else if(e.Modifiers == Keys.Control && e.KeyCode == Keys.A) {
                menuSelectAll_Click(sender, e);
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
            //if(!chkDebug.Checked) {
            //    uiAction(() => listBoxDebug.Items.Clear());
            //}
        }

        private void btnCmd_Click(object sender, EventArgs e)
        {
            var sb = new StringBuilder();
            sb.Append(Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location));

            if(chkInterruptedClick.Checked 
                && (chkInterruptedBtnL.Checked || chkInterruptedBtnM.Checked || chkInterruptedBtnR.Checked))
            {
                sb.Append(" ");
                sb.Append("-InterruptedClick ");
                if(chkInterruptedBtnL.Checked) {
                    sb.Append("L");
                }
                if(chkInterruptedBtnM.Checked) {
                    sb.Append("M");
                }
                if(chkInterruptedBtnR.Checked) {
                    sb.Append("R");
                }
                sb.Append(" ");
                sb.Append(to(numInterruptedClick.Value));
                sb.Append(";");
                sb.Append(to(numIntClickDeltaMin.Value));
                sb.Append(";");
                sb.Append(to(numIntClickDeltaMax.Value));
            }

            if(chkMixedClicks.Checked 
                && (chkMixedBtnL.Checked || chkMixedBtnM.Checked || chkMixedBtnR.Checked))
            {
                sb.Append(" ");
                sb.Append("-MixedClicks ");
                if(chkMixedBtnL.Checked) {
                    sb.Append("L");
                }
                if(chkMixedBtnM.Checked) {
                    sb.Append("M");
                }
                if(chkMixedBtnR.Checked) {
                    sb.Append("R");
                }
                sb.Append(" ");
                sb.Append(chkMixedClicksOnlyDown.Checked ? 1 : 0);
            }

            if(chkDoubleClick.Checked 
                && (chkDoubleBtnL.Checked || chkDoubleBtnM.Checked || chkDoubleBtnR.Checked))
            {
                sb.Append(" ");
                sb.Append("-DoubleClicks ");
                if(chkDoubleBtnL.Checked) {
                    sb.Append("L");
                }
                if(chkDoubleBtnM.Checked) {
                    sb.Append("M");
                }
                if(chkDoubleBtnR.Checked) {
                    sb.Append("R");
                }
                sb.Append(" ");
                sb.Append(to(numDoubleClick.Value));
            }

            if(chkHyperactiveScroll.Checked)
            {
                sb.Append(" ");
                sb.Append("-HyperactiveScroll ");
                sb.Append(to(numHyperScrollCapacity.Value));
                sb.Append(";");
                sb.Append(to(numHyperScrollLimit.Value));
            }

            MessageBox.Show(sb.ToString(), "Press Ctrl+C to copy:");
        }

        private void btnNewFilter_Click(object sender, EventArgs e)
        {
            menuSrcCode_Click(sender, e);
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

        private void numIntClickDeltaMin_ValueChanged(object sender, EventArgs e)
        {
            if(fic.Data == null) {
                fic.Data = new InterruptedClickFilter.TData();
            }
            ((InterruptedClickFilter.TData)fic.Data).deltaMin = (uint)numIntClickDeltaMin.Value;
        }

        private void numIntClickDeltaMax_ValueChanged(object sender, EventArgs e)
        {
            if(fic.Data == null) {
                fic.Data = new InterruptedClickFilter.TData();
            }
            ((InterruptedClickFilter.TData)fic.Data).deltaMax = (uint)numIntClickDeltaMax.Value;
        }

        private void chkMixedClicksOnlyDown_CheckedChanged(object sender, EventArgs e)
        {
            if(fmc.Data == null) {
                fmc.Data = new MixedClicksFilter.TData();
            }
            ((MixedClicksFilter.TData)fmc.Data).onlyDownCodes = chkMixedClicksOnlyDown.Checked;
        }

        private void numDoubleClick_ValueChanged(object sender, EventArgs e)
        {
            fdc.Value = (double)numDoubleClick.Value;
        }

        private void numHyperScrollCapacity_ValueChanged(object sender, EventArgs e)
        {
            if(fhs.Data == null) {
                fhs.Data = new HyperactiveScrollFilter.TData();
            }
            ((HyperactiveScrollFilter.TData)fhs.Data).capacity = (int)numHyperScrollCapacity.Value;
        }

        private void numHyperScrollLimit_ValueChanged(object sender, EventArgs e)
        {
            if(fhs.Data == null) {
                fhs.Data = new HyperactiveScrollFilter.TData();
            }
            ((HyperactiveScrollFilter.TData)fhs.Data).limit = (uint)numHyperScrollLimit.Value;
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
