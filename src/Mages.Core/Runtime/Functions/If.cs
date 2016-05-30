namespace Mages.Core.Runtime.Functions
{
    using System;

    static class If
    {
        public static Object Is<T>(Object[] args, Func<T, Object> f)
        {
            return args[0] is T ? f((T)args[0]) : null;
        }

        public static Object Is<Tx, Ty>(Object[] args, Func<Tx, Ty, Object> f)
        {
            return args[0] is Tx && args[1] is Ty ? f((Tx)args[0], (Ty)args[1]) : null;
        }
    }
}
