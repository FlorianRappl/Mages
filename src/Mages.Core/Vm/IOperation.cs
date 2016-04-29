namespace Mages.Core.Vm
{
    /// <summary>
    /// Represents the core interface of an interpreted operation.
    /// </summary>
    public interface IOperation
    {
        void Invoke(IExecutionContext context);
    }
}
