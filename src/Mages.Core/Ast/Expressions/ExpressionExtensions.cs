using System;

namespace Mages.Core.Ast.Expressions;

static class ExpressionExtensions
{
    public static TextPosition GetStart(this IExpression[] expressions)
    {
        var length = expressions.Length;
        return length > 0 ? expressions[0].Start : default;
    }

    public static TextPosition GetEnd(this IExpression[] expressions)
    {
        var length = expressions.Length;
        return length > 0 ? expressions[length - 1].End : default;
    }

    public static void Validate(this IExpression[] expressions, IValidationContext context)
    {
        foreach (var expression in expressions)
        {
            expression.Validate(context);
        }
    }

    public static Boolean Is(this IExpression first, IExpression second)
    {
        if (Object.ReferenceEquals(first, second))
        {
            return true;
        }

        if (first is not null && second is not null)
        {
            if (first is IdentifierExpression i1 && second is IdentifierExpression i2)
            {
                return i1.Name == i2.Name;
            }

            if (first is VariableExpression v1 && second is VariableExpression v2)
            {
                return v1.Scope == v2.Scope && v1.Name == v2.Name;
            }

            if (first is MemberExpression m1 && second is MemberExpression m2)
            {
                return m1.Object.Is(m2.Object) && m1.Member.Is(m2.Member);
            }

            if (first is ConstantExpression c1 && second is ConstantExpression c2)
            {
                return c1.Value == c2.Value;
            }
        }

        return false;
    }
}
