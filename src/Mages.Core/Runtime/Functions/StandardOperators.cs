namespace Mages.Core.Runtime.Functions;

/// <summary>
/// The collection of all standard operators.
/// </summary>
public static class StandardOperators
{
    /// <summary>
    /// Contains the add operator.
    /// </summary>
    public static readonly Function Add = Helpers.DeclareFunction(BinaryOperators.Add, ["y", "x"]);

    /// <summary>
    /// Contains the and operator.
    /// </summary>
    public static readonly Function And = Helpers.DeclareFunction(BinaryOperators.And, ["b", "a"]);

    /// <summary>
    /// Contains the left division operator.
    /// </summary>
    public static readonly Function LDiv = Helpers.DeclareFunction(BinaryOperators.LDiv, ["y", "x"]);

    /// <summary>
    /// Contains the modulo operator.
    /// </summary>
    public static readonly Function Mod = Helpers.DeclareFunction(BinaryOperators.Mod, ["d", "n"]);

    /// <summary>
    /// Contains the multiplication operator.
    /// </summary>
    public static readonly Function Mul = Helpers.DeclareFunction(BinaryOperators.Mul, ["y", "x"]);

    /// <summary>
    /// Contains the or operator.
    /// </summary>
    public static readonly Function Or = Helpers.DeclareFunction(BinaryOperators.Or, ["b", "a"]);

    /// <summary>
    /// Contains the power operator.
    /// </summary>
    public static readonly Function Pow = Helpers.DeclareFunction(BinaryOperators.Pow, ["y", "x"]);

    /// <summary>
    /// Contains the right division operator.
    /// </summary>
    public static readonly Function RDiv = Helpers.DeclareFunction(BinaryOperators.RDiv, ["x", "y"]);

    /// <summary>
    /// Contains the subtraction operator.
    /// </summary>
    public static readonly Function Sub = Helpers.DeclareFunction(BinaryOperators.Sub, ["y", "x"]);

    /// <summary>
    /// Contains the equality operator.
    /// </summary>
    public static readonly Function Eq = Helpers.DeclareFunction(BinaryOperators.Eq, ["y", "x"]);

    /// <summary>
    /// Contains the not equals operator.
    /// </summary>
    public static readonly Function Neq = Helpers.DeclareFunction(BinaryOperators.Neq, ["y", "x"]);

    /// <summary>
    /// Contains the greater or equals operator.
    /// </summary>
    public static readonly Function Geq = Helpers.DeclareFunction(BinaryOperators.Geq, ["y", "x"]);

    /// <summary>
    /// Contains the greater than operator.
    /// </summary>
    public static readonly Function Gt = Helpers.DeclareFunction(BinaryOperators.Gt, ["y", "x"]);

    /// <summary>
    /// Contains the less or equals operator.
    /// </summary>
    public static readonly Function Leq = Helpers.DeclareFunction(BinaryOperators.Leq, ["y", "x"]);

    /// <summary>
    /// Contains the less than operator.
    /// </summary>
    public static readonly Function Lt = Helpers.DeclareFunction(BinaryOperators.Lt, ["y", "x"]);

    /// <summary>
    /// Contains the pipe operator.
    /// </summary>
    public static readonly Function Pipe = Helpers.DeclareFunction(BinaryOperators.Pipe, ["fn", "arg"]);

    /// <summary>
    /// Contains the factorial function.
    /// </summary>
    public static readonly Function Factorial = Helpers.DeclareFunction(UnaryOperators.Factorial, ["n"]);

    /// <summary>
    /// Contains the transpose operator.
    /// </summary>
    public static readonly Function Transpose = Helpers.DeclareFunction(UnaryOperators.Transpose, ["mat"]);

    /// <summary>
    /// Contains the negation operator.
    /// </summary>
    public static readonly Function Not = Helpers.DeclareFunction(UnaryOperators.Not, ["value"]);

    /// <summary>
    /// Contains the positive operator.
    /// </summary>
    public static readonly Function Positive = Helpers.DeclareFunction(UnaryOperators.Positive, ["x"]);

    /// <summary>
    /// Contains the negative operator.
    /// </summary>
    public static readonly Function Negative = Helpers.DeclareFunction(UnaryOperators.Negative, ["x"]);

    /// <summary>
    /// Wraps the Math.Abs function.
    /// </summary>
    public static readonly Function Abs = Helpers.DeclareFunction(UnaryOperators.Abs, ["value"]);

    /// <summary>
    /// Contains the type operator.
    /// </summary>
    public static readonly Function Type = Helpers.DeclareFunction(UnaryOperators.Type, ["instance"]);
}
