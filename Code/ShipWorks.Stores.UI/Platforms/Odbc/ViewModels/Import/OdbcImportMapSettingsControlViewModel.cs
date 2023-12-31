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
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Editions;
using ShipWorks.Stores.Platforms.Odbc;
using ShipWorks.Stores.Platforms.Odbc.DataAccess;
using ShipWorks.Stores.Platforms.Odbc.DataSource.Schema;
using ShipWorks.Stores.Platforms.Odbc.Download;
using ShipWorks.Stores.Platforms.Odbc.Mapping;
using ShipWorks.Stores.Warehouse.StoreData;

namespace ShipWorks.Stores.UI.Platforms.Odbc.ViewModels.Import
{
    /// <summary>
    /// ViewModel for OdbcMapSettingsControl
    /// </summary>
    public class OdbcImportMapSettingsControlViewModel : OdbcMapSettingsControlViewModel
    {
        private bool columnSourceIsTable = true;
        private bool isSubquery = true;
        private OdbcImportStrategy importStrategy = OdbcImportStrategy.ByModifiedTime;
        private OdbcImportOrderItemStrategy importOrderItemStrategy = OdbcImportOrderItemStrategy.SingleLine;
        private IOdbcFieldMap fieldMap;
        private readonly Func<IOpenFileDialog> openFileDialogFactory;
        private readonly IOdbcImportSettingsFile importSettingsFile;
        private readonly ILicenseService licenseService;
        private readonly IOdbcStoreRepository odbcStoreRepository;
        private bool parameterizedQueryAllowed;

        /// <summary>
        /// Initializes a new instance of the <see cref="OdbcImportMapSettingsControlViewModel"/> class.
        /// </summary>
        public OdbcImportMapSettingsControlViewModel(IMessageHelper messageHelper,
            Func<string, IOdbcColumnSource> columnSourceFactory,
            IOdbcFieldMap fieldMap,
            Func<IOpenFileDialog> openFileDialogFactory,
            IOdbcImportSettingsFile importSettingsFile,
            ILicenseService licenseService,
            IOdbcStoreRepository odbcStoreRepository) :
                base(messageHelper, columnSourceFactory)
        {
            this.fieldMap = fieldMap;
            this.openFileDialogFactory = openFileDialogFactory;
            this.importSettingsFile = importSettingsFile;
            this.licenseService = licenseService;
            this.odbcStoreRepository = odbcStoreRepository;
            OpenMapSettingsFileCommand = new RelayCommand(OpenMapSettingsFile);
        }

        /// <summary>
        /// Gets the load map command.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand OpenMapSettingsFileCommand { get; }

        /// <summary>
        /// Whether the column source selected is table
        /// </summary>
        [Obfuscation(Exclude = true)]
        public override bool ColumnSourceIsTable
        {
            get => columnSourceIsTable;
            set
            {
                Handler.Set(nameof(ColumnSourceIsTable), ref columnSourceIsTable, value);

                // Set query type to subquery query is selected
                if (!value)
                {
                    IsSubquery = true;
                }

                ColumnSource = value ? SelectedTable : CustomQueryColumnSource;
                ParameterizedQueryAllowed = !value && ImportStrategy != OdbcImportStrategy.All;
            }
        }

        /// <summary>
        /// The stores import strategy
        /// </summary>
        [Obfuscation(Exclude = true)]
        public OdbcImportStrategy ImportStrategy
        {
            get => importStrategy;
            set
            {
                Handler.Set(nameof(ImportStrategy), ref importStrategy, value);

                // Parameterized query is only allowed when not using all
                ParameterizedQueryAllowed = value != OdbcImportStrategy.All && !ColumnSourceIsTable;

                // if the user changes their import strategy to all and they are using query, make sure we set it subquery
                // since that is their only option
                if (!ColumnSourceIsTable && value == OdbcImportStrategy.All)
                {
                    IsSubquery = true;
                }
            }
        }

        /// <summary>
        /// The stores import strategy
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool IsSubquery
        {
            get => isSubquery;
            set => Handler.Set(nameof(IsSubquery), ref isSubquery, value);
        }

        /// <summary>
        /// Whether or not a parameterized query is allowed
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool ParameterizedQueryAllowed
        {
            get => parameterizedQueryAllowed;
            set => Handler.Set(nameof(ParameterizedQueryAllowed), ref parameterizedQueryAllowed, value);
        }

