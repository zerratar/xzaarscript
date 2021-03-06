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

namespace Shinobytes.XzaarScript.Parser.Ast.Expressions
{
    public class ConditionalExpression : XzaarExpression
    {
        private XzaarExpression test;
        private XzaarExpression whenTrue;
        private XzaarExpression whenFalse;
        private readonly XzaarType type;

        public ConditionalExpression(XzaarExpression test, XzaarExpression whenTrue, XzaarExpression whenFalse, XzaarType type)
        {
            this.test = test;
            this.whenTrue = whenTrue;
            this.whenFalse = whenFalse;
            this.type = type;
        }

        public sealed override ExpressionType NodeType => ExpressionType.Conditional;

        public override XzaarType Type => type ?? WhenTrue.Type;

        public XzaarExpression WhenTrue => this.whenTrue;

        public XzaarExpression Test => test;

        public XzaarExpression WhenFalse => this.whenFalse;
    }

    public class IfElseExpression : XzaarExpression
    {
        private XzaarExpression test;
        private XzaarExpression ifTrue;

        internal IfElseExpression(XzaarExpression test, XzaarExpression ifTrue)
        {
            this.test = test;
            this.ifTrue = ifTrue;
        }

        internal static IfElseExpression Make(XzaarExpression test, XzaarExpression ifTrue, XzaarExpression ifFalse, XzaarType type)
        {
            if (ifTrue.Type != type || ifFalse.Type != type)
            {
                return new FullIfElseExpressionWithType(test, ifTrue, ifFalse, type);
            }
            if (ifFalse is DefaultExpression && ifFalse.Type == XzaarBaseTypes.Void)
            {
                return new IfElseExpression(test, ifTrue);
            }
            else
            {
                return new FullIfElseExpression(test, ifTrue, ifFalse);
            }
        }

        public sealed override ExpressionType NodeType => ExpressionType.Conditional;

        public override XzaarType Type => IfTrue.Type;

        public XzaarExpression IfTrue => this.ifTrue;

        public XzaarExpression Test => test;

        public XzaarExpression IfFalse => GetFalse();

        internal virtual XzaarExpression GetFalse()
        {
            return XzaarExpression.Empty();
        }

        public IfElseExpression Update(XzaarExpression test, XzaarExpression ifTrue, XzaarExpression ifFalse)
        {
            if (test == Test && ifTrue == IfTrue && ifFalse == IfFalse)
            {
                return this;
            }
            return XzaarExpression.IfElse(test, ifTrue, ifFalse, Type);
        }
    }

    internal class FullIfElseExpression : IfElseExpression
    {
        private readonly XzaarExpression _false;

        internal FullIfElseExpression(XzaarExpression test, XzaarExpression ifTrue, XzaarExpression ifFalse)
            : base(test, ifTrue)
        {
            _false = ifFalse;
        }

        internal override XzaarExpression GetFalse()
        {
            return _false;
        }
    }
    internal class FullIfElseExpressionWithType : FullIfElseExpression
    {
        private readonly XzaarType _type;

        internal FullIfElseExpressionWithType(XzaarExpression test, XzaarExpression ifTrue, XzaarExpression ifFalse, XzaarType type)
            : base(test, ifTrue, ifFalse)
        {
            _type = type;
        }

        public sealed override XzaarType Type => _type;
    }
    public partial class XzaarExpression
    {
        public static ConditionalExpression Conditional(XzaarExpression test, XzaarExpression whenTrue, XzaarExpression whenFalse)
        {
            return new ConditionalExpression(test, whenTrue, whenFalse, null);
        }

        public static ConditionalExpression Conditional(XzaarExpression test, XzaarExpression whenTrue, XzaarExpression whenFalse, XzaarType type)
        {
            return new ConditionalExpression(test, whenTrue, whenFalse, type);
        }

        public static IfElseExpression IfElse(XzaarExpression test, XzaarExpression ifTrue, XzaarExpression ifFalse)
        {
            RequiresCanRead(test, "test");
            RequiresCanRead(ifTrue, "ifTrue");
            RequiresCanRead(ifFalse, "ifFalse");

            if (test.Type != XzaarBaseTypes.Boolean && test.Type != XzaarBaseTypes.Any)
            {
                throw new InvalidOperationException("Argument must be boolean");
            }


            // only true for the '??' operator, but we don't have that in XzaarScript (yet)

            //if (!XzaarTypeUtils.AreEquivalent(ifTrue.Type, ifFalse.Type))
            //{
            //    // throw Error.ArgumentTypesMustMatch();
            //    throw new InvalidOperationException("Argument types must match");
            //}

            return IfElseExpression.Make(test, ifTrue, ifFalse, ifTrue.Type);
        }
        public static IfElseExpression IfElse(XzaarExpression test, XzaarExpression ifTrue, XzaarExpression ifFalse, XzaarType type)
        {
            RequiresCanRead(test, "test");
            RequiresCanRead(ifTrue, "ifTrue");
            RequiresCanRead(ifFalse, "ifFalse");

            if (test.Type != XzaarBaseTypes.Boolean)
            {
                // throw Error.ArgumentMustBeBoolean();
                throw new InvalidOperationException("Argument must be boolean");
            }

            if (type != XzaarBaseTypes.Void)
            {
                if (!XzaarTypeUtils.AreReferenceAssignable(type, ifTrue.Type) ||
                    !XzaarTypeUtils.AreReferenceAssignable(type, ifFalse.Type))
                {
                    throw new InvalidOperationException("Argument types must match");
                }
            }

            return IfElseExpression.Make(test, ifTrue, ifFalse, type);
        }

        public static IfElseExpression IfThen(XzaarExpression test, XzaarExpression ifTrue)
        {
            return IfElse(test, ifTrue, XzaarExpression.Empty(), XzaarBaseTypes.Void);
        }

        public static IfElseExpression IfThenElse(XzaarExpression test, XzaarExpression ifTrue, XzaarExpression ifFalse)
        {
            return IfElse(test, ifTrue, ifFalse, XzaarBaseTypes.Void);
        }
    }
}