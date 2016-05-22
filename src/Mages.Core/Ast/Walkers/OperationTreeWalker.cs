namespace Mages.Core.Ast.Walkers
{
    using Mages.Core.Ast.Expressions;
    using Mages.Core.Ast.Statements;
    using Mages.Core.Runtime;
    using Mages.Core.Vm;
    using Mages.Core.Vm.Operations;
    using System;
    using System.Collections.Generic;

    public class OperationTreeWalker : ITreeWalker
    {
        #region Operator Mappings

        private static readonly Dictionary<String, Action<OperationTreeWalker, PreUnaryExpression>> PreUnaryOperatorMapping = new Dictionary<String, Action<OperationTreeWalker, PreUnaryExpression>>
        {
            { "~", (walker, expr) => walker.Handle(expr, UnaryOperators.Not) },
            { "+", (walker, expr) => walker.Handle(expr, UnaryOperators.Positive) },
            { "-", (walker, expr) => walker.Handle(expr, UnaryOperators.Negative) },
            { "++", (walker, expr) => walker.Increment(expr.Value, false) },
            { "--", (walker, expr) => walker.Decrement(expr.Value, false) }
        };

        private static readonly Dictionary<String, Action<OperationTreeWalker, PostUnaryExpression>> PostUnaryOperatorMapping = new Dictionary<String, Action<OperationTreeWalker, PostUnaryExpression>>
        {
            { "!", (walker, expr) => walker.Handle(expr, UnaryOperators.Factorial) },
            { "'", (walker, expr) => walker.Handle(expr, UnaryOperators.Transpose) },
            { "++", (walker, expr) => walker.Increment(expr.Value, true) },
            { "--", (walker, expr) => walker.Decrement(expr.Value, true) }
        };

        private static readonly Dictionary<String, Action<OperationTreeWalker, BinaryExpression>> BinaryOperatorMapping = new Dictionary<String, Action<OperationTreeWalker, BinaryExpression>>
        {
            { "&&", (walker, expr) => walker.Handle(expr, BinaryOperators.And) },
            { "||", (walker, expr) => walker.Handle(expr, BinaryOperators.Or) },
            { "==", (walker, expr) => walker.Handle(expr, BinaryOperators.Eq) },
            { "~=", (walker, expr) => walker.Handle(expr, BinaryOperators.Neq) },
            { ">", (walker, expr) => walker.Handle(expr, BinaryOperators.Gt) },
            { "<", (walker, expr) => walker.Handle(expr, BinaryOperators.Lt) },
            { ">=", (walker, expr) => walker.Handle(expr, BinaryOperators.Geq) },
            { "<=", (walker, expr) => walker.Handle(expr, BinaryOperators.Leq) },
            { "+", (walker, expr) => walker.Handle(expr, BinaryOperators.Add) },
            { "-", (walker, expr) => walker.Handle(expr, BinaryOperators.Sub) },
            { "*", (walker, expr) => walker.Handle(expr, BinaryOperators.Mul) },
            { "/", (walker, expr) => walker.Handle(expr, BinaryOperators.RDiv) },
            { "\\", (walker, expr) => walker.Handle(expr, BinaryOperators.LDiv) },
            { "^", (walker, expr) => walker.Handle(expr, BinaryOperators.Pow) },
            { "%", (walker, expr) => walker.Handle(expr, BinaryOperators.Mod) }
        };

        #endregion

        #region Fields

        private readonly List<IOperation> _operations;
        private Boolean _assigning;

        #endregion

        #region ctor

        public OperationTreeWalker(List<IOperation> operations)
        {
            _operations = operations;
            _assigning = false;
        }

        #endregion

        #region Visitors

        public void Visit(BlockStatement block)
        {
            foreach (var statement in block.Statements)
            {
                statement.Accept(this);
            }
        }

        public void Visit(SimpleStatement statement)
        {
            statement.Expression.Accept(this);
        }

        public void Visit(VarStatement statement)
        {
            statement.Assignment.Accept(this);
        }

        public void Visit(EmptyExpression expression)
        {
        }

        public void Visit(ConstantExpression expression)
        {
            var constant = expression.Value;
            _operations.Add(new LoadOperation(ctx => constant));
        }

        public void Visit(ArgumentsExpression expression)
        {
            var arguments = expression.Arguments;

            for (var i = arguments.Length - 1; i >= 0; i--)
            {
                arguments[i].Accept(this);
            }
        }

        public void Visit(AssignmentExpression expression)
        {
            _assigning = true;
            expression.Variable.Accept(this);
            var store = PopOperation();
            expression.Value.Accept(this);
            _operations.Add(store);
        }

        public void Visit(BinaryExpression expression)
        {
            var action = default(Action<OperationTreeWalker, BinaryExpression>);
            BinaryOperatorMapping.TryGetValue(expression.Operator, out action);
            action.Invoke(this, expression);
        }

        public void Visit(PreUnaryExpression expression)
        {
            var action = default(Action<OperationTreeWalker, PreUnaryExpression>);
            PreUnaryOperatorMapping.TryGetValue(expression.Operator, out action);
            action.Invoke(this, expression);
        }

        public void Visit(PostUnaryExpression expression)
        {
            var action = default(Action<OperationTreeWalker, PostUnaryExpression>);
            PostUnaryOperatorMapping.TryGetValue(expression.Operator, out action);
            action.Invoke(this, expression);
        }

        public void Visit(RangeExpression expression)
        {
            expression.Step.Accept(this);
            expression.To.Accept(this);
            expression.From.Accept(this);

            CallFunction(args => Range.Create((Double)args[0], (Double)args[1], (Double)args[2]), 3);
        }

        public void Visit(ConditionalExpression expression)
        {
            expression.Secondary.Accept(this);
            expression.Primary.Accept(this);
            expression.Condition.Accept(this);

            CallFunction(args => Logic.IsTrue((Double)args[0]) ? args[1] : args[2], 3);
        }

        public void Visit(CallExpression expression)
        {
            expression.Arguments.Accept(this);
            expression.Function.Accept(this);
            _operations.Add(new CallOperation(expression.Arguments.Count));
        }

        public void Visit(ObjectExpression expression)
        {
            var init = new LoadOperation(ctx => new Dictionary<String, Object>());
            _operations.Add(init);

            foreach (var property in expression.Values)
            {
                property.Accept(this);
            }
        }

        public void Visit(PropertyExpression expression)
        {
            expression.Name.Accept(this);
            expression.Value.Accept(this);

            CallFunction(args =>
            {
                Helpers.SetProperty((IDictionary<String, Object>)args[2], (String)args[1], args[0]);
                return args[2];
            }, 3);
        }

        public void Visit(MatrixExpression expression)
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

        public void Visit(FunctionExpression expression)
        {
            var current = _operations.Count;
            expression.Parameters.Accept(this);
            expression.Body.Accept(this);
            var operations = ExtractFrom(current);
            _operations.Add(new FuncOperation(operations));
        }

        public void Visit(InvalidExpression expression)
        {
        }

        public void Visit(IdentifierExpression expression)
        {
            var name = expression.Name;
            _operations.Add(new LoadOperation(ctx => name));
        }

        public void Visit(MemberExpression expression)
        {
            expression.Member.Accept(this);
            expression.Object.Accept(this);

            if (_assigning)
            {
                _operations.Add(new StoreOperation((ctx, val) => Helpers.SetProperty((IDictionary<String, Object>)ctx.Pop(), (String)ctx.Pop(), val)));
            }
            else
            {
                _operations.Add(new LoadOperation(ctx => Helpers.GetProperty((IDictionary<String, Object>)ctx.Pop(), (String)ctx.Pop())));
            }

            _assigning = false;
        }

        public void Visit(ParameterExpression expression)
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

        public void Visit(VariableExpression expression)
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

            _assigning = false;
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
            _operations.Add(new DecOperation(PopOperation(), postOperation));
        }

        private void Increment(IExpression expr, Boolean postOperation)
        {
            expr.Accept(this);
            _assigning = true;
            expr.Accept(this);
            _operations.Add(new IncOperation(PopOperation(), postOperation));
        }

        #endregion
    }
}
