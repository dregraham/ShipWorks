using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping;
using ShipWorks.Data.Model.EntityClasses;
using System.Drawing.Imaging;

namespace ShipWorks.Templates.Processing.TemplateXml.ElementOutlines
{
    /// <summary>
    /// Outline for the 'Labels' element of a shipment
    /// </summary>
    public class LabelsOutline : ElementOutline
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public LabelsOutline(TemplateTranslationContext context, Func<ShipmentEntity> shipment, Lazy<List<TemplateLabelData>> labels, Func<ImageFormat> standardLabelFormat) :
            base(context)
        {
            AddAttribute("type", () => shipment().ActualLabelFormat == null ? "image" : "thermal");

            // Create a  package output entry per package.  We go ahead and evaluate the shipment as we pass it in - it will have already been evaluated anyway, since
            // to get to that point it would have had to have been checked for .Processed
            AddElement("Package", new LabelPackageOutline(context, standardLabelFormat), () => labels.Value.GroupBy(l => l.PackageID).Select(g => Tuple.Create(shipment(), g)));
        }
    }
}
