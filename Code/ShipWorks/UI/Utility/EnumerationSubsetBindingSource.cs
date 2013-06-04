using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using ShipWorks.Shipping.Carriers.Postal;
using System.Data;
using Interapptive.Shared.Utility;
using System.Diagnostics;

namespace ShipWorks.UI.Utility
{
    /// <summary>
    /// DataBinding source for display enumeration values, including a value that isn't in the original source.
    /// </summary>
    class EnumerationSubsetBindingSource : BindingSource
    {
        /// <summary>
        /// Method for instantiating a ShipWorksBindingSource
        /// </summary>
        public static EnumerationSubsetBindingSource Create<T>(List<KeyValuePair<string, T>> visibleValues,
                                                                List<KeyValuePair<string, T>> hiddenValues) where T : struct
        {
            return new EnumerationSubsetBindingSource(ConvertDataSource<T>(visibleValues, hiddenValues));
        }

        /// <summary>
        /// Constructor
        /// </summary>
        private EnumerationSubsetBindingSource(object dataSource)
            : base(dataSource, "")
        {
            // only show active enumeration values
            this.Filter = "Visible = true";

            // obsolete ones will go at the end, and preserve original ordering of active values
            this.Sort = "Rank ASC";
        }

        /// <summary>
        /// Transform the keyvalue pair of name => enum value to a format we want to work with.
        /// </summary>
        private static object ConvertDataSource<T>(List<KeyValuePair<string, T>> visibleValues, List<KeyValuePair<string, T>> hiddenValues) where T : struct
        {
            DataTable newSource = new DataTable();
            newSource.Columns.Add("Key", typeof(string));
            newSource.Columns.Add("Value", typeof(object));
            newSource.Columns.Add("Visible", typeof(bool));
            newSource.Columns.Add("Rank", typeof(int));

            int rank = 0;
            foreach (KeyValuePair<string, T> pair in visibleValues)
            {
                newSource.Rows.Add(pair.Key, (T)pair.Value, true, rank++);
            }

            // add any of the missing values
            foreach (KeyValuePair<string, T> pair in hiddenValues.Except(visibleValues.Where(p => visibleValues.Any(p2 => p2.Value.Equals(p.Value)))))
            {
                newSource.Rows.Add(pair.Key, (T)pair.Value, false, rank++);
            }

            return newSource;
        }

        /// <summary>
        /// Locates the index of a specified Value
        /// </summary>
        public override int Find(System.ComponentModel.PropertyDescriptor prop, object key)
        {
            int index = base.Find(prop, key);

            // the value wasn't found, meaning it was probably filtered out by us
            if (index < 0)
            {
                // include the missing value in the filter
                this.Filter = "Visible = true OR Value = " + (int)((object)key);

                // repeat the search
                index = base.Find(prop, key);
            }

            return index;
        }
    }
}