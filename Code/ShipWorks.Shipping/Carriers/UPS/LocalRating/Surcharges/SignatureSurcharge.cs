using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Carriers.UPS.LocalRating;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating.Surcharges
{
    /// <summary>
    /// Applies a Signature Surcharge to the service rate based on the shipment
    /// </summary>
    /// <seealso cref="ShipWorks.Shipping.Carriers.Ups.LocalRating.Surcharges.IUpsSurcharge" />
    public class SignatureSurcharge : IUpsSurcharge
    {
        private readonly IDictionary<UpsSurchargeType, double> surcharges;

        /// <summary>
        /// Initializes a new instance of the <see cref="SignatureSurcharge"/> class.
        /// </summary>
        public SignatureSurcharge(IDictionary<UpsSurchargeType, double> surcharges)
        {
            this.surcharges = surcharges;
        }

        /// <summary>
        /// Apply the surcharge to the service rate based on the shipment
        /// </summary>
        public void Apply(UpsShipmentEntity shipment, IUpsLocalServiceRate serviceRate)
        {
            UpsSurchargeType? signatureSurcharge = GetSignatureSurchargeType(shipment);

            if (signatureSurcharge.HasValue)
            {
                decimal signatureRate = (decimal) surcharges[signatureSurcharge.Value];
                decimal surcharge = shipment.Packages.Count * signatureRate;

                serviceRate.AddAmount(surcharge, EnumHelper.GetDescription(signatureSurcharge));
            }
        }

        /// <summary>
        /// Gets the type of the signature surcharge.
        /// </summary>
        private static UpsSurchargeType? GetSignatureSurchargeType(UpsShipmentEntity shipment)
        {
            UpsSurchargeType? signatureSurcharge;

            switch ((UpsDeliveryConfirmationType) shipment.DeliveryConfirmation)
            {
                case UpsDeliveryConfirmationType.NoSignature:
                    signatureSurcharge = UpsSurchargeType.NoSignature;
                    break;

                case UpsDeliveryConfirmationType.Signature:
                    signatureSurcharge = UpsSurchargeType.SignatureRequired;
                    break;

                case UpsDeliveryConfirmationType.AdultSignature:
                    signatureSurcharge = UpsSurchargeType.AdultSignatureRequired;
                    break;

                default:
                    signatureSurcharge = null;
                    break;
            }

            return signatureSurcharge;
        }
    }
}
