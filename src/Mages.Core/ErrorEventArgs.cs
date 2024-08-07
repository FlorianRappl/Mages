namespace Mages.Core;

using System;

/// <summary>
/// Arguments for the event when an error occurs.
/// </summary>
public class ErrorEventArgs : EventArgs
{
    /// <summary>
    /// Creates a new event arguments object.
    /// </summary>
    /// <param name="origin">The origin of the error.</param>
    /// <param name="error">The actual error.</param>
    public ErrorEventArgs(Object origin, Exception error)
    {
        Origin = origin;
        Error = error;
    }

    /// <summary>
    /// Gets the origin object of the error.
    /// </summary>
    public Object Origin { get; }

    /// <summary>
    /// Gets the actually emitted error.
    /// </summary>
    public Exception Error { get; }

    /// <summary>
    /// Gets or sets if the error has been actually handled.
    /// Starts false - so if you want to handle it make it true.
    /// </summary>
    public Boolean IsHandled { get; set; }
}
