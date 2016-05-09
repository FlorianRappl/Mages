namespace Mages.Core.Ast
{
    /// <summary>
    /// Represents an abstract (compile-time) scope information.
    /// </summary>
    public sealed class AbstractScope
    {
        #region Fields

        private readonly AbstractScope _parent;

        #endregion

        #region ctor

        public AbstractScope(AbstractScope parent)
        {
            _parent = parent;
        }

        #endregion

        #region Properties

        public AbstractScope Parent
        {
            get { return _parent; }
        }

        #endregion
    }
}
