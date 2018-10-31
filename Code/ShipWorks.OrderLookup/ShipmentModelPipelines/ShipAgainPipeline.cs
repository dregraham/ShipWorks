using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.UI;
using ShipWorks.ApplicationCore;
using ShipWorks.Common.IO.KeyboardShortcuts.Messages;
using ShipWorks.Core.Messaging;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;
using ShipWorks.Shipping.Services;

namespace ShipWorks.OrderLookup.ShipmentModelPipelines
{
    /// <summary>
    /// Pipeline for shipping again
    /// </summary>
    public class ShipAgainPipeline : IOrderLookupShipmentModelPipeline
    {
        private readonly IMessenger messenger;
        private readonly IMainForm mainForm;
        private readonly IShippingManager shippingManager;
        private readonly IMessageHelper messageHelper;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipAgainPipeline(
            IMessenger messenger,
            IMainForm mainForm,
            IShippingManager shippingManager,
            IMessageHelper messageHelper)
        {
            this.messenger = messenger;
            this.mainForm = mainForm;
            this.shippingManager = shippingManager;
            this.messageHelper = messageHelper;
        }

        /// <summary>
        /// Register the pipeline
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IDisposable Register(IOrderLookupShipmentModel model) =>
            messenger.OfType<OrderLookupShipAgainMessage>()
                .Subscribe(x => ShipAgain(x, model));

        /// <summary>
        /// Ship the Shipment again
        /// </summary>
        private void ShipAgain(OrderLookupShipAgainMessage message, IOrderLookupShipmentModel model)
        {
            using (messageHelper.ShowProgressDialog("Shipping", "Creating new shipment"))
            {
                ICarrierShipmentAdapter shipmentAdapter = shippingManager.GetShipment(message.ShipmentID);
                ShipmentEntity shipment = shippingManager.CreateShipmentCopy(shipmentAdapter.Shipment);

                model.LoadOrder(shipment.Order);

                mainForm.SelectOrderLookupTab();
            }
        }
    }
}
