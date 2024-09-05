namespace Mages.Core.Runtime.Functions;

/// <summary>
/// The collection of all standard operators.
/// </summary>
public static class StandardOperators
{
    /// <summary>
    /// Contains the add operator.
    /// </summary>
    public static readonly Function Add = new(BinaryOperators.Add);

    /// <summary>
    /// Contains the and operator.
    /// </summary>
    public static readonly Function And = new(BinaryOperators.And);

    /// <summary>
    /// Contains the left division operator.
    /// </summary>
    public static readonly Function LDiv = new(BinaryOperators.LDiv);

    /// <summary>
    /// Contains the modulo operator.
    /// </summary>
    public static readonly Function Mod = new(BinaryOperators.Mod);

    /// <summary>
    /// Contains the multiplication operator.
    /// </summary>
    public static readonly Function Mul = new(BinaryOperators.Mul);

    /// <summary>
    /// Contains the or operator.
    /// </summary>
    public static readonly Function Or = new(BinaryOperators.Or);

    /// <summary>
    /// Contains the power operator.
    /// </summary>
    public static readonly Function Pow = new(BinaryOperators.Pow);

    /// <summary>
    /// Contains the right division operator.
    /// </summary>
    public static readonly Function RDiv = new(BinaryOperators.RDiv);

    /// <summary>
    /// Contains the subtraction operator.
    /// </summary>
    public static readonly Function Sub = new(BinaryOperators.Sub);

    /// <summary>
    /// Contains the equality operator.
    /// </summary>
    public static readonly Function Eq = new(BinaryOperators.Eq);

    /// <summary>
    /// Contains the not equals operator.
    /// </summary>
    public static readonly Function Neq = new(BinaryOperators.Neq);

    /// <summary>
    /// Contains the greater or equals operator.
    /// </summary>
    public static readonly Function Geq = new(BinaryOperators.Geq);

    /// <summary>
    /// Contains the greater than operator.
    /// </summary>
    public static readonly Function Gt = new(BinaryOperators.Gt);

    /// <summary>
    /// Contains the less or equals operator.
    /// </summary>
    public static readonly Function Leq = new(BinaryOperators.Leq);

    /// <summary>
    /// Contains the less than operator.
    /// </summary>
    public static readonly Function Lt = new(BinaryOperators.Lt);

    /// <summary>
    /// Contains the pipe operator.
    /// </summary>
    public static readonly Function Pipe = new(BinaryOperators.Pipe);

    /// <summary>
    /// Contains the factorial function.
    /// </summary>
    public static readonly Function Factorial = new(UnaryOperators.Factorial);

    /// <summary>
    /// Contains the transpose operator.
    /// </summary>
    public static readonly Function Transpose = new(UnaryOperators.Transpose);

    /// <summary>
    /// Contains the negation operator.
    /// </summary>
    public static readonly Function Not = new(UnaryOperators.Not);

    /// <summary>
    /// Contains the positive operator.
    /// </summary>
    public static readonly Function Positive = new(UnaryOperators.Positive);

    /// <summary>
    /// Contains the negative operator.
    /// </summary>
    public static readonly Function Negative = new(UnaryOperators.Negative);

    /// <summary>
    /// Wraps the Math.Abs function.
    /// </summary>
    public static readonly Function Abs = new(UnaryOperators.Abs);

    /// <summary>
    /// Contains the type operator.
    /// </summary>
    public static readonly Function Type = new(UnaryOperators.Type);
}
