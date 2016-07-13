namespace Mages.Repl
{
    using System;

    public interface IFileSystem
    {
        String[] GetAllFiles(String baseDir, String fileName);

        String RealText(String path);

        Byte[] ReadRaw(String path);

        String GetGacDirectory();

        Boolean IsRelative(String path);

        String GetDirectory(String path);

        String GetPath(params String[] paths);

        String GetFileName(String path);
    }
}
