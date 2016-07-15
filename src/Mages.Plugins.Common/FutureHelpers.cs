namespace Mages.Plugins
{
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
                    future.SetError(tc.Exception.InnerException.Message);
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
                    future.SetError(tc.Exception.InnerException.Message);
                }
                else
                {
                    future.SetResult(transformer.Invoke(tc.Result));
                }
            });

            return future;
        }

        sealed class Future : Dictionary<String, Object>
        {
            public Future()
            {
                Add("done", false);
                Add("result", null);
                Add("error", null);
                Add("notify", null);
            }

            public void SetResult(Object result)
            {
                this["result"] = result;
                SetDone(result, null);
            }

            public void SetError(String error)
            {
                this["error"] = error;
                SetDone(null, error);
            }

            public void SetDone(Object result, String error)
            {
                var notify = this["notify"] as Delegate;
                this["done"] = true;

                if (notify != null)
                {
                    notify.DynamicInvoke(new Object[] { new Object[] { result, error } });
                }
            }
        }
    }
}
