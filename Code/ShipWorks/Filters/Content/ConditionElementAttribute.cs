using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Stores.Platforms;
using ShipWorks.Stores;

namespace ShipWorks.Filters.Content
{
    /// <summary>
    /// Attribute that applies a name to a condition type
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited=false)]
    public sealed class ConditionElementAttribute : Attribute
    {
        string displayName;
        string identifier;
        string applicableTest;

        /// <summary>
        /// Constructor
        /// </summary>
        public ConditionElementAttribute(string displayName, string identifier)
        {
            this.displayName = displayName;
            this.identifier = identifier;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ConditionElementAttribute(string displayName, string identifier, string applicableTest)
        {
            this.displayName = displayName;
            this.identifier = identifier;
            this.applicableTest = applicableTest;
        }

        /// <summary>
        /// The name of the condition.
        /// </summary>
        public string DisplayName
        {
            get
            {
                return displayName;
            }
        }

        /// <summary>
        /// An identifier that uniquely identifies the condition element.  Once applied, this identifier cannot be changed,
        /// as it is used in a key in the serialization process.
        /// </summary>
        public string Identifier
        {
            get
            {
                return identifier;
            }
        }

        /// <summary>
        /// The name of a public static function of the format "bool Func(List&lt;StoreType&gt;)" that controls if 
        /// the condition is applicable based on the current store set.
        /// </summary>
        public string ApplicableTest
        {
            get { return applicableTest; }
        }
    }
}
