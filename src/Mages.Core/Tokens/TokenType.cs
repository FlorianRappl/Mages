namespace Mages.Core.Tokens
{
    /// <summary>
    /// The available token types.
    /// </summary>
    public enum TokenType
    {
        Unknown = 0,
        Keyword = 1,
        Identifier = 2,
        Number = 3,
        Unit = 4,
        Text = 5,
        OpenGroup = 6,
        CloseGroup = 7,
        OpenList = 8,
        CloseList = 9,
        OpenScope = 10,
        CloseScope = 11,
        Comma = 12,
        Operator = 13,
        Trivia = 14,

    }
}
