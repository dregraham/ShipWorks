using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Carriers.Postal.WebTools
{
    /// <summary>
    /// Label Service for WebTools
    /// </summary>
    public class WebToolsLabelService : ILabelService
    {
        /// <summary>
        /// Creates a WebTools label for the given Shipment
        /// </summary>
        public void Create(ShipmentEntity shipment)
        {
            if (shipment.ShipPerson.IsDomesticCountry() && shipment.Postal.Confirmation == (int) PostalConfirmationType.None)
            {
                PostalPackagingType packaging = (PostalPackagingType) shipment.Postal.PackagingType;

                if ((shipment.Postal.Service != (int) PostalServiceType.ExpressMail) &&
                    !(shipment.Postal.Service == (int) PostalServiceType.FirstClass &&
                    (packaging == PostalPackagingType.Envelope || packaging == PostalPackagingType.LargeEnvelope)))
                {
                    throw new ShippingException(
                        $"A confirmation option must be selected when shipping {EnumHelper.GetDescription((PostalServiceType) shipment.Postal.Service)}.");
                }
            }

            if (shipment.Postal.Service == (int) PostalServiceType.ExpressMail &&
                CanUseExpressMail((PostalConfirmationType) shipment.Postal.Confirmation))
            {
                throw new ShippingException("A confirmation option cannot be used with Express mail.");
            }

            // Process the shipment
            PostalWebClientShipping.ProcessShipment(shipment.Postal);
        }

        /// <summary>
        /// Voids the given WebTools label
        /// </summary>
        public void Void(ShipmentEntity shipment)
        {
            // We can't void a web tools label
        }

        /// <summary>
        /// Check whether the confirmation type is valid for Express Mail
        /// </summary>
        private static bool CanUseExpressMail(PostalConfirmationType confirmation)
        {
            return confirmation != PostalConfirmationType.None &&
                confirmation != PostalConfirmationType.AdultSignatureRestricted &&
                confirmation != PostalConfirmationType.AdultSignatureRequired;
        }
    }
}