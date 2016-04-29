namespace Mages.Core.Vm
{
    using System;

    /// <summary>
    /// Represents the model of the MAGES VM.
    /// </summary>
    public interface IExecutionContext
    {
        Int32 Position { get; set; }

        void Push(Object value);

        Object Pop();
    }
}
