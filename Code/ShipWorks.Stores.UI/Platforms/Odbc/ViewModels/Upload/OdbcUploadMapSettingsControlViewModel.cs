﻿using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Input;
using Autofac.Features.Indexed;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using Newtonsoft.Json.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using ShipWorks.Stores.Warehouse.StoreData;

namespace ShipWorks.Stores.UI.Platforms.Odbc.ViewModels.Upload
{
    /// <summary>
    /// ViewModel for OdbcUploadMapSettingsControl
    /// </summary>
    public class OdbcUploadMapSettingsControlViewModel : OdbcMapSettingsControlViewModel
    {
        private readonly Func<string, IDialog> dialogFactory;
        private bool columnSourceIsTable = true;
        private IOdbcFieldMap fieldMap;
        private readonly Func<IOpenFileDialog> openFileDialogFactory;
        private readonly IOdbcSettingsFile uploadSettingsFile;
        private readonly IOdbcStoreRepository odbcStoreRepository;
        private string customQuery;

        private const string InitialQueryComment =
            "/**********************************************************************/\n" +
            "/*                                                                    */\n" +
            "/* A sample query highlighting a few of the tokens that can be        */\n" +
            "/* used for uploading shipment details to your database has           */\n" +
            "/* been provided below.                                               */\n" +
            "/*                                                                    */\n" +
            "/* For more samples and additional information on how to              */\n" +
            "/* leverage ShipWorks tokens when uploading shipment details          */\n" +
            "/* using a custom query, please visit                                 */\n" +
            "/* https://shipworks.zendesk.com/hc/en-us/articles/360022649411       */\n" +
            "/*                                                                    */\n" +
            "/**********************************************************************/\n\n" +
            "UPDATE ShipmentDetails\n" +
            "SET TrackingNumber = '{//TrackingNumber}'\n" +
            "WHERE OrderID = {//Order/Number}";

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcUploadMapSettingsControlViewModel"/> class.
        /// </summary>
        public OdbcUploadMapSettingsControlViewModel(
            Func<string, IDialog> dialogFactory,
            IMessageHelper messageHelper,
            Func<string, IOdbcColumnSource> columnSourceFactory,
            IOdbcFieldMap fieldMap,
            Func<IOpenFileDialog> openFileDialogFactory,
            IOdbcSettingsFile uploadSettingsFile,
            IOdbcStoreRepository odbcStoreRepository) : base(messageHelper, columnSourceFactory)
        {
            this.dialogFactory = dialogFactory;
            this.fieldMap = fieldMap;
            this.openFileDialogFactory = openFileDialogFactory;
            this.uploadSettingsFile = uploadSettingsFile;
            this.odbcStoreRepository = odbcStoreRepository;
            CustomQuery = InitialQueryComment;
            OpenMapSettingsFileCommand = new RelayCommand(OpenMapSettingsFile);
        }

        /// <summary>
        /// Whether the column source selected is table
        /// </summary>
        [Obfuscation(Exclude = true)]
        public override bool ColumnSourceIsTable
        {
            get { return columnSourceIsTable; }
            set
            {
                Handler.Set(nameof(ColumnSourceIsTable), ref columnSourceIsTable, value);

                // Show warning dlg when query is selected
                if (!value)
                {
                    IDialog warningDlg = dialogFactory("OdbcCustomQueryWarningDlg");
                    warningDlg.ShowDialog();
                }

                ColumnSource = value ? SelectedTable : CustomQueryColumnSource;
            }
        }

        /// <summary>
        /// the custom query
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string CustomQuery
        {
            get => customQuery;
            set => Handler.Set(nameof(CustomQuery), ref customQuery, value);
        }

        /// <summary>
        /// Gets the load map command.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand OpenMapSettingsFileCommand { get; }

        /// <summary>
        /// Loads the map.
        /// </summary>
        private void OpenMapSettingsFile()
        {
            IOpenFileDialog fileDialog = openFileDialogFactory();
            fileDialog.DefaultExt = uploadSettingsFile.Extension;
            fileDialog.Filter = uploadSettingsFile.Filter;
            fileDialog.DefaultFileName = fieldMap.Name;

            if (fileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            using (Stream streamToOpen = fileDialog.CreateFileStream())
            using (StreamReader reader = new StreamReader(streamToOpen))
            {
                GenericResult<JObject> openResult = uploadSettingsFile.Open(reader);
                if (openResult.Success)
                {
                    ColumnSourceIsTable = uploadSettingsFile.ColumnSourceType == OdbcColumnSourceType.Table;


                    LoadAndSetColumnSource(uploadSettingsFile.ColumnSource);
                    if (!string.IsNullOrWhiteSpace(uploadSettingsFile.ColumnSource))
                    {
                        CustomQuery = uploadSettingsFile.ColumnSource;
                    }

                    MapName = uploadSettingsFile.OdbcFieldMap.Name;

                    fieldMap = uploadSettingsFile.OdbcFieldMap;

                    // upgrade the map to support alpha numeric order numbers just in case the user is trying to
                    // restore a map created prior to ShipWorks supporting alpha numeric order numbers
                    fieldMap.UpgradeToAlphanumericOrderNumbers();
                }
            }
        }

        /// <summary>
        /// Saves the map settings.
        /// </summary>
        /// <param name="store"></param>
        public override void SaveMapSettings(OdbcStoreEntity store)
        {
            store.UploadColumnSourceType = ColumnSourceIsTable ?
                (int) OdbcColumnSourceType.Table :
                (int) OdbcColumnSourceType.CustomQuery;

            store.UploadColumnSource = ColumnSourceIsTable ?
                SelectedTable.Name :
                CustomQuery;

            fieldMap.Name = MapName;
            store.UploadMap = fieldMap.Serialize();
        }

        /// <summary>
        /// Loads the map settings.
        /// </summary>
        public override void LoadMapSettings(OdbcStoreEntity store)
        {
            OdbcStore odbcStore;
            try
            {
                odbcStore = odbcStoreRepository.GetStore(store);
            }
            catch (ShipWorksOdbcException)
            {
                messageHelper.ShowError("Failed to load upload map");
                return;
            }

            MapName = fieldMap.Name;
            fieldMap.Load(odbcStore.UploadMap);
            
            ColumnSourceIsTable = odbcStore.UploadColumnSourceType == (int) OdbcColumnSourceType.Table;
            if (!String.IsNullOrEmpty(odbcStore.UploadColumnSource))
            {
                customQuery = odbcStore.UploadColumnSource;
            }
        }

        /// <summary>
        /// Validates the required map settings.
        /// </summary>
        public override bool ValidateRequiredMapSettings()
        {
            if (!base.ValidateRequiredMapSettings())
            {
                return false;
            }

            if (!ColumnSourceIsTable && string.IsNullOrWhiteSpace(CustomQuery))
            {
                messageHelper.ShowError("Please enter a valid query before continuing to the next page.");
                return false;
            }

            return true;
        }


        /// <summary>
        /// The column source name to use for custom query
        /// </summary>
        public override string CustomQueryColumnSourceName => "Custom Upload";
    }
}
