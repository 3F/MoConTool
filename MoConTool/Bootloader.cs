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
using System.Linq;
using net.r_eg.MoConTool.Extensions;
using net.r_eg.MoConTool.Filters;
using net.r_eg.MoConTool.Log;

namespace net.r_eg.MoConTool
{
    using AVal = Action<IMouseListener, string>;

    public class Bootloader: IBootloader
    {
        private object sync = new object();

        /// <summary>
        /// Available mouse filters.
        /// </summary>
        public SynchSubscribers<IMouseListener> Filters
        {
            get;
            protected set;
        } = new SynchSubscribers<IMouseListener>();

        /// <summary>
        /// Activated filters only.
        /// </summary>
        public IEnumerable<IMouseListener> ActivatedFilters
        {
            get {
                return Filters.Where(f => f.Activated);
            }
        }

        /// <summary>
        /// To register all default filters.
        /// </summary>
        public virtual void register()
        {
            lock(sync) {
                unregister();
                Filters.register(new CommonFilter());

                Filters.register(new MixedClicksFilter());
                Filters.register(new DoubleClicksFilter());
                Filters.register(new InterruptedClickFilter());

                Filters.register(new HyperactiveScrollFilter());
                Filters.register(new MouseFilter());
            }
        }

        /// <summary>
        /// To unregister all available filters.
        /// </summary>
        public void unregister()
        {
            lock(sync) {
                Filters.reset();
            }
        }

        /// <summary>
        /// To configure data via arguments list.
        /// </summary>
        /// <param name="args"></param>
        public void configure(string[] args)
        {
            if(args == null) {
                throw new ArgumentNullException();
            }

            ArgsHelper h = new ArgsHelper(Filters);
            h.args = args;

            AVal aic = (IMouseListener l, string s) =>
            {
                var v = s.DValues(';');
                l.Value = v[0];
                l.Data = new InterruptedClickFilter.TData() { deltaMin = (uint)v[1], deltaMax = (uint)v[2] };
            };

            AVal amc = (IMouseListener l, string s) =>
            {
                l.Data = new MixedClicksFilter.TData() { onlyDownCodes = s.DValue() != 0 };
            };

            AVal adc = (IMouseListener l, string s) =>
            {
                l.Value = s.DValue();
            };

            AVal ahs = (IMouseListener l, string s) =>
            {
                var v = s.DValues(';');
                l.Data = new HyperactiveScrollFilter.TData() { capacity = (int)v[0], limit = (uint)v[1] };
            };

            for(h.idx = 0; h.idx < args.Length; ++h.idx)
            {
                if(!h.set("-InterruptedClick", typeof(InterruptedClickFilter), aic) 
                    && !h.set("-MixedClicks", typeof(MixedClicksFilter), amc)
                    && !h.set("-DoubleClicks", typeof(DoubleClicksFilter), adc)
                    && !h.set("-HyperactiveScroll", typeof(HyperactiveScrollFilter), ahs, MouseState.Flags.None))
                {
                    throw new ArgumentException($"Incorrect arguments: {args[h.idx]} -> {String.Join(" ", args)}");
                }
            }
        }

        protected sealed class ArgsHelper
        {
            public string[] args;
            public int idx;

            public SynchSubscribers<IMouseListener> Filters
            {
                get;
                private set;
            }

            public ArgsHelper(SynchSubscribers<IMouseListener> filters)
            {
                Filters = filters;
            }

            public bool set(string key, Type filter, AVal aval, MouseState.Flags ifType = MouseState.Flags.LMR)
            {
                if(!args[idx].Eq(key)) {
                    return false;
                }
                LSender.Send(this, $"Found '{key}' key in {idx} position.", Message.Level.Debug);

                MouseState.Flags type;
                if(ifType != MouseState.Flags.None) {
                    type = MouseState.Extract(args[++idx]);
                    LSender.Send(this, $"type = {type} by {idx}", Message.Level.Debug);
                }
                else {
                    type = MouseState.Flags.None;
                }

                string val;
                if(aval != null) {
                    val = args[++idx];
                    LSender.Send(this, $"value = {val} by {idx}", Message.Level.Debug);
                }
                else {
                    val = null;
                }

                Guid g = filter.GUID;
                if(Filters[g] == null) {
                    LSender.Send(this, $"Filter '{g}' is not registered.", Message.Level.Debug);
                    return true;
                }
                Filters[g].Activated = true;

                aval?.Invoke(Filters[g], val);

                if(ifType == MouseState.Flags.None) {
                    return true;
                }

                if((ifType & type) == 0) {
                    throw new ArgumentException($"Incorrect Flags {type} -> {ifType}");
                }

                Filters[g].Handler = type;
                return true;
            }
        }
    }
}
