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
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace Shinobytes.XzaarScript.Parser.Ast.Expressions
{
    public class FunctionCallExpression : XzaarExpression, IArgumentProvider
    {
        private readonly FunctionExpression function;
        private readonly LambdaExpression lambda;
        private readonly XzaarMethodBase method;
        private readonly MemberExpression member;
        private readonly FunctionCallExpression previousInvocation;
        private readonly string alias;

        protected IList<XzaarExpression> arguments;
        

        internal FunctionCallExpression(XzaarMethodBase method, IList<XzaarExpression> args = null)
        {
            arguments = args ?? new List<XzaarExpression>();
            this.method = method;
        }

        internal FunctionCallExpression(FunctionExpression method, IList<XzaarExpression> args = null)
        {
            arguments = args ?? new List<XzaarExpression>();
            this.function = method;
        }
        internal FunctionCallExpression(string alias, FunctionExpression method, IList<XzaarExpression> args = null)
        {
            arguments = args ?? new List<XzaarExpression>();
            this.function = method;
            this.alias = alias;
        }
        internal FunctionCallExpression(LambdaExpression method, IList<XzaarExpression> args = null)
        {
            arguments = args ?? new List<XzaarExpression>();
            this.lambda = method;
        }

        internal FunctionCallExpression(FunctionCallExpression callChain, IList<XzaarExpression> args = null)
        {
            arguments = args ?? new List<XzaarExpression>();
            this.previousInvocation = callChain;
        }

        internal FunctionCallExpression(string alias, LambdaExpression method, IList<XzaarExpression> args = null)
        {
            arguments = args ?? new List<XzaarExpression>();
            this.lambda = method;
            this.alias = alias;
        }

        internal FunctionCallExpression(string alias, MemberExpression method, IList<XzaarExpression> args = null)
        {
            arguments = args ?? new List<XzaarExpression>();
            this.member = method;
            this.alias = alias;
        }



        public sealed override ExpressionType NodeType => ExpressionType.Call;

        public virtual AnonymousFunctionExpression AnonymousMethod => this.lambda;

        public virtual MemberExpression Member => this.member;

        public virtual FunctionCallExpression PreviousInvocation => this.previousInvocation;

        public virtual XzaarExpression GetInstance()
        {
            return null;
        }



        public override XzaarType Type
        {
            get
            {
                if (method != null)
                {
                    return method.ReturnType;
                }

                if (function != null)
                {
                    return function.ReturnType;
                }

                return XzaarBaseTypes.Any;
            }
        }

        public XzaarExpression Object => GetInstance();

        public string MethodName => alias ?? RefMethodName;

        public string RefMethodName => method != null ? method.Name : function != null ? function.Name : lambda?.AssignmentName;

        public XzaarMethodBase Method => method;

        public ReadOnlyCollection<XzaarExpression> Arguments => GetOrMakeArguments();

        internal virtual ReadOnlyCollection<XzaarExpression> GetOrMakeArguments()
        {
            throw new InvalidOperationException();
        }

        internal virtual FunctionCallExpression Rewrite(XzaarExpression instance, IList<XzaarExpression> args)
        {
            throw new InvalidOperationException();
        }
        public XzaarExpression GetArgument(int index)
        {
            return arguments[index];
        }

        public int ArgumentCount => arguments.Count;
    }


    #region Specialized Subclasses

    internal class FunctionCallExpressionN : FunctionCallExpression, IArgumentProvider
    {
        public FunctionCallExpressionN(XzaarMethodBase method, IList<XzaarExpression> args)
            : base(method, args)
        {
            arguments = args;
        }

        public FunctionCallExpressionN(LambdaExpression method, IList<XzaarExpression> args)
            : base(method, args)
        {
            arguments = args;
        }

        public FunctionCallExpressionN(FunctionCallExpression callChain, IList<XzaarExpression> args)
            : base(callChain, args)
        {
            arguments = args;
        }

        public FunctionCallExpressionN(string alias, LambdaExpression method, IList<XzaarExpression> args)
            : base(alias, method, args)
        {
            arguments = args;
        }

        public FunctionCallExpressionN(string alias, MemberExpression method, IList<XzaarExpression> args)
            : base(alias, method, args)
        {
            arguments = args;
        }

        public FunctionCallExpressionN(FunctionExpression method, IList<XzaarExpression> args) : base(method, args)
        {
            arguments = args;
        }
        public FunctionCallExpressionN(string alias, FunctionExpression method, IList<XzaarExpression> args)
            : base(alias, method, args)
        {
            arguments = args;
        }

        XzaarExpression IArgumentProvider.GetArgument(int index)
        {
            return arguments[index];
        }

        int IArgumentProvider.ArgumentCount => arguments.Count;

        internal override ReadOnlyCollection<XzaarExpression> GetOrMakeArguments()
        {
            return ReturnReadOnly(ref arguments);
        }

        internal override FunctionCallExpression Rewrite(XzaarExpression instance, IList<XzaarExpression> args)
        {
            //Debug.Assert(instance == null);
            //Debug.Assert(args == null || args.Count == _arguments.Count);

            return XzaarExpression.Call(instance, Method, args ?? arguments);
        }
    }

    internal class InstanceFunctionCallExpressionN : FunctionCallExpression, IArgumentProvider
    {
        private readonly XzaarExpression _instance;

        public InstanceFunctionCallExpressionN(XzaarMethodBase method, XzaarExpression instance, IList<XzaarExpression> args)
            : base(method)
        {
            _instance = instance;
            arguments = args;
        }

        public InstanceFunctionCallExpressionN(FunctionExpression method, XzaarExpression instance, IList<XzaarExpression> args)
            : base(method)
        {
            _instance = instance;
            arguments = args;
        }

        XzaarExpression IArgumentProvider.GetArgument(int index)
        {
            return arguments[index];
        }

        int IArgumentProvider.ArgumentCount => arguments.Count;

        public override XzaarExpression GetInstance()
        {
            return _instance;
        }

        internal override ReadOnlyCollection<XzaarExpression> GetOrMakeArguments()
        {
            return ReturnReadOnly(ref arguments);
        }

        internal override FunctionCallExpression Rewrite(XzaarExpression instance, IList<XzaarExpression> args)
        {
            Debug.Assert(instance != null);
            Debug.Assert(args == null || args.Count == arguments.Count);

            return XzaarExpression.Call(instance, Method, args ?? arguments);
        }
    }

    internal class FunctionCallExpression1 : FunctionCallExpression, IArgumentProvider
    {
        private object _arg0;       // storage for the 1st argument or a readonly collection.  See IArgumentProvider

        public FunctionCallExpression1(XzaarMethodBase method, XzaarExpression arg0)
            : base(method)
        {
            _arg0 = arg0;
        }
        public FunctionCallExpression1(FunctionExpression method, XzaarExpression arg0)
            : base(method)
        {
            _arg0 = arg0;
        }
        XzaarExpression IArgumentProvider.GetArgument(int index)
        {
            switch (index)
            {
                case 0: return ReturnObject<XzaarExpression>(_arg0);
                default: throw new InvalidOperationException();
            }
        }

        int IArgumentProvider.ArgumentCount => 1;

        internal override ReadOnlyCollection<XzaarExpression> GetOrMakeArguments()
        {
            return ReturnReadOnly(this, ref _arg0);
        }

        internal override FunctionCallExpression Rewrite(XzaarExpression instance, IList<XzaarExpression> args)
        {
            //Debug.Assert(instance == null);
            //Debug.Assert(args == null || args.Count == 1);

            if (args != null)
            {
                return XzaarExpression.Call(Method, args[0]);
            }

            return XzaarExpression.Call(Method, ReturnObject<XzaarExpression>(_arg0));
        }
    }

    internal class FunctionCallExpression2 : FunctionCallExpression, IArgumentProvider
    {
        private object _arg0;               // storage for the 1st argument or a readonly collection.  See IArgumentProvider
        private readonly XzaarExpression _arg1;  // storage for the 2nd arg

        public FunctionCallExpression2(XzaarMethodBase method, XzaarExpression arg0, XzaarExpression arg1)
            : base(method)
        {
            _arg0 = arg0;
            _arg1 = arg1;
        }

        public FunctionCallExpression2(FunctionExpression method, XzaarExpression arg0, XzaarExpression arg1)
            : base(method)
        {
            _arg0 = arg0;
            _arg1 = arg1;
        }
        XzaarExpression IArgumentProvider.GetArgument(int index)
        {
            switch (index)
            {
                case 0: return ReturnObject<XzaarExpression>(_arg0);
                case 1: return _arg1;
                default: throw new InvalidOperationException();
            }
        }

        int IArgumentProvider.ArgumentCount => 2;

        internal override ReadOnlyCollection<XzaarExpression> GetOrMakeArguments()
        {
            return ReturnReadOnly(this, ref _arg0);
        }

        internal override FunctionCallExpression Rewrite(XzaarExpression instance, IList<XzaarExpression> args)
        {
            //Debug.Assert(instance == null);
            //Debug.Assert(args == null || args.Count == 2);

            if (args != null)
            {
                return XzaarExpression.Call(Method, args[0], args[1]);
            }
            return XzaarExpression.Call(Method, ReturnObject<XzaarExpression>(_arg0), _arg1);
        }
    }

    internal class FunctionCallExpression3 : FunctionCallExpression, IArgumentProvider
    {
        private object _arg0;           // storage for the 1st argument or a readonly collection.  See IArgumentProvider
        private readonly XzaarExpression _arg1, _arg2; // storage for the 2nd - 3rd args.

        public FunctionCallExpression3(XzaarMethodBase method, XzaarExpression arg0, XzaarExpression arg1, XzaarExpression arg2)
            : base(method)
        {
            _arg0 = arg0;
            _arg1 = arg1;
            _arg2 = arg2;
        }


        public FunctionCallExpression3(FunctionExpression method, XzaarExpression arg0, XzaarExpression arg1, XzaarExpression arg2)
            : base(method)
        {
            _arg0 = arg0;
            _arg1 = arg1;
            _arg2 = arg2;
        }

        XzaarExpression IArgumentProvider.GetArgument(int index)
        {
            switch (index)
            {
                case 0: return ReturnObject<XzaarExpression>(_arg0);
                case 1: return _arg1;
                case 2: return _arg2;
                default: throw new InvalidOperationException();
            }
        }

        int IArgumentProvider.ArgumentCount => 3;

        internal override ReadOnlyCollection<XzaarExpression> GetOrMakeArguments()
        {
            return ReturnReadOnly(this, ref _arg0);
        }

        internal override FunctionCallExpression Rewrite(XzaarExpression instance, IList<XzaarExpression> args)
        {
            //Debug.Assert(instance == null);
            //Debug.Assert(args == null || args.Count == 3);

            if (args != null)
            {
                return XzaarExpression.Call(Method, args[0], args[1], args[2]);
            }
            return XzaarExpression.Call(Method, ReturnObject<XzaarExpression>(_arg0), _arg1, _arg2);
        }
    }

    internal class FunctionCallExpression4 : FunctionCallExpression, IArgumentProvider
    {
        private object _arg0;               // storage for the 1st argument or a readonly collection.  See IArgumentProvider
        private readonly XzaarExpression _arg1, _arg2, _arg3;  // storage for the 2nd - 4th args.

        public FunctionCallExpression4(XzaarMethodBase method, XzaarExpression arg0, XzaarExpression arg1, XzaarExpression arg2, XzaarExpression arg3)
            : base(method)
        {
            _arg0 = arg0;
            _arg1 = arg1;
            _arg2 = arg2;
            _arg3 = arg3;
        }
        public FunctionCallExpression4(FunctionExpression method, XzaarExpression arg0, XzaarExpression arg1, XzaarExpression arg2, XzaarExpression arg3)
            : base(method)
        {
            _arg0 = arg0;
            _arg1 = arg1;
            _arg2 = arg2;
            _arg3 = arg3;
        }
        XzaarExpression IArgumentProvider.GetArgument(int index)
        {
            switch (index)
            {
                case 0: return ReturnObject<XzaarExpression>(_arg0);
                case 1: return _arg1;
                case 2: return _arg2;
                case 3: return _arg3;
                default: throw new InvalidOperationException();
            }
        }

        int IArgumentProvider.ArgumentCount => 4;

        internal override ReadOnlyCollection<XzaarExpression> GetOrMakeArguments()
        {
            return ReturnReadOnly(this, ref _arg0);
        }

        internal override FunctionCallExpression Rewrite(XzaarExpression instance, IList<XzaarExpression> args)
        {
            //Debug.Assert(instance == null);
            //Debug.Assert(args == null || args.Count == 4);

            if (args != null)
            {
                return XzaarExpression.Call(Method, args[0], args[1], args[2], args[3]);
            }
            return XzaarExpression.Call(Method, ReturnObject<XzaarExpression>(_arg0), _arg1, _arg2, _arg3);
        }
    }

    internal class FunctionCallExpression5 : FunctionCallExpression, IArgumentProvider
    {
        private object _arg0;           // storage for the 1st argument or a readonly collection.  See IArgumentProvider
        private readonly XzaarExpression _arg1, _arg2, _arg3, _arg4;   // storage for the 2nd - 5th args.

        public FunctionCallExpression5(XzaarMethodBase method, XzaarExpression arg0, XzaarExpression arg1, XzaarExpression arg2, XzaarExpression arg3, XzaarExpression arg4)
            : base(method)
        {
            _arg0 = arg0;
            _arg1 = arg1;
            _arg2 = arg2;
            _arg3 = arg3;
            _arg4 = arg4;
        }
        public FunctionCallExpression5(FunctionExpression method, XzaarExpression arg0, XzaarExpression arg1, XzaarExpression arg2, XzaarExpression arg3, XzaarExpression arg4)
            : base(method)
        {
            _arg0 = arg0;
            _arg1 = arg1;
            _arg2 = arg2;
            _arg3 = arg3;
            _arg4 = arg4;
        }
        XzaarExpression IArgumentProvider.GetArgument(int index)
        {
            switch (index)
            {
                case 0: return ReturnObject<XzaarExpression>(_arg0);
                case 1: return _arg1;
                case 2: return _arg2;
                case 3: return _arg3;
                case 4: return _arg4;
                default: throw new InvalidOperationException();
            }
        }

        int IArgumentProvider.ArgumentCount => 5;

        internal override ReadOnlyCollection<XzaarExpression> GetOrMakeArguments()
        {
            return ReturnReadOnly(this, ref _arg0);
        }

        internal override FunctionCallExpression Rewrite(XzaarExpression instance, IList<XzaarExpression> args)
        {
            //Debug.Assert(instance == null);
            //Debug.Assert(args == null || args.Count == 5);

            if (args != null)
            {
                return XzaarExpression.Call(Method, args[0], args[1], args[2], args[3], args[4]);
            }

            return XzaarExpression.Call(Method, ReturnObject<XzaarExpression>(_arg0), _arg1, _arg2, _arg3, _arg4);
        }
    }

    internal class InstanceFunctionCallExpression2 : FunctionCallExpression, IArgumentProvider
    {
        private readonly XzaarExpression _instance;
        private object _arg0;                // storage for the 1st argument or a readonly collection.  See IArgumentProvider
        private readonly XzaarExpression _arg1;   // storage for the 2nd argument

        public InstanceFunctionCallExpression2(XzaarMethodBase method, XzaarExpression instance, XzaarExpression arg0, XzaarExpression arg1)
            : base(method)
        {
            //Debug.Assert(instance != null);

            _instance = instance;
            _arg0 = arg0;
            _arg1 = arg1;
        }
        public InstanceFunctionCallExpression2(FunctionExpression method, XzaarExpression instance, XzaarExpression arg0, XzaarExpression arg1)
            : base(method)
        {
            //Debug.Assert(instance != null);

            _instance = instance;
            _arg0 = arg0;
            _arg1 = arg1;
        }
        XzaarExpression IArgumentProvider.GetArgument(int index)
        {
            switch (index)
            {
                case 0: return ReturnObject<XzaarExpression>(_arg0);
                case 1: return _arg1;
                default: throw new InvalidOperationException();
            }
        }

        int IArgumentProvider.ArgumentCount => 2;

        public override XzaarExpression GetInstance()
        {
            return _instance;
        }

        internal override ReadOnlyCollection<XzaarExpression> GetOrMakeArguments()
        {
            return ReturnReadOnly(this, ref _arg0);
        }

        internal override FunctionCallExpression Rewrite(XzaarExpression instance, IList<XzaarExpression> args)
        {
            //Debug.Assert(instance != null);
            //Debug.Assert(args == null || args.Count == 2);

            if (args != null)
            {
                return XzaarExpression.Call(instance, Method, args[0], args[1]);
            }
            return XzaarExpression.Call(instance, Method, ReturnObject<XzaarExpression>(_arg0), _arg1);
        }
    }

    internal class InstanceFunctionCallExpression3 : FunctionCallExpression, IArgumentProvider
    {
        private readonly XzaarExpression _instance;
        private object _arg0;                       // storage for the 1st argument or a readonly collection.  See IArgumentProvider
        private readonly XzaarExpression _arg1, _arg2;   // storage for the 2nd - 3rd argument

        public InstanceFunctionCallExpression3(XzaarMethodBase method, XzaarExpression instance, XzaarExpression arg0, XzaarExpression arg1, XzaarExpression arg2)
            : base(method)
        {
            // Debug.Assert(instance != null);

            _instance = instance;
            _arg0 = arg0;
            _arg1 = arg1;
            _arg2 = arg2;
        }
        public InstanceFunctionCallExpression3(FunctionExpression method, XzaarExpression instance, XzaarExpression arg0, XzaarExpression arg1, XzaarExpression arg2)
            : base(method)
        {
            // Debug.Assert(instance != null);

            _instance = instance;
            _arg0 = arg0;
            _arg1 = arg1;
            _arg2 = arg2;
        }
        XzaarExpression IArgumentProvider.GetArgument(int index)
        {
            switch (index)
            {
                case 0: return ReturnObject<XzaarExpression>(_arg0);
                case 1: return _arg1;
                case 2: return _arg2;
                default: throw new InvalidOperationException();
            }
        }

        int IArgumentProvider.ArgumentCount => 3;

        public override XzaarExpression GetInstance()
        {
            return _instance;
        }

        internal override ReadOnlyCollection<XzaarExpression> GetOrMakeArguments()
        {
            return ReturnReadOnly(this, ref _arg0);
        }

        internal override FunctionCallExpression Rewrite(XzaarExpression instance, IList<XzaarExpression> args)
        {
            //Debug.Assert(instance != null);
            //Debug.Assert(args == null || args.Count == 3);

            if (args != null)
            {
                return XzaarExpression.Call(instance, Method, args[0], args[1], args[2]);
            }
            return XzaarExpression.Call(instance, Method, ReturnObject<XzaarExpression>(_arg0), _arg1, _arg2);
        }
    }

    #endregion

    public partial class XzaarExpression
    {
        internal static T ReturnObject<T>(object collectionOrT) where T : class
        {
            T t = collectionOrT as T;
            if (t != null)
            {
                return t;
            }

            return ((ReadOnlyCollection<T>)collectionOrT)[0];
        }
        internal static ReadOnlyCollection<T> ReturnReadOnly<T>(ref IList<T> collection)
        {
            IList<T> value = collection;

            // if it's already read-only just return it.
            ReadOnlyCollection<T> res = value as ReadOnlyCollection<T>;
            if (res != null)
            {
                return res;
            }

            // otherwise make sure only readonly collection every gets exposed
            Interlocked.CompareExchange<IList<T>>(
                ref collection,
                new ReadOnlyCollection<T>(value),
                value
            );

            // and return it
            return (ReadOnlyCollection<T>)collection;
        }

        internal static ReadOnlyCollection<XzaarExpression> ReturnReadOnly(IArgumentProvider provider, ref object collection)
        {
            XzaarExpression tObj = collection as XzaarExpression;
            if (tObj != null)
            {
                // otherwise make sure only one readonly collection ever gets exposed
                Interlocked.CompareExchange(
                    ref collection,
                    new ReadOnlyCollection<XzaarExpression>(new ListArgumentProvider(provider, tObj)),
                    tObj
                );
            }

            // and return what is not guaranteed to be a readonly collection
            return (ReadOnlyCollection<XzaarExpression>)collection;
        }
        public static FunctionCallExpression Call(XzaarExpression instance, FunctionExpression method, params XzaarExpression[] arguments)
        {

            if (method == null) throw new ArgumentNullException(nameof(method));

            ReadOnlyCollection<XzaarExpression> argList = new ReadOnlyCollection<XzaarExpression>(arguments);

            ValidateMethodInfo(method);
            ValidateArgumentTypes(method, ExpressionType.Call, ref argList);
            if (instance != null)
            {
                return new InstanceFunctionCallExpressionN(method, instance, argList);
            }
            return new FunctionCallExpressionN(method, argList);
        }

        public static FunctionCallExpression Call(LambdaExpression method, params XzaarExpression[] arguments)
        {
            //if (method == null) throw new ArgumentNullException(nameof(method));
            // method can be null, if type is "any" since this will be a runtime error instead.

            ReadOnlyCollection<XzaarExpression> argList = new ReadOnlyCollection<XzaarExpression>(arguments);

            return new FunctionCallExpressionN(method, argList);
        }

        public static FunctionCallExpression Call(FunctionCallExpression callChain, params XzaarExpression[] arguments)
        {
            //if (method == null) throw new ArgumentNullException(nameof(method));
            // method can be null, if type is "any" since this will be a runtime error instead.

            ReadOnlyCollection<XzaarExpression> argList = new ReadOnlyCollection<XzaarExpression>(arguments);

            return new FunctionCallExpressionN(callChain, argList);
        }

        public static FunctionCallExpression Call(string alias, LambdaExpression method, params XzaarExpression[] arguments)
        {
            //if (method == null) throw new ArgumentNullException(nameof(method));
            // method can be null, if type is "any" since this will be a runtime error instead.

            ReadOnlyCollection<XzaarExpression> argList = new ReadOnlyCollection<XzaarExpression>(arguments);

            return new FunctionCallExpressionN(alias, method, argList);
        }

        public static FunctionCallExpression Call(string alias, MemberExpression method, params XzaarExpression[] arguments)
        {
            //if (method == null) throw new ArgumentNullException(nameof(method));
            // method can be null, if type is "any" since this will be a runtime error instead.

            ReadOnlyCollection<XzaarExpression> argList = new ReadOnlyCollection<XzaarExpression>(arguments);

            return new FunctionCallExpressionN(alias, method, argList);
        }

        // functionAlias

        public static FunctionCallExpression Call(FunctionExpression method, params XzaarExpression[] arguments)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));

            ReadOnlyCollection<XzaarExpression> argList = new ReadOnlyCollection<XzaarExpression>(arguments);

            ValidateMethodInfo(method);
            ValidateArgumentTypes(method, ExpressionType.Call, ref argList);

            return new FunctionCallExpressionN(method, argList);
        }
        public static FunctionCallExpression Call(string alias, FunctionExpression method, params XzaarExpression[] arguments)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));

            ReadOnlyCollection<XzaarExpression> argList = new ReadOnlyCollection<XzaarExpression>(arguments);

            ValidateMethodInfo(method);
            ValidateArgumentTypes(method, ExpressionType.Call, ref argList);

            return new FunctionCallExpressionN(alias, method, argList);
        }

        public static FunctionCallExpression Call(XzaarMethodBase method, params XzaarExpression[] arguments)
        {
            return Call(null, method, arguments);
        }

        public static FunctionCallExpression Call(XzaarMethodBase method, IEnumerable<XzaarExpression> arguments)
        {
            return Call(null, method, arguments);
        }

        public static FunctionCallExpression Call(XzaarExpression instance, XzaarMethodBase method)
        {
            return Call(instance, method, EmptyReadOnlyCollection<XzaarExpression>.Instance);
        }

        public static FunctionCallExpression Call(XzaarExpression instance, XzaarMethodBase method, params XzaarExpression[] arguments)
        {
            return Call(instance, method, (IEnumerable<XzaarExpression>)arguments);
        }

        public static FunctionCallExpression Call(XzaarExpression instance, XzaarMethodBase method, XzaarExpression arg0, XzaarExpression arg1)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (arg0 == null) throw new ArgumentNullException(nameof(arg0));
            if (arg1 == null) throw new ArgumentNullException(nameof(arg1));


            XzaarParameterInfo[] pis = ValidateMethodAndGetParameters(instance, method);

            ValidateArgumentCount(method, ExpressionType.Call, 2, pis);

            arg0 = ValidateOneArgument(method, ExpressionType.Call, arg0, pis[0]);
            arg1 = ValidateOneArgument(method, ExpressionType.Call, arg1, pis[1]);

            if (instance != null)
            {
                return new InstanceFunctionCallExpression2(method, instance, arg0, arg1);
            }

            return new FunctionCallExpression2(method, arg0, arg1);
        }

        private static FunctionCallExpression Call(XzaarExpression instance, XzaarMethodBase method, XzaarExpression arg0, XzaarExpression arg1, XzaarExpression arg2)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));
            if (arg0 == null) throw new ArgumentNullException(nameof(arg0));
            if (arg1 == null) throw new ArgumentNullException(nameof(arg1));
            if (arg2 == null) throw new ArgumentNullException(nameof(arg2));

            XzaarParameterInfo[] pis = ValidateMethodAndGetParameters(instance, method);

            ValidateArgumentCount(method, ExpressionType.Call, 3, pis);

            arg0 = ValidateOneArgument(method, ExpressionType.Call, arg0, pis[0]);
            arg1 = ValidateOneArgument(method, ExpressionType.Call, arg1, pis[1]);
            arg2 = ValidateOneArgument(method, ExpressionType.Call, arg2, pis[2]);

            if (instance != null)
            {
                return new InstanceFunctionCallExpression3(method, instance, arg0, arg1, arg2);
            }
            return new FunctionCallExpression3(method, arg0, arg1, arg2);
        }

        public static FunctionCallExpression Call(XzaarExpression instance, string methodName, XzaarType[] typeArguments, params XzaarExpression[] arguments)
        {
            if (instance == null) throw new ArgumentNullException(nameof(instance));
            if (methodName == null) throw new ArgumentNullException(nameof(methodName));
            if (arguments == null) arguments = new XzaarExpression[0];
            return XzaarExpression.Call(instance, FindMethod(instance.Type, methodName, typeArguments, arguments), arguments);
        }
        public static FunctionCallExpression Call(XzaarType type, string methodName, XzaarType[] typeArguments, params XzaarExpression[] arguments)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (methodName == null) throw new ArgumentNullException(nameof(methodName));
            if (arguments == null) arguments = new XzaarExpression[] { };
            return XzaarExpression.Call(null, FindMethod(type, methodName, typeArguments, arguments), arguments);
        }

        public static FunctionCallExpression Call(XzaarExpression instance, XzaarMethodBase method, IEnumerable<XzaarExpression> arguments)
        {
            if (method == null) throw new ArgumentNullException(nameof(method));

            var argList = new ReadOnlyCollection<XzaarExpression>(arguments.ToList());

            ValidateMethodInfo(method);
            ValidateStaticOrInstanceMethod(instance, method);
            ValidateArgumentTypes(method, ExpressionType.Call, ref argList);

            if (instance == null)
            {
                return new FunctionCallExpressionN(method, argList);
            }
            else
            {
                return new InstanceFunctionCallExpressionN(method, instance, argList);
            }
        }

        private static void ValidateMethodInfo(XzaarMethodBase method)
        {
            //if (method.IsGenericMethodDefinition)
            //    throw Error.MethodIsGeneric(method);
            //if (method.ContainsGenericParameters)
            //    throw Error.MethodContainsGenericParameters(method);
        }

        private static void ValidateMethodInfo(FunctionExpression method)
        {
            //if (method.IsGenericMethodDefinition)
            //    throw Error.MethodIsGeneric(method);
            //if (method.ContainsGenericParameters)
            //    throw Error.MethodContainsGenericParameters(method);
        }

        private static XzaarParameterInfo[] ValidateMethodAndGetParameters(XzaarExpression instance, XzaarMethodBase method)
        {
            ValidateMethodInfo(method);
            ValidateStaticOrInstanceMethod(instance, method);

            return GetParametersForValidation(method, ExpressionType.Call);
        }

        private static void ValidateStaticOrInstanceMethod(XzaarExpression instance, XzaarMethodBase method)
        {
            if (method.IsGlobal)
            {
                if (instance != null) throw new InvalidOperationException(); // throw new ArgumentException(Strings.OnlyStaticMethodsHaveNullInstance, "instance");
            }
            else
            {
                if (instance == null) throw new InvalidOperationException(); // throw new ArgumentException(Strings.OnlyStaticMethodsHaveNullInstance, "method");
                RequiresCanRead(instance, "instance");
                ValidateCallInstanceType(instance.Type, method);
            }
        }

        private static void ValidateCallInstanceType(XzaarType instanceType, XzaarMethodBase method)
        {
            //if (!XzaarTypeUtils.IsValidInstanceType(method, instanceType))
            //{
            //    throw Error.InstanceAndMethodTypeMismatch(method, method.DeclaringType, instanceType);
            //}
        }

        private static void ValidateArgumentTypes(FunctionExpression method, ExpressionType nodeKind, ref ReadOnlyCollection<XzaarExpression> arguments)
        {
            // Debug.Assert(nodeKind == ExpressionType.Invoke || nodeKind == ExpressionType.Call || nodeKind == ExpressionType.Dynamic || nodeKind == ExpressionType.New);

            ParameterExpression[] pis = GetParametersForValidation(method, nodeKind);

            ValidateArgumentCount(method, nodeKind, arguments.Count, pis);

            XzaarExpression[] newArgs = null;
            for (int i = 0, n = pis.Length; i < n; i++)
            {
                XzaarExpression arg = arguments[i];
                ParameterExpression pi = pis[i];
                arg = ValidateOneArgument(method, nodeKind, arg, pi);

                if (newArgs == null && arg != arguments[i])
                {
                    newArgs = new XzaarExpression[arguments.Count];
                    for (int j = 0; j < i; j++)
                    {
                        newArgs[j] = arguments[j];
                    }
                }
                if (newArgs != null)
                {
                    newArgs[i] = arg;
                }
            }
            if (newArgs != null)
            {
                arguments = new TrueReadOnlyCollection<XzaarExpression>(newArgs);
            }
        }

        private static void ValidateArgumentTypes(XzaarMethodBase method, ExpressionType nodeKind, ref ReadOnlyCollection<XzaarExpression> arguments)
        {
            // Debug.Assert(nodeKind == ExpressionType.Invoke || nodeKind == ExpressionType.Call || nodeKind == ExpressionType.Dynamic || nodeKind == ExpressionType.New);

            XzaarParameterInfo[] pis = GetParametersForValidation(method, nodeKind);

            ValidateArgumentCount(method, nodeKind, arguments.Count, pis);

            XzaarExpression[] newArgs = null;
            for (int i = 0, n = pis.Length; i < n; i++)
            {
                XzaarExpression arg = arguments[i];
                XzaarParameterInfo pi = pis[i];
                arg = ValidateOneArgument(method, nodeKind, arg, pi);

                if (newArgs == null && arg != arguments[i])
                {
                    newArgs = new XzaarExpression[arguments.Count];
                    for (int j = 0; j < i; j++)
                    {
                        newArgs[j] = arguments[j];
                    }
                }
                if (newArgs != null)
                {
                    newArgs[i] = arg;
                }
            }
            if (newArgs != null)
            {
                arguments = new TrueReadOnlyCollection<XzaarExpression>(newArgs);
            }
        }

        private static ParameterExpression[] GetParametersForValidation(FunctionExpression method, ExpressionType nodeKind)
        {
            if (nodeKind != ExpressionType.Dynamic) return method.Parameters;
            var methodParam = method.Parameters;
            var param = new ParameterExpression[methodParam.Length - 1];
            Array.Copy(methodParam, 1, param, 0, param.Length);
            return param;
        }


        private static XzaarParameterInfo[] GetParametersForValidation(XzaarMethodBase method, ExpressionType nodeKind)
        {
            if (nodeKind != ExpressionType.Dynamic) return method.GetParameters();
            var methodParam = method.GetParameters();
            var param = new XzaarParameterInfo[methodParam.Length - 1];
            Array.Copy(methodParam, 1, param, 0, param.Length);
            return param;
        }

        private static void ValidateArgumentCount(FunctionExpression method, ExpressionType nodeKind, int count, ParameterExpression[] pis)
        {
            if (pis.Length != count && method.Parameters.Length != pis.Length)
            {
                // Throw the right error for the node we were given
                switch (nodeKind)
                {
                    case ExpressionType.New:
                        throw new InvalidOperationException();
                    // throw Error.IncorrectNumberOfConstructorArguments();
                    case ExpressionType.Invoke:
                        throw new InvalidOperationException();
                    // throw Error.IncorrectNumberOfLambdaArguments();
                    case ExpressionType.Dynamic:
                    case ExpressionType.Call:
                        throw new InvalidOperationException();
                    // throw Error.IncorrectNumberOfMethodCallArguments(method);
                    default:
                        throw new InvalidOperationException();
                        // throw ContractUtils.Unreachable;
                }
            }
        }

        private static void ValidateArgumentCount(XzaarMethodBase method, ExpressionType nodeKind, int count, XzaarParameterInfo[] pis)
        {
            if (pis.Length != count)
            {
                // Throw the right error for the node we were given
                switch (nodeKind)
                {
                    case ExpressionType.New:
                        throw new InvalidOperationException();
                    // throw Error.IncorrectNumberOfConstructorArguments();
                    case ExpressionType.Invoke:
                        throw new InvalidOperationException();
                    // throw Error.IncorrectNumberOfLambdaArguments();
                    case ExpressionType.Dynamic:
                    case ExpressionType.Call:
                        throw new InvalidOperationException();
                    // throw Error.IncorrectNumberOfMethodCallArguments(method);
                    default:
                        throw new InvalidOperationException();
                        // throw ContractUtils.Unreachable;
                }
            }
        }
        private static XzaarExpression ValidateOneArgument(FunctionExpression method, ExpressionType nodeKind, XzaarExpression arg, ParameterExpression pi)
        {
            if (pi == null)
            {
                // this is most likely an 'Any' object
                return arg;
            }

            RequiresCanRead(arg, "arguments");

            var pType = pi.Type.IsByRef ? pi.Type.GetElementType() : pi.Type;

            // XzaarTypeUtils.ValidateType(pType);
            if (!XzaarTypeUtils.AreReferenceAssignable(pType, arg.Type))
            {
                if (TryQuote(pType, ref arg)) return arg;

                // Throw the right error for the node we were given
                switch (nodeKind)
                {
                    case ExpressionType.New:
                        throw new InvalidOperationException();
                    // throw Error.ExpressionTypeDoesNotMatchConstructorParameter(arg.Type, pType);
                    case ExpressionType.Invoke:
                        throw new InvalidOperationException();
                    // throw Error.ExpressionTypeDoesNotMatchParameter(arg.Type, pType);
                    case ExpressionType.Dynamic:
                    case ExpressionType.Call:
                        throw new ExpressionException("Expression type does not match method parameter: '" + arg.Type.Name + "' and '" + pType.Name + "'");
                    // throw Error.ExpressionTypeDoesNotMatchMethodParameter(arg.Type, pType, method);
                    default:
                        throw new InvalidOperationException();
                        // throw ContractUtils.Unreachable;
                }
            }
            return arg;
        }
        private static XzaarExpression ValidateOneArgument(XzaarMethodBase method, ExpressionType nodeKind, XzaarExpression arg, XzaarParameterInfo pi)
        {
            // this is most likely an 'Any' object
            if (pi == null) return arg;

            RequiresCanRead(arg, "arguments");

            var pType = pi.ParameterType;
            if (pType.IsByRef) pType = pType.GetElementType();
            // XzaarTypeUtils.ValidateType(pType);
            if (!XzaarTypeUtils.AreReferenceAssignable(pType, arg.Type))
            {
                if (!TryQuote(pType, ref arg))
                {
                    // Throw the right error for the node we were given
                    switch (nodeKind)
                    {
                        case ExpressionType.New:
                            throw new InvalidOperationException();
                        // throw Error.ExpressionTypeDoesNotMatchConstructorParameter(arg.Type, pType);
                        case ExpressionType.Invoke:
                            throw new InvalidOperationException();
                        // throw Error.ExpressionTypeDoesNotMatchParameter(arg.Type, pType);
                        case ExpressionType.Dynamic:
                        case ExpressionType.Call:
                            throw new InvalidOperationException();
                        // throw Error.ExpressionTypeDoesNotMatchMethodParameter(arg.Type, pType, method);
                        default:
                            throw new InvalidOperationException();
                            // throw ContractUtils.Unreachable;
                    }
                }
            }
            return arg;
        }

        // Attempts to auto-quote the expression tree. Returns true if it succeeded, false otherwise.
        private static bool TryQuote(XzaarType parameterType, ref XzaarExpression argument)
        {
            //XzaarType quoteable = typeof(LambdaExpression);

            //if (XzaarTypeUtils.IsSameOrSubclass(quoteable, parameterType) &&
            //    parameterType.IsAssignableFrom(argument.GetType()))
            //{
            //    argument = XzaarExpression.Quote(argument);
            //    return true;
            //}
            return false;
        }

        private static XzaarMethodBase FindMethod(XzaarType type, string methodName, XzaarType[] typeArgs, XzaarExpression[] args)
        {
            XzaarMemberInfo[] members = type.GetMethods();
            if (members == null || members.Length == 0)
                // throw Error.MethodDoesNotExistOnType(methodName, type);
                throw new InvalidOperationException();

            XzaarMethodBase method;

            var methodInfos = members.Select(t => (XzaarMethodBase)t);
            var count = FindBestMethod(methodInfos, typeArgs, args, out method);
            if (count == 1) return method;

            throw new InvalidOperationException();
        }

        private static int FindBestMethod(IEnumerable<XzaarMethodBase> methods, XzaarType[] typeArgs, XzaarExpression[] args, out XzaarMethodBase method)
        {
            var count = 0;
            method = null;
            foreach (var methodInfo in methods)
            {
                var targetMethod = ApplyTypeArgs(methodInfo, typeArgs);
                if (targetMethod != null && IsCompatible(targetMethod, args))
                {
                    // favor public over non-public methods
                    if (method == null)
                    {
                        method = targetMethod;
                        count = 1;
                    }
                    // only count it as additional method if they both public or both non-public
                    else
                    {
                        count++;
                    }
                }
            }
            return count;
        }

        private static bool IsCompatible(XzaarMethodBase m, XzaarExpression[] args)
        {
            var parms = m.GetParameters();
            if (parms.Length != args.Length) return false;
            for (var i = 0; i < args.Length; i++)
            {
                var arg = args[i];
                //ContractUtils.RequiresNotNull(arg, "argument");
                var argType = arg.Type;
                var pType = parms[i].ParameterType;
                if (pType.IsByRef) pType = pType.GetElementType();
                if (!XzaarTypeUtils.AreReferenceAssignable(pType, argType)) // &&  !(XzaarTypeUtils.IsSameOrSubclass(typeof(LambdaExpression), pType) && pType.IsAssignableFrom(arg.GetType()))
                {
                    return false;
                }
            }
            return true;
        }

        private static XzaarMethodBase ApplyTypeArgs(XzaarMethodBase m, XzaarType[] typeArgs)
        {
            if (typeArgs == null || typeArgs.Length == 0)
            {
                return m;
            }
            return null;
        }

        public static FunctionCallExpression ArrayIndex(XzaarExpression array, params XzaarExpression[] indexes)
        {
            return ArrayIndex(array, (IEnumerable<XzaarExpression>)indexes);
        }

        public static FunctionCallExpression ArrayIndex(XzaarExpression array, IEnumerable<XzaarExpression> indexes)
        {
            // RequiresCanRead(array, "array");
            // ContractUtils.RequiresNotNull(indexes, "indexes");

            XzaarType arrayType = array.Type;
            if (!arrayType.IsArray)
            {
                // throw Error.ArgumentMustBeArray();
                throw new InvalidOperationException();
            }

            ReadOnlyCollection<XzaarExpression> indexList = new ReadOnlyCollection<XzaarExpression>(indexes.ToList());
            // totally ignored
            //if (arrayType.GetArrayRank() != indexList.Count)
            //{
            //    //throw Error.IncorrectNumberOfIndexes();
            //    throw new InvalidOperationException();
            //}

            foreach (XzaarExpression e in indexList)
            {
                RequiresCanRead(e, "indexes");
                if (e.Type != (XzaarType)typeof(int))
                {
                    // throw Error.ArgumentMustBeArrayIndexType();
                    throw new InvalidOperationException();
                }
            }

            XzaarMethodBase mi = array.Type.GetMethod("Get");

            return Call(array, mi, indexList);
        }
    }
}