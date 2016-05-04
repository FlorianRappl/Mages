namespace Mages.Core.Ast.Walkers
{
    using Mages.Core.Ast.Expressions;
    using Mages.Core.Vm;
    using Mages.Core.Vm.Operations;
    using System;
    using System.Collections.Generic;

    public class OperationTreeWalker : ITreeWalker
    {
        private readonly List<IOperation> _operations;
        private readonly Dictionary<IExpression, Int32> _positions;

        public OperationTreeWalker(List<IOperation> operations)
        {
            _operations = operations;
            _positions = new Dictionary<IExpression, Int32>();
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
            throw new NotImplementedException();
        }

        public void Visit(BinaryExpression expression)
        {
            expression.RValue.Accept(this);
            expression.LValue.Accept(this);

            CallFunction(expression.GetFunction(), 2);
        }

        public void Visit(PreUnaryExpression expression)
        {
            expression.Value.Accept(this);

            CallFunction(expression.GetFunction(), 1);
        }

        public void Visit(PostUnaryExpression expression)
        {
            expression.Value.Accept(this);

            CallFunction(expression.GetFunction(), 1);
        }

        public void Visit(RangeExpression expression)
        {
            expression.Step.Accept(this);
            expression.To.Accept(this);
            expression.From.Accept(this);

            CallFunction(expression.GetFunction(), 3);
        }

        public void Visit(ConditionalExpression expression)
        {
            expression.Secondary.Accept(this);
            expression.Primary.Accept(this);
            expression.Condition.Accept(this);

            CallFunction(expression.GetFunction(), 3);
        }

        public void Visit(CallExpression expression)
        {
            expression.Arguments.Accept(this);
            _operations.Add(new CollectArgsOperation(expression.Arguments.Count));
            expression.Function.Accept(this);
            _operations.Add(new CallOperation());
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
            expression.Value.Accept(this);
            expression.Name.Accept(this);

            CallFunction(expression.GetFunction(), 3);
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
                        matrix[i, j] = (Double)args[0];
                        return matrix;
                    }, 2);
                }
            }
        }

        public void Visit(FunctionExpression expression)
        {
            throw new NotImplementedException();
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

            CallFunction(expression.GetFunction(), 2);
        }

        public void Visit(ParameterExpression expression)
        {

        }

        public void Visit(VariableExpression expression)
        {
            var symbol = expression.ReferencedSymbol;
            var position = _positions[symbol];
            
            _operations.Add(new LoadOperation(ctx => new Pointer
            {
                Memory = ctx.Memory,
                Address = position
            }));
        }

        private void CallFunction(Func<Object[], Object> func, Int32 argumentCount)
        {
            _operations.Add(new CollectArgsOperation(argumentCount));
            _operations.Add(new LoadOperation(ctx => func));
            _operations.Add(new CallOperation());
        }
    }
}
