using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.Design;
using System.ComponentModel;

namespace ShipWorks.UI.Controls.Design
{
    /// <summary>
    /// Custom collection editor for OptionPages
    /// </summary>
    public class OptionPageCollectionEditor : CollectionEditor
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public OptionPageCollectionEditor()
            : base(typeof(OptionControl.OptionPageCollection))
        {

        }

        /// <summary>
        /// Set the items of the collection
        /// </summary>
        protected override object SetItems(object collection, object[] items)
        {
            OptionControl optionControl = Context.Instance as OptionControl;
            if (optionControl != null)
            {
                optionControl.SuspendLayout();
            }

            object newCollection = base.SetItems(collection, items);

            if (optionControl != null)
            {
                optionControl.ResumeLayout();
            }

            return newCollection;
        }

    }
}
