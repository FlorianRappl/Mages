namespace Mages.Core
{
    using System.ComponentModel;

    /// <summary>
    /// A list of possible parsing error codes.
    /// </summary>
    public enum ErrorCode
    {
        [Description("A terminator has been expected ( ';' ).")]
        TerminatorExpected,
        [Description("The input does not represent a valid identifier.")]
        IdentifierExpected,
        [Description("The expression term is invalid.")]
        InvalidExpression,
        [Description("A block of statements needs to be properly terminated ( '}' ).")]
        BlockNotTerminated,
        [Description("The given symbol cannot be used.")]
        InvalidSymbol,
        [Description("The scientific notation is missing the exponent.")]
        ScientificMismatch,
        [Description("The floating point number is truncated.")]
        FloatingMismatch,
        [Description("A block comment needs to be properly terminated ( '*/' ).")]
        BlockCommentNotTerminated,
        [Description("The string literal is missing the terminator ( '\"' ).")]
        StringMismatch,
        [Description("The given escape sequence is invalid.")]
        EscapeSequenceInvalid,
        [Description("The ASCII character escape sequence is invalid.")]
        AsciiSequenceInvalid,
        [Description("The unicode character escape sequence is invalid.")]
        UnicodeSequenceInvalid,
        [Description("A unary operator requires at least one operand.")]
        OperandRequired,
        [Description("The increment operand has to be a valid identifier.")]
        IncrementOperand,
        [Description("The decrement operand has to be a valid identifier.")]
        DecrementOperand,
        [Description("The binary operator is missing a left operand.")]
        LeftOperandRequired,
        [Description("The binary operator is missing a right operand.")]
        RightOperandRequired,
        [Description("A range requires a valid start value.")]
        RangeStartRequired,
        [Description("The step of a range has to be valid.")]
        RangeStepError,
        [Description("A range must have a valid end value.")]
        RangeEndRequired,
        [Description("The given member is invalid. Only valid identifiers represent members.")]
        MemberInvalid,
        [Description("The matrix needs to be properly terminated ( ']' ).")]
        MatrixNotTerminated,
        [Description("The object needs to be properly terminated ( '}' ).")]
        ObjectNotTerminated,
        [Description("An expected closing bracket could not be found ( ']' ).")]
        IndicesNotTerminated,
        [Description("An expected closing paranthesis could not be found ( ')' ).")]
        BracketNotTerminated,
        [Description("An assignable expression has been expected.")]
        AssignableExpected,
        [Description("Too many indices provided. A maximum of 2 indices is allowed.")]
        TooManyIndices,
        [Description("The conditional operator is missing the alternative branch.")]
        BranchMissing,
        [Description("The given keyword has been misplaced.")]
        KeywordUnexpected,
        [Description("An expected colon has not been found.")]
        ColonExpected,
    }
}
