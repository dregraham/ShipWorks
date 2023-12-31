﻿using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Licensing.Warehouse;
using ShipWorks.Core.Common.Threading;
using ShipWorks.Data;
using ShipWorks.Users;
using Cursor = System.Windows.Forms.Cursor;
using Cursors = System.Windows.Forms.Cursors;

namespace ShipWorks.ApplicationCore.Settings.Warehouse
{
    /// <summary>
    /// View model for warehouse settings
    /// </summary>
    [Component]
    public class WarehouseSettingsViewModel : ViewModelBase, IWarehouseSettingsViewModel
    {
        private readonly IWarehouseSettings warehouseSettingsView;
        private readonly IWarehouseListViewModel warehouseList;
        private readonly IConfigurationData configurationData;
        private readonly IWarehouseSettingsApi warehouseSettingsApi;
        private readonly IMessageHelper messageHelper;
        private readonly HubMigrator hubMigrator;
        private string warehouseName;
        private int modifiedProducts;
        private bool canLinkWarehouse;
        private bool canUploadSKUs;
        private readonly IUserSession userSession;
        private readonly bool isAdmin;

        /// <summary>
        /// Constructor
        /// </summary>
        [NDependIgnoreTooManyParamsAttribute]
        public WarehouseSettingsViewModel(
            IWarehouseSettings warehouseSettingsView,
            IWarehouseListViewModel warehouseList,
            IWarehouseSettingsApi warehouseSettingsApi,
            IConfigurationData configurationData,
            IUserSession userSession,
            IMessageHelper messageHelper,
            HubMigrator hubMigrator)
        {
            this.userSession = userSession;
            this.messageHelper = messageHelper;
            this.warehouseSettingsApi = warehouseSettingsApi;
            this.warehouseSettingsView = warehouseSettingsView;
            this.warehouseList = warehouseList;
            this.configurationData = configurationData;
            this.hubMigrator = hubMigrator;

            SelectWarehouse = new RelayCommand(() => OnSelectWarehouse().Forget());
            UploadSKUs = new RelayCommand(() => OnUploadSKUs().Forget());

            warehouseSettingsView.ViewModel = this;

            var config = configurationData.FetchReadOnly();
            isAdmin = userSession.User.IsAdmin;

            if (string.IsNullOrEmpty(config.WarehouseID))
            {
                WarehouseName = "No warehouse selected";
                CanLinkWarehouse = isAdmin;
            }
            else
            {
                WarehouseName = config.WarehouseName;
                CanUploadSKUs = isAdmin;
            }

            UpdateCountOfProductsThatNeedUpload().Forget();
        }

        /// <summary>
        /// Select warehouse command
        /// </summary>
        [Obfuscation]
        public ICommand SelectWarehouse { get; }

        /// <summary>
        /// Upload SKUs from the products catalog
        /// </summary>
        [Obfuscation]
        public ICommand UploadSKUs { get; }

        /// <summary>
        /// Name of the warehouse associated with this instance of ShipWorks
        /// </summary>
        [Obfuscation]
        public string WarehouseName
        {
            get => warehouseName;
            set => Set(ref warehouseName, value);
        }

        /// <summary>
        /// Number of products that have been modified since last update.
        /// </summary>
        [Obfuscation]
        public int ModifiedProducts
        {
            get => modifiedProducts;
            set => Set(ref modifiedProducts, value);
        }

        /// <summary>
        /// Is this ShipWorks database already associated with a warehouse
        /// </summary>
        [Obfuscation]
        public bool CanLinkWarehouse
        {
            get => canLinkWarehouse;
            set => Set(ref canLinkWarehouse, value);
        }

        /// <summary>
        /// Can SKUs be uploaded
        /// </summary>
        [Obfuscation]
        public bool CanUploadSKUs
        {
            get => canUploadSKUs;
            set => Set(ref canUploadSKUs, value);
        }

        /// <summary>
        /// Get the control associated with the view model
        /// </summary>
        public Control Control => warehouseSettingsView.Control;

        /// <summary>
        /// Save configuration data
        /// </summary>
        public void Save() { }

        /// <summary>
        /// Handle the select warehouse command
        /// </summary>
        private async Task OnSelectWarehouse()
        {
            WarehouseViewModel warehouse = warehouseList.ChooseWarehouse();
            if (warehouse != null)
            {
                CanLinkWarehouse = false;

                Result associationResponse;
                using (messageHelper.ShowProgressDialog("Linking warehouse...", "Linking warehouse..."))
                {
                    associationResponse = await warehouseSettingsApi.Link(warehouse.Id).ConfigureAwait(true);
                }

                Cursor.Current = Cursors.WaitCursor;

                if (associationResponse.Success)
                {
                    configurationData.UpdateConfiguration(x =>
                    {
                        x.WarehouseID = warehouse.Id;
                        x.WarehouseName = warehouse.Name;
                    });

                    // If linking was successful, upload the sql config to the hub
                    await hubMigrator.MigrateSqlConfigToHub().ConfigureAwait(false);

                    WarehouseName = warehouse.Name;
                    CanUploadSKUs = isAdmin;
                }
                else
                {
                    messageHelper.ShowError("Could not link this ShipWorks database with the warehouse.");
                    CanLinkWarehouse = true;
                }
            }
        }

        /// <summary>
        /// Upload changed SKUs to the warehouse web application
        /// </summary>
        /// <returns></returns>
        private async Task OnUploadSKUs()
        {
            try
            {
                using (var progressItem = messageHelper.ShowProgressDialog("Uploading products", "Uploading products to warehouse"))
                {
                    await warehouseSettingsApi.UploadProducts(progressItem);
                    await progressItem.Provider.Terminated.ConfigureAwait(false);
                }

                UpdateCountOfProductsThatNeedUpload().Forget();
            }
            catch (Exception ex)
            {
                messageHelper.ShowError("Error while uploading products", ex);
            }
        }

        /// <summary>
        /// Update the count of products that need to be uploaded
        /// </summary>
        private async Task UpdateCountOfProductsThatNeedUpload()
        {
            ModifiedProducts = await warehouseSettingsApi.GetCountOfProductsThatNeedUpload().ConfigureAwait(true);
        }
    }
}
