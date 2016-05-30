namespace Mages.Core.Runtime.Functions
{
    using System;

    static class Curry
    {
        public static Object MinOne(this Function function, Object[] args)
        {
            return args.Length < 1 ? function : null;
        }

        public static Object MinTwo(this Function function, Object[] args)
        {
            if (args.Length < 2)
            {
                return args.Length == 0 ? function : _ => function(Recombine(args, _));
            }

            return null;
        }

        private static Object[] Recombine(Object[] oldArgs, Object[] newArgs)
        {
            return newArgs.Length > 0 ? new[] { oldArgs[0], newArgs[0] } : oldArgs;
        }
    }
}
