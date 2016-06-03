namespace Mages.Core.Ast
{
    using System;

    /// <summary>
    /// An abstract statement from the AST.
    /// </summary>
    public interface IStatement : IValidatable, IWalkable
    {
        /// <summary>
        /// Gets if the statement is used as a container.
        /// </summary>
        Boolean IsContainer { get; }
    }
}
