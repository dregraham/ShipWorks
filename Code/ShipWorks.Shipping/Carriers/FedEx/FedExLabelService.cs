using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using log4net;
using ShipWorks.ApplicationCore.Logging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.FedEx.Api;
using ShipWorks.Shipping.Carriers.FedEx.Api.Shipping;
using ShipWorks.Shipping.Carriers.FedEx.Enums;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Shipping.ShipEngine.DTOs;

namespace ShipWorks.Shipping.Carriers.FedEx
{
    /// <summary>
    /// Label service for FedEx
    /// </summary>
    [KeyedComponent(typeof(ILabelService), ShipmentTypeCode.FedEx)]
    public class FedExLabelService : ShipEngineLabelService
    {
        private readonly IFedExAccountRepository accountRepository;
        private readonly IFedExShippingClerkFactory shippingClerkFactory;
        private readonly Func<IEnumerable<IFedExShipResponse>, FedFimsExDownloadedLabelData> createFedExFimsDownloadedLabelData;

        /// <summary>
        /// Constructor
        /// </summary>
        public FedExLabelService(
            IShipEngineWebClient shipEngineWebClient,
            IFedExAccountRepository accountRepository,
            IFedExShippingClerkFactory shippingClerkFactory,
            IIndex<ShipmentTypeCode, ICarrierShipmentRequestFactory> shipmentRequestFactory,
            Func<ShipmentEntity, Label, FedExDownloadedLabelData> createDownloadedLabelData,
            Func<IEnumerable<IFedExShipResponse>, FedFimsExDownloadedLabelData> createFedExFimsDownloadedLabelData,
            Func<Type, ILog> logFactory)
            : base(shipEngineWebClient, shipmentRequestFactory, createDownloadedLabelData, logFactory)
        {
            log = logFactory(typeof(FedExLabelService));
            this.accountRepository = accountRepository;
            this.shippingClerkFactory = shippingClerkFactory;
            this.createFedExFimsDownloadedLabelData = createFedExFimsDownloadedLabelData;
        }

        /// <summary>
        /// The api log source for this label service
        /// </summary>
        public override ApiLogSource ApiLogSource => ApiLogSource.FedEx;

        /// <summary>
        /// The shipment type code for this label service
        /// </summary>
        public override ShipmentTypeCode ShipmentTypeCode => ShipmentTypeCode.FedEx;

        /// <summary>
        /// Get the ShipEngine carrier ID from the shipment
        /// </summary>
        protected override string GetShipEngineCarrierID(ShipmentEntity shipment) => accountRepository.GetAccount(shipment)?.ShipEngineCarrierID ?? string.Empty;

        /// <summary>
        /// Get the ShipEngine label ID from the shipment
        /// </summary>
        protected override string GetShipEngineLabelID(ShipmentEntity shipment) => shipment.FedEx.ShipEngineLabelId;

        /// <summary>
        /// Create the label
        /// </summary>
        public override async Task<TelemetricResult<IDownloadedLabelData>> Create(ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));
            TelemetricResult<IDownloadedLabelData> telemetricResult = new TelemetricResult<IDownloadedLabelData>(TelemetricResultBaseName.ApiResponseTimeInMilliseconds);

            // If this isn't a ShipEngine shipment, create the label the old way
            var serviceType = (FedExServiceType) shipment.FedEx.Service;
            if (serviceType == FedExServiceType.FedExFimsMailView ||
                serviceType == FedExServiceType.FedExFimsMailViewLite ||
                serviceType == FedExServiceType.FedExFimsPremium ||
                serviceType == FedExServiceType.FedExFimsStandard)
            {
                log.Info("FIMS FedEx shipment should go directly to ShipEngine");
                var shippingClerk = shippingClerkFactory.Create(shipment);

                try
                {
                    try
                    {
                        TelemetricResult<GenericResult<IEnumerable<IFedExShipResponse>>> telemetricShipResult = shippingClerk.Ship(shipment);

                        FedFimsExDownloadedLabelData labelData = telemetricShipResult
                            .Value
                            .Map(createFedExFimsDownloadedLabelData)
                            .Match(x => x, ex => { throw ex; });

                        TelemetricResult<IDownloadedLabelData> telemetry = new TelemetricResult<IDownloadedLabelData>(TelemetricResultBaseName.ApiResponseTimeInMilliseconds);
                        telemetricShipResult.CopyTo(telemetry);
                        telemetry.SetValue(labelData);
                        return telemetry;
                    }
                    catch (FedExException ex)
                    {
                        throw new ShippingException(ex.Message, ex);
                    }
                }
                catch (FedExException ex)
                {
                    throw new ShippingException(ex.Message, ex);
                }

            }

            return await CreateLabelInternal(shipment,
                () => shipEngineWebClient.PurchaseLabel(shipmentRequestFactory.CreatePurchaseLabelRequest(shipment), ApiLogSource, telemetricResult), telemetricResult);
        }

        /// <summary>
        /// Void the shipment
        /// </summary>
        public override void Void(ShipmentEntity shipment)
        {
            // If this isn't a ShipEngine shipment, void the old way
            if (!shipment.FedEx.ShipEngineLabelId.HasValue())
            {
                log.Info("Voiding FedEx shipment with no ShipEngineLabelId");
                var shippingClerk = shippingClerkFactory.Create(shipment);

                try
                {
                    shippingClerk.Void(shipment);

                    return;
                }
                catch (FedExException ex)
                {
                    throw new ShippingException(ex.Message, ex);
                }
            }

            base.Void(shipment);
        }
    }
}
