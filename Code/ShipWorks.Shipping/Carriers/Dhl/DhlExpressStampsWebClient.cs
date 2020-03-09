using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using Autofac;
using Autofac.Features.OwnedInstances;
using Interapptive.Shared.Business;
using Interapptive.Shared.Business.Geography;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.Net;
using Interapptive.Shared.Security;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.AddressValidation;
using ShipWorks.ApplicationCore;
using ShipWorks.ApplicationCore.Licensing;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Common.IO.Hardware.Printers;
using ShipWorks.Core.Common.Threading;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Carriers.BestRate;
using ShipWorks.Shipping.Carriers.Postal;
using ShipWorks.Shipping.Carriers.Postal.Usps;
using ShipWorks.Shipping.Carriers.Postal.Usps.Api.Net;
using ShipWorks.Shipping.Carriers.Postal.Usps.Contracts;
using ShipWorks.Shipping.Carriers.Postal.Usps.Registration;
using ShipWorks.Shipping.Carriers.Postal.Usps.WebServices;
using ShipWorks.Shipping.Carriers.Postal.WebTools;
using ShipWorks.Shipping.Editing;
using ShipWorks.Shipping.Editing.Rating;
using ShipWorks.Shipping.Insurance;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Shipping.Tracking;

namespace ShipWorks.Shipping.Carriers.Dhl
{
    /// <summary>
    /// Webclient for DhlExpress from Stamps.com
    /// </summary>
    public class DhlExpressStampsWebClient : UspsWebClient, IDhlExpressStampsWebClient
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="lifetimeScope"></param>
        public DhlExpressStampsWebClient(ILifetimeScope lifetimeScope)
            :base(lifetimeScope, UspsResellerType.None)
        {
        }

        /// <summary>
        /// Create a label for the given Shipment
        /// </summary>
        /// <param name="shipment"></param>
        /// <returns></returns>
        public async Task<TelemetricResult<StampsLabelResponse>> CreateLabel(ShipmentEntity shipment)
        {
            return await base.ProcessShipmentInternal(shipment, new UspsAccountEntity(), false, Guid.NewGuid(), false);
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
            DhlExpressServiceType serviceType = (DhlExpressServiceType) shipment.DhlExpress.Service;
            PostalPackagingType packagingType = PostalPackagingType.Package;

            RateV33 rate = CreateRateForRating(shipment, account);
            rate.ServiceType = UspsUtility.GetApiServiceType(GetServiceType(serviceType));
            rate.PrintLayout = "Normal4X6";

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
        /// Translate from the DhlExpressServiceType enum to PostalServiceType
        /// </summary>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        private PostalServiceType GetServiceType(DhlExpressServiceType serviceType)
        {
            switch (serviceType)
            {
                case DhlExpressServiceType.ExpressWorldWide:
                    break;
                case DhlExpressServiceType.ExpressEnvelope:
                    break;
                default:
                    break;
            }

            return PostalServiceType.FirstClass;
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

            rate.PackageType = UspsUtility.GetApiPackageType(PostalPackagingType.Package, new DimensionsAdapter(shipment.DhlExpress.Packages[0]));
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
            // shipment.DhlExpress.StampsTransactionId = result.StampsTxID;
            shipment.BilledWeight = result.Rate.EffectiveWeightInOunces / 16D;

            // Set the thermal type for the shipment
            shipment.ActualLabelFormat = (int?) thermalType;
        }
    } 
}
