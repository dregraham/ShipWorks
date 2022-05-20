using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac.Features.Indexed;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.ShipEngine;
using ShipWorks.Shipping.ShipEngine.DTOs;
using ShipWorks.ApplicationCore.Logging;
using Interapptive.Shared.ComponentRegistration;
using log4net;

namespace ShipWorks.Shipping.Carriers.Ups.ShipEngine
{
    /// <summary>
    /// Label client for getting Ups Labels from ShipEngine
    /// </summary>
    [Component(RegistrationType.Self)]
    public class UpsShipEngineLabelClient : ShipEngineLabelService, IUpsLabelClient
    {
        private readonly ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity> accountRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        public UpsShipEngineLabelClient(
            IShipEngineWebClient shipEngineWebClient,
            IIndex<ShipmentTypeCode, ICarrierShipmentRequestFactory> shipmentRequestFactory,
            Func<ShipmentEntity, Label, UpsShipEngineDownloadedLabelData> createDownloadedLabelData,
            ICarrierAccountRepository<UpsAccountEntity, IUpsAccountEntity> accountRepository,
            Func<Type, ILog> logFactory)
            : base(shipEngineWebClient, shipmentRequestFactory, createDownloadedLabelData, logFactory)
        {
            this.accountRepository = accountRepository;
        }

        /// <summary>
        /// The log source
        /// </summary>
        public override ApiLogSource ApiLogSource => ApiLogSource.UPS;

        /// <summary>
        /// The shipment type
        /// </summary>
        public override ShipmentTypeCode ShipmentTypeCode => ShipmentTypeCode.UpsOnLineTools;

        /// <summary>
        /// Get a label for the given shipment
        /// </summary>
        public Task<TelemetricResult<IDownloadedLabelData>> GetLabel(ShipmentEntity shipment) =>
            base.Create(shipment);

        /// <summary>
        /// Void the given shipment
        /// </summary>
        public void VoidLabel(ShipmentEntity shipment) =>
            Void(shipment);

        /// <summary>
        /// Get the ShipEngine carrier id
        /// </summary>
        protected override string GetShipEngineCarrierID(ShipmentEntity shipment) =>
            accountRepository.GetAccountReadOnly(shipment).ShipEngineCarrierId;

        /// <summary>
        /// Get the ShipEngine label Id
        /// </summary>
        /// <param name="shipment"></param>
        /// <returns></returns>
        protected override string GetShipEngineLabelID(ShipmentEntity shipment) =>
            shipment.Ups.ShipEngineLabelID;
    }
}
