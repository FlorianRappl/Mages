namespace Mages.Core.Vm
{
    /// <summary>
    /// Extension methods for IExecutionContext instances.
    /// </summary>
    public static class ExecutionContextExtensions
    {
        /// <summary>
        /// Stops the execution of the given context.
        /// </summary>
        /// <param name="context">The context to stop.</param>
        public static void Stop(this IExecutionContext context)
        {
            context.Position = context.End;
        }
    }
}
