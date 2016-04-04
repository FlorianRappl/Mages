namespace Mages.Repl
{
    using System;

    class ReplCore
    {
        private readonly IInteractivity _interactivity;

        public ReplCore(IInteractivity interactivity)
        {
            _interactivity = interactivity;
        }

        public void Run()
        {
            throw new NotImplementedException();
        }
    }
}
