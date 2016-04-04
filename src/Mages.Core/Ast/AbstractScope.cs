namespace Mages.Core.Ast
{
    using Mages.Core.Ast.Expressions;
    using System;
    using System.Collections.Generic;

    sealed class AbstractScope
    {
        #region Fields

        private readonly AbstractScope _parent;
        private readonly Dictionary<String, List<VariableExpression>> _references;

        #endregion

        #region ctor

        public AbstractScope(AbstractScope parent)
        {
            _parent = parent;
            _references = new Dictionary<String, List<VariableExpression>>();
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
