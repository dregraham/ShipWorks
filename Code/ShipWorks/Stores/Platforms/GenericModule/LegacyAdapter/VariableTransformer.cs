using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Net;

namespace ShipWorks.Stores.Platforms.GenericModule.LegacyAdapter
{
    /// <summary>
    /// Delegate for providing a new transformation method to the PostVariableTransformer
    /// </summary>
    public delegate string HttpVariableValueTransformer(string originalValue);
    
    /// <summary>
    /// Used to transform HttpPost variables when communicating with legacy
    /// ShipWorks modules.
    /// </summary>
    public class VariableTransformer
    {
        // new varialbe name, null indicates no name change
        string newName = null;

        // value transformation method
        HttpVariableValueTransformer valueTransformer;

        /// <summary>
        /// Constructor for renaming HttpPostVariables before they are sent
        /// </summary>
        public VariableTransformer(string newVariableName) 
            : this(newVariableName, null)
        {
        }

        /// <summary>
        /// Constructor for specifying both parameter renaming and value transformation
        /// </summary>
        public VariableTransformer(string newVariableName, HttpVariableValueTransformer transformMethod)
        {
            this.newName = newVariableName;
            this.valueTransformer = transformMethod;
        }

        /// <summary>
        /// Constructor for specifying just a value transformation method
        /// </summary>
        public VariableTransformer(HttpVariableValueTransformer transformMethod)
        {
            this.valueTransformer = transformMethod;
        }

        /// <summary>
        /// Creates a new, transformed HttpPostVariable based on the one provided
        /// </summary>
        public HttpVariable TransformVariable(HttpVariable variable)
        {
            string newValue = TransformValue(variable.Value);

            if (newValue == null)
            {
                // null will allow a parameter to be removed alltogether
                return null;
            }

            // if a new name wasn't registered, keep the old one
            string newVariableName = variable.Name;
            if (newName != null)
            {
                newVariableName = newName;
            }

            return new HttpVariable(newVariableName, newValue);
        }

        /// <summary>
        /// Transforms the value of an HttpPostVariable before it is sent
        /// </summary>
        protected virtual string TransformValue(string originalValue)
        {
            if (valueTransformer != null)
            {
                return valueTransformer(originalValue);
            }
            else
            {
                // return the value provided
                return originalValue;
            }
        }
    }
}
