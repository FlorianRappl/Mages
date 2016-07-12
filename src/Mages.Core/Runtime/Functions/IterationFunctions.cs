using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mages.Core.Runtime.Functions
{
    class IterationFunctions
    {
        /// <summary>
        /// Foreach runtime implementation
        /// </summary>
        public static readonly Function Each = new Function(args =>
        {
            if(args.Length == 1)
            {
                if (args[0] is IEnumerable)
                    return new Function(innerArg =>
                    {
                        var f = innerArg[0] as Function;
                        var enu = args[0] as IEnumerable;

                        foreach (var item in enu)
                        {
                            f.Invoke(new[] { item });
                        }
                        return args[0];
                    });

                if (args[0] is Function)
                    return new Function(innerArg =>
                    {
                        var f = args[0] as Function;
                        var enu = innerArg[0] as IEnumerable;

                        foreach (var item in enu)
                        {
                            f.Invoke(new[] { item });
                        }
                        return args[0];
                    });
            }

            if (args.Length == 2 && args[0] is IEnumerable && args[1] is Function)
            {
                var enu = args[0] as IEnumerable;
                var f = args[1] as Function;

                foreach (var item in enu)
                {
                    f.Invoke(new[] { item });
                }
                return args[0];
            }
            return null;
        });
    }
}