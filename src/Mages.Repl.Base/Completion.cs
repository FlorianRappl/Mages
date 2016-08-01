namespace Mages.Repl
{
    using System;

    public class Completion
    {
        public static readonly Completion Empty = new Completion(null, null);

        public Completion(String prefix, String[] result)
        {
            Prefix = prefix;
            Result = result;
        }

        public String[] Result
        {
            get;
            private set;
        }

        public String Prefix
        {
            get;
            private set;
        }
    }
}
