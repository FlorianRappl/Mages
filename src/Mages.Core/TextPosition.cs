namespace Mages.Core;

using System;

/// <summary>
/// Represents a position within a text source.
/// </summary>
/// <remarks>
/// Creates a new text position.
/// </remarks>
/// <param name="row">The row number.</param>
/// <param name="column">The column number.</param>
/// <param name="index">The character index.</param>
public struct TextPosition(Int32 row, Int32 column, Int32 index) : IEquatable<TextPosition>
{
    #region Fields

    private Int32 _row = row;
    private Int32 _column = column;
    private Int32 _index = index;

    #endregion
    #region ctor

    #endregion

    #region Properties

    /// <summary>
    /// Gets the row in the source code.
    /// </summary>
    public Int32 Row => _row;

    /// <summary>
    /// Gets the column in the source code.
    /// </summary>
    public Int32 Column => _column;

    /// <summary>
    /// Gets the index (absolute position) in the source code.
    /// </summary>
    public Int32 Index => _index;

    #endregion

    #region Operators

    /// <summary>
    /// Compares the index of the left text position against the index of the right text position.
    /// </summary>
    public static Boolean operator <(TextPosition left, TextPosition right)
    {
        return left.Index < right.Index;
    }

    /// <summary>
    /// Compares the index of the left text position against the index of the right text position.
    /// </summary>
    public static Boolean operator >(TextPosition left, TextPosition right)
    {
        return left.Index > right.Index;
    }

    /// <summary>
    /// Compares the index of the left text position against the index of the right text position.
    /// </summary>
    public static Boolean operator <=(TextPosition left, TextPosition right)
    {
        return left.Index <= right.Index;
    }

    /// <summary>
    /// Compares the index of the left text position against the index of the right text position.
    /// </summary>
    public static Boolean operator >=(TextPosition left, TextPosition right)
    {
        return left.Index >= right.Index;
    }

    /// <summary>
    /// Compares the index of the left text position against the index of the right text position.
    /// </summary>
    public static Boolean operator ==(TextPosition left, TextPosition right)
    {
        return left.Index == right.Index;
    }

    /// <summary>
    /// Compares the index of the left text position against the index of the right text position.
    /// </summary>
    public static Boolean operator !=(TextPosition left, TextPosition right)
    {
        return left.Index != right.Index;
    }

    #endregion

    #region Equatable

    /// <summary>
    /// Checks the types for equality.
    /// </summary>
    public override Boolean Equals(Object obj)
    {
        var other = obj as TextPosition?;
        return other.HasValue ? Equals(other.Value) : false;
    }

    /// <summary>
    /// Returns the index of the text position.
    /// </summary>
    public override Int32 GetHashCode()
    {
        return _index;
    }

    /// <summary>
    /// Checks the types for equality.
    /// </summary>
    public Boolean Equals(TextPosition other)
    {
        return this == other;
    }

    #endregion

    #region ToString

    /// <inheritdoc />
    public override String ToString()
    {
        return $"{_index}:{_row},{_column}";
    }

    #endregion
}
