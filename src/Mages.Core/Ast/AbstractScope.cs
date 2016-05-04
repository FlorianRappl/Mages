namespace Mages.Core.Ast
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Represents an abstract (compile-time) scope information.
    /// </summary>
    public sealed class AbstractScope
    {
        #region Fields

        private readonly AbstractScope _parent;
        private readonly Dictionary<String, IExpression> _references;

        #endregion

        #region ctor

        public AbstractScope(AbstractScope parent)
        {
            _parent = parent;
            _references = new Dictionary<String, IExpression>();
        }

        #endregion

        #region Properties

        public AbstractScope Parent
        {
            get { return _parent; }
        }

        #endregion

        #region Methods

        public void Provide(String identifier, IExpression expression)
        {
            _references.Add(identifier, expression);
        }

        public IExpression Find(String identifier)
        {
            var expression = default(IExpression);
            _references.TryGetValue(identifier, out expression);
            return expression;
        }

        #endregion
    }
}
