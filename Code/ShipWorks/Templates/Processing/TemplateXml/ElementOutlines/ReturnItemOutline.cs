using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Templates.Processing.TemplateXml.ElementOutlines
{
    /// <summary>
    /// XML source outline for return items
    /// </summary>
    /// <seealso cref="ShipWorks.Templates.Processing.TemplateXml.ElementOutlines.ElementOutline" />
    public class ReturnItemOutline : ElementOutline
    {
        ShipmentReturnItemEntity returnItem;

        /// <summary>
        /// Constructor
        /// </summary>
        public ReturnItemOutline(TemplateTranslationContext context)
            : base(context)
        {
            AddElement("Name", () => returnItem.Name);
            AddElement("Quantity", () => returnItem.Quantity);
            AddElement("Weight", () => returnItem.Weight);
            AddElement("TotalWeight", () => returnItem.Weight * returnItem.Quantity);
            AddElement("SKU", () => returnItem.SKU);
            AddElement("Code", () => returnItem.Code);
            AddElement("Notes", () => returnItem.Notes);
        }

        /// <summary>
        /// Bind a new cloned instance to the specified data
        /// </summary>
        public override ElementOutline CreateDataBoundClone(object data)
        {
            return new ReturnItemOutline(Context) { returnItem = (ShipmentReturnItemEntity) data };
        }
    }
}