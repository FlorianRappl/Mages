namespace Mages.Plugins.FileSystem
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    static class Helpers
    {
        public static Object AsFuture(this Task task, Action cleanup)
        {
            var obj = new Dictionary<String, Object>
            {
                { "done", false },
                { "result", null },
                { "error", null }
            };
            task.ContinueWith(tc =>
            {
                cleanup.Invoke();

                if (tc.IsFaulted)
                {
                    obj["error"] = tc.Exception.InnerException.Message;
                }

                obj["done"] = true;
            });

            return obj;
        }

        public static Object AsFuture<T>(this Task<T> task, Action cleanup)
        {
            return task.AsFuture(cleanup, m => m);
        }

        public static Object AsFuture<T>(this Task<T> task, Action cleanup, Func<T, Object> transformer)
        {
            var obj = new Dictionary<String, Object>
            {
                { "done", false },
                { "result", null },
                { "error", null }
            };
            task.ContinueWith(tc =>
            {
                cleanup.Invoke();

                if (tc.IsFaulted)
                {
                    obj["error"] = tc.Exception.InnerException.Message;
                }
                else
                {
                    obj["result"] = transformer.Invoke(tc.Result);
                }

                obj["done"] = true;
            });

            return obj;
        }
    }
}
