using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Xml.XPath;
using ActiproSoftware.SyntaxEditor.Addons.Xml.Ast;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.ShipSense;
using ShipWorks.Stores.Platforms.ChannelAdvisor.WebServices.Order;
using ShipWorks.Stores.Platforms.Ebay.WebServices;

namespace ShipWorks.Templates.Processing.TemplateXml.ElementOutlines
{
    /// <summary>
    /// Creates an ElementOutline for ShipSense info
    /// </summary>
    public class ShipSenseOutline : ElementOutline
    {
        private ShipmentEntity shipment;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipSenseOutline(TemplateTranslationContext context)
            : base(context)
        {
            AddElement("Status", () => ((ShipSenseStatus)shipment.ShipSenseStatus).ToString());

            AddElement("ChangeSets", new ShipSenseChangeSetOutline(context), () =>
            {
                // Parse the ShipSense change sets xml and return it as an XElement list
                XElement element = XElement.Parse(shipment.ShipSenseChangeSets);
                return new List<XElement> { element };
            });
        }

        /// <summary>
        /// Create a clone of the outline that is bound to specific data
        /// </summary>
        public override ElementOutline CreateDataBoundClone(object data)
        {
            return new ShipSenseOutline(Context) { shipment = (ShipmentEntity) data };
        }
    }

    /// <summary>
    /// Base class for common fields/methods used in ShipSense outlines
    /// </summary>
    public class ShipSenseOutlineBase : ElementOutline
    {
        protected XElement element;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipSenseOutlineBase(TemplateTranslationContext context)
            : base(context)
        {
        }

        /// <summary>
        /// Queries the XElement for given descendants
        /// </summary>
        /// <param name="name">Name of descendants to return</param>
        public IEnumerable<XElement> GetElements(string name)
        {
            IEnumerable<XElement> descendents = element.Descendants(name);
            return descendents;
        }

        /// <summary>
        /// Gets the Value of an XElement of Name name 
        /// </summary>
        /// <param name="name"></param>
        public string GetValue(string name)
        {
            return element.XPathSelectElement(name)?.Value ?? string.Empty;
        }
    }

    /// <summary>
    /// Adds the ChangeSet node
    /// </summary>
    public class ShipSenseChangeSetOutline : ShipSenseOutlineBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShipSenseChangeSetOutline(TemplateTranslationContext context)
            : base(context)
        {
            AddElement("ChangeSet", new ShipSensePackagesAndCustomsItemsOutline(context), () => GetElements("ChangeSet"));
        }

