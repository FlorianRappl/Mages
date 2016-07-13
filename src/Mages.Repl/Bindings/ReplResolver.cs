namespace Mages.Repl.Bindings
{
    sealed class ReplResolver : IResolver
    {
        private readonly IFileSystem _fs;
        private readonly IInteractivity _interactivity;

        public ReplResolver(IFileSystem fs, IInteractivity interactivity)
        {
            _fs = fs;
            _interactivity = interactivity;
        }

        public IFileSystem FileSystem
        {
            get { return _fs; }
        }

        public IInteractivity Interactivity
        {
            get { return _interactivity; }
        }
    }
}
