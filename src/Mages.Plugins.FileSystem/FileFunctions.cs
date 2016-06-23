namespace Mages.Plugins.FileSystem
{
    using System;
    using System.IO;
    using System.Text;

    static class FileFunctions
    {
        public static Boolean Exists(String fileName)
        {
            return File.Exists(fileName);
        }

        public static Object Create(String fileName, String content)
        {
            var rawContent = Encoding.UTF8.GetBytes(content);
            return Create(fileName, rawContent);
        }

        public static Object Create(String fileName, Byte[] rawContent)
        {
            var fs = new FileStream(fileName, FileMode.Create);
            var ms = new MemoryStream(rawContent);
            return ms.CopyToAsync(fs).AsFuture(Dispose(fs, ms));
        }

        public static Object Append(String fileName, String content)
        {
            var rawContent = Encoding.UTF8.GetBytes(content);
            return Append(fileName, rawContent);
        }

        public static Object Append(String fileName, Byte[] rawContent)
        {
            var fs = new FileStream(fileName, FileMode.Append);
            var ms = new MemoryStream(rawContent);
            return ms.CopyToAsync(fs).AsFuture(Dispose(fs, ms));
        }

        public static Object ReadText(String fileName)
        {
            var fs = new FileStream(fileName, FileMode.Open);
            var sw = new StreamReader(fs);
            return sw.ReadToEndAsync().AsFuture(Dispose(fs, sw));
        }

        public static Object ReadBinary(String fileName)
        {
            var fs = new FileStream(fileName, FileMode.Open);
            var ms = new MemoryStream();
            return fs.CopyToAsync(ms).ContinueWith<Byte[]>(_ => ms.ToArray()).AsFuture(Dispose(fs, ms));
        }

        public static void Delete(String fileName)
        {
            File.Delete(fileName);
        }

        public static void Move(String source, String target)
        {
            File.Move(source, target);
        }

        private static Action Dispose(params IDisposable[] disposables)
        {
            return () =>
            {
                foreach (var disposable in disposables)
                {
                    disposable.Dispose();
                }
            };
        }
    }
}
