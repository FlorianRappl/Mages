namespace Mages.Core.Vm.Operations
{
    using System;
    using System.Collections.Generic;

    sealed class AwaitOperation : IOperation
    {
        public static readonly IOperation Instance = new AwaitOperation();

        private AwaitOperation()
        {
        }

        public void Invoke(IExecutionContext context)
        {
            var value = context.Pop();
            var task = value as IDictionary<String, Object>;

            if (IsFuture(task))
            {
                if ((Boolean)task["done"] == false)
                {
                    var position = context.Position;
                    context.Stop();

                    task["notify"] = new Function(args =>
                    {
                        Conclude(task, context);
                        context.Position = position + 1;
                        (context as ExecutionContext).Execute();
                        return null;
                    });
                }
                else
                {
                    Conclude(task, context);
                }
            }
            else
            {
                context.Push(value);
            }
        }

        private void Conclude(IDictionary<String, Object> task, IExecutionContext context)
        {
            var error = task["error"] as String;

            if (error != null)
                throw new Exception(error);

            context.Push(task["result"]);
        }

        private static Boolean IsFuture(IDictionary<String, Object> task)
        {
            return task != null && 
                task.ContainsKey("result") &&
                task.ContainsKey("notify") &&
                task.ContainsKey("error") &&
                task.ContainsKey("done") && task["done"] is Boolean;
        }

        public override String ToString()
        {
            return "cond";
        }
    }
}
