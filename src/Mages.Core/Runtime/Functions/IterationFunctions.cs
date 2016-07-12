using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

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

        /// <summary>
        /// Linq style Where
        /// </summary>
        public static readonly Function Where = new Function(args =>
        {
            if (args.Length == 1)
            {
                if (args[0] is IEnumerable)
                    return new Function(innerArg =>
                    {
                        var f = innerArg[0] as Function;
                        var enu = args[0] as IEnumerable;

                        var list = new List<object>();
                        foreach (var item in enu)
                        {
                            if ((bool)f.Invoke(new[] { item }))
                            {
                                list.Add(item);
                            }
                        }
                        return list;
                    });

                if (args[0] is Function)
                    return new Function(innerArg =>
                    {
                        var f = args[0] as Function;
                        var enu = innerArg[0] as IEnumerable;

                        var list = new List<object>();
                        foreach (var item in enu)
                        {
                            if ((bool)f.Invoke(new[] { item }))
                            {
                                list.Add(item);
                            }
                        }
                        return list;
                    });
            }

            if (args.Length == 2 && args[0] is IEnumerable && args[1] is Function)
            {
                var enu = args[0] as IEnumerable;
                var f = args[1] as Function;

                var list = new List<object>();
                foreach (var item in enu)
                {
                    if ((bool)f.Invoke(new[] { item }))
                    {
                        list.Add(item);
                    }
                }
                return list;
            }
            return null;
        });


        /// <summary>
        /// Linq style Select
        /// </summary>
        public static readonly Function Select = new Function(args =>
        {
            if (args.Length == 1)
            {
                if (args[0] is IEnumerable)
                    return new Function(innerArg =>
                    {
                        var f = innerArg[0] as Function;
                        var enu = args[0] as IEnumerable;

                        var list = new List<object>();
                        foreach (var item in enu)
                        {
                            list.Add(f.Invoke(new[] { item }));
                        }
                        return list;
                    });

                if (args[0] is Function)
                    return new Function(innerArg =>
                    {
                        var f = args[0] as Function;
                        var enu = innerArg[0] as IEnumerable;

                        var list = new List<object>();
                        foreach (var item in enu)
                        {
                            list.Add(f.Invoke(new[] { item }));
                        }
                        return list;
                    });
            }

            if (args.Length == 2 && args[0] is IEnumerable && args[1] is Function)
            {
                var enu = args[0] as IEnumerable;
                var f = args[1] as Function;

                var list = new List<object>();
                foreach (var item in enu)
                {
                    list.Add(f.Invoke(new[] { item }));
                }
                return list;
            }
            return null;
        });
    }
}