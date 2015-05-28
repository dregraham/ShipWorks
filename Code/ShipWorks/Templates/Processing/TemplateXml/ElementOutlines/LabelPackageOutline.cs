using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Filters.Content.Conditions.Shipments;
using ShipWorks.Shipping;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using System.Windows.Forms;
using System.Drawing.Imaging;
using ShipWorks.Common.IO.Hardware.Printers;

namespace ShipWorks.Templates.Processing.TemplateXml.ElementOutlines
{
    /// <summary>
    /// Outline for the 'Package' element of the labels element
    /// </summary>
    public class LabelPackageOutline : ElementOutline
    {
        ShipmentEntity shipment;

        long? packageID;
        List<TemplateLabelData> labelData;

        ImageFormat standardLabelFormat;

        /// <summary>
        /// Constructor
        /// </summary>
        public LabelPackageOutline(TemplateTranslationContext context, ImageFormat standardLabelFormat)
            : base(context)
        {
            this.standardLabelFormat = standardLabelFormat;

            AddAttribute("ID", () => packageID, If(() => packageID.HasValue));

            // Add all the category elements
            AddCategoryElement(TemplateLabelCategory.Primary);
            AddCategoryElement(TemplateLabelCategory.Supplemental);
        }

        /// <summary>
        /// Add the element for the given category
        /// </summary>
        private void AddCategoryElement(TemplateLabelCategory category)
        {
            // Add the element to represent this category
            ElementOutline outline = AddElement(EnumHelper.GetDescription(category), If(() => labelData.Any(l => l.Category == category)));
            
            // Need one Label element per label in the category
            outline.AddElement("Label", new LabelThermalOutline(Context), () => GetThermalLabels(labelData.Where(l => l.Category == category)));
            outline.AddElement("Label", new LabelStandardOutline(Context, standardLabelFormat), () => GetStandardLabels(labelData.Where(l => l.Category == category)));
        }

        /// <summary>
        /// Generate the list of labels that are thermal labels
        /// </summary>
        private IEnumerable<object> GetThermalLabels(IEnumerable<TemplateLabelData> source)
        {
            return shipment.ActualLabelFormat == null ? 
                null : 
                source.Where(l => l.CanPrintThermal)
                      .Select(l => Tuple.Create(l, (ThermalLanguage)shipment.ActualLabelFormat));
        }

        /// <summary>
        /// Get the list of labels that are standard labels
        /// </summary>
        private IEnumerable<object> GetStandardLabels(IEnumerable<TemplateLabelData> source)
        {
            if (shipment.ActualLabelFormat != null)
            {
                source = source.Where(x => !x.CanPrintThermal).ToList();
            }

            // We need two outputs for each - a 'tall' version and a 'wide' version
            return source.Select(l => Tuple.Create(l, Orientation.Vertical)).Concat(
                   source.Select(l => Tuple.Create(l, Orientation.Horizontal)));
        }

        /// <summary>
        /// Create a clone of this element, bound to the given specific data
        /// </summary>
        public override ElementOutline CreateDataBoundClone(object data)
        {
            var tuple = (Tuple<ShipmentEntity, IGrouping<long?, TemplateLabelData>>) data;

            return new LabelPackageOutline(Context, standardLabelFormat) { shipment = tuple.Item1, packageID = tuple.Item2.Key, labelData = tuple.Item2.ToList() };
        }
    }
}
