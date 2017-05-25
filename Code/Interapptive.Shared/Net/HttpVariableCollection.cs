using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Interapptive.Shared.Net
{
    /// <summary>
    /// Collection of HttpVariable
    /// </summary>
    public class HttpVariableCollection : Collection<HttpVariable>, IHttpVariableCollection
    {
        /// <summary>
        /// Gets a variable's value by name
        /// </summary>
        public string this[string name]
        {
            get
            {
                foreach (HttpVariable variable in this)
                {
                    if (String.Compare(variable.Name, name, StringComparison.OrdinalIgnoreCase) == 0)
                    {
                        return variable.Value;
                    }
                }

                return null;
            }
        }

        /// <summary>
        /// Add a variable of the specified name and value
        /// </summary>
        public void Add(string name, string value)
        {
            this.Add(new HttpVariable(name, value));
        }

        /// <summary>
        /// Remove all variables of the specivied name
        /// </summary>
        public void Remove(string name)
        {
            foreach (HttpVariable variable in this.Where(v => v.Name == name).ToList())
            {
                this.Remove(variable);
            }
        }

        /// <summary>
        /// Sorts/Reorders the variables using the keySelector func provided
        /// </summary>
        public void Sort<TKey>(Func<HttpVariable, TKey> keySelector)
        {
            // return the variables sorted by the keySelector
            List<HttpVariable> variables = this.OrderBy(keySelector).ToList();

            // clear out the variables
            this.Clear();

            // add them back in, in order
            variables.ForEach(v => this.Add(v));
        }
    }
}