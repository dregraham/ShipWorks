using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;

namespace ShipWorks.Products.Import
{
    /// <summary>
    /// View model for the product importer
    /// </summary>
    [Component]
    public class ProductImporterViewModel : ViewModelBase, IProductImporterViewModel, IProductImporterStateManager
    {
        private readonly IMessageHelper messageHelper;
        private readonly Func<IProductImporterViewModel, IProductImporterDialog> createDialog;
        private IProductImporterDialog dialog;

        private IProductImportState currentState;
        private readonly Func<IProductImporterStateManager, SetupImportState> createInitialState;

        /// <summary>
        /// Constructor
        /// </summary>
        public ProductImporterViewModel(
            Func<IProductImporterViewModel, IProductImporterDialog> createDialog,
            Func<IProductImporterStateManager, SetupImportState> createInitialState,
            IMessageHelper messageHelper)
        {
            this.createInitialState = createInitialState;
            this.createDialog = createDialog;
            this.messageHelper = messageHelper;

            Closing = new RelayCommand<CancelEventArgs>(ClosingAction);
        }

        /// <summary>
        /// The dialog is closing
        /// </summary>
        private void ClosingAction(CancelEventArgs e) => CurrentState.CloseRequested(e);

        /// <summary>
        /// Command to close the dialog
        /// </summary>
        [Obfuscation]
        public ICommand Closing { get; }

        /// <summary>
        /// Current state of the importer
        /// </summary>
        [Obfuscation]
        public IProductImportState CurrentState
        {
            get => currentState;
            set => Set(ref currentState, value);
        }

        /// <summary>
        /// Import products
        /// </summary>
        public Result ImportProducts()
        {
            dialog = createDialog(this);

            CurrentState = createInitialState(this);

            messageHelper.ShowDialog(dialog);

            return CurrentState.ShouldReloadProducts ?
                Result.FromSuccess() :
                Result.FromError("Dialog canceled");
        }

        /// <summary>
        /// Change the state
        /// </summary>
        public void ChangeState(IProductImportState nextState) => CurrentState = nextState;

        /// <summary>
        /// Close the dialog
        /// </summary>
        public void Close() => dialog.Close();
    }
}
