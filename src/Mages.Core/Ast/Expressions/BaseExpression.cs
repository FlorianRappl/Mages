namespace Mages.Core.Ast.Expressions
{
    abstract class BaseExpression
    {
        #region Fields

        private readonly ITextPosition _start;
        private readonly ITextPosition _end;

        #endregion

        #region ctor

        public BaseExpression(ITextPosition start, ITextPosition end)
        {
            _start = start;
            _end = end;
        }

        #endregion

        #region Properties

        public ITextPosition Start
        {
            get { return _start; }
        }

        public ITextPosition End
        {
            get { return _end; }
        }

        #endregion
    }
}
