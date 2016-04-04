namespace Mages.Core.Ast.Expressions
{
    static class ExpressionExtensions
    {
        public static ITextPosition GetStart(this IExpression[] expressions)
        {
            var length = expressions.Length;
            return length > 0 ? expressions[0].Start : default(ITextPosition);
        }

        public static ITextPosition GetEnd(this IExpression[] expressions)
        {
            var length = expressions.Length;
            return length > 0 ? expressions[length - 1].End : default(ITextPosition);
        }

        public static void Validate(this IExpression[] expressions, IValidationContext context)
        {
            foreach (var expression in expressions)
            {
                expression.Validate(context);
            }
        }
    }
}
