using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Interapptive.Shared.Business;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;
using ShipWorks.Shipping.ShipEngine;

namespace ShipWorks.Shipping.Carriers.Dhl.API.Stamps
{
    /// <summary>
    /// Webclient for DhlExpress from Stamps.com
    /// </summary>
    [Component]
    public class DhlExpressStampsWebClient : UspsWebClient, IDhlExpressStampsWebClient
    {
        private readonly ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity> uspsAccountRepository;
        private readonly ICarrierAccountRepository<DhlExpressAccountEntity, IDhlExpressAccountEntity> dhlExpressAccountRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public DhlExpressStampsWebClient(ILifetimeScope lifetimeScope,
            ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity> uspsAccountRepository,
            ICarrierAccountRepository<DhlExpressAccountEntity, IDhlExpressAccountEntity> dhlExpressAccountRepository)
            :base(lifetimeScope, UspsResellerType.None)
        {
            this.uspsAccountRepository = uspsAccountRepository;
            this.dhlExpressAccountRepository = dhlExpressAccountRepository;
        }

        /// <summary>
        /// Create a label for the given Shipment
        /// </summary>
        /// <param name="shipment"></param>
        /// <returns></returns>
        public override async Task<TelemetricResult<StampsLabelResponse>> ProcessShipment(ShipmentEntity shipment)
        {
            shipment.DhlExpress.IntegratorTransactionID = Guid.NewGuid();

            var uspsAccount = GetStampsAccountAssociatedWithDhlAccount(shipment);

            if (uspsAccount == null)
            {
                throw new DhlExpressException("The Stamps.com account associated with this DHL Express account no longer exists.");
            }

            return await ExceptionWrapperAsync(() => 
                base.ProcessShipmentInternal(shipment, uspsAccount, false, shipment.DhlExpress.IntegratorTransactionID.Value), uspsAccount).ConfigureAwait(false);
        }

        /// <summary>
        /// Void the given shipment
        /// </summary>
        public override void VoidShipment(ShipmentEntity shipment)
        {
            UspsAccountEntity account = GetStampsAccountAssociatedWithDhlAccount(shipment);

            VoidShipmentInternal(account, shipment.DhlExpress.StampsTransactionID.Value);
        }

        /// <summary>
        /// Create customs information for the given shipment
        /// </summary>
        protected override CustomsV5 CreateCustoms(ShipmentEntity shipment)
        {
            if (!CustomsManager.IsCustomsRequired(shipment))
            {
                return null;
            }

            CustomsV5 customs = new CustomsV5();

            // Content type
            ShipEngineContentsType contents = (ShipEngineContentsType) shipment.DhlExpress.Contents;
            customs.ContentType = GetContentType(contents);

            List<CustomsLine> lines = new List<CustomsLine>();

            // Go through each of the customs contents to create  the USPS custom line entity
            foreach (ShipmentCustomsItemEntity customsItem in shipment.CustomsItems)
            {
                WeightValue weightValue = new WeightValue(customsItem.Weight);

                CustomsLine line = new CustomsLine();
                line.Description = customsItem.Description.Truncate(60);
                line.Quantity = customsItem.Quantity;
                line.Value = customsItem.UnitValue;

                line.WeightLb = weightValue.PoundsOnly;
                line.WeightOz = weightValue.OuncesOnly;

                line.HSTariffNumber = customsItem.HarmonizedCode;
                line.CountryOfOrigin = customsItem.CountryOfOrigin;

                line.sku = customsItem.Description.Truncate(20);

                lines.Add(line);
            }

            customs.CustomsLines = lines.ToArray();

            return customs;
        }

        /// <summary>
        /// Get the stamps account that is associated with the DHL account being used
        /// </summary>
        private UspsAccountEntity GetStampsAccountAssociatedWithDhlAccount(ShipmentEntity shipment)
        {
            IDhlExpressAccountEntity dhlExpressAccount = dhlExpressAccountRepository.GetAccountReadOnly(shipment);

            UspsAccountEntity uspsAccount =
                uspsAccountRepository.Accounts.FirstOrDefault(a => a.AccountId == dhlExpressAccount.UspsAccountId);
            return uspsAccount;
        }

        /// <summary>
        /// translate from the SE content type to stamps content type
        /// </summary>
        private static ContentTypeV2 GetContentType(ShipEngineContentsType type)
        {
            switch (type)
            {
                case ShipEngineContentsType.Merchandise:
                    return ContentTypeV2.Merchandise;
                case ShipEngineContentsType.Documents:
                    return ContentTypeV2.Document;
                case ShipEngineContentsType.Gift:
                    return ContentTypeV2.Gift;
                case ShipEngineContentsType.ReturnedGoods:
                    return ContentTypeV2.ReturnedGoods;
                case ShipEngineContentsType.Sample:
                    return ContentTypeV2.CommercialSample;
                default:
                    throw new UspsException($"Unknown custom contents type {type}.");
            }
        }

