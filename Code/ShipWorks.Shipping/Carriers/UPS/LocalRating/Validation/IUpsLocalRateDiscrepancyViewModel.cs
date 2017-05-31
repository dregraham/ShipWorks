using System;
using System.Windows.Input;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating.Validation
{
    /// <summary>
    /// ViewModel for the UpsLocalRateDiscrepancyDialog
    /// </summary>
    public interface IUpsLocalRateDiscrepancyViewModel
    {
        /// <summary>
        /// The validation message
        /// </summary>
        string Message { get; }

        /// <summary>
        /// Url to a help article
        /// </summary>
        Uri HelpArticleUrl { get; }

        /// <summary>
        /// Gets or sets the method to close the window.
        /// If user cancels, we will pass in false, else pass in true
        /// </summary>
        Action Close { get; set; }
        
        /// <summary>
        /// Snoozes validation so the user is not warned at every validation error
        /// </summary>
        ICommand SnoozeClickCommand { get; }

        /// <summary>
        /// Closes the dialog
        /// </summary>
        ICommand CloseClickCommand { get; }

        /// <summary>
        /// Loads the specified message.
        /// </summary>
        void Load(string message, Uri helpArticleUrl);
    }
}