        /// <summary>
        /// Info regarding the two query options
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string QueryInfo =>
            "When Subquery is selected, ShipWorks will use your query and filter the results based on the selected download option. \n\n" +
            "When Parameterized Query is selected, ShipWorks will give you a parameter to use when writing your query, based on the selected download option. ShipWorks will run your query as is. " +
            "This option is best for users who want full control over the import query or users whose ODBC Driver does not support subqueries.";

        /// <summary>
        /// Loads the map from disk.
        /// </summary>
        private void OpenMapSettingsFile()
        {
            IOpenFileDialog fileDialog = openFileDialogFactory();
            fileDialog.DefaultExt = importSettingsFile.Extension;
            fileDialog.Filter = importSettingsFile.Filter;
            fileDialog.DefaultFileName = fieldMap.Name;

            if (fileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            using (Stream streamToOpen = fileDialog.CreateFileStream())
            using (StreamReader reader = new StreamReader(streamToOpen))
            {
                GenericResult<JObject> openResults = importSettingsFile.Open(reader);
                if (openResults.Success)
                {
                    ImportStrategy = importSettingsFile.OdbcImportStrategy;

                    ColumnSourceIsTable = importSettingsFile.ColumnSourceType == OdbcColumnSourceType.Table;
                    LoadAndSetColumnSource(importSettingsFile.ColumnSource);
                    MapName = importSettingsFile.OdbcFieldMap.Name;
                    importOrderItemStrategy = importSettingsFile.OdbcImportItemStrategy;

                    fieldMap = importSettingsFile.OdbcFieldMap;

                    // upgrade the map to support alpha numeric order numbers just in case the user is trying to
                    // restore a map created prior to ShipWorks supporting alpha numeric order numbers
                    fieldMap.UpgradeToAlphanumericOrderNumbers();
                }
            }
        }

        /// <summary>
        /// Validate warehoue customer sets store up to be on demand and perform base validation.
        /// </summary>
        public override bool ValidateRequiredMapSettings()
        {
            if (IsWarehouseAllowed && ImportStrategy == OdbcImportStrategy.All)
            {
                messageHelper.ShowError("Warehouse customers can not select \"All orders\"");
                return false;
            }

            return base.ValidateRequiredMapSettings();
        }

        /// <summary>
        /// Saves the map settings.
        /// </summary>
        public override void SaveMapSettings(OdbcStoreEntity store)
        {
            store.ImportStrategy = (int) ImportStrategy;

            if (ColumnSourceIsTable)
            {
                store.ImportColumnSourceType = (int) OdbcColumnSourceType.Table;
                store.ImportColumnSource = SelectedTable?.Name;
            }
            else
            {
                store.ImportColumnSourceType = IsSubquery ?
                    (int) OdbcColumnSourceType.CustomQuery :
                    (int) OdbcColumnSourceType.CustomParameterizedQuery;
            }

            store.ImportOrderItemStrategy = (int) importOrderItemStrategy;

            fieldMap.Name = MapName;
            store.ImportMap = fieldMap.Serialize();
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
                messageHelper.ShowError("Failed to load import map");
                return;
            }

            fieldMap.Load(odbcStore.ImportMap);
            MapName = fieldMap.Name;

            ImportStrategy = (OdbcImportStrategy) odbcStore.ImportStrategy;

            ColumnSourceIsTable = odbcStore.ImportColumnSourceType == (int) OdbcColumnSourceType.Table;
            IsSubquery = odbcStore.ImportColumnSourceType == (int) OdbcColumnSourceType.CustomQuery;

            importOrderItemStrategy = (OdbcImportOrderItemStrategy) odbcStore.ImportOrderItemStrategy;
        }

        /// <summary>
        /// The column source name to use for custom query
        /// </summary>
        public override string CustomQueryColumnSourceName => "Custom Import";

        /// <summary>
        /// Indicates if warehouse is allowed
        /// </summary>
        private bool IsWarehouseAllowed
        {
            get
            {
                EditionRestrictionLevel restrictionLevel = licenseService.CheckRestriction(EditionFeature.Warehouse, null);

                // If warehouse is not allowed, return false
                return restrictionLevel == EditionRestrictionLevel.None;
            }
        }
    }
}
