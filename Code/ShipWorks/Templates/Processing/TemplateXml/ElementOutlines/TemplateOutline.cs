using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Templates.Media;

namespace ShipWorks.Templates.Processing.TemplateXml.ElementOutlines
{
    /// <summary>
    /// The outline for the 'Template' element
    /// </summary>
    public class TemplateOutline : ElementOutline
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public TemplateOutline(TemplateTranslationContext context)
            : base(context)
        {
            TemplateEntity template = context.Template;

            AddElement("Name", template.Name);
            AddElement("Folder", template.FolderPath.Remove(template.FolderPath.Length - 1, 1));
            AddElement("Output", CreateOutputOutline(context));
        }

        /// <summary>
        /// Create the outline for the 'Output' child element
        /// </summary>
        private static ElementOutline CreateOutputOutline(TemplateTranslationContext context)
        {
            TemplateEntity template = context.Template;

            ElementOutline outputOutline = new ElementOutline(context);

            double contentWidth = 0;
            double contentHeight = 0;

            if ((TemplateType) template.Type == TemplateType.Label)
            {
                LabelSheetEntity sheet = LabelSheetManager.GetLabelSheet(template.LabelSheetID);
                if (sheet != null)
                {
                    contentWidth = sheet.LabelWidth;
                    contentHeight = sheet.LabelHeight;
                }
            }
            else if (template.Type == (int) TemplateType.Thermal)
            {
                contentWidth = 0;
                contentHeight = 0;
            }
            else
            {
                contentWidth = template.PageWidth - template.PageMarginLeft - template.PageMarginRight;
                contentHeight = template.PageHeight - template.PageMarginTop - template.PageMarginBottom;
            }

            outputOutline.AddElement("ContentWidth", contentWidth);
            outputOutline.AddElement("ContentHeight", contentHeight);

            return outputOutline;
        }
    }
}
