namespace Mages.Core.Tokens;

using System;

sealed class CharacterToken(TokenType type, Int32 character, TextPosition position) : IToken
{
    private readonly TokenType _type = type;
    private readonly Int32 _character = character;
    private readonly TextPosition _position = position;

    public TokenType Type => _type;

    public String Payload => Char.ConvertFromUtf32(_character);

    public TextPosition Start => _position;

    public TextPosition End => _position;

    public override String ToString()
    {
        return $"Character / {_position} / '{Char.ConvertFromUtf32(_character)}'#{_character}";
    }
}
