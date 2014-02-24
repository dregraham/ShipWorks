using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.Net;
using Interapptive.Shared.Utility;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.OnTrac.Enums;
using ShipWorks.Shipping.Carriers.OnTrac.Schemas.Rate;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Enums;
using ShipWorks.Shipping.Editing.Rating;
using log4net;
using Interapptive.Shared.Business;

namespace ShipWorks.Shipping.Carriers.OnTrac.Net.Rates
{
    /// <summary>
    /// An implementation of IOnTracRateRequest that queries OnTrac
    /// </summary>
    public class OnTracRates : OnTracRequest
    {
        private readonly HttpVariableRequestSubmitter httpVariableRequestSubmitter;

        /// <summary>
        /// Constructor
        /// </summary>
        public OnTracRates(OnTracAccountEntity account)
            : base(
                account.AccountNumber,
                SecureText.Decrypt(account.Password, account.AccountNumber.ToString()),
                "OnTracRateRequest")
        {
            httpVariableRequestSubmitter = new HttpVariableRequestSubmitter();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public OnTracRates(
            long onTracAccountNumber,
            string onTracPassword,
            IApiLogEntry apiLogEntry,
            HttpVariableRequestSubmitter httpVariableRequestSubmitter)
            : base(onTracAccountNumber, onTracPassword, apiLogEntry)
        {
            this.httpVariableRequestSubmitter = httpVariableRequestSubmitter;
        }

        /// <summary>
        /// Get Rates from OnTrac
        /// </summary>
        /// <exception cref="OnTracException"></exception>
        public RateGroup GetRates(ShipmentEntity shipment)
        {
            RateShipment rateShipment = GetRatesFromOnTrac(shipment);

            List<RateResult> rates = new List<RateResult>();

            foreach (RateQuote rateQuote in rateShipment.Rates)
            {
                OnTracServiceType onTracServiceType =
                    EnumHelper.GetEnumByApiValue<OnTracServiceType>(rateQuote.Service.ToString());

                DateTime? expectedDeliveryDate = ShippingManager.CalculateExpectedDeliveryDate(rateQuote.TransitDays, DayOfWeek.Saturday, DayOfWeek.Sunday);

                rates.Add(
                    new RateResult(
                        EnumHelper.GetDescription(onTracServiceType),
                        rateQuote.TransitDays.ToString(),
                        (decimal)rateQuote.TotalCharge,
                        onTracServiceType)
                    {
                        ExpectedDeliveryDate = expectedDeliveryDate,
                        ServiceLevel = GetServiceLevel(onTracServiceType, rateQuote.TransitDays),
                        ShipmentType = ShipmentTypeCode.OnTrac
                    });
            }

            return new RateGroup(rates);
        }

        /// <summary>
        /// Gets the service level.
        /// </summary>
        private static ServiceLevelType GetServiceLevel(OnTracServiceType serviceType, int transitDays)
        {
            switch (serviceType)
            {
                case OnTracServiceType.Sunrise:
                case OnTracServiceType.SunriseGold:
                case OnTracServiceType.PalletizedFreight:
                    return ServiceLevelType.OneDay;
                default:
                case OnTracServiceType.None:
                case OnTracServiceType.Ground:
                    return ShippingManager.GetServiceLevel(transitDays);
            }
        }

        /// <summary>
        /// Gets rates from OnTrac
        /// </summary>
        /// <returns> RateShipmentList is a class defined by the OnTrac XSDs </returns>
        private RateShipment GetRatesFromOnTrac(ShipmentEntity shipment)
        {
            OnTracShipmentEntity onTracShipment = shipment.OnTrac;

            var sbRequestUrl = new StringBuilder();

            //base string
            sbRequestUrl.AppendFormat(
                "{0}{1}/rates?pw={2}&packages=",
                BaseUrlUsedToCallOnTrac,
                AccountNumber,
                OnTracPassword);

            //append package
            sbRequestUrl.AppendFormat("{0}", "uid");
            sbRequestUrl.AppendFormat(";{0}", PersonUtility.GetZip5(shipment.OriginPostalCode));
            sbRequestUrl.AppendFormat(";{0}", PersonUtility.GetZip5(shipment.ShipPostalCode));
            sbRequestUrl.AppendFormat(";{0}", shipment.ResidentialResult);
            sbRequestUrl.AppendFormat(";{0}", onTracShipment.CodAmount);
            sbRequestUrl.AppendFormat(";{0}", onTracShipment.SaturdayDelivery);
            sbRequestUrl.AppendFormat(";{0}", onTracShipment.DeclaredValue);


            //dimensions - if letter, no dimensions and 0 weight
            if (onTracShipment.PackagingType == (int) OnTracPackagingType.Letter)
            {
                sbRequestUrl.Append(";0;0X0X0");
            }
            else
            {
                sbRequestUrl.AppendFormat(";{0}", shipment.TotalWeight);
                sbRequestUrl.AppendFormat(
                    ";{0}X{1}X{2}",
                    onTracShipment.DimsLength,
                    onTracShipment.DimsWidth,
                    onTracShipment.DimsHeight);
            }

            //This is the field for service. blank is to return all
            sbRequestUrl.AppendFormat(";");

            httpVariableRequestSubmitter.Uri = new Uri(sbRequestUrl.ToString());
            httpVariableRequestSubmitter.Verb = HttpVerb.Get;

            RateShipmentList result = ExecuteLoggedRequest<RateShipmentList>(httpVariableRequestSubmitter);

            // OnTrac may not return any rating results
            if (result.Shipments == null || !result.Shipments.Any())
            {
                throw new OnTracException("OnTrac did not provide any rate results to ShipWorks.");
            }

            // We only ever request 1 shipment
            return result.Shipments[0];
        }
    }
}