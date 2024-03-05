namespace Mages.Core.Ast
{
    /// <summary>
    /// Represents an abstract (compile-time) scope information.
    /// </summary>
    /// <remarks>
    /// Creates a new abstract scope.
    /// </remarks>
    /// <param name="parent">The parent scope to use, if any.</param>
    public sealed class AbstractScope(AbstractScope parent)
    {
        #region Fields

        private readonly AbstractScope _parent = parent;

        #endregion
        #region ctor

        #endregion

        #region Properties

        /// <summary>
        /// Gets the parent scope.
        /// </summary>
        public AbstractScope Parent => _parent;

        #endregion
    }
}
