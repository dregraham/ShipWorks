using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Enums;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.Api.Enums;
using ShipWorks.Shipping.Carriers.iParcel;
using ShipWorks.Stores.Content;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.UI.Controls;
using ShipWorks.Data;
using Interapptive.Shared.Utility;
using ShipWorks.Shipping.Carriers.Postal.Stamps;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Settings.Origin;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.Carriers.FedEx;
using Interapptive.Shared.Business;
using ShipWorks.Shipping.Carriers.Postal.Endicia;
using ShipWorks.Shipping.Carriers.UPS;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Stores.Platforms.Shopify.Enums;
using ShipWorks.Shipping.Carriers.iParcel.Enums;
using ShipWorks.Stores.Platforms.Ebay.Enums;

namespace ShipWorks.Users.Audit
{
    /// <summary>
    /// How to format the display of an audit column
    /// </summary>
    public static class AuditDisplayFormat
    {
        public const int Default  = 0;
        public const int Hidden   = 1;
        public const int Currency = 2;
        public const int Weight   = 3;
        public const int Entity   = 4;
        public const int State    = 5;
        public const int Country  = 6;
        public const int DateOnly = 7;

        // Maps code values for enums to their enum type that should be used to do the formatting
        static Dictionary<int, Type> enumMapping = new Dictionary<int, Type>();

        /// <summary>
        /// Static constructor
        /// </summary>
        static AuditDisplayFormat()
        {
            enumMapping[101] = typeof(NoteSource);
            enumMapping[102] = typeof(NoteVisibility);
            enumMapping[103] = typeof(ShipmentTypeCode);
            enumMapping[104] = typeof(PostalServiceType);
            enumMapping[105] = typeof(PostalConfirmationType);
            enumMapping[106] = typeof(PostalPackagingType);
            enumMapping[107] = typeof(PostalCustomsContentType);
            enumMapping[108] = typeof(FedExServiceType);
            enumMapping[109] = typeof(FedExPackagingType);
            enumMapping[110] = typeof(FedExPayorType);
            enumMapping[111] = typeof(ResidentialDeterminationType);
            enumMapping[112] = typeof(InsuranceProvider);
            enumMapping[113] = typeof(FedExHomeDeliveryType);
            enumMapping[114] = typeof(FedExSignatureType);
            enumMapping[115] = typeof(UpsServiceType);
            enumMapping[116] = typeof(UpsDeliveryConfirmationType);
            enumMapping[117] = typeof(UpsPayorType);
            enumMapping[121] = typeof(ShopifyPaymentStatus);
            enumMapping[122] = typeof(ShopifyFulfillmentStatus);
            enumMapping[123] = typeof(FedExDropoffType);
            enumMapping[124] = typeof(FedExReturnType);
            enumMapping[125] = typeof(WeightUnitOfMeasure);
            enumMapping[126] = typeof(FedExLinearUnitOfMeasure);
            enumMapping[127] = typeof(iParcelServiceType);
            enumMapping[128] = typeof (EbayShippingMethod);
        }

        /// <summary>
        /// Format the given audit value based on the format specifier
        /// </summary>
        public static string FormatAuditValue(object data, int format)
        {
            if (data == null)
            {
                return string.Empty;
            }

            switch (format)
            {
                case AuditDisplayFormat.Currency:
                    return string.Format("{0:c}", data);

                case AuditDisplayFormat.Weight:
                    return WeightControl.FormatWeight(Convert.ToDouble(data), (WeightDisplayFormat) UserSession.User.Settings.ShippingWeightFormat);

                case AuditDisplayFormat.Entity:
                    return GetEntityLabel(Convert.ToInt64(data));

                case AuditDisplayFormat.State:
                    return Geography.GetStateProvName(data.ToString());

                case AuditDisplayFormat.Country:
                    return Geography.GetCountryName(data.ToString());

                case AuditDisplayFormat.DateOnly:
                    return ((DateTime) data).ToLocalTime().ToShortDateString();
            }

            if (data is DateTime)
            {
                return ((DateTime) data).ToLocalTime().ToString();
            }

            Type enumType;
            if (enumMapping.TryGetValue(format, out enumType))
            {
                return EnumHelper.GetDescription((Enum) Enum.ToObject(enumType, Convert.ToInt32(data)));
            }

            // Default
            return data.ToString();
        }

        /// <summary>
        /// Get the label for the given entityID
        /// </summary>
        private static string GetEntityLabel(long entityID)
        {
            // Entity values of less than 1000 (our minimum PK value) represent a "null" entity
            if (entityID < 1000)
            {
                return string.Empty;
            }

            ObjectLabel label = ObjectLabelManager.GetLabel(entityID, true);
            if (label != null)
            {
                return label.GetCustomText(true, true, false);
            }

            EntityType entityType = EntityUtility.GetEntityType(entityID);
            switch (entityType)
            {
                case EntityType.StampsAccountEntity:
                    {
                        StampsAccountEntity account = StampsAccountManager.GetAccount(entityID);
                        return account != null ? account.Username : "(Deleted)";
                    }

                case EntityType.ShippingOriginEntity:
                    {
                        ShippingOriginEntity shipper = ShippingOriginManager.GetOrigin(entityID);
                        return shipper != null ? shipper.Description : "(Deleted)";
                    }

                case EntityType.FedExAccountEntity:
                    {
                        FedExAccountEntity account = FedExAccountManager.GetAccount(entityID);
                        return account != null ? account.Description : "(Deleted)";
                    }

                case EntityType.UpsAccountEntity:
                    {
                        UpsAccountEntity account = UpsAccountManager.GetAccount(entityID);
                        return account != null ? account.Description : "(Deleted)";
                    }

                case EntityType.EndiciaAccountEntity:
                    {
                        EndiciaAccountEntity account = EndiciaAccountManager.GetAccount(entityID);
                        return account != null ? account.Description : "(Deleted)";
                    }

                case EntityType.IParcelAccountEntity:
                    {
                        IParcelAccountEntity account = iParcelAccountManager.GetAccount(entityID);
                        return account != null ? account.Description : "(Deleted)";
                    }
            }

            throw new InvalidOperationException("No formatting specified for entity " + entityType);
        }
    }
}
