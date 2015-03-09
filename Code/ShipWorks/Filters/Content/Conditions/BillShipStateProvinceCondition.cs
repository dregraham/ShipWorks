using System;
using System.Collections.Generic;
using System.Text;
using ShipWorks.Filters.Content.Editors.ValueEditors;

namespace ShipWorks.Filters.Content.Conditions
{
    /// <summary>
    /// Base for conditions dealing with states\provinces
    /// </summary>
    public abstract class BillShipStateProvinceCondition : BillShipAddressStringCondition
    {
        /// <summary>
        /// Create the state condition editor
        /// </summary>
        public override ValueEditor CreateEditor()
        {
            return new BillShipStateProvinceValueEditor(this);
        }
    }
}
