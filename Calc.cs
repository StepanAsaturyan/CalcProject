using System;
using System.Collections.Generic;

namespace CalcProject
{
    public class Calc
    {
        private readonly string _expression;
        private readonly Stack<Operations> _operationsStack;
        private readonly Stack<double> _numbersStack;
        private readonly ArgumentException _exception;

        private Calc()
        {
            _operationsStack = new Stack<Operations>();
            _numbersStack = new Stack<double>();
            _exception = new ArgumentException($"Invalid operations/symbols or parentheses mistake.\n");
        }

        public Calc(string input) : this()
        {
            input = input.Replace(" ", "");
            if (string.IsNullOrEmpty(input))
                throw new ArgumentException("Incorrect (empty) expression\n");

            _expression = input.Replace(" ", "");
        }

        public double Calculate()
        {
            for (int i = 0; i < _expression.Length; i++)
            {
                var element = _expression[i];

                if (IsDelimeter(element))
                {
                    DoDelimeterOperation((Operations)element);
                }
                else if (char.IsDigit(element))
                {
                    i = DoDigitOperation(i);
                }
                else
                    throw _exception;
            }

            while (_operationsStack.Count > 0)
            {
                if (_operationsStack.Peek().Equals(Operations.OpenParenthesis))
                    throw _exception;

                else
                    DoCalculations(_operationsStack.Pop());
            }

            return _numbersStack.Peek();
        }


        private void DoCalculations(Operations op)
        {
            double operand2 = _numbersStack.Pop();
            double operand1 = _numbersStack.Count > 0 ? _numbersStack.Pop() : 0;

            switch (op)
            {
                case Operations.Add:
                    _numbersStack.Push(operand1 + operand2);
                    break;

                case Operations.Multiply:
                    _numbersStack.Push(operand1 * operand2);
                    break;

                case Operations.Divide:
                    if (operand2 == 0)
                        throw new DivideByZeroException("Dividing by zero is not allowed");

                    _numbersStack.Push(operand1 / operand2);
                    break;

                case Operations.Substract:
                    _numbersStack.Push(operand1 - operand2);
                    break;
            }
        }

        private void DoDelimeterOperation(Operations op)
        {
            if (op.Equals(Operations.OpenParenthesis) || op.Equals(Operations.CloseParenthesis))
                CheckParentheses(op);
            else
                CheckOperation(op);
        }

        private int DoDigitOperation(int i)
        {
            string numStr = string.Empty;

            while (!IsDelimeter(_expression[i]))
            {
                numStr += _expression[i];
                i++;

                if (i >= _expression.Length)
                    break;
            }

            if (!double.TryParse(numStr, out double num))
                throw _exception;

            if (_numbersStack.Count == 0 && _operationsStack.Count > 0 && _operationsStack.Peek().Equals(Operations.Substract))
            {
                _numbersStack.Push(-num);
                _operationsStack.Pop();
            }

            else
                _numbersStack.Push(num);
            return --i;
        }

        private void CheckParentheses(Operations op)
        {
            if (op.Equals(Operations.OpenParenthesis))
                _operationsStack.Push(op);

            else if (op.Equals(Operations.CloseParenthesis))
            {
                try
                {
                    while (!_operationsStack.Peek().Equals(Operations.OpenParenthesis) && _numbersStack.Count > 1)
                    {
                        DoCalculations(_operationsStack.Pop());
                    }

                    _operationsStack.Pop();
                }

                catch (InvalidOperationException)
                {
                    throw new ArgumentException("Invalid expression. Check quantity of closing parentheses.");
                }
            }
        }

        private void CheckOperation(Operations op)
        {
            if (_operationsStack.Count > 0 && !_operationsStack.Peek().Equals(Operations.OpenParenthesis))
            {
                while (_operationsStack.Count > 0 && !_operationsStack.Peek().Equals(Operations.OpenParenthesis) && (GetOperationRank(_operationsStack.Peek()) >= GetOperationRank(op)))
                {
                    if (_numbersStack.Count > 1)
                        DoCalculations(_operationsStack.Pop());
                    else
                        throw _exception;
                }
            }
            _operationsStack.Push(op);
        }

        private bool IsDelimeter(char c)
        {
            return Enum.IsDefined(typeof(Operations), (int)c);
        }

        private int GetOperationRank(Operations c)
        {
            return c.Equals(Operations.Add) || c.Equals(Operations.Substract) ? 1 : 2;
        }
    }
}
