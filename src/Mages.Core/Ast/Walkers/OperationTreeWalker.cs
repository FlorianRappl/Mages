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

        public OperationTreeWalker(List<IOperation> operations)
        {
            _operations = operations;
        }

        public void Visit(EmptyExpression expression)
        {
        }

        public void Visit(ConstantExpression expression)
        {
            var operation = new LoadOperation(expression.Value);
            _operations.Add(operation);
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

            var arguments = new CollectArgsOperation(expression.Arguments.Count);
            _operations.Add(arguments);

            expression.Function.Accept(this);

            var call = new CallOperation();
            _operations.Add(call);
        }

        public void Visit(ObjectExpression expression)
        {
            var obj = new Dictionary<String, Object>();
            var init = new LoadOperation(obj);
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
            throw new NotImplementedException();
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
            var load = new LoadOperation(expression.Name);
            _operations.Add(load);
        }

        public void Visit(MemberExpression expression)
        {
            expression.Member.Accept(this);
            expression.Object.Accept(this);

            CallFunction(expression.GetFunction(), 2);
        }

        public void Visit(ParameterExpression expression)
        {
            throw new NotImplementedException();
        }

        public void Visit(VariableExpression expression)
        {
            throw new NotImplementedException();
        }

        private void CallFunction(Func<Object[], Object> func, Int32 argumentCount)
        {
            var arguments = new CollectArgsOperation(argumentCount);
            _operations.Add(arguments);

            var function = new LoadOperation(func);
            _operations.Add(function);

            var call = new CallOperation();
            _operations.Add(call);
        }
    }
}
