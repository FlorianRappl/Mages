namespace Mages.Core.Tokens;

static class TextPositionExtensions
{
    public static ITextRange ToRange(this TextPosition position)
    {
        var start = position;
        var end = new TextPosition(start.Row, start.Column + 1, start.Index + 1);
        return start.To(end);
    }

    public static ITextRange From(this TextPosition end, TextPosition start) => start.To(end);

    public static ITextRange To(this TextPosition start, TextPosition end) => new TextRange(start, end);

    readonly struct TextRange(TextPosition start, TextPosition end) : ITextRange
    {
        private readonly TextPosition _start = start;
        private readonly TextPosition _end = end;

        public TextPosition Start => _start;

        public TextPosition End => _end;
    }
}
