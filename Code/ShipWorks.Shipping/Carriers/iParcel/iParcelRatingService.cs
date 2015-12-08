using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.iParcel.Enums;
using ShipWorks.Shipping.Editing.Enums;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Settings;
using ShipWorks.Stores.Content;

namespace ShipWorks.Shipping.Carriers.iParcel
{
    public class iParcelRatingService : IRatingService
    {
        private readonly ICarrierAccountRepository<IParcelAccountEntity> accountRepository;
        private readonly IiParcelServiceGateway serviceGateway;
        private readonly iParcelShipmentType iParcelShipmentType;
        private readonly IExcludedServiceTypeRepository excludedServiceTypeRepository;
        private readonly IOrderManager orderManager;

        public iParcelRatingService(ICarrierAccountRepository<IParcelAccountEntity> accountRepository, IiParcelServiceGateway serviceGateway, iParcelShipmentType iParcelShipmentType, IExcludedServiceTypeRepository excludedServiceTypeRepository, IOrderManager orderManager)
        {
            this.accountRepository = accountRepository;
            this.serviceGateway = serviceGateway;
            this.iParcelShipmentType = iParcelShipmentType;
            this.excludedServiceTypeRepository = excludedServiceTypeRepository;
            this.orderManager = orderManager;
        }

        public RateGroup GetRates(ShipmentEntity shipment)
        {
            IParcelAccountEntity iParcelAccount;

            try
            {
                iParcelAccount = accountRepository.GetAccount(shipment.IParcel.IParcelAccountID);
            }
            catch (iParcelException ex)
            {
                if (ex.Message == "No i-parcel account is selected for the shipment.")
                {
                    // Provide a message with additional context
                    throw new iParcelException("An i-parcel account is required to view rates.", ex);
                }
                throw;
            }

            // i-parcel requires that we upload item information, so fetch the order and order items
            orderManager.PopulateOrderDetails(shipment);

            List<RateResult> results = new List<RateResult>();

            iParcelCredentials credentials = new iParcelCredentials(iParcelAccount.Username, iParcelAccount.Password, true, serviceGateway);

            try
            {
                DataSet ratesResult = serviceGateway.GetRates(credentials, shipment);
                if (RateResultAreValid(ratesResult))
                {
                    // i-parcel will return a negative value if there was some sort of error or the shipment is not eligible for
                    // this service type (e.g. initial testing indicates rates won't come back for any package over 66 pounds)

                    DataTable costInfoTable = ratesResult.Tables["CostInfo"];

                    // Find the service types where a valid rate (shipping cost > 0) was given for each of the packages in the shipment
                    IEnumerable<iParcelServiceType> supportedServiceTypes = costInfoTable.AsEnumerable()
                                                                                            .Where(r => decimal.Parse(r.Field<string>("PackageShipping")) >= 0)
                                                                                            .GroupBy(r => r["Service"])
                                                                                            .Where(grp => grp.Count() == shipment.IParcel.Packages.Count)
                                                                                            .Select(grp => EnumHelper.GetEnumByApiValue<iParcelServiceType>(grp.Key.ToString()));

                    IEnumerable<iParcelServiceType> disabledServices = iParcelShipmentType.GetExcludedServiceTypes(excludedServiceTypeRepository)
                        .Select(s => (iParcelServiceType)s);


                    // Filter out the excluded service types before creating rate results
                    foreach (iParcelServiceType serviceType in supportedServiceTypes.Except(disabledServices.Where(s => s != (iParcelServiceType)shipment.IParcel.Service)))
                    {
                        // Calculate the total shipment cost for all the package rates for the service type
                        List<DataRow> serviceRows = costInfoTable.AsEnumerable()
                                                .Where(row => EnumHelper.GetEnumByApiValue<iParcelServiceType>(row["Service"].ToString()) == serviceType)
                                                .ToList();

                        decimal shippingCost = serviceRows.Sum(row => decimal.Parse(row["PackageShipping"].ToString()) + decimal.Parse(row["PackageInsurance"].ToString()));
                        decimal taxCost = serviceRows.Sum(row => decimal.Parse(row["PackageTax"].ToString()));
                        decimal dutyCost = serviceRows.Sum(row => decimal.Parse(row["PackageDuty"].ToString()));
                        decimal totalCost = shippingCost + taxCost + dutyCost;

                        RateResult serviceRate = new RateResult(EnumHelper.GetDescription(serviceType), string.Empty,
                            totalCost, new RateAmountComponents(taxCost, dutyCost, shippingCost),
                            new iParcelRateSelection(serviceType))
                        {
                            ServiceLevel = ServiceLevelType.Anytime,
                            ShipmentType = ShipmentTypeCode.iParcel,
                            ProviderLogo = EnumHelper.GetImage(ShipmentTypeCode.iParcel)
                        };

                        results.Add(serviceRate);
                    }
                }
            }
            catch (iParcelException ex)
            {
                throw new ShippingException(ex.Message, ex);
            }

            return new RateGroup(results);
        }

        private static bool RateResultAreValid(DataSet ratesResult)
        {
            return ratesResult != null && ratesResult.Tables.Count != 0 && ratesResult.Tables[0].Rows.Count != 0 &&
                   ratesResult.Tables.Contains("CostInfo");
        }
    }
}