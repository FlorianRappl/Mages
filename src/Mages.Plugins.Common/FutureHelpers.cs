namespace Mages.Plugins
{
    using Mages.Core.Runtime;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public static class FutureHelpers
    {
        public static IDictionary<String, Object> AsFuture(this Task task)
        {
            return task.AsFuture(() => { });
        }

        public static IDictionary<String, Object> AsFuture(this Task task, Action cleanup)
        {
            var future = new Future();
            task.ContinueWith(tc =>
            {
                cleanup.Invoke();

                if (tc.IsFaulted)
                {
                    future.SetError(tc.Exception.InnerException);
                }
                else
                {
                    future.SetResult(null);
                }
            });

            return future;
        }

        public static IDictionary<String, Object> AsFuture<T>(this Task<T> task)
        {
            return task.AsFuture(m => m);
        }

        public static IDictionary<String, Object> AsFuture<T>(this Task<T> task, Action cleanup)
        {
            return task.AsFuture(cleanup, m => m);
        }

        public static IDictionary<String, Object> AsFuture<T>(this Task<T> task, Func<T, Object> transformer)
        {
            return task.AsFuture(() => { }, transformer);
        }

        public static IDictionary<String, Object> AsFuture<T>(this Task<T> task, Action cleanup, Func<T, Object> transformer)
        {
            var future = new Future();
            task.ContinueWith(tc =>
            {
                cleanup.Invoke();

                if (tc.IsFaulted)
                {
                    future.SetError(tc.Exception.InnerException);
                }
                else
                {
                    future.SetResult(transformer.Invoke(tc.Result));
                }
            });

            return future;
        }
    }
}
