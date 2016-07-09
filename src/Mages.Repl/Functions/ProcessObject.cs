namespace Mages.Repl.Functions
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    sealed class ProcessObject
    {
        private readonly Process _process;
        private readonly IDictionary<String, Object> _env;

        public ProcessObject()
        {
            _process = Process.GetCurrentProcess();
            _env = Environment.GetEnvironmentVariables().OfType<DictionaryEntry>().ToDictionary(m => (String)m.Key, m => m.Value);
        }

        public String[] argv
        {
            get { return Environment.GetCommandLineArgs(); }
        }

        public Int32 id
        {
            get { return _process.Id; }
        }

        public String workingDirectory
        {
            get { return Environment.CurrentDirectory; }
        }

        public Int32 processors
        {
            get { return Environment.ProcessorCount; }
        }

        public String machine
        {
            get { return _process.MachineName; }
        }

        public TimeSpan systemTime
        {
            get { return _process.TotalProcessorTime; }
        }

        public TimeSpan userTime
        {
            get { return _process.UserProcessorTime; }
        }

        public Int64 memory
        {
            get { return _process.VirtualMemorySize64; }
        }

        public Int32 threads
        {
            get { return _process.Threads.Count; }
        }

        public IDictionary<String, Object> env
        {
            get { return _env; }
        }
    }
}
