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
            if (args.Length == 0) return null;

            if (args.Length == 1 && args[0] is Function)
                return new Function(arr =>
                {
                    var outer = args[0] as Function;

                    if (arr[0] is IEnumerable)
                    {
                        var inner = arr[0] as IEnumerable;

                        foreach (var item in inner)
                        {
                            outer.Invoke(new[] { item });
                        }
                    }
                    return outer.Invoke(new[] { args[0] });
                });

            var f = args[1] as Function;

            if (args.Length == 2 && f!=null)
            {
                if (args[0] is IEnumerable)
                    return new Function(arr =>
                    {
                        var enu = args[0] as IEnumerable;

                        foreach (var item in enu)
                        {
                            f.Invoke(new[] { item });
                        }
                        return null;
                    });

                if (args[0] is Function)
                    return new Function(arr =>
                    {
                        var enu = (args[0] as Function).Invoke(new object[] { }) as IEnumerable;
                       
                        foreach (var item in enu)
                        {
                            f.Invoke(new[] { item });
                        }
                        return null;
                    });
            }

            return null;
        });
    }
}