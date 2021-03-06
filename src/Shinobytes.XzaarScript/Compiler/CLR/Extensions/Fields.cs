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
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

using Shinobytes.XzaarScript.Compiler.Types;

namespace Shinobytes.XzaarScript.Compiler.Extensions
{
    /// <summary>
    /// Contains extension methods for manipulating fields
    /// </summary>
    public static partial class Fields
    {
        #region LoadField

        /// <summary>
        /// Pops a reference from the evaluation stack and pushes the value of the given field for that object
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="field">The field to load</param>

        public static XsILGenerator LoadField(this XsILGenerator generator, FieldInfo field)
            => generator.FluentEmit(field.IsStatic ? OpCodes.Ldsfld : OpCodes.Ldfld, field);

        public static XsILGenerator LoadField(this XsILGenerator generator, XsField field)
            => generator.LoadField(field.FIeldInfo);

        /// <summary>
        /// Pops a reference from the evaluation stack and pushes the value of the field (with the given name on the given type) for that object
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="type">The type the field is on</param>
        /// <param name="fieldName">The name of the field</param>

        public static XsILGenerator LoadField(this XsILGenerator generator, Type type, string fieldName)
            => generator.LoadField(GetFieldInfo(type, fieldName));

        /// <summary>
        /// Pops a reference from the evaluation stack and pushes the value of the field (with the given name on the given type) for that object
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldName">The name of the field</param>

        public static XsILGenerator LoadField<T>(this XsILGenerator generator, string fieldName)
            => generator.LoadField(typeof(T), fieldName);

        /// <summary>
        /// Pushes the value of the static field represented by the given expression
        /// </summary>
        /// <typeparam name="TField">The type of the field</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>

        public static XsILGenerator LoadField<TField>(this XsILGenerator generator, Expression<Func<TField>> fieldExpression)
            => generator.LoadField(GetFieldInfo(fieldExpression));

        /// <summary>
        /// Pops a reference from the evaluation stack and pushes the value of the field represented by the given expression for that object
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <typeparam name="TField">The type of the field</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>

        public static XsILGenerator LoadField<T, TField>(this XsILGenerator generator, Expression<Func<T, TField>> fieldExpression)
            => generator.LoadField(GetFieldInfo(fieldExpression));

        #endregion
        #region LoadFieldVolatile
        /// <summary>
        /// Pops a reference from the evaluation stack and pushes the value of the given field for that object, with volatile semantics
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="field">The field to load</param>

        public static XsILGenerator LoadFieldVolatile(this XsILGenerator generator, FieldInfo field)
        {
            return generator.FluentEmit(OpCodes.Volatile)
                            .LoadField(field);
        }

        public static XsILGenerator LoadFieldVolatile(this XsILGenerator generator, XsField field)
            => generator.LoadFieldVolatile(field.FIeldInfo);


        /// <summary>
        /// Pops a reference from the evaluation stack and pushes the value of the field (with the given name on the given type) for that object, with volatile semantics
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="type">The type the field is on</param>
        /// <param name="fieldName">The name of the field</param>

        public static XsILGenerator LoadFieldVolatile(this XsILGenerator generator, Type type, string fieldName)
            => generator.LoadFieldVolatile(GetFieldInfo(type, fieldName));

        /// <summary>
        /// Pops a reference from the evaluation stack and pushes the value of the field (with the given name on the given type) for that object, with volatile semantics
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldName">The name of the field</param>

        public static XsILGenerator LoadFieldVolatile<T>(this XsILGenerator generator, string fieldName)
            => generator.LoadFieldVolatile(typeof(T), fieldName);

        /// <summary>
        ///Pushes the value of the static field represented by the given expression, with volatile semantics
        /// </summary>
        /// <typeparam name="TField">The type of the field</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>

        public static XsILGenerator LoadFieldVolatile<TField>(this XsILGenerator generator, Expression<Func<TField>> fieldExpression)
            => generator.LoadFieldVolatile(GetFieldInfo(fieldExpression));

        /// <summary>
        /// Pops a reference from the evaluation stack and pushes the value of the field represented by the given expression for that object, with volatile semantics
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <typeparam name="TField">The type of the field</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>

        public static XsILGenerator LoadFieldVolatile<T, TField>(this XsILGenerator generator, Expression<Func<T, TField>> fieldExpression)
            => generator.LoadFieldVolatile(GetFieldInfo(fieldExpression));

        #endregion
        #region LoadFieldAddress

        /// <summary>
        /// Pops a reference from the evaluation stack and pushes the address of the given field for that object
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="field">The field to load</param>

