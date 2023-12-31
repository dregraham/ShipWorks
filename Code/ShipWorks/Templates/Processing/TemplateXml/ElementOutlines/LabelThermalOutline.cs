﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using ShipWorks.ApplicationCore;
using System.Drawing.Imaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Shipping;

namespace ShipWorks.Templates.Processing.TemplateXml.ElementOutlines
{
    /// <summary>
    /// Outline for an individual Label element
    /// </summary>
    public class LabelThermalOutline : ElementOutline
    {
        ThermalLanguage thermalType;
        TemplateLabelData labelData;

        /// <summary>
        /// Constructor
        /// </summary>
        public LabelThermalOutline(TemplateTranslationContext context)
            : base(context)
        {
            AddAttribute("name", () => labelData.Name);
            AddAttribute("thermal", () => (thermalType == ThermalLanguage.EPL) ? "EPL" : "ZPL");

            AddTextContent(() => Convert.ToBase64String(File.ReadAllBytes(labelData.Resource.GetCachedFilename())));
        }

        /// <summary>
        /// Clone a new instance of the outline bound to the given data
        /// </summary>
        public override ElementOutline CreateDataBoundClone(object data)
        {
            var tuple = (Tuple<TemplateLabelData, ThermalLanguage>) data;

            return new LabelThermalOutline(Context) { labelData = tuple.Item1, thermalType = tuple.Item2 };
        }
    }
}
