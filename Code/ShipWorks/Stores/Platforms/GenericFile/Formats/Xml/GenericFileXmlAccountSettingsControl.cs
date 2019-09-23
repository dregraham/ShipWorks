using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Stores.Management;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.GenericFile.Sources;

namespace ShipWorks.Stores.Platforms.GenericFile.Formats.Xml
{
    /// <summary>
    /// Account settings editor for a Generic File based store
    /// </summary>
    public partial class GenericFileXmlAccountSettingsControl : AccountSettingsControlBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public GenericFileXmlAccountSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the data from the given store into the control
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            GenericFileStoreEntity generic = (GenericFileStoreEntity) store;

            xmlSetupControl.LoadStore(generic);
            fileSourceControl.LoadStore(generic);
        }

        /// <summary>
        /// Save the configured values to the store
        /// </summary>
        public override bool SaveToEntity(StoreEntity store)
        {
            GenericFileStoreEntity generic = (GenericFileStoreEntity) store;

            if (!fileSourceControl.SaveToEntity(generic))
            {
                return false;
            }

            if (generic.FileSource != (int) GenericFileSourceTypeCode.Warehouse)
            {
                if (!xmlSetupControl.SaveToEntity(generic))
                {
                    return false;
                }
            }

            return true;
        }
    }
}