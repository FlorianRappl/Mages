namespace Mages.Core.Ast.Walkers
{
    using Mages.Core.Ast.Expressions;
    using Mages.Core.Ast.Statements;
    using Mages.Core.Runtime;
    using Mages.Core.Runtime.Functions;
    using Mages.Core.Vm;
    using Mages.Core.Vm.Operations;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents the walker to create operations.
    /// </summary>
    public class OperationTreeWalker : ITreeWalker
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
            statement.Assignment.Accept(this);
        }

        void ITreeWalker.Visit(EmptyExpression expression)
        {
        }

        void ITreeWalker.Visit(ConstantExpression expression)
        {
            var constant = expression.Value;
            _operations.Add(new LoadOperation(ctx => constant));
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
            var autoStep = expression.Step is EmptyExpression;

            if (!autoStep)
            {
                expression.Step.Accept(this);
            }

            expression.To.Accept(this);
            expression.From.Accept(this);

            if (autoStep)
            {
                CallFunction(args => Range.Create((Double)args[0], (Double)args[1]), 2);
            }
            else
            {
                CallFunction(args => Range.Create((Double)args[0], (Double)args[1], (Double)args[2]), 3);
            }
        }

        void ITreeWalker.Visit(ConditionalExpression expression)
        {
            expression.Secondary.Accept(this);
            expression.Primary.Accept(this);
            expression.Condition.Accept(this);

            CallFunction(args => Logic.IsTrue((Double)args[0]) ? args[1] : args[2], 3);
        }

        void ITreeWalker.Visit(CallExpression expression)
        {
            expression.Arguments.Accept(this);
            expression.Function.Accept(this);
            _operations.Add(new CallOperation(expression.Arguments.Count));
        }

        void ITreeWalker.Visit(ObjectExpression expression)
        {
            var init = new LoadOperation(ctx => new Dictionary<String, Object>());
            _operations.Add(init);

            foreach (var property in expression.Values)
            {
                property.Accept(this);
            }
        }

        void ITreeWalker.Visit(PropertyExpression expression)
        {
            expression.Name.Accept(this);
            expression.Value.Accept(this);

            CallFunction(args =>
            {
                Helpers.SetProperty((IDictionary<String, Object>)args[2], (String)args[1], args[0]);
                return args[2];
            }, 3);
        }

        void ITreeWalker.Visit(MatrixExpression expression)
        {
            var values = expression.Values;
            var rows = values.Length;
            var cols = rows > 0 ? values[0].Length : 0;
            var init = new LoadOperation(ctx => new Double[rows, cols]);
            _operations.Add(init);

            for (var row = 0; row < rows; row++)
            {
                for (var col = 0; col < cols; col++)
                {
                    var value = values[row][col];
                    var i = row;
                    var j = col;
                    value.Accept(this);

                    CallFunction(args =>
                    {
                        var matrix = (Double[,])args[1];
                        matrix.SetValue(i, j, (Double)args[0]);
                        return matrix;
                    }, 2);
                }
            }
        }

        void ITreeWalker.Visit(FunctionExpression expression)
        {
            var current = _operations.Count;
            expression.Parameters.Accept(this);
            expression.Body.Accept(this);
            var operations = ExtractFrom(current);
            _operations.Add(new FuncOperation(operations));
        }

        void ITreeWalker.Visit(InvalidExpression expression)
        {
        }

        void ITreeWalker.Visit(IdentifierExpression expression)
        {
            var name = expression.Name;
            _operations.Add(new LoadOperation(ctx => name));
        }

        void ITreeWalker.Visit(MemberExpression expression)
        {
            expression.Member.Accept(this);
            expression.Object.Accept(this);

            if (_assigning)
            {
                _operations.Add(SetOperation.Instance);
            }
            else
            {
                _operations.Add(GetOperation.Instance);
            }
        }

        void ITreeWalker.Visit(ParameterExpression expression)
        {
            var expressions = expression.Expressions;

            for (var i = 0; i < expressions.Length; i++)
            {
                var identifier = (VariableExpression)expressions[i];
                var name = identifier.Name;
                var index = i;

                _operations.Add(new StoreOperation((ctx, val) =>
                {
                    var parameters = (Object[])val;
                    var value = parameters.Length > index ? parameters[index] : null;
                    ctx.Scope.SetProperty(name, value);
                }));
            }
        }

        void ITreeWalker.Visit(VariableExpression expression)
        {
            var name = expression.Name;

            if (_assigning)
            {
                _operations.Add(new StoreOperation((ctx, val) => ctx.Scope.SetProperty(name, val)));
            }
            else
            {
                _operations.Add(new LoadOperation(ctx => ctx.Scope.GetProperty(name)));
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
            _operations.Add(new LoadOperation(ctx => func));
            _operations.Add(new CallOperation(argumentCount));
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
