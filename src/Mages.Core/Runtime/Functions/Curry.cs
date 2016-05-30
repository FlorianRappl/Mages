namespace Mages.Core.Runtime.Functions
{
    using System;

    static class Curry
    {
        public static Object MinOne(Function function, Object[] args)
        {
            return args.Length < 1 ? function : null;
        }

        public static Object MinTwo(Function function, Object[] args)
        {
            if (args.Length < 2)
            {
                return args.Length == 0 ? function : _ => function(Recombine2(args, _));
            }

            return null;
        }

        public static Object Min(Int32 count, Function function, Object[] args)
        {
            if (args.Length < count)
            {
                return args.Length == 0 ? function : _ => function(RecombineN(args, _));
            }

            return null;
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
