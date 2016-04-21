namespace Mages.Core.Tests.Mocks
{
    using Mages.Core.Ast;
    using System;

    sealed class ValidationContextMock : IValidationContext
    {
        private readonly Action<ParseError> _callback;

        public ValidationContextMock(Action<ParseError> callback)
        {
            _callback = callback;
        }

        public void Report(ParseError error)
        {
            _callback.Invoke(error);
        }
    }
}
