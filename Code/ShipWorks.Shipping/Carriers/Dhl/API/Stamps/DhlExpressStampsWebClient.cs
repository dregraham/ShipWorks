using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Services.Protocols;
using Autofac;
using Interapptive.Shared.Business;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.ShipEngine;

namespace ShipWorks.Shipping.Carriers.Dhl.API.Stamps
{
    /// <summary>
    /// Webclient for DhlExpress from Stamps.com
    /// </summary>
    [Component]
    public class DhlExpressStampsWebClient : UspsWebClient, IDhlExpressStampsWebClient
    {
        // We don't include delivery confirmation because we want to treat that like None, because it is
        // included at no charge for services to which it applies.
        private readonly static IDictionary<PostalConfirmationType, AddOnTypeV16> confirmationLookup =
            new Dictionary<PostalConfirmationType, AddOnTypeV16>
            {
                { PostalConfirmationType.Signature, AddOnTypeV16.USASC },
                { PostalConfirmationType.AdultSignatureRequired, AddOnTypeV16.USAASR },
                { PostalConfirmationType.AdultSignatureRestricted, AddOnTypeV16.USAASRD }
            };

        private readonly ICarrierAccountRepository<UspsAccountEntity, IUspsAccountEntity> uspsAccountRepository;
        private readonly ICarrierAccountRepository<DhlExpressAccountEntity, IDhlExpressAccountEntity> dhlExpressAccountRepository;
        private readonly ILog log;

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
            log = LogManager.GetLogger(typeof(DhlExpressStampsWebClient));
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

            try
            {
                VoidShipmentInternal(account, shipment.DhlExpress.StampsTransactionID.Value);
            }
            catch (SoapException ex) when (ex.Message == "The DHL Express mail class print was already voided.")
            {
                log.Info("Stamps says we already voided the label. Swallowing error.", ex);
            }           
        }


        /// <summary>
        /// Get the rates for the given shipment based on its settings
        /// </summary>
        public override (IEnumerable<RateResult> rates, IEnumerable<Exception> errors) GetRates(ShipmentEntity shipment)
        {
            UspsAccountEntity account = GetStampsAccountAssociatedWithDhlAccount(shipment);

            try
            {
                return ExceptionWrapper(() => GetRatesInternal(shipment, account, Carrier.DHLExpress), account)
                    .Select(uspsRate => GetSerivceType(uspsRate)
                        .Map(x => BuildRateResult(shipment, account, uspsRate, x)))
                    .Aggregate((rates: Enumerable.Empty<RateResult>(), errors: Enumerable.Empty<Exception>()),
                        (accumulator, x) => x.Match(
                                rate => (accumulator.rates.Append(rate), accumulator.errors),
                                ex => (accumulator.rates, accumulator.errors.Append(ex))));
            }
            catch (UspsApiException ex)
            {
                if (ex.Message.ToUpperInvariant().Contains("THE USERNAME OR PASSWORD ENTERED IS NOT CORRECT"))
                {
                    // Provide a little more context as to which user name/password was incorrect in the case
                    // where there's multiple accounts or Express1 for USPS is being used to compare rates
                    string message = string.Format("ShipWorks was unable to connect to {0} with account {1}.{2}{2}Check that your account credentials are correct.",
                                    UspsAccountManager.GetResellerName((UspsResellerType) account.UspsReseller),
                                    account.Username,
                                    Environment.NewLine);

                    throw new UspsException(message, ex);
                }

                // This isn't an authentication exception, so just throw the original exception
                throw;
            }
        }


        /// <summary>
        /// Build a RateResult from a USPS rate
        /// </summary>
        private RateResult BuildRateResult(ShipmentEntity shipment, UspsAccountEntity account, RateV33 uspsRate, DhlExpressServiceType serviceType)
        {
            var (description, amount) = GetRateAddOnDetails((PostalConfirmationType) shipment.Postal.Confirmation, uspsRate.AddOns);

            var baseRate = new RateResult(
                EnumHelper.GetDescription(serviceType) + description,
                PostalUtility.GetDaysForRate(uspsRate.DeliverDays, uspsRate.DeliveryDate),
                uspsRate.Amount + amount,
                EnumHelper.GetApiValue(serviceType))
            {
                ProviderLogo = EnumHelper.GetImage((ShipmentTypeCode) shipment.ShipmentType)
            };

            return baseRate;
        }

        /// <summary>
        /// Get details for required rate addons
        /// </summary>
        private static (string description, decimal amount) GetRateAddOnDetails(PostalConfirmationType confirmation, IEnumerable<AddOnV16> addOns) =>
            addOns
                .Where(x => confirmationLookup.TryGetValue(confirmation, out AddOnTypeV16 type) && type == x.AddOnType)
                .Select(x => (description: " (" + EnumHelper.GetDescription(confirmation) + ")", amount: x.Amount))
                .FirstOrDefault();

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

            if (dhlExpressAccount == null)
            {
                throw new ShippingException("There is no DHL Express account associated with this shipment.");
            }

            UspsAccountEntity uspsAccount =
                uspsAccountRepository.Accounts.FirstOrDefault(a => a.AccountId == dhlExpressAccount.UspsAccountId);

            if (uspsAccount == null)
            {
                throw new ShippingException("There is no Stamps.com account associated with this DHL Express account.");
            }

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

            List<AddOnV16> addOns = new List<AddOnV16>();

            if (shipment.DhlExpress.SaturdayDelivery)
            {
                addOns.Add(new AddOnV16 { AddOnType = AddOnTypeV16.CARASAT });
            }

            if (shipment.DhlExpress.ResidentialDelivery)
            {
                addOns.Add(new AddOnV16 { AddOnType = AddOnTypeV16.CARARES });
            }

            if (addOns.Count > 0)
            {
                rate.AddOns = addOns.ToArray();
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
        private static GenericResult<DhlExpressServiceType> GetSerivceType(RateV33 rate)
        {
            if (rate.ServiceType != ServiceType.DHLEWW)
            {
                throw new ShippingException("Not a DhlExpress ServiceType");
            }

            switch (rate.PackageType)
            {
                case PackageTypeV10.ExpressEnvelope:
                    return DhlExpressServiceType.ExpressEnvelope;
                case PackageTypeV10.Package:
                    return DhlExpressServiceType.ExpressWorldWide;
                case PackageTypeV10.Documents:
                    return DhlExpressServiceType.ExpressWorldWideDocuments;
                default:
                    throw new ShippingException("Not a DhlExpress ServiceType");
            }
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
