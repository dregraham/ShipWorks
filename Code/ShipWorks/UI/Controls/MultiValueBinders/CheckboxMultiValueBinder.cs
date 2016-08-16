using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShipWorks.UI.Controls.MultiValueBinders
{
    /// <summary>
    /// Class to help bind UI Checkbox controls with a list of items.
    /// </summary>
    /// <typeparam name="TDataSource">The list of items on which to bind.</typeparam>
    public class CheckboxMultiValueBinder<TDataSource> : GenericMultiValueBinder<TDataSource, bool>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dataSource">Generic list of items on which to bind.</param>
        /// <param name="selectFunc">Function that returns a property value of TDataSource.  This is used to determine if the dataSource has distinct values. </param>
        /// <param name="updateFunc">Action that updates each of the items in dataSource.</param>
        public CheckboxMultiValueBinder(IEnumerable<TDataSource> dataSource, string propertyName, Func<TDataSource, bool> selectFunc, Action<TDataSource, bool> updateFunc, Func<TDataSource, bool> readOnly) : base(dataSource, propertyName, selectFunc, updateFunc, readOnly)
        {
        }

        /// <summary>
        /// The CheckState value.
        /// </summary>
        public CheckState CheckStateValue
        {
            get
            {
                if (IsMultiValued)
                {
                    return CheckState.Indeterminate;
                }

                if (DistinctPropertyValues.All(dpv => dpv))
                {
                    return CheckState.Checked;
                }

                return CheckState.Unchecked;
            }
            set
            {
            }
        }
    }
}
