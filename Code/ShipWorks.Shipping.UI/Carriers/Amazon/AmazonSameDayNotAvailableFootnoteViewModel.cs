using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore.ComponentRegistration;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.UI;

namespace ShipWorks.Shipping.UI.Carriers.Amazon
{
    /// <summary>
    /// View model for the Amazon same day not available footnote
    /// </summary>
    [Component]
    [WpfView(typeof(AmazonSameDayNotAvailableFootnote))]
    public class AmazonSameDayNotAvailableFootnoteViewModel : IAmazonSameDayNotAvailableFootnoteViewModel
    {
        private readonly IMessageHelper messageHelper;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonSameDayNotAvailableFootnoteViewModel(IMessageHelper messageHelper)
        {
            ShowDialog = new RelayCommand(ShowDialogAction);
            this.messageHelper = messageHelper;
        }

        /// <summary>
        /// Show the more information dialog
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand ShowDialog { get; }

        /// <summary>
        /// Show the information dialog
        /// </summary>
        private void ShowDialogAction()
        {
            messageHelper.ShowInformation(@"Same day rates could not be retrieved from Amazon.

This can happen if it is too late in the day to ship same day,
or if the 'from' address does not qualify for same day rates.

You may be able to process a same day shipment directly on your
Amazon Seller Central account page for the associated order.");
        }
    }
}
