namespace Mages.Core
{
    using System;

    /// <summary>
    /// Represents a position within a text source.
    /// </summary>
    public interface ITextPosition
    {
        /// <summary>
        /// Gets the row in the source code.
        /// </summary>
        Int32 Row { get; }

        /// <summary>
        /// Gets the column in the source code.
        /// </summary>
        Int32 Column { get; }

        /// <summary>
        /// Gets the index (absolute position) in the source code.
        /// </summary>
        Int32 Index { get; }
    }
}
