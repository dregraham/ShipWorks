using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ShipWorks.UI.Controls
{
    /// <summary>
    /// Extended Checkbox that allows attaching an underlying value
    /// Helpfull when an object needs to be passed to an event handler
    /// </summary>
    public class ValueCheckBox<T> : CheckBox
    {
        /// <summary>
        /// The underlying value of the checkbox
        /// </summary>
        public T Value { get; set; }
    }
}
