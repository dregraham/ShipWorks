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
using ShipWorks.Shipping.Editing.Enums;
using ShipWorks.Shipping.Editing.Rating;
using log4net;
using Interapptive.Shared.Business;
using Interapptive.Shared.Security;

namespace ShipWorks.Shipping.Carriers.OnTrac.Net.Rates
{
    /// <summary>
    /// An implementation of IOnTracRateRequest that queries OnTrac
    /// </summary>
    public class OnTracRates : OnTracRequest
    {
        private readonly HttpVariableRequestSubmitter httpVariableRequestSubmitter;
        readonly ILog log;


        /// <summary>
        /// Constructor
        /// </summary>
        public OnTracRates(OnTracAccountEntity account)
            : this(
                account.AccountNumber,
                SecureText.Decrypt(account.Password, account.AccountNumber.ToString()),
                new HttpVariableRequestSubmitter(), 
                new LogEntryFactory(),
                LogManager.GetLogger(typeof(OnTracRates)))
        {
            httpVariableRequestSubmitter = new HttpVariableRequestSubmitter();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public OnTracRates(long onTracAccountNumber, string onTracPassword, HttpVariableRequestSubmitter httpVariableRequestSubmitter, ILogEntryFactory logEntryFactory, ILog log)
            : base(onTracAccountNumber, onTracPassword, logEntryFactory, ApiLogSource.OnTrac, "OnTracRateRequest", LogActionType.GetRates)
        {
            this.httpVariableRequestSubmitter = httpVariableRequestSubmitter;
            this.log = log;
        }

        /// <summary>
        /// Get Rates from OnTrac
        /// </summary>
        /// <exception cref="OnTracException"></exception>
        public RateGroup GetRates(ShipmentEntity shipment, IEnumerable<OnTracServiceType> availableServiceTypes)
        {
            ILookup<OnTracServiceType, OnTracServiceType> availableServices = availableServiceTypes.ToLookup(x => x);

            RateShipment rateShipment = GetRatesFromOnTrac(shipment);

            List<RateResult> rates = new List<RateResult>();

            foreach (RateQuote rateQuote in rateShipment.Rates)
            {
                try
                {
                    OnTracServiceType onTracServiceType =
                        EnumHelper.GetEnumByApiValue<OnTracServiceType>(rateQuote.Service);

                    if (!availableServices.Contains(onTracServiceType))
                    {
                        continue;
                    }

                    DateTime? expectedDeliveryDate = GetExpectedDeliveryDate(rateQuote);
                    string deliveryDateDescription = rateQuote.TransitDays.ToString();

                    if (expectedDeliveryDate.HasValue)
                    {
                        deliveryDateDescription += " " + ShippingManager.GetArrivalDescription(expectedDeliveryDate.Value);
                    }
                    else
                    {
                        expectedDeliveryDate = ShippingManager.CalculateExpectedDeliveryDate(rateQuote.TransitDays, DayOfWeek.Saturday, DayOfWeek.Sunday);
                    }

                    rates.Add(
                        new RateResult(
                            EnumHelper.GetDescription(onTracServiceType),
                            deliveryDateDescription,
                            (decimal)rateQuote.TotalCharge,
                            onTracServiceType)
                        {
                            ExpectedDeliveryDate = expectedDeliveryDate,
                            ServiceLevel = GetServiceLevel(onTracServiceType, rateQuote.TransitDays),
                            ShipmentType = ShipmentTypeCode.OnTrac,
                            ProviderLogo = EnumHelper.GetImage(ShipmentTypeCode.OnTrac)
                        });
                }
                catch (InvalidOperationException ex)
                {
                    log.Info(string.Format("Unknown Service Type {0}", rateQuote.Service), ex);
                }
            }

            return new RateGroup(rates);
        }

        /// <summary>
        /// Try to get an expected delivery date from the rate quote
        /// </summary>
        private static DateTime? GetExpectedDeliveryDate(RateQuote rateQuote)
        {
            if (rateQuote.ExpectedDeliveryDate == null || rateQuote.ExpectedDeliveryDate.Length != 8 || rateQuote.CommitTime == null)
            {
                return null;
            }

            int year;
            int month;
            int day;
            DateTime commitTimeDate;

            if (!int.TryParse(rateQuote.ExpectedDeliveryDate.Substring(0, 4), out year) ||
                !int.TryParse(rateQuote.ExpectedDeliveryDate.Substring(4, 2), out month) ||
                !int.TryParse(rateQuote.ExpectedDeliveryDate.Substring(6, 2), out day) ||
                !DateTime.TryParse(rateQuote.CommitTime, out commitTimeDate))
            {
                return null;
            }

            DateTime createdDate = new DateTime(year, month, day, commitTimeDate.Hour, commitTimeDate.Minute, commitTimeDate.Second);
                
            // OnTrac returns their times in Pacific Time zone
            var pacificTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
                
            return pacificTimeZone != null ? 
                TimeZoneInfo.ConvertTime(createdDate, pacificTimeZone, TimeZoneInfo.Local) : 
                createdDate;
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