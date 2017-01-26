using System;
using System.Windows.Forms;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoPrintConfirmationDlgFactory"/> class.
        /// </summary>
        public AutoPrintConfirmationDlgFactory(IMessenger messenger, Func<IMessenger, IAutoPrintConfirmationDlgViewModel> autoPrintConfirmationDlgViewModel)
        {
            this.messenger = messenger;
            this.autoPrintConfirmationDlgViewModel = autoPrintConfirmationDlgViewModel;
        }

        /// <summary>
        /// Create an AutoPrintConfirmationDlg with the given display text
        /// </summary>
        /// <param name="scanMessageText">The scan message that the dialog is listening for to confirm and close</param>
        /// <param name="title">The title of the dialog</param>
        /// <param name="displayText">The text that is displayed to the user</param>
        /// <returns></returns>
        public IForm Create(string scanMessageText, string title, string displayText)
        {
            IAutoPrintConfirmationDlgViewModel viewModel = autoPrintConfirmationDlgViewModel(messenger);
            viewModel.Load(scanMessageText, displayText);

            return new AutoPrintConfirmationDialog(viewModel) { Text = title };
        }
    }
}