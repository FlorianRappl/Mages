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

        private const Int32 MinWidth = 12;
        private const Int32 MaxWidth = 50;

        #endregion

        #region Fields

        private readonly List<Line> _lines;
        private readonly Handler[] _handlers;
        private readonly History _history;
        private Boolean _done;
        private String _prompt;
        private Int32 _line;
        private Thread _editThread;
        private String _killBuffer;
        private String _search;
        private String _lastSearch;
        private Int32 _searching;
        private Int32 _matchAt;
        private Action _lastHandler;
        private CompletionState _completion;

        #endregion

        #region Events

        public event AutoCompleteHandler AutoCompleteEvent;

        #endregion

        #region ctor

        public LineEditor(History history)
        {
            _killBuffer = String.Empty;
            _search = String.Empty;
            _done = false;
            _handlers = new[]
            {
			    new Handler (ConsoleKey.Home, CmdHome),
			    new Handler (ConsoleKey.End, CmdEnd),
			    new Handler (ConsoleKey.LeftArrow, CmdLeft),
			    new Handler (ConsoleKey.RightArrow, CmdRight),
			    new Handler (ConsoleKey.UpArrow, CmdUp, resetCompletion: false),
			    new Handler (ConsoleKey.DownArrow, CmdDown, resetCompletion: false),
                new Handler (ConsoleKey.Enter, ConsoleModifiers.Shift, CmdNewLine, resetCompletion: false),
                new Handler (ConsoleKey.Enter, CmdDone, resetCompletion: false),
			    new Handler (ConsoleKey.Backspace, CmdBackspace, resetCompletion: false),
			    new Handler (ConsoleKey.Delete, CmdDeleteChar),
			    new Handler (ConsoleKey.Tab, CmdTabOrComplete, resetCompletion: false),
				
			    // Emacs keys
			    Handler.Control ('A', CmdHome),
			    Handler.Control ('E', CmdEnd),
			    Handler.Control ('B', CmdLeft),
			    Handler.Control ('F', CmdRight),
			    Handler.Control ('P', CmdUp, resetCompletion: false),
			    Handler.Control ('N', CmdDown, resetCompletion: false),
			    Handler.Control ('K', CmdKillToEOF),
			    Handler.Control ('Y', CmdYank),
			    Handler.Control ('D', CmdDeleteChar),
			    Handler.Control ('L', CmdRefresh),
			    Handler.Control ('R', CmdReverseSearch),
			    Handler.Control ('G', delegate {} ),
			    Handler.Alt ('B', ConsoleKey.B, CmdBackwardWord),
			    Handler.Alt ('F', ConsoleKey.F, CmdForwardWord),
				
			    Handler.Alt ('D', ConsoleKey.D, CmdDeleteWord),
			    Handler.Alt ((char) 8, ConsoleKey.Backspace, CmdDeleteBackword),
			    Handler.Control ('Q', delegate { HandleChar (Console.ReadKey (true).KeyChar); })
            };
            _history = history;
            _lines = new List<Line>();
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
            get { return String.Join(Environment.NewLine, _lines.Select(m => m.Text)); }
        }

        public String Prompt
        {
            get { return _prompt; }
        }

        private Line CurrentLine
        {
            get { return _lines[_line]; }
        }

        #endregion

        #region Methods

        public void Interrupt()
        {
            // Interrupt the editor
            _editThread.Abort();
        }

        public String Edit(String prompt, String initial)
        {
            _editThread = Thread.CurrentThread;
            _searching = 0;

            _done = false;
            _history.CursorToEnd();
            _lines.Clear();
            _lines.Add(new Line(prompt, 0, initial));
            _line = 0;

            _prompt = prompt;
            _history.Append(initial);

            do
            {
                try
                {
                    EditLoop();
                }
                catch (ThreadAbortException)
                {
                    Thread.ResetAbort();
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
                return CurrentLine.IsCompleting();
            }

            return false;
        }

        private void HandleChar(Char c)
        {
            if (_searching == 0)
            {
                var completing = _completion != null;
                HideCompletions();
                CurrentLine.InsertChar(c);

                if (HeuristicAutoComplete(completing, c))
                {
                    UpdateCompletionWindow();
                }
            }
            else
            {
                SearchAppend(c);
            }
        }

        private void EditLoop()
        {
            while (!_done)
            {
                var mod = default(ConsoleModifiers);
                var cki = Console.ReadKey(true);

                if (cki.Key == ConsoleKey.Escape)
                {
                    if (_completion != null)
                    {
                        HideCompletions();
                        continue;
                    }
                    else
                    {
                        cki = Console.ReadKey(true);
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
                        _lastHandler = handler.KeyHandler;
                        break;
                    }
                }

                if (handled)
                {
                    if (_searching != 0)
                    {
                        if (_lastHandler != CmdReverseSearch)
                        {
                            _searching = 0;
                            CurrentLine.SetPrompt(_prompt);
                        }
                    }

                    continue;
                }

                if (cki.KeyChar != (Char)0)
                {
                    HandleChar(cki.KeyChar);
                }
            }
        }

        private void SetText(String newtext)
        {
            var lines = newtext.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
            _lines.Clear();

            foreach (var line in lines)
            {
                _lines.Add(new Line(_prompt, 0, line));
            }

            _line = 0;
        }

        private void HistoryUpdateLine()
        {
            _history.Update(AvailableText);
        }

        private void InsertNewLine()
        {
            var prompt = String.Empty.PadRight(_prompt.Length, ' ');
            _lines.Add(new Line(prompt, 1));
            _line++;
        }

        private void ShowCompletions(String prefix, String[] completions)
        {
            var height = Math.Min(completions.Length, Console.WindowHeight / 5);
            var width = MinWidth;
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
                CurrentLine.Render();
            }

            foreach (var s in completions)
            {
                width = Math.Max(prefix.Length + s.Length, width);
            }

            width = Math.Min(width, MaxWidth);

            if (_completion == null)
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
            if (_completion != null)
            {
                _completion.Remove();
                _completion = null;
            }
        }

        private void Complete()
        {
            var completion = AutoCompleteEvent(AvailableText, CurrentLine.Position);
            var completions = completion.Result;

            if (completions == null)
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
                CurrentLine.InsertTextAtCursor(completions[0]);
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
                    CurrentLine.InsertTextAtCursor(completions[0].Substring(0, last + 1));

                    // Adjust the completions to skip the common prefix
                    prefix += completions[0].Substring(0, last + 1);

                    for (var i = 0; i < completions.Length; i++)
                    {
                        completions[i] = completions[i].Substring(last + 1);
                    }
                }

                ShowCompletions(prefix, completions);
                CurrentLine.ReRender();
            }
        }

        private void UpdateCompletionWindow()
        {
            if (_completion != null)
                throw new Exception("This method should only be called if the window has been hidden");

            var completion = AutoCompleteEvent(AvailableText, CurrentLine.Position);
            var completions = completion.Result;

            if (completions != null)
            {
                var ncompletions = completions.Length;

                if (ncompletions > 0)
                {
                    ShowCompletions(completion.Prefix, completion.Result);
                    CurrentLine.ReRender();
                }
            }
        }

        private void ReverseSearch()
        {
            if (CurrentLine.Position == CurrentLine.Length)
            {
                var p = CurrentLine.Text.LastIndexOf(_search);

                if (p != -1)
                {
                    _matchAt = p;
                    CurrentLine.UpdateCursor(p);
                    return;
                }
            }
            else
            {
                var start = CurrentLine.Position - ((CurrentLine.Position == _matchAt) ? 1 : 0);

                if (start != -1)
                {
                    var p = CurrentLine.Text.LastIndexOf(_search, start);

                    if (p != -1)
                    {
                        _matchAt = p;
                        CurrentLine.UpdateCursor(p);
                        return;
                    }
                }
            }

            HistoryUpdateLine();
            var s = _history.SearchBackward(_search);

            if (s != null)
            {
                _matchAt = -1;
                SetText(s);
                ReverseSearch();
            }
        }

        private void SearchAppend(Char c)
        {
            _search = _search + c;
            CurrentLine.SetSearchPrompt(_search);

            if (CurrentLine.Position < CurrentLine.Length)
            {
                var r = CurrentLine.Text.Substring(CurrentLine.Position);

                if (r.StartsWith(_search))
                {
                    return;
                }
            }

            ReverseSearch();
        }

        #endregion

        #region Commands

        private void CmdReverseSearch()
        {
            if (_searching == 0)
            {
                _matchAt = -1;
                _lastSearch = _search;
                _searching = -1;
                _search = String.Empty;
                CurrentLine.SetSearchPrompt(String.Empty);
            }
            else if (String.IsNullOrEmpty(_search) && !String.IsNullOrEmpty(_lastSearch))
            {
                _search = _lastSearch;
                CurrentLine.SetSearchPrompt(_search);
                ReverseSearch();
            }
            else
            {
                ReverseSearch();
            }
        }

        private void CmdRefresh()
        {
            Console.Clear();
            CurrentLine.Refresh();
        }

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
            if (_completion == null)
            {
                CmdHistoryPrev();
            }
            else
            {
                _completion.SelectPrevious();
            }
        }

        private void CmdDown()
        {
            if (_completion == null)
            {
                CmdHistoryNext();
            }
            else
            {
                _completion.SelectNext();
            }
        }

        private void CmdKillToEOF()
        {
            var text = CurrentLine.Text;
            _killBuffer = text.Substring(CurrentLine.Position);
            CurrentLine.RemoveRest();
        }

        private void CmdYank()
        {
            CurrentLine.InsertTextAtCursor(_killBuffer);
        }

        private void CmdDeleteWord()
        {
            var pos = CurrentLine.WordForward();

            if (pos != -1)
            {
                var text = CurrentLine.Text;
                var len = pos - CurrentLine.Position;
                var k = text.Substring(CurrentLine.Position, len);
                var o = _lastHandler == CmdDeleteWord ? _killBuffer : String.Empty;
                _killBuffer = k + o;
                CurrentLine.RemoveWord(CurrentLine.Position, len);
            }
        }

        private void CmdDeleteBackword()
        {
            var pos = CurrentLine.WordBackward();

            if (pos != -1)
            {
                var text = CurrentLine.Text;
                var len = CurrentLine.Position - pos;
                var k = text.Substring(pos, len);
                var o = _lastHandler == CmdDeleteBackword ? _killBuffer : String.Empty;
                _killBuffer = k + o;
                CurrentLine.RemoveWord(pos, len);
            }
        }

        private void CmdNewLine()
        {
            HideCompletions();
            InsertNewLine();
        }

        private void CmdDone()
        {
            if (_completion != null)
            {
                CurrentLine.InsertTextAtCursor(_completion.Current);
                HideCompletions();
                return;
            }

            _done = true;
        }

        private void CmdTabOrComplete()
        {
            var complete = false;

            if (AutoCompleteEvent != null)
            {
                if (IsFirstTabCompleting)
                {
                    complete = true;
                }
                else
                {
                    complete = CurrentLine.IsComplete();
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
            CurrentLine.UpdateCursor(0);
        }

        private void CmdEnd()
        {
            CurrentLine.UpdateCursor(CurrentLine.Length);
        }

        private void CmdLeft()
        {
            if (CurrentLine.Position != 0)
            {
                CurrentLine.UpdateCursor(CurrentLine.Position - 1);
            }
            else if (_line > 0)
            {
                _line--;
                CurrentLine.UpdateCursor(CurrentLine.Length);
            }
        }

        private void CmdBackwardWord()
        {
            var p = CurrentLine.WordBackward();

            if (p != -1)
            {
                CurrentLine.UpdateCursor(p);
            }
        }

        private void CmdForwardWord()
        {
            var p = CurrentLine.WordForward();

            if (p != -1)
            {
                CurrentLine.UpdateCursor(p);
            }
        }

        private void CmdRight()
        {
            if (CurrentLine.Position != CurrentLine.Length)
            {
                CurrentLine.UpdateCursor(CurrentLine.Position + 1);
            }
            else if (_line < _lines.Count - 1)
            {
                _line++;
                CurrentLine.UpdateCursor(0);
            }
        }

        private void CmdBackspace()
        {
            if (CurrentLine.Position != 0)
            {
                var completing = _completion != null;
                HideCompletions();
                CurrentLine.DeletePreviousChar();

                if (completing)
                {
                    UpdateCompletionWindow();
                }
            }
            else if (_line > 0)
            {
                var line = _lines[_line - 1];
                _lines.Remove(line);
                _line--;
                CurrentLine.Prepend(line);
            }
        }

        private void CmdDeleteChar()
        {
            // If there is no input, this behaves like EOF
            if (AvailableText.Length == 0)
            {
                _done = true;
                Console.WriteLine();
            }
            else if (CurrentLine.Position != CurrentLine.Length)
            {
                CurrentLine.DeleteNextChar();
            }
            else if (_line < _lines.Count - 1)
            {
                var line = _lines[_line + 1];
                CurrentLine.Append(line);
                _lines.Remove(line);
            }
        }

        #endregion

        class Line
        {
            private readonly StringBuilder _availableText;
            private readonly StringBuilder _renderedText;
            private Int32 _maxRendered;
            private String _prompt;
            private String _shownPrompt;
            private Int32 _cursorPosition;
            private Int32 _homeRow;

            public Line(String prompt, Int32 offset = 0, String text = null)
            {
                _renderedText = new StringBuilder();
                _availableText = new StringBuilder();
                _prompt = prompt;
                _shownPrompt = prompt;
                _cursorPosition = 0;
                _maxRendered = 0;
                _homeRow = Console.CursorTop + offset;
                InitText(text ?? String.Empty);
            }

            public Int32 Position
            {
                get { return _cursorPosition; }
            }

            public Int32 Length
            {
                get { return _availableText.Length; }
            }

            public String Text
            {
                get { return _availableText.ToString(); }
            }

            public Int32 LineCount
            {
                get { return (_prompt.Length + _renderedText.Length) / Console.WindowWidth; }
            }

            public Boolean IsCompleting()
            {
                if (_cursorPosition > 1 && Char.IsDigit(_availableText[_cursorPosition - 2]))
                {
                    for (var p = _cursorPosition - 3; p >= 0; p--)
                    {
                        var c = _availableText[p];

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

            public Int32 WordForward()
            {
                var p = _cursorPosition;

                if (p < _availableText.Length)
                {
                    var i = p;

                    if (Char.IsPunctuation(_availableText[p]) || Char.IsSymbol(_availableText[p]) || Char.IsWhiteSpace(_availableText[p]))
                    {
                        for (; i < _availableText.Length; i++)
                        {
                            if (Char.IsLetterOrDigit(_availableText[i]))
                                break;
                        }

                        for (; i < _availableText.Length; i++)
                        {
                            if (!Char.IsLetterOrDigit(_availableText[i]))
                                break;
                        }
                    }
                    else
                    {
                        for (; i < _availableText.Length; i++)
                        {
                            if (!Char.IsLetterOrDigit(_availableText[i]))
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

            public Int32 WordBackward()
            {
                var p = _cursorPosition;

                if (p != 0)
                {
                    var i = p - 1;

                    if (i == 0)
                    {
                        return 0;
                    }

                    if (Char.IsPunctuation(_availableText[i]) || Char.IsSymbol(_availableText[i]) || Char.IsWhiteSpace(_availableText[i]))
                    {
                        for (; i >= 0; i--)
                        {
                            if (Char.IsLetterOrDigit(_availableText[i]))
                                break;
                        }

                        for (; i >= 0; i--)
                        {
                            if (!Char.IsLetterOrDigit(_availableText[i]))
                                break;
                        }
                    }
                    else
                    {
                        for (; i >= 0; i--)
                        {
                            if (!Char.IsLetterOrDigit(_availableText[i]))
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

            public void InsertChar(Char c)
            {
                var prev_lines = LineCount;
                _availableText.Insert(_cursorPosition, c);
                ComputeRendered();

                if (prev_lines != LineCount)
                {
                    Console.SetCursorPosition(0, _homeRow);
                    Render();
                    ForceCursor(++_cursorPosition);
                }
                else
                {
                    RenderFrom(_cursorPosition);
                    ForceCursor(++_cursorPosition);
                    UpdateHomeRow(TextToScreenPos(_cursorPosition));
                }
            }

            private void RenderFrom(Int32 pos)
            {
                var rpos = TextToRenderPos(pos);
                var i = rpos;

                while (i < _renderedText.Length)
                {
                    Console.Write(_renderedText[i++]);
                }

                if ((_shownPrompt.Length + _renderedText.Length) > _maxRendered)
                {
                    _maxRendered = _shownPrompt.Length + _renderedText.Length;
                }
                else
                {
                    var max_extra = _maxRendered - _shownPrompt.Length;

                    for (; i < max_extra; i++)
                    {
                        Console.Write(' ');
                    }
                }
            }

            private void ComputeRendered()
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

            private Int32 TextToRenderPos(Int32 pos)
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

            private Int32 TextToScreenPos(Int32 pos)
            {
                return _shownPrompt.Length + TextToRenderPos(pos);
            }

            private void ForceCursor(Int32 newpos)
            {
                _cursorPosition = newpos;
                var actual_pos = _shownPrompt.Length + TextToRenderPos(_cursorPosition);
                var row = _homeRow + (actual_pos / Console.WindowWidth);
                var col = actual_pos % Console.WindowWidth;

                if (row >= Console.BufferHeight)
                {
                    row = Console.BufferHeight - 1;
                }

                Console.SetCursorPosition(col, row);
            }

            public void Render()
            {
                var max = Math.Max(_renderedText.Length + _shownPrompt.Length, _maxRendered);
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write(_shownPrompt);
                Console.ResetColor();
                Console.Write(_renderedText);

                for (var i = _renderedText.Length + _shownPrompt.Length; i < _maxRendered; i++)
                {
                    Console.Write(' ');
                }

                _maxRendered = _shownPrompt.Length + _renderedText.Length;
                Console.Write(' ');
                UpdateHomeRow(max);
            }

            public void UpdateCursor(Int32 newpos)
            {
                if (_cursorPosition != newpos)
                {
                    ForceCursor(newpos);
                }
            }

            private void InitText(String initial)
            {
                Console.SetCursorPosition(0, _homeRow);
                _availableText.Clear().Append(initial);
                ComputeRendered();
                _cursorPosition = _availableText.Length;
                Render();
                ForceCursor(_cursorPosition);
            }

            public void SetPrompt(String newprompt)
            {
                _shownPrompt = newprompt;
                Console.SetCursorPosition(0, _homeRow);
                Render();
                ForceCursor(_cursorPosition);
            }

            public void SetSearchPrompt(String s)
            {
                SetPrompt("(reverse-i-search)`" + s + "': ");
            }

            public void InsertTextAtCursor(String str)
            {
                var prev_lines = LineCount;
                _availableText.Insert(_cursorPosition, str);
                ComputeRendered();

                if (prev_lines != LineCount)
                {
                    Console.SetCursorPosition(0, _homeRow);
                    Render();
                    _cursorPosition += str.Length;
                    ForceCursor(_cursorPosition);
                }
                else
                {
                    RenderFrom(_cursorPosition);
                    _cursorPosition += str.Length;
                    ForceCursor(_cursorPosition);
                    UpdateHomeRow(TextToScreenPos(_cursorPosition));
                }
            }

            public void ReRender()
            {
                Render();
                ForceCursor(_cursorPosition);
            }

            private void UpdateHomeRow(Int32 screenpos)
            {
                var lines = 1 + (screenpos / Console.WindowWidth);
                _homeRow = Math.Max(0, Console.CursorTop - (lines - 1));
            }

            private void RenderAfter(Int32 p)
            {
                ForceCursor(p);
                RenderFrom(p);
                ForceCursor(_cursorPosition);
            }

            public void DeleteNextChar()
            {
                _availableText.Remove(_cursorPosition, 1);
                ComputeRendered();
                RenderAfter(_cursorPosition);
            }

            public void DeletePreviousChar()
            {
                _availableText.Remove(--_cursorPosition, 1);
                ComputeRendered();
                RenderAfter(_cursorPosition);
            }

            public void Append(Line line)
            {
                _cursorPosition = Length;
                _availableText.Append(line._availableText.ToString());
                _renderedText.Append(line._renderedText.ToString());
                RenderAfter(_cursorPosition);
            }

            public void Prepend(Line line)
            {
                _availableText.Insert(0, line._availableText.ToString());
                _renderedText.Insert(0, line._renderedText.ToString());
                _homeRow--;
                _cursorPosition = line.Length;
                RenderAfter(_cursorPosition);
            }

            public Boolean IsComplete()
            {
                for (var i = 0; i < _cursorPosition; i++)
                {
                    if (!Char.IsWhiteSpace(_availableText[i]))
                    {
                        return true;
                    }
                }

                return false;
            }

            public void Refresh()
            {
                _maxRendered = 0;
                Render();
                ForceCursor(_cursorPosition);
            }

            public void RemoveRest()
            {
                _availableText.Length = _cursorPosition;
                ComputeRendered();
                RenderAfter(_cursorPosition);
            }

            public void RemoveWord(Int32 from, Int32 length)
            {
                _availableText.Remove(from, length);
                ComputeRendered();
                RenderAfter(from);
            }
        }
    }
}
