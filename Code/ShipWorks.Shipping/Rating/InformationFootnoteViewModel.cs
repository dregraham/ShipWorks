using System.Reflection;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Rating
{
    /// <summary>
    /// Information footnote view model
    /// </summary>
    public class InformationFootnoteViewModel : IInformationFootnoteViewModel
    {
        /// <summary>
        /// Text that should be displayed
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string InformationText { get; set; }
    }
}
