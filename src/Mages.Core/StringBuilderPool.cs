namespace Mages.Core
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// A pool for recycled resources.
    /// </summary>
    static class StringBuilderPool
    {
        #region Fields

		private static readonly Stack<WeakReference> _builder = new Stack<WeakReference>();
        private static readonly Object _lock = new Object();

        #endregion

        #region Methods

        /// <summary>
        /// Either creates a fresh stringbuilder or gets a (cleaned) used one.
        /// </summary>
        /// <returns>A stringbuilder to use.</returns>
        public static StringBuilder Pull()
        {
            lock (_lock)
            {
                while (_builder.Count != 0)
                {
                    var reference = _builder.Pop();
                    var builder = default(StringBuilder);

                    if (reference.IsAlive && (builder = reference.Target as StringBuilder) != null)
                    {
                        return builder.Clear();
                    }
                }
                
                return new StringBuilder();
            }
        }

        /// <summary>
        /// Returns the given stringbuilder to the pool and gets the current
        /// string content.
        /// </summary>
        /// <param name="sb">The stringbuilder to recycle.</param>
        /// <returns>The string that is contained in the stringbuilder.</returns>
        public static String Stringify(this StringBuilder sb)
        {
            lock (_lock)
            {
                var reference = new WeakReference(sb);
                _builder.Push(reference);
                return sb.ToString();
            }
        }

        #endregion
    }
}
