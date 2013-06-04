using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Filters.Content.Editors.ValueEditors;

namespace ShipWorks.Filters.Content.Conditions
{
    /// <summary>
    /// Base for conditions dealing with a single country
    /// </summary>
    public abstract class CountryCondition : StringCondition
    {
        /// <summary>
        /// Create the editor to use for country-based conditions
        /// </summary>
        /// <returns></returns>
        public override ValueEditor CreateEditor()
        {
            return new CountryValueEditor(this);
        }
    }
}
