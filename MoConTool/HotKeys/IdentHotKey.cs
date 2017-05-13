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
using System.Collections.Generic;

namespace net.r_eg.MoConTool.HotKeys
{
    /// <summary>
    /// Represents identifier of the global hot key.
    /// </summary>
    public class IdentHotKey
    {
        public const int MAX_VALUE = int.MaxValue;
        public const int MIN_VALUE = 1;

        protected volatile int id;
        private object _lock = new object();

        public IEnumerable<int> Iter
        {
            get
            {
                for(int i = MIN_VALUE; i < Current; ++i) {
                    yield return i;
                }
            }
        }

        public int Current
        {
            get { return id; }
        }

        public void Reset()
        {
            id = MIN_VALUE;
        }

        public IdentHotKey Next()
        {
            lock(_lock)
            {
                if(id + 1 > MAX_VALUE
                    || (id > MIN_VALUE && id + 1 <= MIN_VALUE)) // case of overflow for type
                {
                    throw new OverflowException($"no free values for new identifier: {id} / {MAX_VALUE}");
                }
                ++id;

                return this;
            }
        }

        public IdentHotKey()
        {
            Reset();
        }
    }
}
