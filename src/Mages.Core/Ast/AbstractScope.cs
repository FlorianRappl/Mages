namespace Mages.Core.Ast
{
    using System;
    using System.Collections.Generic;

    sealed class AbstractScope
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

        public AbstractScope Find(String identifier)
        {
            if (!_references.ContainsKey(identifier))
            {
                if (_parent == null)
                {
                    return null;
                }

                return _parent.Find(identifier);
            }

            return this;
        }

        #endregion
    }
}
