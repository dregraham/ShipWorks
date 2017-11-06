using System;
using System.Linq;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Api;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;
using log4net;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Response.Manipulators
{
    /// <summary>
    /// Manipulator to add cost to shipment
    /// </summary>
    public class FedExShipmentCostManipulator : ICarrierResponseManipulator
    {
        private readonly ILog log = LogManager.GetLogger(typeof(FedExShipmentCostManipulator));

        private IFedExNativeShipmentReply reply;

        private ShipmentEntity shipment;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExShipmentCostManipulator()
        {
        }

        /// <summary>
        /// Constructor - Anticipating this will only used for unit tests.
        /// </summary>
        public FedExShipmentCostManipulator(ILog log)
        {
            this.log = log;
        }

        /// <summary>
        /// Add Cost to shipment
        /// </summary>
        /// <param name="carrierResponse"></param>
        public void Manipulate(ICarrierResponse carrierResponse)
        {
            FedExShipResponse fedExShipResponse = (FedExShipResponse) carrierResponse;

            shipment = fedExShipResponse.Shipment;
            reply = fedExShipResponse.NativeResponse as IFedExNativeShipmentReply;

            ShipmentRating ratingInfo = reply.CompletedShipmentDetail.ShipmentRating;

            if (ratingInfo != null)
            {
                // By default use the first one
                ShipmentRateDetail rateDetail = ratingInfo.ShipmentRateDetails[0];

                // If there is more than one, try to use the actual one
                if (ratingInfo.ShipmentRateDetails.Length > 1 && ratingInfo.ActualRateTypeSpecified)
                {
                    ShipmentRateDetail actualDetail = ratingInfo.ShipmentRateDetails.FirstOrDefault(d => d.RateType == ratingInfo.ActualRateType);
                    if (actualDetail != null)
                    {
                        rateDetail = actualDetail;
                    }
                }

                // Save the rate used
                shipment.ShipmentCost = rateDetail.TotalNetCharge.Amount;

                // per FedEx PIW
                if (shipment.AdjustedOriginCountryCode().ToUpper() == "CA")
                {
                    shipment.ShipmentCost = rateDetail.TotalNetFedExCharge.Amount;
                }

                // Set the shipment's billed type and billed weight.
                SetBilledTypeAndWeight(rateDetail);
            }
            else
            {
                shipment.ShipmentCost = 0;

                log.WarnFormat("FedEx did not return rating details for shipment {0}", shipment.ShipmentID);
            }
        }

        /// <summary>
        /// Sets the shipment's billed type and billed weight.
        /// </summary>
        private void SetBilledTypeAndWeight(ShipmentRateDetail rateDetail)
        {
            if (rateDetail.RatedWeightMethodSpecified)
            {
                switch (rateDetail.RatedWeightMethod)
                {
                    case RatedWeightMethod.ACTUAL:
                        shipment.BilledType = (int)BilledType.ActualWeight;
                        if (rateDetail.TotalBillingWeight != null)
                        {
                            shipment.BilledWeight = (double) rateDetail.TotalBillingWeight.Value;
                        }
                        break;
                    case RatedWeightMethod.DIM:
                        shipment.BilledType = (int)BilledType.DimensionalWeight;
                        if (rateDetail.TotalDimWeight != null)
                        {
                            shipment.BilledWeight = (double) rateDetail.TotalDimWeight.Value;
                        }
                        else if (rateDetail.TotalBillingWeight != null)
                        {
                            shipment.BilledWeight = (double) rateDetail.TotalBillingWeight.Value;
                        }
                        break;
                    default:
                        shipment.BilledType = (int)BilledType.Unknown;
                        break;
                }
            }
            else
            {
                shipment.BilledType = (int)BilledType.Unknown;
            }
        }
    }
}
