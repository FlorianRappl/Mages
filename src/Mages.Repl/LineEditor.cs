namespace Mages.Repl
{
    using System;
    using System.Text;
    using System.Threading;

    sealed class LineEditor
    {
        #region Constants

        private const Int32 MaxWidth = 50;

        #endregion

        #region Fields

        private readonly StringBuilder _availableText;
        private readonly StringBuilder _renderedText;
        private readonly Handler[] _handlers;
        private readonly History _history;
        private Boolean _done;
        private String _prompt;
        private String _shownPrompt;
        private Int32 _cursorPosition;
        private Int32 _homeRow;
        private Int32 _maxRendered;
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
            _done = false;
            _handlers = new[]
            {
			    new Handler (ConsoleKey.Home, CmdHome),
			    new Handler (ConsoleKey.End, CmdEnd),
			    new Handler (ConsoleKey.LeftArrow, CmdLeft),
			    new Handler (ConsoleKey.RightArrow, CmdRight),
			    new Handler (ConsoleKey.UpArrow, CmdUp, resetCompletion: false),
			    new Handler (ConsoleKey.DownArrow, CmdDown, resetCompletion: false),
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
            _renderedText = new StringBuilder();
            _availableText = new StringBuilder();
            _history = history;
        }

        #endregion

        #region Properties

        public Boolean TabAtStartCompletes 
        { 
            get; 
            set; 
        }

        public Int32 LineCount
        {
            get { return (_shownPrompt.Length + _renderedText.Length) / Console.WindowWidth; }
        }

        private String Prompt
        {
            get { return _prompt; }
            set { _prompt = value; }
        }

        #endregion

        #region Methods

        public String Edit(String prompt, String initial)
        {
            _editThread = Thread.CurrentThread;
            _searching = 0;
            Console.CancelKeyPress += InterruptEdit;

            _done = false;
            _history.CursorToEnd();
            _maxRendered = 0;

            Prompt = prompt;
            _shownPrompt = prompt;
            InitText(initial);
            _history.Append(initial);

            do
            {
                try
                {
                    EditLoop();
                }
                catch (ThreadAbortException)
                {
                    _searching = 0;
                    Thread.ResetAbort();
                    Console.WriteLine();
                    SetPrompt(prompt);
                    SetText("");
                }
            }
            while (!_done);

            Console.WriteLine();
            Console.CancelKeyPress -= InterruptEdit;

            if (_availableText != null)
            {
                var result = _availableText.ToString();

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

            _history.Close();
            return null;
        }

        public void SaveHistory()
        {
            if (_history != null)
            {
                _history.Close();
            }
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

        private void InterruptEdit(Object sender, ConsoleCancelEventArgs a)
        {
            // Do not abort our program:
            a.Cancel = true;

            // Interrupt the editor
            _editThread.Abort();
        }

        private Boolean HeuristicAutoComplete(Boolean wasCompleting, Char insertedChar)
        {
            if (wasCompleting)
            {
                return insertedChar != ' ';
            }

            if (insertedChar == '.')
            {
                if (_cursorPosition > 1 && Char.IsDigit(_availableText[_cursorPosition - 2]))
                {
                    for (var p = _cursorPosition - 3; p >= 0; p--)
                    {
                        var c = _availableText[p];

                        if (Char.IsDigit(c))
                            continue;

                        if (c == '_' || Char.IsLetter(c) || Char.IsPunctuation(c) || Char.IsSymbol(c) || Char.IsControl(c))
                            return true;
                    }

                    return false;
                }

                return true;
            }

            return false;
        }

        private void HandleChar(Char c)
        {
            if (_searching == 0)
            {
                var completing = _completion != null;
                HideCompletions();
                InsertChar(c);

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
                    var t = handler.CKI;

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
                            SetPrompt(_prompt);
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

        private void InitText(String initial)
        {
            _availableText.Clear().Append(initial);
            ComputeRendered();
            _cursorPosition = _availableText.Length;
            Render();
            ForceCursor(_cursorPosition);
        }

        private void SetText(String newtext)
        {
            Console.SetCursorPosition(0, _homeRow);
            InitText(newtext);
        }

        private void SetPrompt(String newprompt)
        {
            _shownPrompt = newprompt;
            Console.SetCursorPosition(0, _homeRow);
            Render();
            ForceCursor(_cursorPosition);
        }

        private void SearchAppend(Char c)
        {
            _search = _search + c;
            SetSearchPrompt(_search);

            if (_cursorPosition < _availableText.Length)
            {
                var r = _availableText.ToString(_cursorPosition, _availableText.Length - _cursorPosition);

                if (r.StartsWith(_search))
                {
                    return;
                }
            }

            ReverseSearch();
        }

        private void HistoryUpdateLine()
        {
            _history.Update(_availableText.ToString());
        }

        private void InsertTextAtCursor(String str)
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

        private void SetSearchPrompt(String s)
        {
            SetPrompt("(reverse-i-search)`" + s + "': ");
        }

        private void ReverseSearch()
        {
            if (_cursorPosition == _availableText.Length)
            {
                var p = _availableText.ToString().LastIndexOf(_search);

                if (p != -1)
                {
                    _matchAt = p;
                    _cursorPosition = p;
                    ForceCursor(_cursorPosition);
                    return;
                }
            }
            else
            {
                var start = (_cursorPosition == _matchAt) ? _cursorPosition - 1 : _cursorPosition;

                if (start != -1)
                {
                    var p = _availableText.ToString().LastIndexOf(_search, start);

                    if (p != -1)
                    {
                        _matchAt = p;
                        _cursorPosition = p;
                        ForceCursor(_cursorPosition);
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

        private Int32 WordForward(Int32 p)
        {
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

        private Int32 WordBackward(Int32 p)
        {
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

        private void Render()
        {
            var max = Math.Max(_renderedText.Length + _shownPrompt.Length, _maxRendered);
            Console.Write(_shownPrompt);
            Console.Write(_renderedText);

            for (var i = _renderedText.Length + _shownPrompt.Length; i < _maxRendered; i++)
            {
                Console.Write(' ');
            }

            _maxRendered = _shownPrompt.Length + _renderedText.Length;
            Console.Write(' ');
            UpdateHomeRow(max);
        }

        private void UpdateHomeRow(Int32 screenpos)
        {
            var lines = 1 + (screenpos / Console.WindowWidth);
            _homeRow = Math.Max(0, Console.CursorTop - (lines - 1));
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

        private void UpdateCursor(Int32 newpos)
        {
            if (_cursorPosition != newpos)
            {
                ForceCursor(newpos);
            }
        }

        private void InsertChar(Char c)
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

        private void ShowCompletions(String prefix, String[] completions)
        {
            // Ensure we have space, determine window size
            var window_height = Math.Min(completions.Length, Console.WindowHeight / 5);
            var target_line = Console.WindowHeight - window_height - 1;
            var window_width = 12;
            var plen = prefix.Length;

            if (Console.CursorTop > target_line)
            {
                var saved_left = Console.CursorLeft;
                var delta = Console.CursorTop - target_line;
                Console.CursorLeft = 0;
                Console.CursorTop = Console.WindowHeight - 1;

                for (var i = 0; i < delta + 1; i++)
                {
                    for (var c = Console.WindowWidth; c > 0; c--)
                    {
                        Console.Write(" ");
                    }
                }

                Console.CursorTop = target_line;
                Console.CursorLeft = 0;
                Render();
            }

            foreach (var s in completions)
            {
                window_width = Math.Max(plen + s.Length, window_width);
            }

            window_width = Math.Min(window_width, MaxWidth);

            if (_completion == null)
            {
                var left = Console.CursorLeft - prefix.Length;

                if (left + window_width + 1 >= Console.WindowWidth)
                {
                    left = Console.WindowWidth - window_width - 1;
                }

                _completion = new CompletionState(left, Console.CursorTop + 1, window_width, window_height)
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
            var completion = AutoCompleteEvent(_availableText.ToString(), _cursorPosition);
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
                ForceCursor(_cursorPosition);
            }
        }

        private void UpdateCompletionWindow()
        {
            if (_completion != null)
                throw new Exception("This method should only be called if the window has been hidden");

            var completion = AutoCompleteEvent(_availableText.ToString(), _cursorPosition);
            var completions = completion.Result;

            if (completions != null)
            {
                var ncompletions = completions.Length;

                if (ncompletions > 0)
                {
                    ShowCompletions(completion.Prefix, completion.Result);
                    Render();
                    ForceCursor(_cursorPosition);
                }
            }
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
                _search = "";
                SetSearchPrompt("");
            }
            else
            {
                if (_search == "")
                {
                    if (_lastSearch != "" && _lastSearch != null)
                    {
                        _search = _lastSearch;
                        SetSearchPrompt(_search);

                        ReverseSearch();
                    }
                    return;
                }
                ReverseSearch();
            }
        }

        private void CmdRefresh()
        {
            Console.Clear();
            _maxRendered = 0;
            Render();
            ForceCursor(_cursorPosition);
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
                _history.Update(_availableText.ToString());
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
            _killBuffer = _availableText.ToString(_cursorPosition, _availableText.Length - _cursorPosition);
            _availableText.Length = _cursorPosition;
            ComputeRendered();
            RenderAfter(_cursorPosition);
        }

        private void CmdYank()
        {
            InsertTextAtCursor(_killBuffer);
        }

        private void CmdDeleteWord()
        {
            var pos = WordForward(_cursorPosition);

            if (pos != -1)
            {
                var k = _availableText.ToString(_cursorPosition, pos - _cursorPosition);
                var o = _lastHandler == CmdDeleteWord ? _killBuffer : String.Empty;
                _killBuffer = k + o;
                _availableText.Remove(_cursorPosition, pos - _cursorPosition);
                ComputeRendered();
                RenderAfter(_cursorPosition);
            }
        }

        private void CmdDeleteBackword()
        {
            var pos = WordBackward(_cursorPosition);

            if (pos != -1)
            {
                var k = _availableText.ToString(pos, _cursorPosition - pos);
                var o = _lastHandler == CmdDeleteBackword ? _killBuffer : String.Empty;
                _killBuffer = k + o;
                _availableText.Remove(pos, _cursorPosition - pos);
                ComputeRendered();
                RenderAfter(pos);
            }
        }

        private void CmdDone()
        {
            if (_completion != null)
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

            if (AutoCompleteEvent != null)
            {
                if (TabAtStartCompletes)
                {
                    complete = true;
                }
                else
                {
                    for (var i = 0; i < _cursorPosition; i++)
                    {
                        if (!Char.IsWhiteSpace(_availableText[i]))
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
            UpdateCursor(_availableText.Length);
        }

        private void CmdLeft()
        {
            if (_cursorPosition != 0)
            {
                UpdateCursor(_cursorPosition - 1);
            }
        }

        private void CmdBackwardWord()
        {
            var p = WordBackward(_cursorPosition);

            if (p != -1)
            {
                UpdateCursor(p);
            }
        }

        private void CmdForwardWord()
        {
            var p = WordForward(_cursorPosition);

            if (p != -1)
            {
                UpdateCursor(p);
            }
        }

        private void CmdRight()
        {
            if (_cursorPosition != _availableText.Length)
            {
                UpdateCursor(_cursorPosition + 1);
            }
        }

        private void RenderAfter(Int32 p)
        {
            ForceCursor(p);
            RenderFrom(p);
            ForceCursor(_cursorPosition);
        }

        private void CmdBackspace()
        {
            if (_cursorPosition != 0)
            {
                var completing = _completion != null;
                HideCompletions();
                _availableText.Remove(--_cursorPosition, 1);
                ComputeRendered();
                RenderAfter(_cursorPosition);

                if (completing)
                {
                    UpdateCompletionWindow();
                }
            }
        }

        private void CmdDeleteChar()
        {
            // If there is no input, this behaves like EOF
            if (_availableText.Length == 0)
            {
                _done = true;
                _availableText.Clear();
                Console.WriteLine();
            }
            else if (_cursorPosition != _availableText.Length)
            {
                _availableText.Remove(_cursorPosition, 1);
                ComputeRendered();
                RenderAfter(_cursorPosition);
            }
        }

        #endregion
    }
}
