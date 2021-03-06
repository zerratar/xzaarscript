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
 
using System.Linq;

namespace Shinobytes.XzaarScript.Assembly
{
    public partial class Instruction : Operation
    {
        internal Instruction()
        {
            this.Arguments = new InstructionVariableCollection();
            this.OperandArguments = new InstructionVariableCollection();
        }

        internal Instruction(int offset, OpCode opcode) : this()
        {
            this.OpCode = opcode;
            this.Offset = offset;
        }

        internal Instruction(OpCode opcode, Label targetLabel) : this(0, opcode)
        {
            TargetLabel = targetLabel;
        }

        internal Instruction(OpCode opcode, MemberReference comparison, Label targetLabel) : this(0, opcode)
        {
            TargetLabel = targetLabel;
            Arguments.Add(comparison);
        }

        internal Instruction(OpCode opcode, MemberReference arg) : this(0, opcode)
        {
            Arguments.Add(arg);
        }
        internal Instruction(OpCode opcode, VariableReference targetVariable, MemberReference arg) : this(0, opcode)
        {
            Arguments.Add(targetVariable);
            Arguments.Add(arg);
        }


        internal Instruction(OpCode opcode, MemberReference[] args) : this(0, opcode)
        {
            foreach (var arg in args)
                Arguments.Add(arg);
            // operand arguments only used for function calls
        }

        public bool IsJump => this.OpCode == OpCode.Jmp || this.OpCode == OpCode.Jmpf || this.OpCode == OpCode.Jmpt;

        public OpCode OpCode { get; internal set; }

        public Label TargetLabel { get; }

        public InstructionVariableCollection Arguments { get; }
        public InstructionVariableCollection OperandArguments { get; }

        public Instruction Previous { get; internal set; }
        public Instruction Next { get; internal set; }
        public MethodDefinition Method { get; internal set; }

        public override string ToString()
        {
            return (this.OpCode + " " + this.Arguments.ToString(", ") + " " +
                    this.OperandArguments.ToString(", ")).Trim();
        }
    }

    //public class InstructionProcessor
    //{
    //    private readonly MethodBody body;

    //    private InstructionCollection instructionCollection;

    //    public InstructionProcessor(MethodBody body)
    //    {
    //        this.body = body;
    //    }

    //    public MethodBody Body
    //    {
    //        get { return body; }
    //    }


    //}

    public partial class Instruction
    {
        public static Label Label(string name)
        {
            return new Label(name);
        }

        public static Label Label()
        {
            return new Label();
        }

        public static Instruction Create(OpCode opcode)
        {
            return new Instruction(opcode, new MemberReference[0]);
        }

        public static Instruction Create(OpCode opcode, TypeReference type)
        {
            return new Instruction(opcode, type);
        }

        public static Instruction Create(OpCode opcode, VariableDefinition item)
        {
            return new Instruction(opcode, item);
        }

        public static Instruction Create(OpCode opcode, VariableReference variable, TypeReference targetType)
        {
            return new Instruction(opcode, variable, targetType);
        }

        public static Instruction Create(OpCode opcode, VariableReference comparison, Label targetLabel)
        {
            return new Instruction(opcode, comparison, targetLabel);
        }

        public static Instruction Create(OpCode opcode, Label targetLabel)
        {
            return new Instruction(opcode, targetLabel);
        }

        public static Instruction Create(OpCode opcode, VariableReference item)
        {
            return new Instruction(opcode, item);
        }

        public static Instruction Create(OpCode opcode, params VariableReference[] items)
        {
            return new Instruction(opcode, items.ToArray());
        }
        //public static Instruction Create(OpCode opcode, VariableReference item, object item2)
        //{
        //    var vr2 = item2 as VariableReference;
        //    return new Instruction(opcode, item, vr2);
        //}

        public static Instruction Create(OpCode opcode, ParameterDefinition item)
        {
            return new Instruction(opcode, item);
        }

        public static Instruction Create(OpCode opcode, MethodReference item)
        {
            return new Instruction(opcode, item);
        }

        public static Instruction Create(OpCode opcode, MethodReference item, VariableReference[] args)
        {
            var inst = new Instruction(opcode, item);
            inst.Arguments.Add(args[0]);
            foreach (var a in args.Skip(1)) inst.OperandArguments.Add(a);
            return inst;
        }

        public static Instruction Create(OpCode opcode, MethodReference item, VariableReference resultVariable, VariableReference[] args)
        {
            var inst = new Instruction(opcode, item);
            inst.Arguments.Add(resultVariable);
            foreach (var a in args) inst.OperandArguments.Add(a);
            return inst;
        }

        //public static Instruction Create(OpCode opcode, string item)
        //{
        //    return new Instruction(opcode, item);
        //}
        //public static Instruction Create(OpCode opcode, char item)
        //{
        //    return new Instruction(opcode, item);
        //}
        //public static Instruction Create(OpCode opcode, bool item)
        //{
        //    return new Instruction(opcode, item);
        //}
        //public static Instruction Create(OpCode opcode, byte item)
        //{
        //    return new Instruction(opcode, item);
        //}
        //public static Instruction Create(OpCode opcode, int item)
        //{
        //    return new Instruction(opcode, item);
        //}
        //public static Instruction Create(OpCode opcode, short item)
        //{
        //    return new Instruction(opcode, item);
        //}
        //public static Instruction Create(OpCode opcode, long item)
        //{
        //    return new Instruction(opcode, item);
        //}
        //public static Instruction Create(OpCode opcode, float item)
        //{
        //    return new Instruction(opcode, item);
        //}
        //public static Instruction Create(OpCode opcode, double item)
        //{
        //    return new Instruction(opcode, item);
        //}
        //public static Instruction Create(OpCode opcode, decimal item)
        //{
        //    return new Instruction(opcode, item);
        //}
        //public static Instruction Create(OpCode opcode, Instruction item)
        //{
        //    return new Instruction(opcode, item);
        //}
        //public static Instruction Create(OpCode opcode, Instruction[] tems)
        //{
        //    return new Instruction(opcode, tems);
        //}
    }
}