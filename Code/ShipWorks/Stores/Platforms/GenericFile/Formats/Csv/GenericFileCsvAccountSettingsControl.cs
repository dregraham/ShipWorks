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
using ShipWorks.Data.Import.Spreadsheet;
using ShipWorks.Data.Import.Spreadsheet.OrderSchema;
using ShipWorks.Data.Import.Spreadsheet.Types.Csv;
using log4net;
using ShipWorks.Stores.Platforms.GenericFile.Sources;
using Interapptive.Shared.UI;

namespace ShipWorks.Stores.Platforms.GenericFile.Formats.Csv
{
    /// <summary>
    /// Account settings control for CSV
    /// </summary>
    public partial class GenericFileCsvAccountSettingsControl : AccountSettingsControlBase
    {
        static readonly ILog log = LogManager.GetLogger(typeof(GenericFileCsvAccountSettingsControl));

        /// <summary>
        /// Constructor
        /// </summary>
        public GenericFileCsvAccountSettingsControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Load the data from the given store into the control
        /// </summary>
        public override void LoadStore(StoreEntity store)
        {
            GenericFileStoreEntity generic = (GenericFileStoreEntity) store;

            var schema = new GenericSpreadsheetOrderSchema();

            csvMapChooser.Initialize(schema);

            try
            {
                csvMapChooser.Map = new GenericCsvMap(schema, generic.FlatImportMap);
            }
            catch (GenericSpreadsheetException ex)
            {
                log.Error("Failed to load import map", ex);
            }

            fileSourceControl.LoadStore(generic, false);
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
                string map = csvMapChooser?.Map?.SerializeToXml() ?? string.Empty;

                if (map == string.Empty)
                {
                    MessageHelper.ShowError(this, "Map cannot be empty!");
                    return false;
                }

                generic.FlatImportMap = map;
            }

            return true;
        }
    }
}
