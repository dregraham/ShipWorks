using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.ComponentRegistration;
using log4net;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.EntityInterfaces;
using ShipWorks.Stores.Content;
using ShipWorks.Stores.Platforms.Jet.OnlineUpdating;
using System.Collections.Generic;
using System.Linq;

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
        private readonly IJetOrderSearchProvider orderProvider;
        private readonly ILog log;

        /// <summary>
        /// Constructor
        /// </summary>
        public JetOnlineUpdater(IOrderManager orderManager,
            IOrderRepository orderRepository,
            IJetWebClient webClient,
            IJetOrderSearchProvider orderProvider,
            Func<Type, ILog> logFactory)
        {
            this.orderProvider = orderProvider;
            this.orderManager = orderManager;
            this.orderRepository = orderRepository;
            this.webClient = webClient;
            log = logFactory(typeof(JetOnlineUpdater));
        }

        /// <summary>
        /// Updates the shipment details.
        /// </summary>
        public async Task UpdateShipmentDetails(long orderID, IJetStoreEntity store)
        {
            ShipmentEntity shipment = orderManager.GetLatestActiveShipment(orderID);

            // Check to see if shipment exists and order has shippable line item
            if (shipment == null)
            {
                return;
            }

            var order = shipment.Order;
            var identifiers = await orderProvider.GetOrderIdentifiers(order).ConfigureAwait(false);

            if (identifiers.None())
            {
                return;
            }

            List<JetException> exceptions = new List<JetException>();
            foreach (var merchantOrderId in identifiers)
            {
                try
                {
                    webClient.UploadShipmentDetails(merchantOrderId, shipment, store);
                }
                catch (JetException ex)
                {
                    exceptions.Add(ex);
                }
            }

            if (exceptions.Any())
            {
                throw exceptions.First();
            }

            try
            {
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