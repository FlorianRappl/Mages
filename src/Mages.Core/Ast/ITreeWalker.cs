namespace Mages.Core.Ast
{
    using Mages.Core.Ast.Expressions;

    /// <summary>
    /// Represents a syntax tree walker.
    /// </summary>
    public interface ITreeWalker
    {
        void Visit(EmptyExpression expression);

        void Visit(ConstantExpression expression);

        void Visit(ArgumentsExpression expression);

        void Visit(AssignmentExpression expression);

        void Visit(BinaryExpression expression);

        void Visit(PreUnaryExpression expression);

        void Visit(PostUnaryExpression expression);

        void Visit(RangeExpression expression);

        void Visit(ConditionalExpression expression);

        void Visit(CallExpression expression);

        void Visit(ObjectExpression expression);

        void Visit(PropertyExpression expression);

        void Visit(MatrixExpression expression);

        void Visit(FunctionExpression expression);

        void Visit(InvalidExpression expression);

        void Visit(IdentifierExpression expression);

        void Visit(MemberExpression expression);

        void Visit(ParameterExpression expression);

        void Visit(VariableExpression expression);
    }
}
