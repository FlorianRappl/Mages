namespace Mages.Repl
{
    using System;
    using System.IO;

    sealed class History
    {
        private readonly String[] _history;
        private readonly String _histfile;
        private Int32 _cursor;
        private Int32 _head;
        private Int32 _tail;
        private Int32 _count;

        public History(String app, Int32 size)
        {
            if (size < 1)
                throw new ArgumentException("size");

            if (app is not null)
            {
                var dir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

                if (!Directory.Exists(dir))
                {
                    try { Directory.CreateDirectory(dir); }
                    catch { app = null; }
                }

                if (app is not null)
                {
                    _histfile = Path.Combine(dir, app) + ".history";
                }
            }

            _history = new String[size];
            _head = _tail = _cursor = 0;

            if (File.Exists(_histfile))
            {
                using (var sr = File.OpenText(_histfile))
                {
                    var line = String.Empty;

                    while ((line = sr.ReadLine()) is not null)
                    {
                        if (!String.IsNullOrEmpty(line))
                        {
                            Append(line);
                        }
                    }
                }
            }
        }

        public void Close()
        {
            if (_histfile is not null)
            {
                try
                {
                    using (var sw = File.CreateText(_histfile))
                    {
                        var start = (_count == _history.Length) ? _head : _tail;

                        for (var i = start; i < start + _count; i++)
                        {
                            var p = i % _history.Length;
                            sw.WriteLine(_history[p]);
                        }
                    }
                }
                catch
                {
                    // ignore
                }
            }
        }

        public void Append(String s)
        {
            _history[_head] = s;
            _head = (_head + 1) % _history.Length;

            if (_head == _tail)
            {
                _tail = (_tail + 1 % _history.Length);
            }

            if (_count != _history.Length)
            {
                _count++;
            }
        }

        public void Update(String s)
        {
            _history[_cursor] = s;
        }

        public void RemoveLast()
        {
            _head = _head - 1;

            if (_head < 0)
            {
                _head = _history.Length - 1;
            }
        }

        public void Accept(String s)
        {
            var t = _head - 1;

            if (t < 0)
            {
                t = _history.Length - 1;
            }

            _history[t] = s;
        }

        public Boolean PreviousAvailable()
        {
            if (_count != 0)
            {
                var next = _cursor - 1;

                if (next < 0)
                    next = _count - 1;

                return next != _head;
            }

            return false;
        }

        public Boolean NextAvailable()
        {
            if (_count != 0)
            {
                var next = (_cursor + 1) % _history.Length;
                return next != _head;
            }
                
            return false;
        }

        public String Previous()
        {
            if (PreviousAvailable())
            {
                _cursor--;

                if (_cursor < 0)
                    _cursor = _history.Length - 1;

                return _history[_cursor];
            }

            return null;
        }

        public String Next()
        {
            if (NextAvailable())
            {
                _cursor = (_cursor + 1) % _history.Length;
                return _history[_cursor];
            }
                
            return null;
        }

        public void CursorToEnd()
        {
            if (_head != _tail)
            {
                _cursor = _head;
            }
        }

        public String SearchBackward(String term)
        {
            for (var i = 0; i < _count; i++)
            {
                var slot = _cursor - i - 1;

                if (slot < 0)
                    slot = _history.Length + slot;

                if (slot >= _history.Length)
                    slot = 0;

                if (_history[slot] is not null && _history[slot].IndexOf(term) != -1)
                {
                    _cursor = slot;
                    return _history[slot];
                }
            }

            return null;
        }
    }
}
