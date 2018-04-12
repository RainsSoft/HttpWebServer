using System;
using System.Collections;
using System.Collections.Generic;

namespace HttpServer.Mvc
{
    /// <summary>
    /// Dictionary containing view data information
    /// </summary>
    public class ViewDataDictionary : IEnumerable<KeyValuePair<string, object>>
    {
        private readonly Dictionary<string, object> _items = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets an item in the view data.
        /// </summary>
        /// <param name="name">Name of item</param>
        /// <returns>Item if found; otherwise <c>null</c>.</returns>
        public object this[string name]
        {
            get
            {
                object value;
                return _items.TryGetValue(name, out value) ? value : null;
            }
            set { _items[name] = value; }
        }

        /// <summary>
        /// Add an item
        /// </summary>
        /// <param name="name">Name of item</param>
        /// <param name="value">Item</param>
        /// <remarks>
        /// Another item with the same name must not exist in the collection.
        /// </remarks>
        public void Add(string name, object value)
        {
            _items.Add(name, value);
        }

        /// <summary>
        /// Try get an item
        /// </summary>
        /// <param name="name">Name of item</param>
        /// <param name="value">Item</param>
        /// <returns><c>true</c> if found; otherwise <c>false</c>.</returns>
        public bool TryGetValue(string name, out object value)
        {
            return _items.TryGetValue(name, out value);
        }

        #region IEnumerable<KeyValuePair<string,object>> Members

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        /// <summary>
        /// Remove everything from the dictionary.
        /// </summary>
        public void Clear()
        {
            _items.Clear();
        }

        /// <summary>
        /// Checks if view data contains a parameter.
        /// </summary>
        /// <param name="key">Parameter key</param>
        /// <returns><c>true</c> if found; otherwise <c>false</c>.</returns>        
        public bool Contains(string key)
        {
            return _items.ContainsKey(key);
        }
    }
}