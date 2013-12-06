using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Editing;

namespace ShipWorks.Templates.Processing.TemplateXml.ElementOutlines
{
    /// <summary>
    /// Outline for 'Dimensions' node of a package
    /// </summary>
    public class DimensionsOutline : ElementOutline
    {
         DimensionsAdapter dimensions;

        /// <summary>
        /// Constructor
        /// </summary>
         public DimensionsOutline(TemplateTranslationContext context)
            : base(context)
        {
            AddElement("Length", () => dimensions.Length);
            AddElement("Width", () => dimensions.Width);
            AddElement("Height", () => dimensions.Height);
        }

        /// <summary>
        /// Bind a new cloned instance to the specified data
        /// </summary>
        public override ElementOutline CreateDataBoundClone(object data)
        {
            return new DimensionsOutline(Context) { dimensions = (DimensionsAdapter) data };
        }
   }
}
