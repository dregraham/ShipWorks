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

    [KeyedComponent(typeof(ICarrierRateShipmentRequestFactory), ShipmentTypeCode.DhlExpress)]
    public class DhlExpressRateShipmentRequestFactory : ICarrierRateShipmentRequestFactory
    {
        private readonly IDhlExpressAccountRepository accountRepository;
        private readonly IShipmentElementFactory shipmentElementFactory;

        public DhlExpressRateShipmentRequestFactory(IDhlExpressAccountRepository accountRepository, IShipmentElementFactory shipmentElementFactory)
        {
            this.accountRepository = accountRepository;
            this.shipmentElementFactory = shipmentElementFactory;
        }
        public RateShipmentRequest Create(ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));
            MethodConditions.EnsureArgumentIsNotNull(shipment.DhlExpress, nameof(shipment.DhlExpress));

            DhlExpressAccountEntity account = accountRepository.GetAccount(shipment);

            RateShipmentRequest request = shipmentElementFactory.CreateRateRequest(shipment);
            request.RateOptions = new RateRequest() { CarrierIds = new List<string> { account.ShipEngineCarrierId } };

            request.Shipment.AdvancedOptions = new Dictionary<string, object>();
            request.Shipment.AdvancedOptions.Add("delivered_duty_paid", shipment.DhlExpress.DeliveredDutyPaid);
            request.Shipment.AdvancedOptions.Add("non_machinable", shipment.DhlExpress.NonMachinable);
            request.Shipment.AdvancedOptions.Add("saturday_delivery", shipment.DhlExpress.SaturdayDelivery);

            request.Shipment.Customs =  CreateCustoms(shipment);

            List<IPackageAdapter> packages = new List<IPackageAdapter>();
            foreach (DhlExpressPackageEntity package in shipment.DhlExpress.Packages)
            {
                DhlExpressPackageAdapter packageAdapter = new DhlExpressPackageAdapter(shipment, package, 0);
                packages.Add(packageAdapter);
            }

            request.Shipment.Packages = shipmentElementFactory.CreatePackages(packages);

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
