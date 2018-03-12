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
        private readonly IWin32Window owner;
        private const string htmlContent = "<html><head><title></title></head><body>{{BARCODEDATA}}</body></html>";

        /// <summary>
        /// Constructor
        /// </summary>
        public BarcodePrintJob(IPrintJobFactory printJobFactory, IEnumerable<IShippingProfile> shippingProfiles, IWin32Window owner)
        {
            this.printJobFactory = printJobFactory;
            this.shippingProfiles = shippingProfiles;
            this.owner = owner;
        }
        
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

            return new List<TemplateResult>() { new TemplateResult(null, htmlContent.Replace("{{BARCODEDATA}}", builder.ToString())) };
        }

        /// <summary>
        /// Preview the barcode print job
        /// </summary>
        public void PreviewAsync(Form parent)
        {
            IPrintJob job = printJobFactory.CreatePrintJob(CreateTemplateResults());

            job.PreviewAsync((Form)owner);
        }
    }
}
