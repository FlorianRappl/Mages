namespace Mages.Core.Ast.Statements
{
    /// <summary>
    /// Represents the shared core of all statements.
    /// </summary>
    abstract class BaseStatement
    {
        #region Fields

        private readonly TextPosition _start;
        private readonly TextPosition _end;

        #endregion

        #region ctor

        public BaseStatement(TextPosition start, TextPosition end)
        {
            _start = start;
            _end = end;
        }

        #endregion

        #region Properties

        public TextPosition Start
        {
            get { return _start; }
        }

        public TextPosition End
        {
            get { return _end; }
        }

        #endregion
    }
}
