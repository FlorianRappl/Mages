namespace Mages.Repl
{
    using System;

    sealed class CompletionState
    {
        private Int32 _selected;
        private Int32 _top;

        public CompletionState(Int32 column, Int32 row, Int32 width, Int32 height)
        {
            Column = column;
            Row = row;
            Width = width;
            Height = height;

            if (Column < 0)
                throw new ArgumentException("Cannot be less than zero" + Column, "column");

            if (Row < 0)
                throw new ArgumentException("Cannot be less than zero", "row");

            if (Width < 1)
                throw new ArgumentException("Cannot be less than one", "width");

            if (Height < 1)
                throw new ArgumentException("Cannot be less than one", "height");
        }

        public String Current
        {
            get { return Completions[_selected]; }
        }

        public String Prefix 
        { 
            get; 
            set; 
        }

        public String[] Completions 
        { 
            get; 
            set; 
        }

        public Int32 Column 
        { 
            get; 
            set; 
        }

        public Int32 Row 
        { 
            get; 
            set; 
        }

        public Int32 Width
        { 
            get; 
            set; 
        }

        public Int32 Height 
        { 
            get;
            set; 
        }

        public void Show()
        {
            LineEditor.SaveExcursion(DrawSelection);
        }

        public void SelectNext()
        {
            if (_selected + 1 < Completions.Length)
            {
                _selected++;

                if (_selected - _top >= Height)
                {
                    _top++;
                }

                LineEditor.SaveExcursion(DrawSelection);
            }
        }

        public void SelectPrevious()
        {
            if (_selected > 0)
            {
                _selected--;

                if (_selected < _top)
                {
                    _top = _selected;
                }

                LineEditor.SaveExcursion(DrawSelection);
            }
        }

        public void Remove()
        {
            LineEditor.SaveExcursion(Clear);
        }

        private void Clear()
        {
            for (var r = 0; r < Height; r++)
            {
                Console.CursorLeft = Column;
                Console.CursorTop = Row + r;

                for (var space = 0; space <= Width; space++)
                {
                    Console.Write(" ");
                }
            }
        }

        private void DrawSelection()
        {
            for (var r = 0; r < Height; r++)
            {
                var item_idx = _top + r;
                var selected = (item_idx == _selected);
                var item = Prefix + Completions[item_idx];

                Console.ForegroundColor = selected ? ConsoleColor.Black : ConsoleColor.Gray;
                Console.BackgroundColor = selected ? ConsoleColor.Cyan : ConsoleColor.Blue;
                
                if (item.Length > Width)
                {
                    item = item.Substring(0, Width);
                }

                Console.CursorLeft = Column;
                Console.CursorTop = Row + r;
                Console.Write(item);

                for (var space = item.Length; space <= Width; space++)
                {
                    Console.Write(" ");
                }
            }
        }
    }
}
