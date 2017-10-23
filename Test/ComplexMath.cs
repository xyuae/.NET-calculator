﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Test
{
    public class ComplexMath : BasicMath, IComplexMath
    {
        public float EvaluateExpression(string expr)
        {
            expr = expr.Replace(" ", "");
            Valid(expr);    // validate string input
            // append () outside of string as sign for evaluation
            expr = "(" + expr + ")";
            string[] exprArray = Arraylize(expr);
            

            
            Stack<string> ops = new Stack<string>();
            Stack<float> vals = new Stack<float>();

            for (int i = 0; i < exprArray.Length; i++)
            {
                string s = exprArray[i];
                if (s.Equals("(")) { }
                else if (Regex.IsMatch(s, @"[\+\-]")) ops.Push(s);
                else if (Regex.IsMatch(s, @"[\*/]"))
                {
                    try
                    {
                        float val = float.Parse(exprArray[++i]);
                        float v = vals.Pop();
                        if (s == "*")
                        {
                            v = Multiply(v, val);
                            vals.Push(v);
                        }
                        if (s == "/")
                        {
                            v = Divide(v, val);
                            vals.Push(v);
                        }
                    }
                    catch
                    {
                        throw new InvalidCastException("Error: Non matching number after operator");
                    }
                    
                }
                else if (s.Equals(")")) // evaluation needed
                {
                    // reverse the stack
                    Stack<float> vals2 = new Stack<float>();
                    Stack<string> ops2 = new Stack<string>();
                    while (vals.Count != 0)
                    {
                        vals2.Push(vals.Pop());
                    }

                    while (ops.Count != 0)
                    {
                        ops2.Push(ops.Pop());
                    }

                    vals = vals2;
                    ops = ops2;
                    int count = ops.Count;
                    while (count > 0)
                    {
                        string op = ops.Pop();
                        float v = vals.Pop();
                        switch (op)
                        {
                            case "+":
                                v = Add(v, vals.Pop());
                                break;
                            case "*":
                                v = Multiply(v, vals.Pop());
                                break;
                            case "-":
                                v = Subtract(v, vals.Pop());
                                break;
                            case "/":
                                v = Divide(v, vals.Pop());
                                break;
                            default:
                                throw new InvalidOperationException("Operator not found");
                        }
                        vals.Push(v);
                        count--;    // may subject to change in more complex situation
                    }
                }   // end if (s.Equals(")")
                else if (Regex.IsMatch(s, @"\d+"))
                {
                    //Console.WriteLine(s);
                    vals.Push(float.Parse(s));
                }
                
            }
            return vals.Pop();
        }

        public string[] Arraylize(string expr)
        {
            string pattern = @"(\()|(\))|(\d+)|([\+\-\/\*])";
            //return Regex.Split(expr, pattern);
            return Regex.Matches(expr, pattern).Cast<Match>().Select(x=>x.Value).ToArray();
        }

        public void Valid(string expr)
        {
            if (expr.Contains("(") || expr.Contains(")"))
                throw new InvalidOperationException("Parenthesis not allowed");
            if (Regex.IsMatch(expr, @"[A-Za-z]+"))
                throw new InvalidOperationException("Alphebet not allowed");
        }
    }
}
