using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using ShipWorks.Shipping.Profiles;
using ShipWorks.Templates.Processing;

namespace ShipWorks.Templates.Printing
{
    /// <summary>
    /// Barcode print job
    /// </summary>
    public class BarcodePrintJob : IPrintJob
    {
        private readonly IPrintJobFactory printJobFactory;
        private readonly IEnumerable<IShippingProfile> shippingProfiles;
        private const string HTMLContent = "<html><head><title></title><style>body {{font-family:Arial; text-align:center;}}table {{margin-bottom:50px;}} td {{text-align:center;}} .barcode {{font-family:Free 3 of 9 Extended;font-size:32pt;}}</style></head><body>{0}</body></html>";

        /// <summary>
        /// Constructor
        /// </summary>
        public BarcodePrintJob(IPrintJobFactory printJobFactory, IEnumerable<IShippingProfile> shippingProfiles)
        {
            this.printJobFactory = printJobFactory;
            this.shippingProfiles = shippingProfiles;
        }
        
        /// <summary>
        /// Preview the barcode print job
        /// </summary>
        public void PreviewAsync(Form parent) =>
            printJobFactory.CreatePrintJob(CreateTemplateResults()).PreviewAsync(parent);

        /// <summary>
        /// Create a list of template results to display 
        /// </summary>
        private IList<TemplateResult> CreateTemplateResults()
        {
            StringBuilder builder = new StringBuilder();
            foreach (IShippingProfile profile in shippingProfiles)
            {
                builder.AppendLine(CreateBarcodeElement(profile.ShippingProfileEntity.Name, profile.Shortcut.Barcode, profile.ShortcutKey));
            }

            return new List<TemplateResult>()
            {
                new TemplateResult(null,string.Format(HTMLContent, builder.ToString()))
            };
        }

        /// <summary>
        /// Create the barcode element
        /// </summary>
        private static string CreateBarcodeElement(string name, string barcode, string hotkey) =>
            $"<table><tr><td> {name} </td></tr><tr><td class='barcode'>{barcode}</td></tr><tr><td>{hotkey}</td></tr></table>";
    }
}
