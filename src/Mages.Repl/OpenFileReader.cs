namespace Mages.Repl
{
    using System;
    using System.IO;

    sealed class OpenFileReader : IFileReader
    {
        private readonly IInteractivity _interactivity;

        public OpenFileReader(IInteractivity interactivity)
        {
            _interactivity = interactivity;
        }

        public String GetContent(String fileName)
        {
            var content = String.Empty;

            if (!File.Exists(fileName))
            {
                _interactivity.Error(String.Format("The file {0} does not exist.", fileName));
                Environment.Exit(1);
            }

            try
            {
                content = File.ReadAllText(fileName);
            }
            catch
            {
                _interactivity.Error(String.Format("Error while reading the file {0}.", fileName));
                Environment.Exit(1);
            }

            return content;
        }
    }
}
