using ShipWorks.Shipping.ShipEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShipEngine.ApiClient.Model;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Shipping.Carriers.Dhl
{
    /// <summary>
    /// Factory for creating DHL RateShipmentRequests
    /// </summary>
    [KeyedComponent(typeof(ICarrierShipmentRequestFactory), ShipmentTypeCode.DhlExpress)]
    public class DhlExpressShipmentRequestFactory : ICarrierShipmentRequestFactory
    {
        private readonly IDhlExpressAccountRepository accountRepository;
        private readonly IShipEngineRequestFactory shipmentElementFactory;
        private readonly IShipmentTypeManager shipmentTypeManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public DhlExpressShipmentRequestFactory(IDhlExpressAccountRepository accountRepository, 
            IShipEngineRequestFactory shipmentElementFactory, 
            IShipmentTypeManager shipmentTypeManager)
        {
            this.accountRepository = accountRepository;
            this.shipmentElementFactory = shipmentElementFactory;
            this.shipmentTypeManager = shipmentTypeManager;
        }

        /// <summary>
        /// Create a RateShipmentRequest
        /// </summary>
        public RateShipmentRequest CreateRateShipmentRequest(ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));
            MethodConditions.EnsureArgumentIsNotNull(shipment.DhlExpress, nameof(shipment.DhlExpress));

            DhlExpressAccountEntity account = accountRepository.GetAccount(shipment);

            if (account == null)
            {
                throw new DhlExpressException("Invalid account associated with shipment.");
            }

            RateShipmentRequest request = shipmentElementFactory.CreateRateRequest(shipment);
            request.RateOptions = new RateRequest() { CarrierIds = new List<string> { account.ShipEngineCarrierId } };

            request.Shipment.AdvancedOptions = new Dictionary<string, object>();
            request.Shipment.AdvancedOptions.Add("delivered_duty_paid", shipment.DhlExpress.DeliveredDutyPaid);
            request.Shipment.AdvancedOptions.Add("non_machinable", shipment.DhlExpress.NonMachinable);
            request.Shipment.AdvancedOptions.Add("saturday_delivery", shipment.DhlExpress.SaturdayDelivery);

            request.Shipment.Customs = CreateCustoms(shipment);

            List<IPackageAdapter> packages = shipmentTypeManager.Get(ShipmentTypeCode.DhlExpress).GetPackageAdapters(shipment).ToList();
            request.Shipment.Packages = shipmentElementFactory.CreatePackages(packages);

            return request;
        }

        /// <summary>
        /// Creates a PurchaseLabelRequest with DHL Express specific details from the given shipment
        /// </summary>
        public PurchaseLabelRequest CreatePurchaseLabelRequest(ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));
            MethodConditions.EnsureArgumentIsNotNull(shipment.DhlExpress, nameof(shipment.DhlExpress));

            DhlExpressAccountEntity account = accountRepository.GetAccount(shipment);

            if (account == null)
            {
                throw new DhlExpressException("Invalid account associated with shipment.");
            }

            List<IPackageAdapter> packages = shipmentTypeManager.Get(ShipmentTypeCode.DhlExpress).GetPackageAdapters(shipment).ToList();

            DhlExpressServiceType service = (DhlExpressServiceType) shipment.DhlExpress.Service;
            string serviceApiValue = EnumHelper.GetApiValue(service);

            PurchaseLabelRequest request = shipmentElementFactory.CreatePurchaseLabelRequest(shipment, packages, serviceApiValue);
            request.Shipment.CarrierId = account.ShipEngineCarrierId;

            request.Shipment.AdvancedOptions = new Dictionary<string, object>();
            request.Shipment.AdvancedOptions.Add("delivered_duty_paid", shipment.DhlExpress.DeliveredDutyPaid);
            request.Shipment.AdvancedOptions.Add("non_machinable", shipment.DhlExpress.NonMachinable);
            request.Shipment.AdvancedOptions.Add("saturday_delivery", shipment.DhlExpress.SaturdayDelivery);

            request.Shipment.Customs = CreateCustoms(shipment);

            return request;
        }

        /// <summary>
        /// Creates customs for a ShipEngine request
        /// </summary>
        private InternationalOptions CreateCustoms(ShipmentEntity shipment)
        {
            InternationalOptions customs = new InternationalOptions()
            {
                Contents = (InternationalOptions.ContentsEnum) shipment.DhlExpress.Contents,
                CustomsItems = shipmentElementFactory.CreateCustomsItems(shipment),
                NonDelivery = (InternationalOptions.NonDeliveryEnum) shipment.DhlExpress.NonDelivery
            };

            return customs;
        }
    }
}
