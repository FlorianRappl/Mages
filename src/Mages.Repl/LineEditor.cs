namespace Mages.Repl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading;

    sealed class LineEditor
    {
        #region Constants

        private const Int32 CompletionMinWidth = 12;
        private const Int32 CompletionMaxWidth = 50;

        #endregion

        #region Fields

        private readonly List<Line> _lines;
        private readonly Handler[] _handlers;
        private readonly History _history;
        private readonly Action _onCancelled;
        private Int32 _lineIndex;
        private Boolean _done;
        private String _prompt;
        private Int32 _position;
        private Int32 _homeRow;
        private Thread _editThread;
        private CompletionState _completion;

        #endregion

        #region Line

        class Line
        {
            public readonly StringBuilder _availableText;
            public readonly StringBuilder _renderedText;
            private Int32 _maxRendered;

            public Line(String text)
            {
                _availableText = new StringBuilder(text);
                _renderedText = new StringBuilder();
                _maxRendered = 0;
                ComputeRendered();
            }

            public Int32 Length
            {
                get { return _availableText.Length; }
            }

            public Int32 GetLineCount(Int32 offset)
            {
                return 1 + (offset + _renderedText.Length) / Console.WindowWidth;
            }

            public void ComputeRendered()
            {
                _renderedText.Length = 0;

                for (var i = 0; i < _availableText.Length; i++)
                {
                    var c = (Int32)_availableText[i];

                    if (c < 26)
                    {
                        if (c == '\t')
                        {
                            _renderedText.Append("    ");
                        }
                        else
                        {
                            _renderedText.Append('^');
                            _renderedText.Append((Char)(c + (Int32)'A' - 1));
                        }
                    }
                    else
                    {
                        _renderedText.Append((Char)c);
                    }
                }
            }

            public void RenderFrom(Int32 pos)
            {
                var i = TextToRenderPos(pos);

                while (i < _renderedText.Length)
                {
                    Console.Write(_renderedText[i++]);
                }

                if (_renderedText.Length > _maxRendered)
                {
                    _maxRendered = _renderedText.Length;
                }
                else
                {
                    for (; i < _maxRendered; i++)
                    {
                        Console.Write(' ');
                    }
                }
            }

            public Int32 TextToRenderPos(Int32 pos)
            {
                var p = 0;

                for (var i = 0; i < pos; i++)
                {
                    var c = (Int32)_availableText[i];

                    if (c < 26)
                    {
                        p += (c == 9 ? 4 : 2);
                    }
                    else
                    {
                        p++;
                    }
                }

                return p;
            }

            public void Render()
            {
                Console.Write(_renderedText);

                for (var i = _renderedText.Length; i <= _maxRendered; i++)
                {
                    Console.Write(' ');
                }

                _maxRendered = _renderedText.Length;
            }

            public void Append(Line line)
            {
                _availableText.Append(line._availableText.ToString());
                _renderedText.Append(line._renderedText.ToString());
                _maxRendered = Math.Max(_maxRendered, _renderedText.Length);
            }

            public String Cut(Int32 pos)
            {
                var len = _availableText.Length - pos;
                var rest = _availableText.ToString(pos, len);
                _availableText.Remove(pos, len);
                ComputeRendered();
                return rest;
            }
        }

        #endregion

        #region Events

        public event AutoCompleteHandler AutoCompleteEvent;

        #endregion

        #region ctor

        public LineEditor(History history, Action onCancelled)
        {
            _done = false;
            _onCancelled = onCancelled;
            _handlers = new[]
            {
                new Handler(ConsoleKey.Home, CmdHome),
                new Handler(ConsoleKey.End, CmdEnd),
                new Handler(ConsoleKey.LeftArrow, CmdLeft),
                new Handler(ConsoleKey.RightArrow, CmdRight),
                new Handler(ConsoleKey.LeftArrow, ConsoleModifiers.Control, CmdBackwardWord),
                new Handler(ConsoleKey.RightArrow, ConsoleModifiers.Control, CmdForwardWord),
                new Handler(ConsoleKey.UpArrow, CmdUp, resetCompletion: false),
                new Handler(ConsoleKey.DownArrow, CmdDown, resetCompletion: false),
                new Handler(ConsoleKey.Enter, CmdDone, resetCompletion: false),
                new Handler(ConsoleKey.Backspace, CmdBackspace, resetCompletion: false),
                new Handler(ConsoleKey.Delete, CmdDeleteChar),
                new Handler(ConsoleKey.Tab, CmdTabOrComplete, resetCompletion: false),
                new Handler(ConsoleKey.Backspace, ConsoleModifiers.Control, CmdDeleteBackword),
                new Handler(ConsoleKey.Delete, ConsoleModifiers.Control, CmdDeleteWord),
                new Handler(ConsoleKey.Enter, ConsoleModifiers.Shift, CmdNewLine),
            };
            _lines = new List<Line>();
            _history = history;
        }

        #endregion

        #region Properties

        public Boolean IsFirstTabCompleting
        {
            get;
            set;
        }

        public Boolean IsDotCompleting
        {
            get;
            set;
        }

        public String AvailableText
        {
            get { return String.Join(Environment.NewLine, _lines.Select(line => line._availableText.ToString())); }
        }

        public Int32 LineCount
        {
            get { return _lines.Sum(line => line.GetLineCount(_prompt.Length)); }
        }

        public String Prompt
        {
            get { return _prompt; }
        }

        private Line CurrentLine
        {
            get { return _lines[_lineIndex]; }
        }

        #endregion

        #region Methods

        public String Edit(String prompt, String initial)
        {
            _editThread = Thread.CurrentThread;

            _done = false;
            _history.CursorToEnd();
            _prompt = prompt;
            _homeRow = Console.CursorTop;
            _history.Append(initial);

            InitText(initial);

            do
            {
                try
                {
                    EditLoop();
                }
                catch (OperationCanceledException)
                {
                    _onCancelled.Invoke();
                    Console.WriteLine();
                    return null;
                }
            }
            while (!_done);

            Console.WriteLine();
            var result = AvailableText;

            if (String.IsNullOrEmpty(result))
            {
                _history.RemoveLast();
            }
            else
            {
                _history.Accept(result);
            }

            return result;
        }

        public static void SaveExcursion(Action code)
        {
            var saved_col = Console.CursorLeft;
            var saved_row = Console.CursorTop;
            var saved_fore = Console.ForegroundColor;
            var saved_back = Console.BackgroundColor;

            code.Invoke();

            Console.CursorLeft = saved_col;
            Console.CursorTop = saved_row;
            Console.ForegroundColor = saved_fore;
            Console.BackgroundColor = saved_back;
        }

        #endregion

        #region Helpers

        private Boolean HeuristicAutoComplete(Boolean wasCompleting, Char insertedChar)
        {
            if (wasCompleting)
            {
                return insertedChar != ' ';
            }

            if (IsDotCompleting && insertedChar == '.')
            {
                if (_position > 1 && Char.IsDigit(CurrentLine._availableText[_position - 2]))
                {
                    for (var p = _position - 3; p >= 0; p--)
                    {
                        var c = CurrentLine._availableText[p];

                        if (Char.IsDigit(c))
                        {
                            continue;
                        }

                        if (c == '_' || Char.IsLetter(c) || Char.IsPunctuation(c) || Char.IsSymbol(c) || Char.IsControl(c))
                        {
                            return true;
                        }
                    }

                    return false;
                }

                return true;
            }

            return false;
        }

        private void HandleChar(Char c)
        {
            var completing = _completion is not null;
            HideCompletions();
            InsertChar(c);

            if (HeuristicAutoComplete(completing, c))
            {
                UpdateCompletionWindow();
            }
        }

        private static ConsoleKeyInfo ReadKey()
        {
            var key = Console.ReadKey(true);

            if (key.Modifiers == ConsoleModifiers.Control && key.Key == ConsoleKey.C)
            {
                throw new OperationCanceledException();
            }

            return key;
        }

        private void EditLoop()
        {
            while (!_done)
            {
                ConsoleModifiers mod;
                var cki = ReadKey();

                if (cki.Key == ConsoleKey.Escape)
                {
                    if (_completion is not null)
                    {
                        HideCompletions();
                        continue;
                    }
                    else
                    {
                        cki = ReadKey();
                        mod = ConsoleModifiers.Alt;
                    }
                }
                else
                {
                    mod = cki.Modifiers;
                }

                var handled = false;

                foreach (var handler in _handlers)
                {
                    var t = handler.KeyInfo;

                    if ((t.Key == cki.Key && t.Modifiers == mod) || (t.KeyChar == cki.KeyChar && t.Key == ConsoleKey.Zoom))
                    {
                        handled = true;

                        if (handler.ResetCompletion)
                        {
                            HideCompletions();
                        }

                        handler.KeyHandler();
                        break;
                    }
                }
                
                if (!handled && cki.KeyChar != (Char)0)
                {
                    HandleChar(cki.KeyChar);
                }
            }
        }

        private void InitText(String initial)
        {
            var lines = initial.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            _lines.Clear();

            foreach (var line in lines)
            {
                _lineIndex = _lines.Count;
                _lines.Add(new Line(line));
            }

            CurrentLine.ComputeRendered();
            _position = CurrentLine.Length;
            Render();
            ForceCursor(_position);
        }

        private void SetText(String newtext)
        {
            Clear(LineCount);
            Console.SetCursorPosition(0, _homeRow);
            InitText(newtext ?? String.Empty);
        }

        private void HistoryUpdateLine()
        {
            _history.Update(AvailableText);
        }

        private void InsertTextAtCursor(String str)
        {
            var prev_lines = LineCount;
            CurrentLine._availableText.Insert(_position, str);
            CurrentLine.ComputeRendered();

            if (prev_lines != LineCount)
            {
                Console.SetCursorPosition(0, _homeRow);
                Render();
                _position += str.Length;
                ForceCursor(_position);
            }
            else
            {
                CurrentLine.RenderFrom(_position);
                _position += str.Length;
                ForceCursor(_position);
            }
        }

        private Int32 WordForward(Int32 p)
        {
            var currentText = CurrentLine._availableText;

            if (p < currentText.Length)
            {
                var i = p;

                if (Char.IsPunctuation(currentText[p]) || Char.IsSymbol(currentText[p]) || Char.IsWhiteSpace(currentText[p]))
                {
                    for (; i < currentText.Length; i++)
                    {
                        if (Char.IsLetterOrDigit(currentText[i]))
                            break;
                    }

                    for (; i < currentText.Length; i++)
                    {
                        if (!Char.IsLetterOrDigit(currentText[i]))
                            break;
                    }
                }
                else
                {
                    for (; i < currentText.Length; i++)
                    {
                        if (!Char.IsLetterOrDigit(currentText[i]))
                            break;
                    }
                }

                if (i != p)
                {
                    return i;
                }
            }

            return -1;
        }

        private Int32 WordBackward(Int32 p)
        {
            if (p != 0)
            {
                var currentText = CurrentLine._availableText;

                var i = p - 1;

                if (i == 0)
                {
                    return 0;
                }

                if (Char.IsPunctuation(currentText[i]) || Char.IsSymbol(currentText[i]) || Char.IsWhiteSpace(currentText[i]))
                {
                    for (; i >= 0; i--)
                    {
                        if (Char.IsLetterOrDigit(currentText[i]))
                            break;
                    }

                    for (; i >= 0; i--)
                    {
                        if (!Char.IsLetterOrDigit(currentText[i]))
                            break;
                    }
                }
                else
                {
                    for (; i >= 0; i--)
                    {
                        if (!Char.IsLetterOrDigit(currentText[i]))
                            break;
                    }
                }

                if (++i != p)
                {
                    return i;
                }
            }

            return -1;
        }

        private void Render()
        {
            var offset = _prompt.Length;
            var rows = 0;
            Console.SetCursorPosition(0, _homeRow);
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write(_prompt);
            Console.ResetColor();

            for (var i = 0; i < _lines.Count; i++)
            {
                var row = _homeRow + rows;
                Console.SetCursorPosition(offset, row);
                _lines[i].Render();
                rows += _lines[i].GetLineCount(offset);
            }
        }

        private Int32 TextToScreenPos(Int32 pos)
        {
            return _prompt.Length + CurrentLine.TextToRenderPos(pos);
        }

        private void ForceCursor(Int32 newpos)
        {
            _position = Math.Min(newpos, CurrentLine.Length);
            var homeRow = GetCurrentHomeRow();
            var actual_pos = _prompt.Length + CurrentLine.TextToRenderPos(_position);
            var row = homeRow + (actual_pos / Console.WindowWidth);
            var col = actual_pos % Console.WindowWidth;

            if (row >= Console.BufferHeight)
            {
                row = Console.BufferHeight - 1;
            }

            Console.SetCursorPosition(col, row);
        }

        private Int32 GetCurrentHomeRow()
        {
            var row = _homeRow;

            for (var i = 0; i < _lineIndex; i++)
            {
                row += _lines[i].GetLineCount(_prompt.Length);
            }

            return row;
        }

        private void UpdateCursor(Int32 newpos)
        {
            if (_position != newpos)
            {
                ForceCursor(newpos);
            }
        }

        private void InsertChar(Char c)
        {
            var prev_lines = LineCount;
            CurrentLine._availableText.Insert(_position, c);
            CurrentLine.ComputeRendered();

            if (prev_lines != LineCount)
            {
                Console.SetCursorPosition(0, _homeRow);
                Render();
                ForceCursor(++_position);
            }
            else
            {
                CurrentLine.RenderFrom(_position);
                ForceCursor(++_position);
            }
        }

        private void ShowCompletions(String prefix, String[] completions)
        {
            var height = Math.Min(completions.Length, Console.WindowHeight / 5);
            var width = CompletionMinWidth;
            var current = Console.CursorTop;

            if (current >= Console.WindowHeight - height)
            {
                Console.SetCursorPosition(0, current + 1);

                for (var i = 0; i < height; i++)
                {
                    for (var c = Console.WindowWidth; c > 0; c--)
                    {
                        Console.Write(' ');
                    }
                }

                Console.SetCursorPosition(0, current);
                Render();
            }

            foreach (var s in completions)
            {
                width = Math.Max(prefix.Length + s.Length, width);
            }

            width = Math.Min(width, CompletionMaxWidth);

            if (_completion is null)
            {
                var left = Console.CursorLeft - prefix.Length;

                if (left + width + 1 >= Console.WindowWidth)
                {
                    left = Console.WindowWidth - width - 1;
                }

                _completion = new CompletionState(left, Console.CursorTop + 1, width, height)
                {
                    Prefix = prefix,
                    Completions = completions,
                };
            }
            else
            {
                _completion.Prefix = prefix;
                _completion.Completions = completions;
            }

            _completion.Show();
            Console.CursorLeft = 0;
        }

        private void HideCompletions()
        {
            if (_completion is not null)
            {
                _completion.Remove();
                _completion = null;
            }
        }

        private void Complete()
        {
            var completion = AutoCompleteEvent(AvailableText, _position);
            var completions = completion.Result;

            if (completions is null)
            {
                HideCompletions();
                return;
            }

            var ncompletions = completions.Length;

            if (ncompletions == 0)
            {
                HideCompletions();
                return;
            }

            if (completions.Length == 1)
            {
                InsertTextAtCursor(completions[0]);
                HideCompletions();
            }
            else
            {
                var last = -1;

                for (var p = 0; p < completions[0].Length; p++)
                {
                    var c = completions[0][p];

                    for (var i = 1; i < ncompletions; i++)
                    {
                        if (completions[i].Length < p)
                        {
                            goto mismatch;
                        }

                        if (completions[i][p] != c)
                        {
                            goto mismatch;
                        }
                    }

                    last = p;
                }

                mismatch:
                var prefix = completion.Prefix;

                if (last != -1)
                {
                    InsertTextAtCursor(completions[0].Substring(0, last + 1));

                    // Adjust the completions to skip the common prefix
                    prefix += completions[0].Substring(0, last + 1);

                    for (var i = 0; i < completions.Length; i++)
                    {
                        completions[i] = completions[i].Substring(last + 1);
                    }
                }

                ShowCompletions(prefix, completions);
                Render();
                ForceCursor(_position);
            }
        }

        private void UpdateCompletionWindow()
        {
            if (_completion is not null)
            {
                throw new Exception("This method should only be called if the window has been hidden");
            }

            var completion = AutoCompleteEvent(AvailableText, _position);
            var completions = completion.Result;

            if (completions is not null)
            {
                var ncompletions = completions.Length;

                if (ncompletions > 0)
                {
                    ShowCompletions(completion.Prefix, completion.Result);
                    Render();
                    ForceCursor(_position);
                }
            }
        }

        private void RenderAfter(Int32 p)
        {
            ForceCursor(p);
            CurrentLine.RenderFrom(p);
            ForceCursor(_position);
        }

        private void MergeLines(Int32 startIndex)
        {
            var primary = _lines[startIndex];
            var secondary = _lines[startIndex + 1];
            var position = primary.Length;
            var height = LineCount;
            _lines.Remove(secondary);
            _lineIndex = startIndex;
            primary.Append(secondary);
            Clear(height);
            Render();
            ForceCursor(position);
        }

        private void Clear(Int32 height)
        {
            for (var j = 0; j < height; j++)
            {
                Console.SetCursorPosition(0, _homeRow + j);

                for (var i = 0; i < Console.WindowWidth; i++)
                {
                    Console.Write(' ');
                }
            }
        }

        #endregion

        #region Commands

        private void CmdHistoryPrev()
        {
            if (_history.PreviousAvailable())
            {
                HistoryUpdateLine();
                SetText(_history.Previous());
            }
        }

        private void CmdHistoryNext()
        {
            if (_history.NextAvailable())
            {
                _history.Update(AvailableText);
                SetText(_history.Next());
            }
        }

        private void CmdUp()
        {
            if (_completion is not null)
            {
                _completion.SelectPrevious();
            }
            else if (_lineIndex > 0)
            {
                _lineIndex--;
                ForceCursor(_position);
            }
            else
            {
                CmdHistoryPrev();
            }
        }

        private void CmdDown()
        {
            if (_completion is not null)
            {
                _completion.SelectNext();
            }
            else if (_lineIndex + 1 < _lines.Count)
            {
                _lineIndex++;
                ForceCursor(_position);
            }
            else
            {
                CmdHistoryNext();
            }
        }

        private void CmdDeleteWord()
        {
            var pos = WordForward(_position);

            if (pos != -1)
            {
                CurrentLine._availableText.Remove(_position, pos - _position);
                CurrentLine.ComputeRendered();
                RenderAfter(_position);
            }
        }

        private void CmdDeleteBackword()
        {
            var pos = WordBackward(_position);

            if (pos != -1)
            {
                CurrentLine._availableText.Remove(pos, _position - pos);
                CurrentLine.ComputeRendered();
                RenderAfter(pos);
            }
        }

        private void CmdNewLine()
        {
            var rest = CurrentLine.Cut(_position);
            _lines.Insert(++_lineIndex, new Line(rest));
            Render();
            ForceCursor(0);
        }

        private void CmdDone()
        {
            if (_completion is not null)
            {
                InsertTextAtCursor(_completion.Current);
                HideCompletions();
                return;
            }

            _done = true;
        }

        private void CmdTabOrComplete()
        {
            var complete = false;

            if (AutoCompleteEvent is not null)
            {
                if (IsFirstTabCompleting)
                {
                    complete = true;
                }
                else
                {
                    for (var i = 0; i < _position; i++)
                    {
                        if (!Char.IsWhiteSpace(CurrentLine._availableText[i]))
                        {
                            complete = true;
                            break;
                        }
                    }
                }

                if (complete)
                {
                    Complete();
                }
                else
                {
                    HandleChar('\t');
                }
            }
            else
            {
                HandleChar('t');
            }
        }

        private void CmdHome()
        {
            UpdateCursor(0);
        }

        private void CmdEnd()
        {
            UpdateCursor(CurrentLine.Length);
        }

        private void CmdBackwardWord()
        {
            var p = WordBackward(_position);

            if (p != -1)
            {
                UpdateCursor(p);
            }
        }

        private void CmdForwardWord()
        {
            var p = WordForward(_position);

            if (p != -1)
            {
                UpdateCursor(p);
            }
        }

        private void CmdLeft()
        {
            if (_position != 0)
            {
                UpdateCursor(_position - 1);
            }
            else if (_lineIndex > 0)
            {
                _lineIndex--;
                UpdateCursor(CurrentLine.Length);
            }
        }

        private void CmdRight()
        {
            if (_position != CurrentLine.Length)
            {
                UpdateCursor(_position + 1);
            }
            else if (_lineIndex + 1 < _lines.Count)
            {
                _lineIndex++;
                UpdateCursor(0);
            }
        }

        private void CmdBackspace()
        {
            if (_position != 0)
            {
                var completing = _completion is not null;
                HideCompletions();
                CurrentLine._availableText.Remove(--_position, 1);
                CurrentLine.ComputeRendered();
                RenderAfter(_position);

                if (completing)
                {
                    UpdateCompletionWindow();
                }
            }
            else if (_lineIndex > 0)
            {
                MergeLines(_lineIndex - 1);
            }
        }

        private void CmdDeleteChar()
        {
            if (_position != CurrentLine.Length)
            {
                CurrentLine._availableText.Remove(_position, 1);
                CurrentLine.ComputeRendered();
                RenderAfter(_position);
            }
            else if (_lineIndex + 1 < _lines.Count)
            {
                MergeLines(_lineIndex);
            }
        }

        #endregion
    }
}