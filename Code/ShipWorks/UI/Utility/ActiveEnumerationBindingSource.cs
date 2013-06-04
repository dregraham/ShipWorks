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
    /// DataBinding Source for filtering out enum values that have been
    /// marked as Deprecated in ShipWorks.
    /// </summary>
    class ActiveEnumerationBindingSource : BindingSource
    {
        /// <summary>
        /// Method for instantiating a ShipWorksBindingSource
        /// </summary>
        public static ActiveEnumerationBindingSource Create<T>(List<KeyValuePair<string, T>> dataSource) where T : struct
        {
            return new ActiveEnumerationBindingSource(ConvertDataSource<T>(dataSource));
        }

        /// <summary>
        /// Constructor
        /// </summary>
        private ActiveEnumerationBindingSource(object dataSource) : base (dataSource, "")
        {
            // only show active enumeration values
            this.Filter = "Deprecated = false";

            // obsolete ones will go at the end, and preserve original ordering of active values
            this.Sort = "Deprecated ASC, Rank ASC";
        }

        /// <summary>
        /// Transform the keyvalue pair of name => enum value to a format we want to work with.
        /// </summary>
        private static object ConvertDataSource<T>(object dataSource) where T : struct
        {
            DataTable newSource = new DataTable();
            newSource.Columns.Add("Key", typeof(string));
            newSource.Columns.Add("Value", typeof(object));
            newSource.Columns.Add("Deprecated", typeof(bool));
            newSource.Columns.Add("Rank", typeof(int));

            IList list = dataSource as IList;
            if (list != null)
            {
                int rank = 0;
                foreach (KeyValuePair<string, T> pair in list)
                {
                    newSource.Rows.Add(pair.Key, (T)pair.Value, EnumHelper.GetDeprecated((Enum)(object)pair.Value), rank++);
                }
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
                if (EnumHelper.GetDeprecated((Enum)((object)key)))
                {
                    // change the Filter to allow this one obsolete value to appear
                    this.Filter = "Deprecated = false OR Value = " + (int)((object)key);

                    // repeat the search
                    index = base.Find(prop, key);
                }
            }

            return index;
        }

        /// <summary>
        /// Remove all enumeration values that are Deprecated
        /// </summary>
        public void RemoveAllDeprecated()
        {
            this.Filter = "Deprecated = false";
        }
    }
}