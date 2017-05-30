using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating.Validation
{
    /// <summary>
    /// ViewModel for the UpsLocalRateDiscrepancyDialog
    /// </summary>
    interface IUpsLocalRateDiscrepancyViewModel
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
        Action<bool> Close { get; set; }
        
        /// <summary>
        /// Snoozes validation so the user is not warned at every validation error
        /// </summary>
        ICommand SnoozeClickCommand();

        /// <summary>
        /// Closes the dialog
        /// </summary>
        ICommand CloseClickCommand();
    }
}
