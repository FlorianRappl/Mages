namespace Mages.Core.Vm
{
    static class ExecutionContextExtensions
    {
        public static void Stop(this IExecutionContext context)
        {
            context.Position = context.End;
        }
    }
}
