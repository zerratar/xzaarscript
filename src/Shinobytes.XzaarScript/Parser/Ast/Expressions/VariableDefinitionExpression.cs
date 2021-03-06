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
 
namespace Shinobytes.XzaarScript.Parser.Ast.Expressions
{
    public class VariableDefinitionExpression : ParameterExpression
    {
        private XzaarType type;

        internal VariableDefinitionExpression(XzaarType type, string name, XzaarExpression assignmentExpression)
            : base(name)
        {
            this.type = type;
            AssignmentExpression = assignmentExpression;
        }

        public override XzaarType Type => type;

        public XzaarExpression AssignmentExpression { get; }

        public override AnonymousFunctionExpression FunctionReference
        {
            get
            {
                if (base.FunctionReference != null)
                {
                    return base.FunctionReference;
                }

                if (AssignmentExpression is ParameterExpression param)
                {
                    return param.FunctionReference;
                }

                return null;
            }
        }

        public override bool IsFunctionReference
        {
            get
            {
                if (base.IsFunctionReference)
                {
                    return true;
                }

                if (AssignmentExpression is ParameterExpression param)
                {
                    return param.IsFunctionReference;
                }

                return false;
            }
        }

        //public bool IsFunctionReference
        //{
        //    get
        //    {
        //        if (AssignmentExpression is ParameterExpression param)
        //        {
        //            return param.IsFunctionReference;
        //        }

        //        return false;
        //    }
        //}
    }

    public partial class XzaarExpression
    {
        public static VariableDefinitionExpression DefineVariable(XzaarType type, string name)
        {
            return DefineVariable(type, name, null);
        }

        public static VariableDefinitionExpression DefineVariable(XzaarType type, string name, XzaarExpression assignmentExpression)
        {
            return new VariableDefinitionExpression(type, name, assignmentExpression);
        }
    }
}