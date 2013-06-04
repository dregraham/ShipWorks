using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Filters.Content.Editors.ValueEditors;

namespace ShipWorks.Filters.Content.Conditions
{
    /// <summary>
    /// Base class for conditions that compare againast a single state\province
    /// </summary>
    public abstract class StateProvinceCondition : StringCondition
    {
        /// <summary>
        /// Create the editor for choosing states\provinces
        /// </summary>
        public override ValueEditor CreateEditor()
        {
            return new StateProvinceValueEditor(this);
        }
    }
}
