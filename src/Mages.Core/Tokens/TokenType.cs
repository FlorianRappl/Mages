namespace Mages.Core.Tokens
{
    /// <summary>
    /// The available token types.
    /// </summary>
    public enum TokenType
    {
        /// <summary>
        /// Unknown token type.
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// Keyword token.
        /// </summary>
        Keyword = 1,
        /// <summary>
        /// Identifier token.
        /// </summary>
        Identifier = 2,
        /// <summary>
        /// Number token.
        /// </summary>
        Number = 3,
        /// <summary>
        /// String token.
        /// </summary>
        Text = 5,
        /// <summary>
        /// Round bracket open.
        /// </summary>
        OpenGroup = 6,
        /// <summary>
        /// Round bracket close.
        /// </summary>
        CloseGroup = 7,
        /// <summary>
        /// Square bracket open.
        /// </summary>
        OpenList = 8,
        /// <summary>
        /// Square bracket close.
        /// </summary>
        CloseList = 9,
        /// <summary>
        /// Curly bracket open.
        /// </summary>
        OpenScope = 10,
        /// <summary>
        /// Curly bracket close.
        /// </summary>
        CloseScope = 11,
        /// <summary>
        /// Comma symbol.
        /// </summary>
        Comma = 12,
        /// <summary>
        /// Colon symbol.
        /// </summary>
        Colon = 13,
        /// <summary>
        /// Dot symbol.
        /// </summary>
        Dot = 14,
        /// <summary>
        /// Power operator.
        /// </summary>
        Power = 15,
        /// <summary>
        /// Right divide operator.
        /// </summary>
        RightDivide = 16,
        /// <summary>
        /// Left divide operator.
        /// </summary>
        LeftDivide = 17,
        /// <summary>
        /// Multiply operator.
        /// </summary>
        Multiply = 18,
        /// <summary>
        /// Modulo operator.
        /// </summary>
        Modulo = 19,
        /// <summary>
        /// Factorial operator.
        /// </summary>
        Factorial = 20,
        /// <summary>
        /// Add operator.
        /// </summary>
        Add = 21,
        /// <summary>
        /// Subtract operator.
        /// </summary>
        Subtract = 22,
        /// <summary>
        /// Increment operator.
        /// </summary>
        Increment = 23,
        /// <summary>
        /// Decrement operator.
        /// </summary>
        Decrement = 24,
        /// <summary>
        /// Or operator.
        /// </summary>
        Or = 25,
        /// <summary>
        /// And operator.
        /// </summary>
        And = 26,
        /// <summary>
        /// Equal operator.
        /// </summary>
        Equal = 27,
        /// <summary>
        /// Not equal operator.
        /// </summary>
        NotEqual = 28,
        /// <summary>
        /// Less operator.
        /// </summary>
        Less = 29,
        /// <summary>
        /// Less or equal operator.
        /// </summary>
        LessEqual = 30,
        /// <summary>
        /// Greater or equal operator.
        /// </summary>
        GreaterEqual = 31,
        /// <summary>
        /// Greater operator.
        /// </summary>
        Greater = 32,
        /// <summary>
        /// Negate operator.
        /// </summary>
        Negate = 33,
        /// <summary>
        /// Condition operator.
        /// </summary>
        Condition = 34,
        /// <summary>
        /// Transpose operator.
        /// </summary>
        Transpose = 35,
        /// <summary>
        /// Any kind of space symbol.
        /// </summary>
        Space = 36,
        /// <summary>
        /// Assignment operator.
        /// </summary>
        Assignment = 37,
        /// <summary>
        /// Lambda operator.
        /// </summary>
        Lambda = 38,
        /// <summary>
        /// Type operator.
        /// </summary>
        Type = 39,
        /// <summary>
        /// Pipe operator.
        /// </summary>
        Pipe = 40,
        /// <summary>
        /// Semicolon symbol.
        /// </summary>
        SemiColon = 0xff,
        /// <summary>
        /// Comment token.
        /// </summary>
        Comment = 0xfffe,
        /// <summary>
        /// EOF.
        /// </summary>
        End = 0xffff,
    }
}
