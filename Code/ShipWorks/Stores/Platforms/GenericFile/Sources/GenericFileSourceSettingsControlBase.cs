using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Stores.Platforms.GenericFile.Sources
{
    /// <summary>
    /// Base for all file source settings controls
    /// </summary>
    public partial class GenericFileSourceSettingsControlBase : UserControl
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GenericFileSourceSettingsControlBase()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Must be implemented by derived types to load file source specific data
        /// </summary>
        public virtual void LoadStore(GenericFileStoreEntity store)
        {

        }

        /// <summary>
        /// Must be implemented by derived types to save the settings to the store.  Return false if there is an error
        /// and a message was displayed to the user.
        /// </summary>
        public virtual bool SaveToEntity(GenericFileStoreEntity store)
        {
            return true;
        }
    }
}
