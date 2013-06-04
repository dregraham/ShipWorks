using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Business;
using ShipWorks.Stores;
using System.Xml;

namespace ShipWorks.Templates.Processing.TemplateXml.ElementOutlines
{
    /// <summary>
    /// Outline for the 'Store' node
    /// </summary>
    public class StoreOutline : ElementOutline
    {
        StoreEntity store;

        /// <summary>
        /// Constructor
        /// </summary>
        public StoreOutline(TemplateTranslationContext context)
            : base(context)
        {
            AddAttribute("ID", () => Store.StoreID);

            AddElement("StoreName", () => Store.StoreName);

            Lazy<StoreType> storeType = new Lazy<StoreType>(() => StoreTypeManager.GetType(Store));

            var storeTypeElement = AddElement("StoreType");
            storeTypeElement.AddAttributeLegacy2x();
            storeTypeElement.AddTextContent(() => GetBackwardsCompatibleStoreTypeName(storeType.Value));

            var platformElement = AddElement("StoreType");
            platformElement.AddAttribute("ID", () => (int) storeType.Value.TypeCode);
            platformElement.AddElement("Code", () => storeType.Value.TangoCode);
            platformElement.AddElement("Name", () => storeType.Value.StoreTypeName);

            AddElement("LastDownload", () => GetLastDownloadDateTime());

            AddElement("Address", new AddressOutline(context, "company", false), () => new PersonAdapter(Store, ""));
        }

        /// <summary>
        /// Create a clone of the outline, but bound to the data of a specific store.
        /// </summary>
        public override ElementOutline CreateDataBoundClone(object data)
        {
            return new StoreOutline(Context) { Store = (StoreEntity) data };
        }

        /// <summary>
        /// Get the last date\time that a download was done for the bound store
        /// </summary>
        private string GetLastDownloadDateTime()
        {
            var lastDownloadInfo = StoreManager.GetLastDownloadTimes();

            DateTime? lastDownload;
            if (lastDownloadInfo.TryGetValue(Store.StoreID, out lastDownload) && lastDownload != null)
            {
                return XmlConvert.ToString(lastDownload.Value, XmlDateTimeSerializationMode.Utc);
            }

            return null;
        }

        /// <summary>
        /// The StoreEntity represented by the bound outline
        /// </summary>
        private StoreEntity Store
        {
            get { return store; }
            set { store = value; }
        }

        /// <summary>
        /// We used to output the user-visible name\description of a StoreType as our sole text content of the StoreType node.  This would
        /// break templates every time we'd update that text description. Now we have a new StoreType node that contains Code and Name.  This
        /// returns what the StoreTypeName _used_ to originally be for the OrderNumber snippet that historically still uses it.
        /// </summary>
        private string GetBackwardsCompatibleStoreTypeName(StoreType storeType)
        {
            switch (storeType.TypeCode)
            {
                case StoreTypeCode.Amazon:
                    return "Amazon Seller Central";
            }

            return storeType.StoreTypeName;
        }
   }
}
