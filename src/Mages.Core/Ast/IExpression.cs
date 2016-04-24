namespace Mages.Core.Ast
{
    using System;

    /// <summary>
    /// An abstract expression from the AST.
    /// </summary>
    public interface IExpression : IValidatable
    {
        /// <summary>
        /// Gets if the expression can be used as a value container.
        /// </summary>
        Boolean IsAssignable { get; }

        /// <summary>
        /// Accepts the visitor by showing him around.
        /// </summary>
        /// <param name="visitor">The visitor walking the tree.</param>
        void Accept(ITreeWalker visitor);
    }
}
