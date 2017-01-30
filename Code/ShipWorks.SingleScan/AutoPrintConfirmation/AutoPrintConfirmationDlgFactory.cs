using System;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Core.Messaging;
using ShipWorks.Shipping.Services;

namespace ShipWorks.SingleScan.AutoPrintConfirmation
{
    /// <summary>
    /// Creates a AutoPrintConfirmationDlg
    /// </summary>
    /// <seealso cref="ShipWorks.Shipping.Services.IAutoPrintConfirmationDlgFactory" />
    [Component]
    public class AutoPrintConfirmationDlgFactory : IAutoPrintConfirmationDlgFactory
    {
        private readonly IMessenger messenger;
        private readonly Func<IMessenger, IAutoPrintConfirmationDlgViewModel> autoPrintConfirmationDlgViewModel;
        private readonly Func<IAutoPrintConfirmationDlgViewModel, IAutoPrintConfirmationDialog> autoPrintConfirmationDialog;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoPrintConfirmationDlgFactory"/> class.
        /// </summary>
        public AutoPrintConfirmationDlgFactory(IMessenger messenger, Func<IMessenger, IAutoPrintConfirmationDlgViewModel> autoPrintConfirmationDlgViewModel, Func<IAutoPrintConfirmationDlgViewModel, IAutoPrintConfirmationDialog> autoPrintConfirmationDialog)
        {
            this.messenger = messenger;
            this.autoPrintConfirmationDlgViewModel = autoPrintConfirmationDlgViewModel;
            this.autoPrintConfirmationDialog = autoPrintConfirmationDialog;
        }

        /// <summary>
        /// Create an AutoPrintConfirmationDlg with the given display text
        /// </summary>
        /// <param name="scanMessageText">The scan message text that the dialog is listening for to confirm and close</param>
        /// <param name="messaging">The messaging text to display on the IForm</param>
        /// <returns></returns>
        public IForm Create(string scanMessageText, MessagingText messaging)
        {
            IAutoPrintConfirmationDlgViewModel viewModel = autoPrintConfirmationDlgViewModel(messenger);
            viewModel.Load(scanMessageText, messaging.Body, messaging.Continue);

            IAutoPrintConfirmationDialog dialog = autoPrintConfirmationDialog(viewModel);
            dialog.Text = messaging.Title;
            return dialog;
        }
    }
}