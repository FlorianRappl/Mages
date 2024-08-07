namespace Mages.Core;

using System;
using Mages.Core.Vm;

/// <summary>
/// Arguments for the event when the execution ended.
/// </summary>
public class StoppedEventArgs : EventArgs
{
    /// <summary>
    /// Creates a new event arguments object.
    /// </summary>
    /// <param name="context">The created context.</param>
    public StoppedEventArgs(IExecutionContext context)
    {
        Context = context;
    }

    /// <summary>
    /// Gets the associated execution context.
    /// </summary>
    public IExecutionContext Context { get; }
}
