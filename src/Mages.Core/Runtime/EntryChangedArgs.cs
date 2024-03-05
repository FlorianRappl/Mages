namespace Mages.Core.Runtime
{
    using System;

    /// <summary>
    /// Contains the event data for changes observed in a dictionary.
    /// </summary>
    /// <remarks>
    /// Creates a new event data container.
    /// </remarks>
    public class EntryChangedArgs(String key, Object oldValue, Object newValue) : EventArgs
    {

        /// <summary>
        /// Gets the key that changed.
        /// </summary>
        public String Key
        {
            get;
            private set;
        } = key;

        /// <summary>
        /// Gets the previously assigned value, if any.
        /// </summary>
        public Object OldValue
        {
            get;
            private set;
        } = oldValue;

        /// <summary>
        /// Gets the currently assigned value, if any.
        /// </summary>
        public Object NewValue
        {
            get;
            private set;
        } = newValue;
    }
}
