using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Utility;
using ShipWorks.Api.Orders.Shipments;
using ShipWorks.Data;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Messaging.Messages.Shipping;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Services;

namespace ShipWorks.Api.Orders
{
    /// <summary>
    /// Factory for generating order responses
    /// </summary>
    [Component]
    public class OrdersResponseFactory : IOrdersResponseFactory
    {
        private readonly ICarrierShipmentAdapterFactory carrierShipmentAdapterFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        public OrdersResponseFactory(ICarrierShipmentAdapterFactory carrierShipmentAdapterFactory)
        {
            this.carrierShipmentAdapterFactory = carrierShipmentAdapterFactory;
        }

        /// <summary>
        /// Create an order response using the given order
        /// </summary>
        public OrderResponse CreateOrdersResponse(IOrderEntity order) =>
            new OrderResponse()
            {
                OrderId = order.OrderID,
                OrderNumber = order.OrderNumberComplete,
                OrderDate = order.OrderDate,
                LastModifiedDate = order.OnlineLastModified,
                OrderTotal = order.OrderTotal,
                StoreStatus = order.OnlineStatus,
                ShipAddress = new Address()
                {
                    RecipientName = order.ShipUnparsedName,
                    Street1 = order.ShipStreet1,
                    Street2 = order.ShipStreet2,
                    Street3 = order.ShipStreet3,
                    City = order.ShipCity,
                    StateProvince = order.ShipStateProvCode,
                    CountryCode = order.ShipCountryCode,
                    PostalCode = order.ShipPostalCode
                },
                BillAddress = new Address()
                {
                    RecipientName = order.BillUnparsedName,
                    Street1 = order.BillStreet1,
                    Street2 = order.BillStreet2,
                    Street3 = order.BillStreet3,
                    City = order.BillCity,
                    StateProvince = order.BillStateProvCode,
                    CountryCode = order.BillCountryCode,
                    PostalCode = order.BillPostalCode
                },
            };

        /// <summary>
        /// Create a ProcessShipmentResponse from a ProcessShipmentResult
        /// </summary>
        public ProcessShipmentResponse CreateProcessShipmentResponse(ProcessShipmentResult processShipmentResult)
        {
            var response = new ProcessShipmentResponse();


            ShipmentEntity shipment = processShipmentResult.Shipment;
            ICarrierShipmentAdapter adapter = carrierShipmentAdapterFactory.Get(shipment);

            response.Carrier = EnumHelper.GetDescription(shipment.ShipmentTypeCode);
            response.Service = adapter.ServiceTypeName;
            response.Cost = shipment.ShipmentCost;
            response.Tracking = shipment.TrackingNumber;

            AddLabelData(response, adapter);

            return response;
        }

        /// <summary>
        /// Add label data to the response
        /// </summary>
        private void AddLabelData(ProcessShipmentResponse response, ICarrierShipmentAdapter adapter)
        {
            if (adapter.SupportsMultiplePackages)
            {
                // Add labels for each package
                foreach (IPackageAdapter package in adapter.GetPackageAdapters())
                {
                    DataResourceManager.GetConsumerResourceReferences(package.PackageId)
                        .ForEach(r => response.Labels.Add(new LabelData(r.Label, Convert.ToBase64String(Encoding.UTF8.GetBytes(r.ReadAllText())))));
                }
            }
            else
            {
                DataResourceManager.GetConsumerResourceReferences(adapter.Shipment.ShipmentID)
                    .ForEach(r => response.Labels.Add(new LabelData(r.Label, Convert.ToBase64String(Encoding.UTF8.GetBytes(r.ReadAllText())))));
            }
        }
    }
}