        /// <summary>
        /// Create a clone of the outline that is bound to specific data
        /// </summary>
        public override ElementOutline CreateDataBoundClone(object data)
        {
            return new ShipSenseChangeSetOutline(Context) { element = (XElement)data };
        }
    }

    /// <summary>
    /// Adds the Packages and CustomsItems nodes
    /// </summary>
    public class ShipSensePackagesAndCustomsItemsOutline : ShipSenseOutlineBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShipSensePackagesAndCustomsItemsOutline(TemplateTranslationContext context)
            : base(context)
        {
            AddAttribute("Timestamp", () => element.Attribute("Timestamp").Value);
            AddElement("Packages", new ShipSensePackagesBeforeAndAfterOutline(context), () => GetElements("Packages"));
            AddElement("CustomsItems", new ShipSenseCustomsItemsBeforeAndAfterOutline(context), () => GetElements("CustomsItems"));
        }

        /// <summary>
        /// Create a clone of the outline that is bound to specific data
        /// </summary>
        public override ElementOutline CreateDataBoundClone(object data)
        {
            return new ShipSensePackagesAndCustomsItemsOutline(Context) { element = (XElement)data };
        }
    }

    /// <summary>
    /// Adds the Before and After nodes
    /// </summary>
    public class ShipSensePackagesBeforeAndAfterOutline : ShipSenseOutlineBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShipSensePackagesBeforeAndAfterOutline(TemplateTranslationContext context)
            : base(context)
        {
            AddElement("Before", new ShipSensePackagesOutline(context), () => GetElements("Before"));
            AddElement("After", new ShipSensePackagesOutline(context), () => GetElements("After"));
        }

        /// <summary>
        /// Create a clone of the outline that is bound to specific data
        /// </summary>
        public override ElementOutline CreateDataBoundClone(object data)
        {
            return new ShipSensePackagesBeforeAndAfterOutline(Context) { element = (XElement)data };
        }
    }

    /// <summary>
    /// Adds the Package node
    /// </summary>
    public class ShipSensePackagesOutline : ShipSenseOutlineBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShipSensePackagesOutline(TemplateTranslationContext context)
            : base(context)
        {
            AddElement("Package", new ShipSensePackageDetailOutline(context), () => element.Descendants("Package"));
        }

        /// <summary>
        /// Create a clone of the outline that is bound to specific data
        /// </summary>
        public override ElementOutline CreateDataBoundClone(object data)
        {
            return new ShipSensePackagesOutline(Context) { element = (XElement)data };
        }
    }

    /// <summary>
    /// Adds the details to the package node
    /// </summary>
    public class ShipSensePackageDetailOutline : ShipSenseOutlineBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShipSensePackageDetailOutline(TemplateTranslationContext context)
            : base(context)
        {
            AddElement("Weight", () => GetValue("Weight"));
            AddElement("Height", () => GetValue("Height"));
            AddElement("Width", () => GetValue("Width"));
            AddElement("Length", () => GetValue("Length"));
            AddElement("AdditionalWeight", () => GetValue("AdditionalWeight"));
            AddElement("ApplyAdditionalWeight", () => GetValue("ApplyAdditionalWeight"));
        }

        /// <summary>
        /// Create a clone of the outline that is bound to specific data
        /// </summary>
        public override ElementOutline CreateDataBoundClone(object data)
        {
            return new ShipSensePackageDetailOutline(Context) { element = (XElement)data };
        }
    }

    /// <summary>
    /// Adds the CustomsInfo Before and After nodes
    /// </summary>
    public class ShipSenseCustomsItemsBeforeAndAfterOutline : ShipSenseOutlineBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShipSenseCustomsItemsBeforeAndAfterOutline(TemplateTranslationContext context)
            : base(context)
        {
            AddElement("Before", new ShipSenseCustomsItemsOutline(context), () => element.Descendants("Before").ToList());
            AddElement("After", new ShipSenseCustomsItemsOutline(context), () => element.Descendants("After").ToList());
        }

        /// <summary>
        /// Create a clone of the outline that is bound to specific data
        /// </summary>
        public override ElementOutline CreateDataBoundClone(object data)
        {
            return new ShipSenseCustomsItemsBeforeAndAfterOutline(Context) { element = (XElement)data };
        }
    }

    /// <summary>
    /// Adds the CustomsItem node
    /// </summary>
    public class ShipSenseCustomsItemsOutline : ShipSenseOutlineBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShipSenseCustomsItemsOutline(TemplateTranslationContext context)
            : base(context)
        {
            AddElement("CustomsItem", new ShipSenseCustomsItemDetailOutline(context), () => element.Descendants("CustomsItem"));
        }

        /// <summary>
        /// Create a clone of the outline that is bound to specific data
        /// </summary>
        public override ElementOutline CreateDataBoundClone(object data)
        {
            return new ShipSenseCustomsItemsOutline(Context) { element = (XElement)data };
        }
    }

    /// <summary>
    /// Adds the CustomItem details
    /// </summary>
    public class ShipSenseCustomsItemDetailOutline : ShipSenseOutlineBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public ShipSenseCustomsItemDetailOutline(TemplateTranslationContext context)
            : base(context)
        {
            AddElement("Description", () => GetValue("Description"));
            AddElement("CountryOfOrigin", () => GetValue("CountryOfOrigin"));
            AddElement("HarmonizedCode", () => GetValue("HarmonizedCode"));
            AddElement("NumberOfPieces", () => GetValue("NumberOfPieces"));
            AddElement("Quantity", () => GetValue("Quantity"));
            AddElement("UnitPriceAmount", () => GetValue("UnitPriceAmount"));
            AddElement("UnitValue", () => GetValue("UnitValue"));
            AddElement("Weight", () => GetValue("Weight"));
            AddElement("SKU", () => GetValue("SKU"));
        }

        /// <summary>
        /// Create a clone of the outline that is bound to specific data
        /// </summary>
        public override ElementOutline CreateDataBoundClone(object data)
        {
            return new ShipSenseCustomsItemDetailOutline(Context) { element = (XElement) data};
        }
    }
}
