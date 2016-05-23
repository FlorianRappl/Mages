﻿namespace Mages.Core
{
    using System.ComponentModel;

    /// <summary>
    /// A list of possible parsing error codes.
    /// </summary>
    public enum ErrorCode
    {
        /// <summary>
        /// See description.
        /// </summary>
        [Description("A terminator has been expected ( ';' ).")]
        TerminatorExpected,
        /// <summary>
        /// See description.
        /// </summary>
        [Description("The input does not represent a valid identifier.")]
        IdentifierExpected,
        /// <summary>
        /// See description.
        /// </summary>
        [Description("The expression term is invalid.")]
        InvalidExpression,
        /// <summary>
        /// See description.
        /// </summary>
        [Description("A block of statements needs to be properly terminated ( '}' ).")]
        BlockNotTerminated,
        /// <summary>
        /// See description.
        /// </summary>
        [Description("The given symbol cannot be used.")]
        InvalidSymbol,
        /// <summary>
        /// See description.
        /// </summary>
        [Description("The scientific notation is missing the exponent.")]
        ScientificMismatch,
        /// <summary>
        /// See description.
        /// </summary>
        [Description("The floating point number is truncated.")]
        FloatingMismatch,
        /// <summary>
        /// See description.
        /// </summary>
        [Description("A block comment needs to be properly terminated ( '*/' ).")]
        BlockCommentNotTerminated,
        /// <summary>
        /// See description.
        /// </summary>
        [Description("The string literal is missing the terminator ( '\"' ).")]
        StringMismatch,
        /// <summary>
        /// See description.
        /// </summary>
        [Description("The given escape sequence is invalid.")]
        EscapeSequenceInvalid,
        /// <summary>
        /// See description.
        /// </summary>
        [Description("The ASCII character escape sequence is invalid.")]
        AsciiSequenceInvalid,
        /// <summary>
        /// See description.
        /// </summary>
        [Description("The unicode character escape sequence is invalid.")]
        UnicodeSequenceInvalid,
        /// <summary>
        /// See description.
        /// </summary>
        [Description("A unary operator requires at least one operand.")]
        OperandRequired,
        /// <summary>
        /// See description.
        /// </summary>
        [Description("The increment operand has to be a valid identifier.")]
        IncrementOperand,
        /// <summary>
        /// See description.
        /// </summary>
        [Description("The decrement operand has to be a valid identifier.")]
        DecrementOperand,
        /// <summary>
        /// See description.
        /// </summary>
        [Description("The binary operator is missing a left operand.")]
        LeftOperandRequired,
        /// <summary>
        /// See description.
        /// </summary>
        [Description("The binary operator is missing a right operand.")]
        RightOperandRequired,
        /// <summary>
        /// See description.
        /// </summary>
        [Description("A range requires a valid start value.")]
        RangeStartRequired,
        /// <summary>
        /// See description.
        /// </summary>
        [Description("The step of a range has to be valid.")]
        RangeStepError,
        /// <summary>
        /// See description.
        /// </summary>
        [Description("A range must have a valid end value.")]
        RangeEndRequired,
        /// <summary>
        /// See description.
        /// </summary>
        [Description("The given member is invalid. Only valid identifiers represent members.")]
        MemberInvalid,
        /// <summary>
        /// See description.
        /// </summary>
        [Description("The matrix needs to be properly terminated ( ']' ).")]
        MatrixNotTerminated,
        /// <summary>
        /// See description.
        /// </summary>
        [Description("The object needs to be properly terminated ( '}' ).")]
        ObjectNotTerminated,
        /// <summary>
        /// See description.
        /// </summary>
        [Description("An expected closing bracket could not be found ( ']' ).")]
        IndicesNotTerminated,
        /// <summary>
        /// See description.
        /// </summary>
        [Description("An expected closing paranthesis could not be found ( ')' ).")]
        BracketNotTerminated,
        /// <summary>
        /// See description.
        /// </summary>
        [Description("An expected opening bracket could not be found ( '{' ).")]
        BracketExpected,
        /// <summary>
        /// See description.
        /// </summary>
        [Description("An assignable expression has been expected.")]
        AssignableExpected,
        /// <summary>
        /// See description.
        /// </summary>
        [Description("Too many indices provided. A maximum of 2 indices is allowed.")]
        TooManyIndices,
        /// <summary>
        /// See description.
        /// </summary>
        [Description("The conditional operator is missing the alternative branch.")]
        BranchMissing,
        /// <summary>
        /// See description.
        /// </summary>
        [Description("The given keyword has been misplaced.")]
        KeywordUnexpected,
        /// <summary>
        /// See description.
        /// </summary>
        [Description("An expected colon has not been found.")]
        ColonExpected,
    }
}
