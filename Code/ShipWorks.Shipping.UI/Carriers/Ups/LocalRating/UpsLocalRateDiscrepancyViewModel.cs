using System;
using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Shipping.Carriers.Ups.LocalRating.Validation;

namespace ShipWorks.Shipping.UI.Carriers.Ups.LocalRating
{
    /// <summary>
    /// ViewModel for UpsLocalRateDiscrepancyDialog
    /// </summary>
    /// <seealso cref="ShipWorks.Shipping.Carriers.Ups.LocalRating.Validation.IUpsLocalRateDiscrepancyViewModel" />
    [Component]
    public class UpsLocalRateDiscrepancyViewModel : IUpsLocalRateDiscrepancyViewModel
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public UpsLocalRateDiscrepancyViewModel()
        {
            SnoozeClickCommand = new RelayCommand(ExecuteSnooze);
            CloseClickCommand = new RelayCommand(ExecuteClose);
        }

        /// <summary>
        /// Loads the specified message.
        /// </summary>
        public void Load(string message, Uri helpArticleUrl)
        {
            Message = message;
            HelpArticleUrl = helpArticleUrl;
        }

        /// <summary>
        /// The validation message
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string Message { get; private set; }

        /// <summary>
        /// Url to a help article
        /// </summary>
        [Obfuscation(Exclude = true)]
        public Uri HelpArticleUrl { get; private set; }

        /// <summary>
        /// Gets or sets the method to close the window.
        /// If user cancels, we will pass in false, else pass in true
        /// </summary>
        public Action Close { get; set; }

        /// <summary>
        /// Suppresses validation for a limited amount of time or until SW restarts
        /// </summary>
        public Action Snooze { get; set; }

        /// <summary>
        /// Snoozes validation so the user is not warned at every validation error
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand SnoozeClickCommand { get; }

        /// <summary>
        /// Closes the dialog
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand CloseClickCommand { get; }

        /// <summary>
        /// Snoozes validation so the user is not warned at every validation error
        /// </summary>
        private void ExecuteSnooze()
        {
            Snooze?.Invoke();
            ExecuteClose();
        }

        /// <summary>
        /// Executes the close action
        /// </summary>
        private void ExecuteClose()
        {
            Close?.Invoke();
        }
    }
}