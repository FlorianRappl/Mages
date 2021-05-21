namespace Mages.Core.Tokens
{
    using System;

    sealed class CharacterToken : IToken
    {
        private readonly TokenType _type;
        private readonly Int32 _character;
        private readonly TextPosition _position;

        public CharacterToken(TokenType type, Int32 character, TextPosition position)
        {
            _type = type;
            _character = character;
            _position = position;
        }

        public TokenType Type => _type;

        public String Payload => Char.ConvertFromUtf32(_character);

        public TextPosition Start => _position;

        public TextPosition End => _position;

        public override String ToString()
        {
            return $"Character / {_position} / '{Char.ConvertFromUtf32(_character)}'#{_character}";
        }
    }
}
