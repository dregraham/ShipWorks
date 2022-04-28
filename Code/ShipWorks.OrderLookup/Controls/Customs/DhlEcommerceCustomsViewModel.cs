using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using ShipWorks.OrderLookup.FieldManager;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.DhlEcommerce;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.UI;

namespace ShipWorks.OrderLookup.Controls.Customs
{
    /// <summary>
    /// Viewmodel for DhlEcommerce Customs
    /// </summary>
    [KeyedComponent(typeof(ICustomsViewModel), ShipmentTypeCode.DhlEcommerce)]
    [WpfView(typeof(DhlEcommerceCustomsControl))]
    public class DhlEcommerceCustomsViewModel : GenericCustomsViewModel
    {
        private Dictionary<int, string> customsContentTypes;
        private Dictionary<int, string> nonDeliveryTypes;
        private Dictionary<int, string> taxIdTypes;
        private Dictionary<string, string> issuingAuthorityCountries;

        /// <summary>
        /// Constructor
        /// </summary>
        public DhlEcommerceCustomsViewModel(IOrderLookupShipmentModel shipmentModel, IShipmentTypeManager shipmentTypeManager,
            OrderLookupFieldLayoutProvider fieldLayoutProvider) : base(shipmentModel, shipmentTypeManager, fieldLayoutProvider)
        {
            CustomsContentTypes = EnumHelper.GetEnumList<PostalCustomsContentType>().ToDictionary(x => (int) x.Value, x => x.Description);
            NonDeliveryTypes = EnumHelper.GetEnumList<ShipEngineNonDeliveryType>().ToDictionary(x => (int) x.Value, x => x.Description);
            TaxIdTypes = EnumHelper.GetEnumList<TaxIdType>().ToDictionary(x => (int) x.Value, x => x.Description);
            IssuingAuthorityCountries = Geography.Countries.ToDictionary(x => Geography.GetCountryCode(x), x => x);
        }

        /// <summary>
        /// List of available customs content types for the shipment
        /// </summary>
        [Obfuscation(Exclude = true)]
        public Dictionary<int, string> CustomsContentTypes
        {
            get => customsContentTypes;
            set => Handler.Set(nameof(CustomsContentTypes), ref customsContentTypes, value);
        }

        /// <summary>
        /// List of available non delivery types for the shipment
        /// </summary>
        [Obfuscation(Exclude = true)]
        public Dictionary<int, string> NonDeliveryTypes
        {
            get => nonDeliveryTypes;
            set => Handler.Set(nameof(NonDeliveryTypes), ref nonDeliveryTypes, value);
        }

        /// <summary>
        /// List of available tax id types for the shipment
        /// </summary>
        [Obfuscation(Exclude = true)]
        public Dictionary<int, string> TaxIdTypes
        {
            get => taxIdTypes;
            set => Handler.Set(nameof(TaxIdTypes), ref taxIdTypes, value);
        }

        /// <summary>
        /// List of available issuing authority countries for the shipment
        /// </summary>
        [Obfuscation(Exclude = true)]
        public Dictionary<string, string> IssuingAuthorityCountries
        {
            get => issuingAuthorityCountries;
            set => Handler.Set(nameof(IssuingAuthorityCountries), ref issuingAuthorityCountries, value);
        }
    }
}
