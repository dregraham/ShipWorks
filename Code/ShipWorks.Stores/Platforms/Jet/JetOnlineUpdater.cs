using System;
using System.Data.SqlClient;
using Interapptive.Shared.ComponentRegistration;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Stores.Content;

namespace ShipWorks.Stores.Platforms.Jet
{
    /// <summary>
    /// Online Updater for Jet.com
    /// </summary>
    [Component(RegistrationType.Self)]
    public class JetOnlineUpdater
    {
        private readonly IOrderManager orderManager;
        private readonly IOrderRepository orderRepository;
        private readonly IJetWebClient webClient;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public JetOnlineUpdater(IOrderManager orderManager, IOrderRepository orderRepository, IJetWebClient webClient, Func<Type, ILog> logFactory)
        {
            this.orderManager = orderManager;
            this.orderRepository = orderRepository;
            this.webClient = webClient;
            log = logFactory(typeof(JetOnlineUpdater));
        }

        /// <summary>
        /// Updates the shipment details.
        /// </summary>
        public void UpdateShipmentDetails(long orderID)
        {
            ShipmentEntity shipment = orderManager.GetLatestActiveShipment(orderID);

            // Check to see if shipment exists and order has shippable line item
            if (shipment != null)
            {
                UpdateShipmentDetails(shipment);
            }
        }

        /// <summary>
        /// Updates the shipment details.
        /// </summary>
        public void UpdateShipmentDetails(ShipmentEntity shipment)
        {
            OrderEntity order = shipment.Order;
            orderRepository.PopulateOrderDetails(order);
            
            if (!shipment.Order.IsManual)
            {
                try
                {
                    webClient.UpdateShipmentDetails(shipment);
                    order.OnlineStatus = "Complete";
                    orderRepository.Save(order);
                }
                catch (SqlException ex)
                {
                    string errorMessage = $"Error saving online status for order {order.OrderNumberComplete}.";

                    log.Error(errorMessage, ex);
                    throw new JetException(errorMessage, ex);
                }
            }
        }
    }
}