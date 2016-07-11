using System.Collections;

namespace Mages.Core.Runtime.Functions
{
    class IterationFunctions
    {
        /// <summary>
        /// Foreach runtime implementation
        /// </summary>
        public static readonly Function Each = new Function(args =>
        {
            if (args.Length == 0) return null;

            var outer = args[0] as Function;

            if (args.Length == 1 && outer!=null)
                return new Function(arr =>
                {
                    var inner = arr[0] as IEnumerable;

                    foreach (var item in inner)
                    {
                        outer.Invoke(new[] { item });
                    }
                    return arr[0];
                });

            var f = args[1] as Function;

            if (args.Length == 2 && f!=null)
            {
                var enu = args[0] as IEnumerable;

                if (enu!=null)
                    return new Function(arr =>
                    {
                        foreach (var item in enu)
                        {
                            f.Invoke(new[] { item });
                        }
                        return args[0];
                    });
            }

            return null;
        });
    }
}