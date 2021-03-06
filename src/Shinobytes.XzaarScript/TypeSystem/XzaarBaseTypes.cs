﻿/* 
 * This file is part of XzaarScript.
 * Copyright (c) 2017-2018 XzaarScript, Karl Patrik Johansson, zerratar@gmail.com
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
 **/
 
using System;
using System.Collections.Generic;
using Shinobytes.XzaarScript.Parser.Ast.Expressions;

namespace Shinobytes.XzaarScript
{
    public static class XzaarBaseTypes
    {
        public static XzaarType Void => _void;
        public static XzaarType String => _string;
        public static XzaarType Number => _number;
        public static XzaarType I8 => _i8;
        public static XzaarType I16 => _i16;
        public static XzaarType I32 => _i32;
        public static XzaarType I64 => _i64;
        public static XzaarType U8 => _u8;
        public static XzaarType U16 => _u16;
        public static XzaarType U32 => _u32;
        public static XzaarType U64 => _u64;
        public static XzaarType F32 => _f32;
        public static XzaarType F64 => _f64;
        public static XzaarType Boolean => _boolean;
        public static XzaarType Any => _any;
        public static XzaarType Char => _char;
        public static XzaarType Date => _date;
        public static XzaarType Array => _array;
        public static XzaarType StringArray => _stringArray;
        public static XzaarType NumberArray => _numberArray;
        public static XzaarType BooleanArray => _booleanArray;
        public static XzaarType AnyArray => _anyArray;
        public static XzaarType CharArray => _charArray;
        public static XzaarType DateArray => _dateArray;
        internal static List<XzaarType> BaseTypes;

        private static readonly XzaarTypeBuilder _void;
        private static readonly XzaarTypeBuilder _string;

        private static readonly XzaarTypeBuilder _number;
        private static readonly XzaarTypeBuilder _i8;
        private static readonly XzaarTypeBuilder _i16;
        private static readonly XzaarTypeBuilder _i32;
        private static readonly XzaarTypeBuilder _i64;
        private static readonly XzaarTypeBuilder _u8;
        private static readonly XzaarTypeBuilder _u16;
        private static readonly XzaarTypeBuilder _u32;
        private static readonly XzaarTypeBuilder _u64;
        private static readonly XzaarTypeBuilder _f32;
        private static readonly XzaarTypeBuilder _f64;

        private static readonly XzaarTypeBuilder _boolean;
        private static readonly XzaarTypeBuilder _any;
        private static readonly XzaarTypeBuilder _char;
        private static readonly XzaarTypeBuilder _date;
        private static readonly XzaarTypeBuilder _array;
        private static readonly XzaarTypeBuilder _stringArray;
        private static readonly XzaarTypeBuilder _numberArray;
        private static readonly XzaarTypeBuilder _booleanArray;
        private static readonly XzaarTypeBuilder _anyArray;
        private static readonly XzaarTypeBuilder _charArray;
        private static readonly XzaarTypeBuilder _dateArray;

