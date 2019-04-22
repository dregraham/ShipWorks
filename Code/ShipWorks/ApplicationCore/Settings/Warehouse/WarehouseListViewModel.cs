using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
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
        private WarehouseViewModel selectedWarehouse;
        private IEnumerable<WarehouseViewModel> warehouses;
        private readonly IDatabaseIdentifier databaseIdentifier;

        /// <summary>
        /// Constructor
        /// </summary>
        public WarehouseListViewModel(Func<IWarehouseListDialog> createDialog, IMessageHelper messageHelper, IDatabaseIdentifier databaseIdentifier)
        {
            this.databaseIdentifier = databaseIdentifier;
            this.createDialog = createDialog;
            this.messageHelper = messageHelper;
            CancelAssociation = new RelayCommand(OnCancelAssociation);
            ConfirmAssociation = new RelayCommand(OnConfirmAssociation, () => SelectedWarehouse != null);
        }

        /// <summary>
        /// Cancel the association process
        /// </summary>
        [Obfuscation]
        public ICommand CancelAssociation { get; }

        /// <summary>
        /// Confirm the association process
        /// </summary>
        [Obfuscation]
        public ICommand ConfirmAssociation { get; }

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
        /// Choose a warehouse
        /// </summary>
        public WarehouseViewModel ChooseWarehouse(IEnumerable<WarehouseViewModel> warehouses)
        {
            Warehouses = warehouses.ToList();

            warehouseListDialog = createDialog();
            warehouseListDialog.DataContext = this;

            if (messageHelper.ShowDialog(warehouseListDialog) == true)
            {
                return SelectedWarehouse;
            }

            return null;
        }

        /// <summary>
        /// Handle cancel association
        /// </summary>
        private void OnCancelAssociation()
        {
            warehouseListDialog.Close();
        }

        /// <summary>
        /// Handle confirm association
        /// </summary>
        private void OnConfirmAssociation()
        {
            if (!SelectedWarehouse.CanBeAssociatedWith(databaseIdentifier.Get()))
            {
                messageHelper.ShowError("The selected warehouse cannot be associated with this ShipWorks instance");
                return;
            }

            if (messageHelper.ShowQuestion(MessageBoxIcon.Exclamation, MessageBoxButtons.YesNo, $"Are you sure you want to associate this ShipWorks database with the warehouse '{SelectedWarehouse.Name}'?\n\nThis cannot be undone.") == System.Windows.Forms.DialogResult.Yes)
            {
                warehouseListDialog.DialogResult = true;
                warehouseListDialog.Close();
            }
        }
    }
}
