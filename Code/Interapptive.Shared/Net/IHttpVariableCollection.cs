using System;
using System.Collections.Generic;

namespace Interapptive.Shared.Net
{
    /// <summary>
    /// Interface for collection of HttpVariables
    /// </summary>
    public interface IHttpVariableCollection : IEnumerable<HttpVariable>
    {
        /// <summary>
        /// Gets a variable's value by name
        /// </summary>
        string this[string name] { get; }

        /// <summary>
        /// Number of variables
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Add a variable of the specified name and value
        /// </summary>
        void Add(string name, string value);

        /// <summary>
        /// Remove all variables of the specivied name
        /// </summary>
        void Remove(string name);

        /// <summary>
        /// Sorts/Reorders the variables using the keySelector func provided
        /// </summary>
        void Sort<TKey>(Func<HttpVariable, TKey> keySelector);

        /// <summary>
        /// Add a variable
        /// </summary>
        void Add(HttpVariable item);

        /// <summary>
        /// Clear the collection of variables
        /// </summary>
        void Clear();

        /// <summary>
        /// Copy 
        /// </summary>
        void CopyTo(HttpVariable[] array, int index);

        /// <summary>
        /// Contains 
        /// </summary>
        bool Contains(HttpVariable item);

        /// <summary>
        /// Index of an item 
        /// </summary>
        int IndexOf(HttpVariable item);

        /// <summary>
        /// Insert an item 
        /// </summary>
        void Insert(int index, HttpVariable item);

        /// <summary>
        /// Remove an item 
        /// </summary>
        bool Remove(HttpVariable item);

        /// <summary>
        /// Remove an item at the specified index
        /// </summary>
        void RemoveAt(int index);

        /// <summary>
        /// Indexer 
        /// </summary>
        HttpVariable this[int index] { get; set; }
    }
}
