using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShipWorks.Stores.Platforms.GenericModule.LegacyAdapter
{
    /// <summary>
    /// PostVariableTransformer for completely removing a variable from the 
    /// outgoing request.
    /// </summary>
    public class RemoveVariableTransformer : VariableTransformer
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public RemoveVariableTransformer()
            : base((string)null)
        {

        }


        /// <summary>
        /// Transforms by simply returning null, indicating the variable is to be
        /// removed from the request alltogether.
        /// </summary>
        protected override string TransformValue(string originalValue)
        {
            return null;
        }
    }
}
