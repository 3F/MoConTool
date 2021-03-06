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
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace net.r_eg.MoConTool.Extensions
{
    public static class StringExtension
    {
        /// <summary>
        /// Gets Guid from hash by any string.
        /// </summary>
        /// <param name="str">String for calculating.</param>
        /// <returns></returns>
        public static Guid Guid(this string str)
        {
            if(str == null) {
                str = String.Empty;
            }
            using(MD5 md5 = MD5.Create()) {
                return new Guid(md5.ComputeHash(Encoding.UTF8.GetBytes(str)));
            }
        }

        public static bool Eq(this string a, string b, StringComparison cmp = StringComparison.InvariantCultureIgnoreCase)
        {
            return a.Equals(b, cmp);
        }

        public static double[] DValues(this string str, params char[] separator)
        {
            List<double> ret = new List<double>();
            foreach(string num in str.Split(separator)) {
                ret.Add(num.DValue());
            }
            return ret.ToArray();
        }

        public static double DValue(this string str)
        {
            double v;
            if(!double.TryParse(str, NumberStyles.Float, CultureInfo.InvariantCulture, out v)) {
                throw new ArgumentException($"The value '{str}' is not correct");
            }
            return v;
        }
    }
}
