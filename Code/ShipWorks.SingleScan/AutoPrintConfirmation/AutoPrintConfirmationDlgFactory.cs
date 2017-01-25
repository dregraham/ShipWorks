using Interapptive.Shared.UI;
using ShipWorks.Core.Messaging;
using ShipWorks.Shipping.Services;

namespace ShipWorks.SingleScan.AutoPrintConfirmation
{
    /// <summary>
    /// Creates a AutoPrintConfirmationDlg
    /// </summary>
    /// <seealso cref="ShipWorks.Shipping.Services.IAutoPrintConfirmationDlgFactory" />
    public class AutoPrintConfirmationDlgFactory : IAutoPrintConfirmationDlgFactory
    {
        private readonly IMessenger messenger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoPrintConfirmationDlgFactory"/> class.
        /// </summary>
        public AutoPrintConfirmationDlgFactory(IMessenger messenger)
        {
            this.messenger = messenger;
        }

        /// <summary>
        /// Create an AutoPrintConfirmationDlg with the given display text
        /// </summary>
        /// <param name="scanMessageText">The scan message that the dialog is listening for to confirm and close</param>
        /// <param name="title">The title of the dialog</param>
        /// <param name="displayText">The text that is displayed to the user</param>
        /// <returns></returns>
        public IDialog Create(string scanMessageText, string title, string displayText)
        {
            AutoPrintConfirmationDlgViewModel viewModel = new AutoPrintConfirmationDlgViewModel(messenger);
            AutoPrintConfirmationDlg view = new AutoPrintConfirmationDlg();

            viewModel.Close = result =>
            {
                viewModel.Dispose();
                view.DialogResult = result;
                view.Close();
            };

            viewModel.Load(scanMessageText, title, displayText);

            return view;
        }
    }
}