using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Licensing.Warehouse.DTO;
using ShipWorks.Data;

namespace ShipWorks.ApplicationCore.Settings.Warehouse
{
    /// <summary>
    /// View model for the warehouse list dialog
    /// </summary>
    [Component]
    public class WarehouseListViewModel : ViewModelBase, IWarehouseListViewModel
    {
        private readonly Func<IWarehouseListDialog> createDialog;
        private readonly IMessageHelper messageHelper;
        private IWarehouseListDialog warehouseListDialog;
        private readonly IDatabaseIdentifier databaseIdentifier;
        private readonly IWarehouseSettingsApi warehouseSettingsApi;

        private WarehouseViewModel selectedWarehouse;
        private IEnumerable<WarehouseViewModel> warehouses;
        private string message;
        private bool loadingFinished;
        private bool showMessage;

        /// <summary>
        /// Constructor
        /// </summary>
        public WarehouseListViewModel(
            Func<IWarehouseListDialog> createDialog,
            IMessageHelper messageHelper,
            IDatabaseIdentifier databaseIdentifier,
            IWarehouseSettingsApi warehouseSettingsApi)
        {
            this.warehouseSettingsApi = warehouseSettingsApi;
            this.databaseIdentifier = databaseIdentifier;
            this.createDialog = createDialog;
            this.messageHelper = messageHelper;
            CancelLink = new RelayCommand(OnCancelLink);
            ConfirmLink = new RelayCommand(OnConfirmLink, () => SelectedWarehouse != null);
        }

        /// <summary>
        /// Cancel the link process
        /// </summary>
        [Obfuscation]
        public ICommand CancelLink { get; }

        /// <summary>
        /// Confirm the link process
        /// </summary>
        [Obfuscation]
        public ICommand ConfirmLink { get; }

        /// <summary>
        /// Warehouse that's been selected
        /// </summary>
        [Obfuscation]
        public WarehouseViewModel SelectedWarehouse
        {
            get => selectedWarehouse;
            set => Set(ref selectedWarehouse, value);
        }

        /// <summary>
        /// List of warehouses from which to choose
        /// </summary>
        [Obfuscation]
        public IEnumerable<WarehouseViewModel> Warehouses
        {
            get => warehouses;
            set => Set(ref warehouses, value);
        }

        /// <summary>
        /// Result message
        /// </summary>
        [Obfuscation]
        public string Message
        {
            get => message;
            set => Set(ref message, value);
        }

        /// <summary>
        /// Are the warehouses being loaded
        /// </summary>
        [Obfuscation]
        public bool LoadingFinished
        {
            get => loadingFinished;
            set => Set(ref loadingFinished, value);
        }

        /// <summary>
        /// Are the warehouses being loaded
        /// </summary>
        [Obfuscation]
        public bool ShowMessage
        {
            get => showMessage;
            set => Set(ref showMessage, value);
        }

        /// <summary>
        /// Choose a warehouse
        /// </summary>
        public WarehouseViewModel ChooseWarehouse()
        {
            Message = string.Empty;
            LoadingFinished = false;
            ShowMessage = false;

            warehouseSettingsApi.GetAllWarehouses()
                .ContinueWith(LoadWarehouses);

            warehouseListDialog = createDialog();
            warehouseListDialog.DataContext = this;

            if (messageHelper.ShowDialog(warehouseListDialog) == true)
            {
                return SelectedWarehouse;
            }

            return null;
        }

        private void LoadWarehouses(Task<GenericResult<WarehouseListDto>> obj)
        {
            if (obj.IsFaulted || obj.Result.Failure)
            {
                Message = "Error loading warehouses";
            }
            else
            {
                Warehouses = obj.Result.Value.warehouses
                    .Select(warehouse => new WarehouseViewModel(warehouse))
                    .OrderBy(w => w.Name)
                    .ToList();
                Message = Warehouses.None() ? "No warehouses" : string.Empty;
            }

            LoadingFinished = true;
            ShowMessage = !string.IsNullOrEmpty(Message);
        }

        /// <summary>
        /// Handle cancel link
        /// </summary>
        private void OnCancelLink()
        {
            warehouseListDialog.Close();
        }

        /// <summary>
        /// Handle confirm link
        /// </summary>
        private void OnConfirmLink()
        {
            if (!SelectedWarehouse.CanBeLinkedWith(databaseIdentifier.Get()))
            {
                messageHelper.ShowError("The selected warehouse cannot be linked with this ShipWorks instance");
                return;
            }

            if (messageHelper.ShowQuestion(MessageBoxIcon.Exclamation, MessageBoxButtons.YesNo, $"Are you sure you want to link this ShipWorks database with the warehouse '{SelectedWarehouse.Name}'?\n\nThis cannot be undone.") == System.Windows.Forms.DialogResult.Yes)
            {
                warehouseListDialog.DialogResult = true;
                warehouseListDialog.Close();
            }
        }
    }
}
