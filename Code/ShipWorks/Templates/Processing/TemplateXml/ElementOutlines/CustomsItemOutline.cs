using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;

namespace ShipWorks.Templates.Processing.TemplateXml.ElementOutlines
{
    /// <summary>
    /// Outline for the 'CustomsItem' entry
    /// </summary>
    public class CustomsItemOutline : ElementOutline
    {
        ShipmentCustomsItemEntity customsItem;

        /// <summary>
        /// Constructor
        /// </summary>
        public CustomsItemOutline(TemplateTranslationContext context)
            : base(context)
        {
            AddElement("Description", () => customsItem.Description);
            AddElement("Quantity", () => customsItem.Quantity);
            AddElement("UnitValue", () => customsItem.UnitValue);
            AddElement("TotalValue", () => customsItem.UnitValue * (decimal) customsItem.Quantity);
            AddElement("Weight", () => customsItem.Weight);
            AddElement("TotalWeight", () => customsItem.Weight * customsItem.Quantity);
            AddElement("OriginCountryCode", () => customsItem.CountryOfOrigin);
            AddElement("OriginCountryName", () => Geography.GetCountryName(customsItem.CountryOfOrigin));
            AddElement("HarmonizedCode", () => customsItem.HarmonizedCode);
        }

        /// <summary>
        /// Bind a new cloned instance to the specified data
        /// </summary>
        public override ElementOutline CreateDataBoundClone(object data)
        {
            return new CustomsItemOutline(Context) { customsItem = (ShipmentCustomsItemEntity) data };
        }
    }
}