        public static XsILGenerator LoadFieldAddress(this XsILGenerator generator, FieldInfo field)
        {
            return generator.FluentEmit(field.IsStatic ? OpCodes.Ldsflda : OpCodes.Ldflda, field);
        }

        /// <summary>
        /// Pops a reference from the evaluation stack and pushes the address of the field (with the given name on the given type) for that object
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="type">The type the field is on</param>
        /// <param name="fieldName">The name of the field</param>

        public static XsILGenerator LoadFieldAddress(this XsILGenerator generator, Type type, string fieldName)
            => generator.LoadFieldAddress(GetFieldInfo(type, fieldName));

        /// <summary>
        /// Pops a reference from the evaluation stack and pushes the address of the field (with the given name on the given type) for that object
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldName">The name of the field</param>

        public static XsILGenerator LoadFieldAddress<T>(this XsILGenerator generator, string fieldName)
            => generator.LoadFieldAddress(typeof(T), fieldName);

        /// <summary>
        /// Pushes the address of the static field represented by the given expression
        /// </summary>
        /// <typeparam name="TField">The type of the field</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>

        public static XsILGenerator LoadFieldAddress<TField>(this XsILGenerator generator, Expression<Func<TField>> fieldExpression)
            => generator.LoadFieldAddress(GetFieldInfo(fieldExpression));

        /// <summary>
        /// Pops a reference from the evaluation stack and pushes the address of the field represented by the given expression for that object
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <typeparam name="TField">The type of the field</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>

        public static XsILGenerator LoadFieldAddress<T, TField>(this XsILGenerator generator, Expression<Func<T, TField>> fieldExpression)
            => generator.LoadFieldAddress(GetFieldInfo(fieldExpression));

        #endregion
        #region LoadFieldAddressVolatile

        /// <summary>
        /// Pops a reference from the evaluation stack and pushes the address of the given field for that object, with volatile semantics
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="field">The field to load</param>

        public static XsILGenerator LoadFieldAddressVolatile(this XsILGenerator generator, FieldInfo field)
        {
            return generator.FluentEmit(OpCodes.Volatile)
                            .LoadFieldAddress(field);
        }

        /// <summary>
        /// Pops a reference from the evaluation stack and pushes the address of the field (with the given name on the given type) for that object, with volatile semantics
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="type">The type the field is on</param>
        /// <param name="fieldName">The name of the field</param>

        public static XsILGenerator LoadFieldAddressVolatile(this XsILGenerator generator, Type type, string fieldName)
            => generator.LoadFieldAddressVolatile(GetFieldInfo(type, fieldName));

        /// <summary>
        /// Pops a reference from the evaluation stack and pushes the address of the field (with the given name on the given type) for that object, with volatile semantics
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldName">The name of the field</param>

        public static XsILGenerator LoadFieldAddressVolatile<T>(this XsILGenerator generator, string fieldName)
            => generator.LoadFieldAddressVolatile(typeof(T), fieldName);

        /// <summary>
        /// Pushes the address of the static field represented by the given expression, with volatile semantics
        /// </summary>
        /// <typeparam name="TField">The type of the field</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>

        public static XsILGenerator LoadFieldAddressVolatile<TField>(this XsILGenerator generator, Expression<Func<TField>> fieldExpression)
            => generator.LoadFieldAddressVolatile(GetFieldInfo(fieldExpression));

        /// <summary>
        /// Pops a reference from the evaluation stack and pushes the address of the field represented by the given expression for that object, with volatile semantics
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <typeparam name="TField">The type of the field</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>

        public static XsILGenerator LoadFieldAddressVolatile<T, TField>(this XsILGenerator generator, Expression<Func<T, TField>> fieldExpression)
            => generator.LoadFieldAddressVolatile(GetFieldInfo(fieldExpression));

        #endregion
        #region StoreInField

        /// <summary>
        /// Pops a reference and a value from the evaluation stack and stores the value in the given field for that object
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="field">The field to store the value in</param>

        public static XsILGenerator StoreInField(this XsILGenerator generator, FieldInfo field)
            => generator.FluentEmit(field.IsStatic ? OpCodes.Stsfld : OpCodes.Stfld, field);

        public static XsILGenerator StoreInField(this XsILGenerator generator, XsField field)
            => generator.StoreInField(field.FIeldInfo);

        /// <summary>
        /// Pops a reference and a value from the evaluation stack and stores the value in the field (with the given name on the given type) for that object
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="type">The type the field is on</param>
        /// <param name="fieldName">The name of the field</param>

