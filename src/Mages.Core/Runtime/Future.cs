namespace Mages.Core.Runtime
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents an awaitable object definition.
    /// </summary>
    public sealed class Future : Dictionary<String, Object>
    {
        /// <summary>
        /// Creates a new future object.
        /// </summary>
        public Future()
        {
            Add("done", false);
            Add("result", null);
            Add("error", null);
            Add("notify", null);
        }

        /// <summary>
        /// Wraps a task in a future object.
        /// </summary>
        /// <param name="task">The task to wrap.</param>
        public Future(Task task) : this()
        {
            task.ContinueWith(result =>
            {
                if (result.IsFaulted)
                {
                    SetError(result.Exception.InnerException);
                }
                else
                {
                    var type = result.GetType();
                    var value = type.GetProperty("Result")?.GetValue(result, null);
                    SetResult(value.WrapObject());
                }
            });
        }

        /// <summary>
        /// Wraps a task in a future object.
        /// </summary>
        /// <param name="task">The task to wrap.</param>
        public Future(Task<Object> task) : this()
        {
            task.ContinueWith(result =>
            {
                if (result.IsFaulted)
                {
                    SetError(result.Exception.InnerException);
                }
                else
                {
                    SetResult(result.Result.WrapObject());
                }
            });
        }

        /// <summary>
        /// Gets if the result is already present.
        /// </summary>
        public Boolean IsCompleted
        {
            get
            {
                var result = this.GetProperty("done") as Boolean?;
                return result.HasValue && result.Value;
            }
        }

        /// <summary>
        /// Gets the result, if any.
        /// </summary>
        public Object Result => this.GetProperty("result");

        /// <summary>
        /// Gets the error message, if any.
        /// </summary>
        public Exception Error => this.GetProperty("error") as Exception;

        /// <summary>
        /// Sets the result in case of success.
        /// </summary>
        /// <param name="result">The concrete result, if any.</param>
        public void SetResult(Object result)
        {
            this["result"] = result;
            SetDone(result, null);
        }

        /// <summary>
        /// Sets the error message in case of failure.
        /// </summary>
        /// <param name="error">The specific error message.</param>
        public void SetError(Exception error)
        {
            this["error"] = error;
            SetDone(null, error);
        }

        /// <summary>
        /// Sets the callback to notify once finished. This function
        /// is immediately called if the result is already determined.
        /// </summary>
        /// <param name="callback">The callback action.</param>
        public void SetCallback(Action<Object, Exception> callback)
        {
            if (IsCompleted)
            {
                callback.Invoke(Result, Error);
            }
            else
            {
                this["notify"] = new Function(args =>
                {
                    callback.Invoke(Result, Error);
                    return null;
                });
            }
        }

        private void SetDone(Object result, Exception error)
        {
            this["done"] = true;

            if (this["notify"] is Function notify)
            {
                notify.Invoke([result, error]);
            }
        }

        /// <summary>
        /// Converts the future to a task.
        /// </summary>
        /// <param name="f">The future to convert to.</param>
        public static explicit operator Task<Object>(Future f)
        {
            var tcs = new TaskCompletionSource<Object>();
            f.SetCallback((value, error) =>
            {
                if (error is not null)
                {
                    tcs.SetException(error);
                }
                else
                {
                    tcs.SetResult(value);
                }
            });
            return tcs.Task;
        }
    }
}
