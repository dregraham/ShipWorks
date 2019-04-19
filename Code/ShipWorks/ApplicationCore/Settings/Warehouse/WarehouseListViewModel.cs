using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;

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
        private IWarehouseViewModel selectedWarehouse;
        private IEnumerable<IWarehouseViewModel> warehouses;

        /// <summary>
        /// Constructor
        /// </summary>
        public WarehouseListViewModel(Func<IWarehouseListDialog> createDialog, IMessageHelper messageHelper)
        {
            this.createDialog = createDialog;
            this.messageHelper = messageHelper;
            CancelAssociation = new RelayCommand(OnCancelAssociation);
            ConfirmAssociation = new RelayCommand(OnConfirmAssociation);
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
        public IWarehouseViewModel SelectedWarehouse
        {
            get => selectedWarehouse;
            set => Set(ref selectedWarehouse, value);
        }

        /// <summary>
        /// List of warehouses from which to choose
        /// </summary>
        [Obfuscation]
        public IEnumerable<IWarehouseViewModel> Warehouses
        {
            get => warehouses;
            set => Set(ref warehouses, value);
        }

        /// <summary>
        /// Choose a warehouse
        /// </summary>
        public IWarehouseViewModel ChooseWarehouse(IEnumerable<WarehouseViewModel> warehouses)
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
            warehouseListDialog.DialogResult = true;
            warehouseListDialog.Close();
        }
    }
}
