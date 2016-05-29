﻿namespace Mages.Core.Ast.Walkers
{
    using Mages.Core.Ast.Expressions;
    using Mages.Core.Ast.Statements;
    using Mages.Core.Runtime.Functions;
    using Mages.Core.Vm;
    using Mages.Core.Vm.Operations;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents the walker to create operations.
    /// </summary>
    public sealed class OperationTreeWalker : ITreeWalker
    {
        #region Operator Mappings

        private static readonly Dictionary<String, Action<OperationTreeWalker, PreUnaryExpression>> PreUnaryOperatorMapping = new Dictionary<String, Action<OperationTreeWalker, PreUnaryExpression>>
        {
            { "~", (walker, expr) => walker.Handle(expr, StandardFunctions.Not) },
            { "+", (walker, expr) => walker.Handle(expr, StandardFunctions.Positive) },
            { "-", (walker, expr) => walker.Handle(expr, StandardFunctions.Negative) },
            { "++", (walker, expr) => walker.Increment(expr.Value, false) },
            { "--", (walker, expr) => walker.Decrement(expr.Value, false) }
        };

        private static readonly Dictionary<String, Action<OperationTreeWalker, PostUnaryExpression>> PostUnaryOperatorMapping = new Dictionary<String, Action<OperationTreeWalker, PostUnaryExpression>>
        {
            { "!", (walker, expr) => walker.Handle(expr, StandardFunctions.Factorial) },
            { "'", (walker, expr) => walker.Handle(expr, StandardFunctions.Transpose) },
            { "++", (walker, expr) => walker.Increment(expr.Value, true) },
            { "--", (walker, expr) => walker.Decrement(expr.Value, true) }
        };

        private static readonly Dictionary<String, Action<OperationTreeWalker, BinaryExpression>> BinaryOperatorMapping = new Dictionary<String, Action<OperationTreeWalker, BinaryExpression>>
        {
            { "&&", (walker, expr) => walker.Handle(expr, StandardFunctions.And) },
            { "||", (walker, expr) => walker.Handle(expr, StandardFunctions.Or) },
            { "==", (walker, expr) => walker.Handle(expr, ComparisonFunctions.Eq) },
            { "~=", (walker, expr) => walker.Handle(expr, ComparisonFunctions.Neq) },
            { ">", (walker, expr) => walker.Handle(expr, ComparisonFunctions.Gt) },
            { "<", (walker, expr) => walker.Handle(expr, ComparisonFunctions.Lt) },
            { ">=", (walker, expr) => walker.Handle(expr, ComparisonFunctions.Geq) },
            { "<=", (walker, expr) => walker.Handle(expr, ComparisonFunctions.Leq) },
            { "+", (walker, expr) => walker.Handle(expr, StandardFunctions.Add) },
            { "-", (walker, expr) => walker.Handle(expr, StandardFunctions.Sub) },
            { "*", (walker, expr) => walker.Handle(expr, StandardFunctions.Mul) },
            { "/", (walker, expr) => walker.Handle(expr, StandardFunctions.RDiv) },
            { "\\", (walker, expr) => walker.Handle(expr, StandardFunctions.LDiv) },
            { "^", (walker, expr) => walker.Handle(expr, StandardFunctions.Pow) },
            { "%", (walker, expr) => walker.Handle(expr, StandardFunctions.Mod) }
        };

        #endregion

        #region Fields

        private readonly List<IOperation> _operations;
        private Boolean _assigning;
        private Boolean _declaring;

        #endregion

        #region ctor

        /// <summary>
        /// Creates a new operation tree walker.
        /// </summary>
        /// <param name="operations">The list of operations to populate.</param>
        public OperationTreeWalker(List<IOperation> operations)
        {
            _operations = operations;
            _assigning = false;
            _declaring = false;
        }

        #endregion

        #region Visitors

        void ITreeWalker.Visit(BlockStatement block)
        {
            foreach (var statement in block.Statements)
            {
                statement.Accept(this);
            }
        }

        void ITreeWalker.Visit(SimpleStatement statement)
        {
            statement.Expression.Accept(this);
        }

        void ITreeWalker.Visit(VarStatement statement)
        {
            _declaring = true;
            statement.Assignment.Accept(this);
            _declaring = false;
        }

        void ITreeWalker.Visit(EmptyExpression expression)
        {
        }

        void ITreeWalker.Visit(ConstantExpression expression)
        {
            var constant = expression.Value;
            _operations.Add(new ConstOperation(constant));
        }

        void ITreeWalker.Visit(ArgumentsExpression expression)
        {
            var arguments = expression.Arguments;

            for (var i = arguments.Length - 1; i >= 0; i--)
            {
                arguments[i].Accept(this);
            }
        }

        void ITreeWalker.Visit(AssignmentExpression expression)
        {
            _assigning = true;
            expression.Variable.Accept(this);
            _assigning = false;
            var store = PopOperation();
            expression.Value.Accept(this);
            _operations.Add(store);
        }

        void ITreeWalker.Visit(BinaryExpression expression)
        {
            var action = default(Action<OperationTreeWalker, BinaryExpression>);
            BinaryOperatorMapping.TryGetValue(expression.Operator, out action);
            action.Invoke(this, expression);
        }

        void ITreeWalker.Visit(PreUnaryExpression expression)
        {
            var action = default(Action<OperationTreeWalker, PreUnaryExpression>);
            PreUnaryOperatorMapping.TryGetValue(expression.Operator, out action);
            action.Invoke(this, expression);
        }

        void ITreeWalker.Visit(PostUnaryExpression expression)
        {
            var action = default(Action<OperationTreeWalker, PostUnaryExpression>);
            PostUnaryOperatorMapping.TryGetValue(expression.Operator, out action);
            action.Invoke(this, expression);
        }

        void ITreeWalker.Visit(RangeExpression expression)
        {
            var hasStep = expression.Step is EmptyExpression == false;

            if (hasStep)
            {
                expression.Step.Accept(this);
            }

            expression.To.Accept(this);
            expression.From.Accept(this);
            _operations.Add(new RangeOperation(hasStep));
        }

        void ITreeWalker.Visit(ConditionalExpression expression)
        {
            expression.Secondary.Accept(this);
            expression.Primary.Accept(this);
            expression.Condition.Accept(this);

            _operations.Add(CondOperation.Instance);
        }

        void ITreeWalker.Visit(CallExpression expression)
        {
            var assigning = _assigning;
            _assigning = false;

            expression.Arguments.Accept(this);
            expression.Function.Accept(this);

            if (assigning)
            {
                _operations.Add(new SetcOperation(expression.Arguments.Count));
            }
            else
            {
                _operations.Add(new GetcOperation(expression.Arguments.Count));
            }

            _assigning = assigning;
        }

        void ITreeWalker.Visit(ObjectExpression expression)
        {
            _operations.Add(NewObjOperation.Instance);

            foreach (var property in expression.Values)
            {
                property.Accept(this);
            }
        }

        void ITreeWalker.Visit(PropertyExpression expression)
        {
            expression.Name.Accept(this);
            expression.Value.Accept(this);

            _operations.Add(InitObjOperation.Instance);
        }

        void ITreeWalker.Visit(MatrixExpression expression)
        {
            var values = expression.Values;
            var rows = values.Length;
            var cols = rows > 0 ? values[0].Length : 0;
            _operations.Add(new NewMatOperation(rows, cols));

            for (var row = 0; row < rows; row++)
            {
                for (var col = 0; col < cols; col++)
                {
                    var value = values[row][col];
                    value.Accept(this);
                    _operations.Add(new InitMatOperation(row, col));
                }
            }
        }

        void ITreeWalker.Visit(FunctionExpression expression)
        {
            var current = _operations.Count;
            expression.Parameters.Accept(this);
            expression.Body.Accept(this);
            var operations = ExtractFrom(current);
            _operations.Add(new NewFuncOperation(operations));
        }

        void ITreeWalker.Visit(InvalidExpression expression)
        {
        }

        void ITreeWalker.Visit(IdentifierExpression expression)
        {
            var name = expression.Name;
            _operations.Add(new ConstOperation(name));
        }

        void ITreeWalker.Visit(MemberExpression expression)
        {
            var assigning = _assigning;
            _assigning = false;

            expression.Member.Accept(this);
            expression.Object.Accept(this);

            if (assigning)
            {
                _operations.Add(SetpOperation.Instance);
            }
            else
            {
                _operations.Add(GetpOperation.Instance);
            }

            _assigning = assigning;
        }

        void ITreeWalker.Visit(ParameterExpression expression)
        {
            var expressions = expression.Expressions;

            for (var i = 0; i < expressions.Length; i++)
            {
                var identifier = (VariableExpression)expressions[i];
                var name = identifier.Name;

                _operations.Add(new ArgOperation(i, name));
            }
        }

        void ITreeWalker.Visit(VariableExpression expression)
        {
            var name = expression.Name;

            if (_assigning)
            {
                if (_declaring)
                {
                    _operations.Add(new AddsOperation(name));
                }
                else
                {
                    _operations.Add(new SetsOperation(name));
                }
            }
            else
            {
                _operations.Add(new GetsOperation(name));
            }
        }

        #endregion

        #region Helpers

        private void Handle(BinaryExpression expression, Function function)
        {
            expression.RValue.Accept(this);
            expression.LValue.Accept(this);
            CallFunction(function, 2);
        }

        private void Handle(PreUnaryExpression expression, Function function)
        {
            expression.Value.Accept(this);
            CallFunction(function, 1);
        }

        private void Handle(PostUnaryExpression expression, Function function)
        {
            expression.Value.Accept(this);
            CallFunction(function, 1);
        }

        private void CallFunction(Function func, Int32 argumentCount)
        {
            _operations.Add(new ConstOperation(func));
            _operations.Add(new GetcOperation(argumentCount));
        }

        private IOperation PopOperation()
        {
            var index = _operations.Count - 1;
            var operation = _operations[index];
            _operations.RemoveAt(index);
            return operation;
        }

        private IOperation[] ExtractFrom(Int32 index)
        {
            var count = _operations.Count;
            var length = count - index;
            var operations = new IOperation[length];

            while (count > index)
            {
                operations[--length] = _operations[--count];
                _operations.RemoveAt(count);
            }

            return operations;
        }

        private void Decrement(IExpression expr, Boolean postOperation)
        {
            expr.Accept(this);
            _assigning = true;
            expr.Accept(this);
            _assigning = false;
            _operations.Add(new DecOperation(PopOperation(), postOperation));
        }

        private void Increment(IExpression expr, Boolean postOperation)
        {
            expr.Accept(this);
            _assigning = true;
            expr.Accept(this);
            _assigning = false;
            _operations.Add(new IncOperation(PopOperation(), postOperation));
        }

        #endregion
    }
}
