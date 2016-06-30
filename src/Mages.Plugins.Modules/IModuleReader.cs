namespace Mages.Plugins.Modules
{
    using System;

    public interface IModuleFileReader
    {
        String GetContent(String path);

        Boolean TryGetPath(String fileName, out String path);
    }
}
