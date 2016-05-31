namespace Mages.Repl.Functions
{
    using System;
    using System.Diagnostics;

    sealed class ProcessObject
    {
        private readonly Process _process;

        public ProcessObject()
        {
            _process = Process.GetCurrentProcess();
        }

        public Int32 id
        {
            get { return _process.Id; }
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
    }
}
