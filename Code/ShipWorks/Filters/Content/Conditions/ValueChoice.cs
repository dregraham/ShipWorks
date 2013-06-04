using System;
using System.Collections.Generic;
using System.Text;

namespace ShipWorks.Filters.Content.Conditions
{
    /// <summary>
    /// A single choice the user could make for a value in a ValueChoiceCondition
    /// </summary>
    public class ValueChoice<T>
    {
        string name;
        T value;

        /// <summary>
        /// Constructor
        /// </summary>
        public ValueChoice()
        {

        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ValueChoice(string name, T value)
        {
            this.name = name;
            this.value = value;
        }

        /// <summary>
        /// The name of the choice as presented by the user
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// The value
        /// </summary>
        public T Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
    }
}
