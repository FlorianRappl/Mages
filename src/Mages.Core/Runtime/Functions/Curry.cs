namespace Mages.Core.Runtime.Functions
{
    using System;

    /// <summary>
    /// Provide helpers to enable currying.
    /// </summary>
    public static class Curry
    {
        /// <summary>
        /// Checks if the provided args deliver at least one argument.
        /// Otherwise returns null.
        /// </summary>
        /// <param name="function">The function to return or capture.</param>
        /// <param name="args">The args to check and potentially capture.</param>
        /// <returns>A curried function or null.</returns>
        public static Object MinOne(Function function, Object[] args)
        {
            return args.Length < 1 ? function : null;
        }

        /// <summary>
        /// Checks if the provided args deliver at least two arguments.
        /// Otherwise returns null.
        /// </summary>
        /// <param name="function">The function to return or capture.</param>
        /// <param name="args">The args to check and potentially capture.</param>
        /// <returns>A curried function or null.</returns>
        public static Object MinTwo(Function function, Object[] args)
        {
            if (args.Length < 2)
            {
                return args.Length == 0 ? function : rest => function(Recombine2(args, rest));
            }

            return null;
        }

        /// <summary>
        /// Checks if the provided args deliver at least three arguments.
        /// Otherwise returns null.
        /// </summary>
        /// <param name="function">The function to return or capture.</param>
        /// <param name="args">The args to check and potentially capture.</param>
        /// <returns>A curried function or null.</returns>
        public static Object MinThree(Function function, Object[] args)
        {
            if (args.Length < 3)
            {
                return args.Length == 0 ? function : rest => function(RecombineN(args, rest));
            }

            return null;
        }

        /// <summary>
        /// Checks if the provided args deliver at least count argument(s).
        /// Otherwise returns null.
        /// </summary>
        /// <param name="count">The required number of arguments.</param>
        /// <param name="function">The function to return or capture.</param>
        /// <param name="args">The args to check and potentially capture.</param>
        /// <returns>A curried function or null.</returns>
        public static Object Min(Int32 count, Function function, Object[] args)
        {
            if (args.Length < count)
            {
                return args.Length == 0 ? function : rest => function(RecombineN(args, rest));
            }

            return null;
        }

        public static Function Shuffle(Object[] args)
        {
            var end = args.Length - 1;
            var target = args[end] as Function;

            if (target != null)
            {
                var wrapper = target.Target as LocalFunction;
                var parameters = wrapper?.Parameters;

                if (parameters != null)
                {
                    var indices = new Int32[parameters.Length];
                    var start = 0;

                    for (var i = 0; i < indices.Length; i++)
                    {
                        indices[i] = i;
                    }

                    foreach (var arg in args)
                    {
                        var s = arg as String;

                        if (s != null)
                        {
                            for (var j = 0; j < parameters.Length; j++)
                            {
                                if (parameters[j].Equals(s, StringComparison.Ordinal))
                                {
                                    for (var i = 0; i < j; i++)
                                    {
                                        if (indices[i] >= start)
                                        {
                                            indices[i]++;
                                        }
                                    }

                                    indices[j] = start++;
                                    break;
                                }
                            }
                        }
                    }

                    return new Function(oldArgs =>
                    {
                        var newArgs = new Object[indices.Length];

                        for (var i = 0; i < newArgs.Length; i++)
                        {
                            var index = indices[i];

                            if (index < oldArgs.Length)
                            {
                                newArgs[i] = oldArgs[index];
                            }
                        }

                        return target.Invoke(newArgs);
                    });
                }
            }

            return target;
        }

        private static Object[] Recombine2(Object[] oldArgs, Object[] newArgs)
        {
            return newArgs.Length > 0 ? new[] { oldArgs[0], newArgs[0] } : oldArgs;
        }

        private static Object[] RecombineN(Object[] oldArgs, Object[] newArgs)
        {
            if (newArgs.Length > 0)
            {
                var args = new Object[oldArgs.Length + newArgs.Length];
                oldArgs.CopyTo(args, 0);
                newArgs.CopyTo(args, oldArgs.Length);
                return args;
            }

            return oldArgs;
        }
    }
}
