using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using ShipWorks.Shipping.Carriers.FedEx;
using ShipWorks.Shipping.Carriers.iParcel;
using ShipWorks.Shipping.Carriers.OnTrac.Enums;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.UPS.Enums;
using ShipWorks.Shipping.Editing.Rating;

namespace ShipWorks.Shipping.Rating
{
    /// <summary>
    /// Factory that creates an IRateSelection based on a RateResult.
    /// </summary>
    public class RateSelectionFactory : IRateSelectionFactory
    {
        /// <summary>
        /// Creates an IRateSelection based on given RateResult
        /// </summary>
        /// <returns></returns>
        [SuppressMessage("SonarLint", "S1541:Methods should not be too complex",
            Justification = "This is the est way to handle this without breaking into multiple classes")]
        public IRateSelection CreateRateSelection(RateResult rateResult)
        {
            RateSelection rateSelection = new RateSelection();

            switch (rateResult.ShipmentType)
            {
                case ShipmentTypeCode.UpsOnLineTools:
                case ShipmentTypeCode.UpsWorldShip:
                    rateSelection.ServiceType = (int)(UpsServiceType) rateResult.OriginalTag;
                    break;
                case ShipmentTypeCode.FedEx:
                    FedExRateSelection fedExRateSelection = rateResult.OriginalTag as FedExRateSelection;
                    rateSelection.ServiceType = (int)fedExRateSelection.ServiceType;
                    break;
                case ShipmentTypeCode.OnTrac:
                    OnTracServiceType onTracServiceType = (OnTracServiceType)rateResult.OriginalTag;
                    rateSelection.ServiceType = (int)onTracServiceType;
                    break;
                case ShipmentTypeCode.iParcel:
                    iParcelRateSelection ipRateSelection = (iParcelRateSelection)rateResult.OriginalTag;
                    rateSelection.ServiceType = (int)ipRateSelection.ServiceType;
                    break;
                case ShipmentTypeCode.Endicia:
                case ShipmentTypeCode.PostalWebTools:
                case ShipmentTypeCode.Express1Endicia:
                case ShipmentTypeCode.Express1Usps:
                case ShipmentTypeCode.Usps:
                    PostalRateSelection postalRateSelection = rateResult.OriginalTag as PostalRateSelection;
                    rateSelection.ServiceType = (int) postalRateSelection.ServiceType;
                    break;
                case ShipmentTypeCode.BestRate:
                case ShipmentTypeCode.Other:
                case ShipmentTypeCode.None:
                    // The RateResult should already be converted to the actual carrier rate result
                    // by the time it gets here, so just continue for BestRate
                    // None and Other don't have rates, so just continue.
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return rateSelection;
        }
    }
}
