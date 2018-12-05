using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.Products.Import
{
    /// <summary>
    /// Import has failed
    /// </summary>
    [Component(RegistrationType.Self)]
    public class ImportFailedState : ViewModelBase, IProductImportState
    {
        private readonly IProductImporterStateManager stateManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public ImportFailedState(Exception exception, IProductImporterStateManager stateManager)
        {
            this.stateManager = stateManager;
            FailureReason = exception.Message;
            ImportErrors = exception.Data
                .OfType<DictionaryEntry>()
                .ToDictionary(x => x.Key?.ToString(), x => x.Value?.ToString());

            CloseDialog = new RelayCommand(CloseDialogAction);
        }

        /// <summary>
        /// Close the dialog
        /// </summary>
        [Obfuscation]
        public ICommand CloseDialog { get; }

        /// <summary>
        /// Reason for the failure
        /// </summary>
        [Obfuscation]
        public string FailureReason { get; }

        /// <summary>
        /// Import specific errors
        /// </summary>
        [Obfuscation]
        public IDictionary<string, string> ImportErrors { get; }

        /// <summary>
        /// The dialog was requested to close
        /// </summary>
        public void CloseRequested(CancelEventArgs e)
        {

        }

        /// <summary>
        /// Action to close the dialog
        /// </summary>
        private void CloseDialogAction() => stateManager.Close();
    }
}
