namespace Mages.Repl
{
    public interface IResolver
    {
        IInteractivity Interactivity { get; }

        IFileSystem FileSystem { get; }
    }
}
