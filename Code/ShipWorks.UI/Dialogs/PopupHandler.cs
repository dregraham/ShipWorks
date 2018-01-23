using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.UI.Dialogs
{
    /// <summary>
    /// Handle showing the popup
    /// </summary>
    [Component(SingleInstance = true)]
    public class PopupHandler : IPopupHandler
    {
        private readonly PopupViewModel viewModel;
        private readonly Popup popup;

        /// <summary>
        /// Constructor
        /// </summary>
        public PopupHandler()
        {
            viewModel = new PopupViewModel();
            popup = new Popup() { DataContext = viewModel };
        }

        /// <summary>
        /// Show the popup window with the appropriate message
        /// </summary>
        public void ShowAction(string message, Form form)
        {
            // Sets the message
            viewModel.Message = message;

            // Positions the window to the center of the form
            popup.LoadOwner(form);

            // Trigger the show action to actually show the window
            popup.ShowAction();
        }
    }
}
