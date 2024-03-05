namespace Mages.Core.Vm.Operations
{
    using Mages.Core.Runtime;
    using System;

    /// <summary>
    /// Pushes one new element on the stack.
    /// </summary>
    sealed class NewFuncOperation(ParameterDefinition[] parameters, IOperation[] operations) : IOperation
    {
        private readonly ParameterDefinition[] _parameters = parameters;
        private readonly IOperation[] _operations = operations;

        public void Invoke(IExecutionContext context)
        {
            var parentScope = context.Scope;
            var function = new LocalFunction(parentScope, _parameters, _operations);
            context.Push(function.Pointer);
        }

        public override String ToString()
        {
            var instructions = new String[3];
            instructions[0] = "newfunc start";
            instructions[1] = _operations.Serialize();
            instructions[2] = "newfunc end";
            return String.Join(Environment.NewLine, instructions);
        }
    }
}
