namespace Mages.Core.Source;

using System;
using System.IO;

sealed class StringScanner(String source) : BaseScanner, IScanner
{
    #region Fields

    private readonly StringReader _source = new(source);

    private Int32 _current = CharacterTable.NullPtr;
    private Int32 _p0 = CharacterTable.NullPtr;
    private Int32 _p1 = CharacterTable.NullPtr;
    private Int32 _p2 = CharacterTable.NullPtr;
    private Int32 _p3 = CharacterTable.NullPtr;
    private Boolean _skip;
    private Int32 _pIndex = 0;

    #endregion
    #region ctor

    #endregion

    #region Properties

    public Int32 Current => _pIndex switch
    {
        0 => _current,
        1 => _p0,
        2 => _p1,
        3 => _p2,
        4 => _p3,
        _ => CharacterTable.NullPtr,
    };

    #endregion

    #region Methods

    public TextPosition GetPositionAt(Int32 index)
    {
        return new TextPosition(0, 0, index + 1);
    }

    public Boolean MoveNext()
    {
        Advance();
        return _current != CharacterTable.End;
    }

    public Boolean MoveBack()
    {
        Retreat();
        return _current != CharacterTable.End;
    }

    #endregion

    #region Helpers

    protected override void Cleanup()
    {
        _source.Dispose();
    }

    private void Retreat()
    {
        if (_pIndex < 4)
        {
            _pIndex++;
        }
    }

    private void Advance()
    {
        if (_pIndex > 0)
        {
            _pIndex--;
        }
        else if (_current != CharacterTable.End)
        {
            if (_current == CharacterTable.LineFeed)
            {
                NextRow();
            }
            else
            {
                NextColumn();
            }

            _p3 = _p2;
            _p2 = _p1;
            _p1 = _p0;
            _p0 = _current;
            _current = Read();
        }
    }

    private Int32 Read()
    {
        var current = _source.Read();

        if (current == CharacterTable.CarriageReturn)
        {
            current = CharacterTable.LineFeed;
            _skip = true;
        }
        else if (_skip && _current == CharacterTable.LineFeed)
        {
            _skip = false;
            return Read();
        }
        else
        {
            _skip = false;
        }

        return current;
    }

    #endregion
}