        static XzaarBaseTypes()
        {
            _boolean = new XzaarTypeBuilder("bool", null, Any, Any);
            _void = new XzaarTypeBuilder("void", null, null, null);
            _any = new XzaarTypeBuilder("any", null, null, null);
            
            _number = new XzaarTypeBuilder("number", null, Any, Any);
            _i8 = new XzaarTypeBuilder("i8", null, Number, Number);
            _i16 = new XzaarTypeBuilder("i16", null, Number, Number);
            _i32 = new XzaarTypeBuilder("i32", null, Number, Number);
            _i64 = new XzaarTypeBuilder("i64", null, Number, Number);

            _u8 = new XzaarTypeBuilder("u8", null, Number, Number);
            _u16 = new XzaarTypeBuilder("u16", null, Number, Number);
            _u32 = new XzaarTypeBuilder("u32", null, Number, Number);
            _u64 = new XzaarTypeBuilder("u64", null, Number, Number);

            _f32 = new XzaarTypeBuilder("f32", null, Number, Number);
            _f64 = new XzaarTypeBuilder("f64", null, Number, Number);

            _string = new XzaarTypeBuilder("string", null, Any, Any);



            _char = new XzaarTypeBuilder("char", null, Any, Any);
            _date = new XzaarTypeBuilder("date", null, Any, Any);
            _array = new XzaarTypeBuilder("array", null, Any, Any, Any);

            _stringArray = new XzaarTypeBuilder("string[]", null, Any, Any, String);

            _numberArray = new XzaarTypeBuilder("number[]", null, Any, Any, Number);
            _booleanArray = new XzaarTypeBuilder("bool[]", null, Any, Any, Boolean);
            _charArray = new XzaarTypeBuilder("char[]", null, Any, Any, Char);
            _anyArray = new XzaarTypeBuilder("any[]", null, Any, Any, Any);
            _dateArray = new XzaarTypeBuilder("date[]", null, Any, Any, Date);

            BaseTypes = new List<XzaarType>(new[] { Void, Any, Boolean, Number, String, Char, Date, Array,
                StringArray, NumberArray, BooleanArray, CharArray, DateArray,AnyArray });


            // add type fields
            _string.AddField(XzaarBaseTypes.Number, "Length");
            _array.AddField(XzaarBaseTypes.Number, "Length");
            _numberArray.AddField(XzaarBaseTypes.Number, "Length");
            _booleanArray.AddField(XzaarBaseTypes.Number, "Length");
            _charArray.AddField(XzaarBaseTypes.Number, "Length");
            _anyArray.AddField(XzaarBaseTypes.Number, "Length");
            _dateArray.AddField(XzaarBaseTypes.Number, "Length");
        }

        internal static void AddTypeToCache(XzaarType type)
        {
            BaseTypes.Add(type);
        }

        public static XzaarType Typeof(Type type)
        {
            if (type == typeof(int) || type == typeof(int?)
                || type == typeof(uint) || type == typeof(uint?)
                || type == typeof(byte) || type == typeof(byte?)
                || type == typeof(sbyte) || type == typeof(sbyte?)
                || type == typeof(short) || type == typeof(short?)
                || type == typeof(ushort) || type == typeof(ushort?)
                || type == typeof(long) || type == typeof(long?)
                || type == typeof(ulong) || type == typeof(ulong?)
                || type == typeof(decimal) || type == typeof(decimal?)
                || type == typeof(float) || type == typeof(float?)
                || type == typeof(double) || type == typeof(double?))
            {
                if (type.IsArray) return NumberArray;
                if (type == typeof(ushort) || type == typeof(ushort?)) return U16;
                if (type == typeof(uint) || type == typeof(uint?)) return U32;
                if (type == typeof(int) || type == typeof(int?)) return I32;
                if (type == typeof(ulong) || type == typeof(ulong?)) return U64;
                if (type == typeof(long) || type == typeof(long?)) return I64;
                if (type == typeof(float) || type == typeof(float?)) return F32;
                if (type == typeof(double) || type == typeof(double?)) return F64;
                return Number;
            }

            if (type == typeof(DateTime) || type == typeof(DateTime?)) return type.IsArray ? DateArray : Date;
            if (type == typeof(string)) return type.IsArray ? StringArray : String;
            if (type == typeof(bool)) return type.IsArray ? BooleanArray : Boolean;
            if (type == typeof(char)) return type.IsArray ? CharArray : Char;
            if (type == typeof(void)) return Void;

            return type.IsArray ? AnyArray : Any;
        }

        public static XzaarType CreateTypeFromStructExpression(StructExpression typeExpression)
        {
            var newType = new XzaarTypeBuilder(typeExpression.Name, null, Any, Any, null);
            foreach (var f in typeExpression.Fields)
            {
                if (f is FieldExpression fn)
                {
                    newType.AddField(fn.Type, fn.Name);
                }
            }
            return newType;
        }
    }
}