        public static XsILGenerator StoreInField(this XsILGenerator generator, Type type, string fieldName)
            => generator.StoreInField(GetFieldInfo(type, fieldName));

        /// <summary>
        /// Pops a reference and a value from the evaluation stack and stores the value in the field (with the given name on the given type) for that object
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldName">The name of the field</param>

        public static XsILGenerator StoreInField<T>(this XsILGenerator generator, string fieldName)
            => generator.StoreInField(typeof(T), fieldName);

        /// <summary>
        /// Pops a value from the evaluation stack and stores the value in the static field represented by the given expression
        /// </summary>
        /// <typeparam name="TField">The type of the field</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>

        public static XsILGenerator StoreInField<TField>(this XsILGenerator generator, Expression<Func<TField>> fieldExpression)
            => generator.StoreInField(GetFieldInfo(fieldExpression));

        /// <summary>
        /// Pops a reference and a value from the evaluation stack and stores the value in the field represented by the given expression for that object
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <typeparam name="TField">The type of the field</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>

        public static XsILGenerator StoreInField<T, TField>(this XsILGenerator generator, Expression<Func<T, TField>> fieldExpression)
            => generator.StoreInField(GetFieldInfo(fieldExpression));

        #endregion
        #region StoreInFieldVolatile

        /// <summary>
        /// Pops a reference and a value from the evaluation stack and stores the value in the given field for that object, with volatile semantics
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="field">The field to store the value in</param>

        public static XsILGenerator StoreInFieldVolatile(this XsILGenerator generator, FieldInfo field)
        {
            return generator.FluentEmit(OpCodes.Volatile)
                            .StoreInField(field);
        }

        /// <summary>
        /// Pops a reference and a value from the evaluation stack and stores the value in the field (with the given name on the given type) for that object, with volatile semantics
        /// </summary>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="type">The type the field is on</param>
        /// <param name="fieldName">The name of the field</param>

        public static XsILGenerator StoreInFieldVolatile(this XsILGenerator generator, Type type, string fieldName)
            => generator.StoreInFieldVolatile(GetFieldInfo(type, fieldName));

        /// <summary>
        /// Pops a reference and a value from the evaluation stack and stores the value in the field (with the given name on the given type) for that object, with volatile semantics
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldName">The name of the field</param>

        public static XsILGenerator StoreInFieldVolatile<T>(this XsILGenerator generator, string fieldName)
            => generator.StoreInFieldVolatile(typeof(T), fieldName);

        /// <summary>
        /// Pops a value from the evaluation stack and stores the value in the static field represented by the given expression, with volatile semantics
        /// </summary>
        /// <typeparam name="TField">The type of the field</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>

        public static XsILGenerator StoreInFieldVolatile<TField>(this XsILGenerator generator, Expression<Func<TField>> fieldExpression)
            => generator.StoreInFieldVolatile(GetFieldInfo(fieldExpression));

        /// <summary>
        /// Pops a reference and a value from the evaluation stack and stores the value in the field represented by the given expression for that object, with volatile semantics
        /// </summary>
        /// <typeparam name="T">The type the field is on</typeparam>
        /// <typeparam name="TField">The type of the field</typeparam>
        /// <param name="generator">The <see cref="T:System.Reflection.Emit.XsILGenerator" /> to emit instructions from</param>
        /// <param name="fieldExpression">An expression representing the field to load</param>

        public static XsILGenerator StoreInFieldVolatile<T, TField>(this XsILGenerator generator, Expression<Func<T, TField>> fieldExpression)
            => generator.StoreInFieldVolatile(GetFieldInfo(fieldExpression));

        #endregion

        #region GetFieldInfo

        private static FieldInfo GetFieldInfo<T, TField>(Expression<Func<T, TField>> expression)
        {
            var field = (expression?.Body as MemberExpression)?.Member as FieldInfo;

            if (field == null)
            {
                throw new InvalidOperationException("Expression does not represent a field");
            }

            return field;
        }

        private static FieldInfo GetFieldInfo<TField>(Expression<Func<TField>> expression)
        {
            var field = (expression?.Body as MemberExpression)?.Member as FieldInfo;

            if (field == null)
            {
                throw new InvalidOperationException("Expression does not represent a field");
            }

            return field;
        }

        private static FieldInfo GetFieldInfo(Type type, string fieldName)
        {
            var field = type.GetField(fieldName, BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);

            if (field == null)
            {
                throw new InvalidOperationException("Field with name `" + fieldName + "` cannot be found on type " + type.Name);
            }

            return field;
        }

        #endregion
    }
}