        /// <summary>
        /// Create a rate object for processing
        /// </summary>
        protected override RateV33 CreateRateForProcessing(ShipmentEntity shipment, UspsAccountEntity account)
        {
            RateV33 rate = CreateRateForRating(shipment, account);

            // for Stamps.com the service type is always DHLEWW, the packaging type denotes the actual service
            rate.ServiceType = ServiceType.DHLEWW;
            rate.PrintLayout = "Normal4X6";

            if (shipment.DhlExpress.SaturdayDelivery)
            {
                rate.AddOns = new[] { new AddOnV16 { AddOnType = AddOnTypeV16.CARASAT } };
            }

            // For APO/FPO, we have to specifically ask for customs docs
            if (PostalUtility.IsMilitaryState(shipment.ShipStateProvCode) || ShipmentTypeManager.GetType(shipment).IsCustomsRequired(shipment))
            {
                rate.PrintLayout = (PostalUtility.GetCustomsForm(shipment) == PostalCustomsForm.CN72) ? "Normal4X6CP72" : "Normal4X6CN22";
            }

            if (shipment.ReturnShipment)
            {
                // Swapping out Normal with Return indicates a return label
                rate.PrintLayout = rate.PrintLayout.Replace("Normal", "Return");
            }

            return rate;
        }

        /// <summary>
        /// Create a rate object 
        /// </summary>
        protected override RateV33 CreateRateForRating(ShipmentEntity shipment, UspsAccountEntity account)
        {
            RateV33 rate = new RateV33();

            string fromZipCode = !string.IsNullOrEmpty(account.MailingPostalCode) ? account.MailingPostalCode : shipment.OriginPostalCode;
            string toZipCode = shipment.ShipPostalCode;
            string toCountry = shipment.AdjustedShipCountryCode();

            // Swap the to/from for return shipments.
            if (shipment.ReturnShipment)
            {
                rate.FromZIPCode = toZipCode;
                rate.ToZIPCode = fromZipCode;
                rate.ToCountry = shipment.AdjustedOriginCountryCode();
            }
            else
            {
                rate.FromZIPCode = fromZipCode;
                rate.ToZIPCode = toZipCode;
                rate.ToCountry = toCountry;
            }

            // Default the weight to 14oz for best rate if it is 0, so we can get a rate without needing the user to provide a value.  We do 14oz so it kicks it into a Priority shipment, which
            // is the category that most of our users will be in.
            double defaultPounds = BestRateScope.IsActive ? 0.88 : .1;
            WeightValue weightValue = new WeightValue(shipment.TotalWeight > 0 ? shipment.TotalWeight : defaultPounds);
            rate.WeightLb = weightValue.PoundsOnly;
            rate.WeightOz = weightValue.OuncesOnly;

            rate.PackageType = GetPackageType((DhlExpressServiceType) shipment.DhlExpress.Service);
            rate.NonMachinable = shipment.DhlExpress.NonMachinable;

            rate.Length = shipment.DhlExpress.Packages[0].DimsLength;
            rate.Width = shipment.DhlExpress.Packages[0].DimsWidth;
            rate.Height = shipment.DhlExpress.Packages[0].DimsHeight;

            rate.ShipDate = shipment.ShipDate;
            rate.DeclaredValue = shipment.CustomsValue;

            if (CustomsManager.IsCustomsRequired(shipment))
            {
                rate.ContentTypeSpecified = true;
                rate.ContentType = GetContentType((ShipEngineContentsType) shipment.DhlExpress.Contents);
            }

            return rate;
        }

        /// <summary>
        /// Get package type for the given service
        /// </summary>
        private static PackageTypeV10 GetPackageType(DhlExpressServiceType service)
        {
            switch (service)
            {
                case DhlExpressServiceType.ExpressEnvelope:
                    return PackageTypeV10.ExpressEnvelope;
                case DhlExpressServiceType.ExpressWorldWideDocuments:
                    return PackageTypeV10.Documents;
                case DhlExpressServiceType.ExpressWorldWide:
                    return PackageTypeV10.Package;
                default:
                    throw new DhlExpressException($"Unknown service type {service}.");
            }
        }

        /// <summary>
        /// Memo text is currently not supported by dhl express
        /// </summary>
        protected override string GetMemoText(ShipmentEntity shipment) =>
            string.Empty;

        /// <summary>
        /// Postage mode is always normal
        /// </summary>
        protected override PostageMode GetPostageMode(ShipmentEntity shipment) =>
            PostageMode.Normal;

        /// <summary>
        /// Save the label info back to the shipment once its done processing
        /// </summary>
        protected override void SaveLabelInformation(ShipmentEntity shipment, CreateIndiciumResult result, ThermalLanguage? thermalType)
        {
            shipment.TrackingNumber = result.TrackingNumber;
            shipment.ShipmentCost = result.ShipmentCost;
            shipment.DhlExpress.StampsTransactionID = result.StampsTxID;
            shipment.BilledWeight = result.Rate.EffectiveWeightInOunces / 16D;

            // Set the thermal type for the shipment
            shipment.ActualLabelFormat = (int?) thermalType;
        }
    } 
}
