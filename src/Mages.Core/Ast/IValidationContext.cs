namespace Mages.Core.Ast
{
    /// <summary>
    /// Represents the validation context.
    /// </summary>
    public interface IValidationContext
    {
        /// <summary>
        /// Adds an error to the validation context.
        /// </summary>
        /// <param name="error">The error to add.</param>
        void Report(ParseError error);
    }
}
