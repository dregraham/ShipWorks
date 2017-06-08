using Interapptive.Shared.UI;
using Interapptive.Shared.ComponentRegistration;

namespace ShipWorks.SingleScan.AutoPrintConfirmation
{
    /// <summary>
    /// Interface that represents the AutoPrintConfirmationDialog
    /// </summary>
    [Service]
    public interface IAutoPrintConfirmationDialog : IForm
    {
        /// <summary>
        /// The title text of the dialog
        /// </summary>
        string Text { get; set; }
    }
}