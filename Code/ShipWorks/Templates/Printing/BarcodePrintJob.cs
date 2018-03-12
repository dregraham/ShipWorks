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
        private const string HTMLContent = "<html><head><title></title></head><body>{{BARCODEDATA}}</body></html>";

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
                builder.AppendLine($"Barcode:{profile.ShortcutKey}");
            }

            return new List<TemplateResult>()
            {
                new TemplateResult(null, HTMLContent.Replace("{{BARCODEDATA}}", builder.ToString()))
            };
        }
    }
}
