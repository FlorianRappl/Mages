namespace Mages.Core
{
    using System;

    /// <summary>
    /// Represents a position within a text source.
    /// </summary>
    public struct TextPosition
    {
        private Int32 _row;
        private Int32 _column;
        private Int32 _index;

        /// <summary>
        /// Creates a new text position.
        /// </summary>
        /// <param name="row">The row number.</param>
        /// <param name="column">The column number.</param>
        /// <param name="index">The character index.</param>
        public TextPosition(Int32 row, Int32 column, Int32 index)
        {
            _row = row;
            _column = column;
            _index = index;
        }

        /// <summary>
        /// Gets the row in the source code.
        /// </summary>
        public Int32 Row
        {
            get { return _row; }
        }

        /// <summary>
        /// Gets the column in the source code.
        /// </summary>
        public Int32 Column
        {
            get { return _column; }
        }

        /// <summary>
        /// Gets the index (absolute position) in the source code.
        /// </summary>
        public Int32 Index
        {
            get { return _index; }
        }
    }
}
