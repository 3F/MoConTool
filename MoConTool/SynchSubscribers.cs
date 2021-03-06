﻿/*
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
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using net.r_eg.MoConTool.Log;

namespace net.r_eg.MoConTool
{
    /// <summary>
    /// Thread-safe container of listeners.
    /// </summary>
    /// <typeparam name="T">IListener based type.</typeparam>
    public class SynchSubscribers<T>: ISynchSubscribers<T>
        where T : IListener
    {
        /// <summary>
        /// justification: A common using of SynchSubscribers should be as an only sequential accessing to all elements at once - that is O(1).
        ///                And most important - it's a ordered container, because we need to save priority by listeners.
        /// But for any single accessing it should be O(n), thus we also use O(1) accessor below to improve performance of the List type.
        /// </summary>
        protected List<T> listeners = new List<T>();

        /// <summary>
        /// A shallow copy of listeners which has O(1) for any single accessing to elements.
        /// This is not an ordered, thread-safe container, and unfortunately we can't use this as primarily container (read justification above).
        /// </summary>
        protected ConcurrentDictionary<Guid, T> accessor = new ConcurrentDictionary<Guid, T>();

        private object sync = new object();

        /// <summary>
        /// Number of elements contained in the thread-safe collection.
        /// </summary>
        public int Count
        {
            get
            {
                lock(sync)
                {
                    return listeners.Count;
                }
            }
        }

        /// <summary>
        /// Gets the object used to synchronize access to the thread-safe collection.
        /// </summary>
        public object SyncRoot
        {
            get {
                return sync;
            }
        }

        /// <summary>
        /// Adds an listener to thread-safe collection.
        /// </summary>
        /// <param name="listener"></param>
        /// <returns></returns>
        public bool register(T listener)
        {
            lock(sync)
            {
                if(contains(listener)) {
                    return false;
                }

                //items.Insert(Count, listener);
                listeners.Add(listener);
                accessor[listener.Id] = listener;

                LSender.Send(this, $"listener has been added into container - {listener.Id}", Message.Level.Debug);
                return true;
            }
        }

        /// <summary>
        /// Removes specified listener from the collection.
        /// </summary>
        /// <param name="listener"></param>
        /// <returns></returns>
        public bool unregister(T listener)
        {
            lock(sync)
            {
                int idx = listeners.FindIndex(p => p.Id == listener.Id);
                if(idx == -1) {
                    return false;
                }

                listeners.RemoveAt(idx);
                T v;
                accessor.TryRemove(listener.Id, out v);

                LSender.Send(this, $"listener has been removed from container - {listener.Id}", Message.Level.Debug);
                return true;
            }
        }

        /// <summary>
        /// Reset all collection.
        /// </summary>
        public void reset()
        {
            lock(sync) {
                listeners.Clear();
                accessor.Clear();
            }
        }

        /// <summary>
        /// Determines whether the collection contains an listener.
        /// </summary>
        /// <param name="listener"></param>
        /// <returns></returns>
        public bool contains(T listener)
        {
            return exists(listener.Id);
        }

        /// <summary>
        /// Checks existence of listener by Guid.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool exists(Guid id)
        {
            lock(sync)
            {
#if DEBUG
                Debug.Assert(accessor.Count == listeners.Count);
#endif
                return accessor.ContainsKey(id);
                //return listeners.Any(l => ((IListener)l).Id == id);
            }
        }

        /// <summary>
        /// Get listener by specific id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>null if not found.</returns>
        public T getById(Guid id)
        {
            lock(sync)
            {
                if(exists(id)) {
                    return accessor[id];
                }
                return default(T);
                //return listeners.Where(l => ((IListener)l).Id == id).FirstOrDefault();
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            IEnumerator<T> result;
            lock(sync) {
                result = listeners.GetEnumerator();
            }
            return result;
        }

        public T this[Guid id]
        {
            get {
                return getById(id);
            }
            set
            {
                lock(sync)
                {
                    T listener  = getById(id);
                    listener    = value;
                }
            }
        }

        public T this[int index]
        {
            get
            {
                lock(sync) {
                    return listeners[index];
                }
            }
            set
            {
                lock(sync)
                {
                    if(index < 0 || index >= listeners.Count) {
                        throw new ArgumentOutOfRangeException("index", index, $"Value must be in range: 0 - {listeners.Count - 1}");
                    }
                    listeners[index] = value;
                }
            }
        }
    }
}
