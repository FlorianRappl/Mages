namespace Mages.Repl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Completion
    {
        public static readonly Completion Empty = new Completion(null, null);

        public Completion(String prefix, String[] result)
        {
            Prefix = prefix;
            Result = result;
        }

        public static Completion From(IEnumerable<String> autocompletion)
        {
            var entries = autocompletion.ToArray();
            var prefix = String.Empty;

            if (entries.Length > 0 && entries[0].Contains("|"))
            {
                var index = entries[0].IndexOf('|');
                prefix = entries[0].Substring(0, index);

                for (var i = 0; i < entries.Length; i++)
                {
                    entries[i] = entries[i].Substring(index + 1);
                }
            }

            return new Completion(prefix, entries);
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
