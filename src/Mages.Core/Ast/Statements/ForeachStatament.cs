using Mages.Core.Ast.Statements;
using System;

namespace Mages.Core.Ast
{
    internal class ForeachStatament : BaseStatement, IStatement
    {
        public ForeachStatament(TextPosition start, TextPosition end) : base(start,end)
        {

        }

        public void Accept(ITreeWalker visitor)
        {
            //visitor.Visit(this);
        }

        public void Validate(IValidationContext context)
        {
            throw new NotImplementedException();
        }
    }
}