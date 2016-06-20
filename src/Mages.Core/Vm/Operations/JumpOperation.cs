namespace Mages.Core.Vm.Operations
{
    using System;

    /// <summary>
    /// Changes the currently executing position.
    /// </summary>
    sealed class JumpOperation : IOperation
    {
        private readonly Int32 _position;

        public JumpOperation(Int32 position)
        {
            _position = position;
        }

        public void Invoke(IExecutionContext context)
        {
            context.Position = _position;
        }

        public override String ToString()
        {
            return String.Concat("jump ", _position.ToString());
        }
    }
}
