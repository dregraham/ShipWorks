using System;
using System.Linq;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.WebServices.Ship;

namespace ShipWorks.Shipping.Carriers.FedEx.Api.Shipping.Response.Manipulators
{
    /// <summary>
    /// Manipulator to add cost to shipment
    /// </summary>
    public class FedExShipmentCostManipulator : IFedExShipResponseManipulator
    {
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExShipmentCostManipulator(Func<Type, ILog> createLog)
        {
            this.log = createLog(GetType());
        }

        /// <summary>
        /// Add Cost to shipment
        /// </summary>
        public GenericResult<ShipmentEntity> Manipulate(ProcessShipmentReply response, ProcessShipmentRequest request, ShipmentEntity shipment)
        {
            ShipmentRating ratingInfo = response.CompletedShipmentDetail.ShipmentRating;

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
                SetBilledTypeAndWeight(rateDetail, shipment);
            }
            else
            {
                shipment.ShipmentCost = 0;

                log.WarnFormat("FedEx did not return rating details for shipment {0}", shipment.ShipmentID);
            }

            return shipment;
        }

        /// <summary>
        /// Sets the shipment's billed type and billed weight.
        /// </summary>
        private void SetBilledTypeAndWeight(ShipmentRateDetail rateDetail, ShipmentEntity shipment)
        {
            if (rateDetail.RatedWeightMethodSpecified)
            {
                switch (rateDetail.RatedWeightMethod)
                {
                    case RatedWeightMethod.ACTUAL:
                        shipment.BilledType = (int) BilledType.ActualWeight;
                        if (rateDetail.TotalBillingWeight != null)
                        {
                            shipment.BilledWeight = (double) rateDetail.TotalBillingWeight.Value;
                        }
                        break;
                    case RatedWeightMethod.DIM:
                        shipment.BilledType = (int) BilledType.DimensionalWeight;
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
                        shipment.BilledType = (int) BilledType.Unknown;
                        break;
                }
            }
            else
            {
                shipment.BilledType = (int) BilledType.Unknown;
            }
        }
    }
}
