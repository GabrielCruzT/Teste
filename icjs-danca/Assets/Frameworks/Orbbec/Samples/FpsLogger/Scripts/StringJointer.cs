using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace OrbbecEx
{
    public class StringJointer
    {
        const string bool_true = "true";
        const string bool_false = "false";
        const char char_null = '\0';
        const uint ten = 10U;
        const ulong tenl = 10UL;

        private StringBuilder string_builder;
        private char[] int_parser = new char[20];
        private int i;
        private int count;

        /// <summary>
        /// 输出的string值.
        /// </summary>
        public string stringValue;

        /// <summary>
        /// 容量.
        /// </summary>
        /// <value>The capacity.</value>
        public int capacity
        {
            get;
            private set;
        }

        /// <summary>
        /// 构造一个SpeedString.
        /// </summary>
        /// <param name="capacity">初始容量.</param>
        public StringJointer (int capacity)
        {
            this.capacity = capacity;
            string_builder = new StringBuilder (capacity);
            stringValue = (string)string_builder.GetType ().GetField (
                "_str",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance).GetValue (string_builder);
                //Clear ();
        }

        /// <summary>
        /// 清空容器.
        /// </summary>
        public void Clear ()
        {
            string_builder.Length = 0;
            //string_builder.Append (char_null,capacity);
            //string_builder.Length = 0;
        }

        private void ResetCapacity()
        {
            int len = string_builder.Length;
            if (len > capacity)
            {
                int sbCap = string_builder.Capacity;
                string_builder.Append (char_null, sbCap - capacity);
                string_builder.Length = len;
                capacity = sbCap;
                stringValue = (string)string_builder.GetType ().GetField (
                    "_str",
                    System.Reflection.BindingFlags.NonPublic |
                    System.Reflection.BindingFlags.Instance).GetValue (string_builder);
            }
            else if (len < capacity)
            {
                string_builder.Append (char_null);
                string_builder.Length = len;
            }
        }

//        private void CheckCapacity(int addLen)
//        {
//            int sbLen = string_builder.Length;
//            int totalLen = sbLen + addLen;
//            if (totalLen > capacity)
//            {
//                int oldCap = capacity;
//                capacity = totalLen * 2 + 2;
//                string_builder.Capacity = capacity;
//                string_builder.Append (' ', capacity - sbLen);
//                string_builder.Length = sbLen;
//                stringValue = (string)string_builder.GetType ().GetField (
//                    "_str",
//                    System.Reflection.BindingFlags.NonPublic |
//                    System.Reflection.BindingFlags.Instance).GetValue (string_builder);
//            }
//        }

        /// <summary>
        /// 添加元素.
        /// </summary>
        /// <param name="value">Value.</param>
        public StringJointer Append (string value)
        {
            //CheckCapacity (value.Length);
            string_builder.Append (value);
            ResetCapacity ();
            return this;
        }

        /// <summary>
        /// 添加元素.
        /// </summary>
        /// <param name="value">Value.</param>
        public StringJointer Append (char value)
        {
            //CheckCapacity (value.Length);
            string_builder.Append (value);
            ResetCapacity ();
            return this;
        }

        /// <summary>
        /// 添加元素.
        /// </summary>
        /// <param name="value">Value.</param>
        public StringJointer Append (char[] value)
        {
            //CheckCapacity (value.Length);
            string_builder.Append (value);
            ResetCapacity ();
            return this;
        }

        /// <summary>
        /// 添加元素.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="repeatCount">Repeat count.</param>
        public StringJointer Append (char value, int repeatCount)
        {
            //CheckCapacity (value.Length);
            string_builder.Append (value, repeatCount);
            ResetCapacity ();
            return this;
        }

        /// <summary>
        /// 添加元素.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="startIndex">Start index.</param>
        /// <param name="count">Count.</param>
        public StringJointer Append (string value,int startIndex, int count)
        {
            //CheckCapacity (value.Length);
            string_builder.Append (value, startIndex, count);
            ResetCapacity ();
            return this;
        }

        /// <summary>
        /// 添加元素.
        /// </summary>
        /// <param name="value">Value.</param>
        /// <param name="startIndex">Start index.</param>
        /// <param name="count">Count.</param>
        public StringJointer Append (char[] value,int startIndex, int count)
        {
            //CheckCapacity (value.Length);
            string_builder.Append (value, startIndex, count);
            ResetCapacity ();
            return this;
        }

        /// <summary>
        /// 添加元素.
        /// </summary>
        /// <param name="value">If set to <c>true</c> value.</param>
        public StringJointer Append (bool value)
        {
            if (value)
            {
                return Append(bool_true);
            }
            return Append(bool_false);
        }

        /// <summary>
        /// 添加元素.
        /// </summary>
        /// <param name="value">Value.</param>
        public StringJointer Append (int value)
        {
            if (value >= 0)
            {
                count = ToCharArray ((uint)value, int_parser, 0);
            }
            else
            {
                int_parser [0] = '-';
                count = ToCharArray ((uint)-value, int_parser, 1) + 1;
            }

            //CheckCapacity (count);
            string_builder.Append (int_parser, 0, count);
            ResetCapacity ();
            return this;
        }

        /// <summary>
        /// 添加元素.
        /// </summary>
        /// <param name="value">Value.</param>
        public StringJointer Append (uint value)
        {
            count = ToCharArray (value, int_parser, 0);

            //CheckCapacity (count);
            string_builder.Append (int_parser, 0, count);
            ResetCapacity ();
            return this;
        }

        /// <summary>
        /// 添加元素.
        /// </summary>
        /// <param name="value">Value.</param>
        public StringJointer Append (long value)
        {
            if (value >= 0L)
            {
                count = ToCharArray ((ulong)value, int_parser, 0);
            }
            else
            {
                int_parser [0] = '-';
                count = ToCharArray ((ulong)-value, int_parser, 1) + 1;
            }

            //CheckCapacity (count);
            string_builder.Append (int_parser, 0, count);
            ResetCapacity ();
            return this;
        }

        /// <summary>
        /// 添加元素.
        /// </summary>
        /// <param name="value">Value.</param>
        public StringJointer Append (ulong value)
        {
            count = ToCharArray (value, int_parser, 0);

            //CheckCapacity (count);
            string_builder.Append (int_parser, 0, count);
            ResetCapacity ();
            return this;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        /// <filterpriority>2</filterpriority>
        public override string ToString ()
        {
            return string_builder.ToString ();
        }

        private static int ToCharArray (uint value, char[] buffer, int bufferIndex)
        {
            if (value == 0)
            {
                buffer [bufferIndex] = '0';
                return 1;
            }

            int len = 1;
            for (uint rem = value / ten; rem > 0; rem /= ten)
            {
                len++;
            }

            for (int i = len - 1; i >= 0; i--)
            {
                buffer [bufferIndex + i] = (char)('0' + (value % ten));
                value /= ten;
            }

            return len;
        }

        private static int ToCharArray (ulong value, char[] buffer, int bufferIndex)
        {
            if (value == 0UL)
            {
                buffer [bufferIndex] = '0';
                return 1;
            }

            int len = 1;
            for (ulong rem = value / tenl; rem > 0UL; rem /= tenl)
            {
                len++;
            }

            for (int i = len - 1; i >= 0; i--)
            {
                buffer [bufferIndex + i] = (char)('0' + (uint)(value % tenl));
                value /= tenl;
            }

            return len;
        }
    }
}