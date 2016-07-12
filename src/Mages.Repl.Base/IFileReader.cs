namespace Mages.Repl
{
    using System;

    public interface IFileReader
    {
        String GetContent(String fileName);
    }
}
