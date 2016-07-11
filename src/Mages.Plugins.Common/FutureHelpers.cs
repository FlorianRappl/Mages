namespace Mages.Plugins
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public static class FutureHelpers
    {
        public static IDictionary<String, Object> AsFuture(this Task task, Action cleanup)
        {
            var obj = new Dictionary<String, Object>
            {
                { "done", false },
                { "result", null },
                { "error", null },
                { "notify", null }
            };
            task.ContinueWith(tc =>
            {
                cleanup.Invoke();
                var notify = obj["notify"] as Delegate;
                var error = default(String);

                if (tc.IsFaulted)
                {
                    error = tc.Exception.InnerException.Message;
                }

                obj["error"] = error;
                obj["done"] = true;

                if (notify != null)
                {
                    notify.DynamicInvoke(new Object[] { new Object[] { null, error } });
                }
            });

            return obj;
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
            var obj = new Dictionary<String, Object>
            {
                { "done", false },
                { "result", null },
                { "error", null },
                { "notify", null }
            };
            task.ContinueWith(tc =>
            {
                cleanup.Invoke();
                var notify = obj["notify"] as Delegate;
                var error = default(String);
                var result = default(Object);

                if (tc.IsFaulted)
                {
                    error = tc.Exception.InnerException.Message;
                }
                else
                {
                    result = transformer.Invoke(tc.Result);
                }

                obj["error"] = error;
                obj["done"] = true;
                obj["result"] = result;

                if (notify != null)
                {
                    notify.DynamicInvoke(new Object[] { new Object[] { result, error } });
                }
            });

            return obj;
        }
    }
}
