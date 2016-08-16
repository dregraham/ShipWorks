using System.Reflection;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using Interapptive.Shared.UI;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Rating
{
    /// <summary>
    /// Exception footnote view model
    /// </summary>
    public class ExceptionsRateFootnoteViewModel : IExceptionsRateFootnoteViewModel
    {
        private readonly IMessageHelper messageHelper;

        /// <summary>
        /// Constructor
        /// </summary>
        public ExceptionsRateFootnoteViewModel(IMessageHelper messageHelper)
        {
            this.messageHelper = messageHelper;
            ShowMoreInformation = new RelayCommand(ShowMoreInformationAction);
        }

        /// <summary>
        /// Text to display in the 'More info' link
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string DetailedMessage { get; set; }

        /// <summary>
        /// Text to display in the footnote
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string ErrorText { get; set; }

        /// <summary>
        /// Command to show more information
        /// </summary>
        [Obfuscation(Exclude = true)]
        public ICommand ShowMoreInformation { get; private set; }

        /// <summary>
        /// Show a dialog with more information
        /// </summary>
        private void ShowMoreInformationAction()
        {
            messageHelper.ShowInformation(DetailedMessage);
        }
    }
}
