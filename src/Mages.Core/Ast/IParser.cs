namespace Mages.Core.Ast
{
    using Mages.Core.Tokens;
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents the core parser interface.
    /// </summary>
    public interface IParser
    {
        /// <summary>
        /// Parses the next expression.
        /// </summary>
        /// <param name="tokens">The stream of tokens.</param>
        /// <returns>The parsed expression.</returns>
        IExpression ParseExpression(IEnumerator<IToken> tokens);
    }
}
