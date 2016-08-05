namespace Mages.Core.Runtime
{
    using System;
    using System.Collections.Generic;

    public sealed class Future : Dictionary<String, Object>
    {
        public Future()
        {
            Add("done", false);
            Add("result", null);
            Add("error", null);
            Add("notify", null);
        }

        public Boolean IsCompleted
        {
            get
            {
                var result = this.GetProperty("done") as Boolean?;
                return result.HasValue && result.Value;
            }
        }

        public Object Result
        {
            get { return this.GetProperty("result"); }
        }

        public String Error
        {
            get { return this.GetProperty("error") as String; }
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

        public void SetCallback(Action<Object, String> callback)
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

        private void SetDone(Object result, String error)
        {
            var notify = this["notify"] as Function;
            this["done"] = true;

            if (notify != null)
            {
                notify.Invoke(new [] { result, error });
            }
        }
    }
}
