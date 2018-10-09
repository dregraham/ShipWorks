using System;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Forms;
using Interapptive.Shared.ComponentRegistration;
using Interapptive.Shared.Extensions;
using Interapptive.Shared.UI;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping;

namespace ShipWorks.OrderLookup.ShipmentHistory
{
    /// <summary>
    /// Process voids for the shipment history view
    /// </summary>
    [Component]
    public class PreviousShipmentVoidActionHandler : IPreviousShipmentVoidActionHandler
    {
        private readonly IShippingManager shippingManager;
        private readonly IShippingErrorManager shippingErrorManager;
        private readonly IMessageHelper messageHelper;
        private readonly IOrderLookupPreviousShipmentLocator shipmentLocator;

        /// <summary>
        /// Constructor
        /// </summary>
        public PreviousShipmentVoidActionHandler(
            IShippingManager shippingManager,
            IShippingErrorManager shippingErrorManager,
            IOrderLookupPreviousShipmentLocator shipmentLocator,
            IMessageHelper messageHelper)
        {
            this.shipmentLocator = shipmentLocator;
            this.messageHelper = messageHelper;
            this.shippingErrorManager = shippingErrorManager;
            this.shippingManager = shippingManager;
        }

        /// <summary>
        /// Void a processed shipment
        /// </summary>
        public GenericResult<ProcessedShipmentEntity> Void(ProcessedShipmentEntity shipment) =>
            VoidInternal(shipment.ShipmentID)
                .Map(() => shipment);

        /// <summary>
        /// Void a processed shipment
        /// </summary>
        public Task<Unit> VoidLast() =>
            shipmentLocator.GetLatestShipmentDetails()
                .Bind(PerformVoid);

        /// <summary>
        /// Perform the void of a previous shipment
        /// </summary>
        private Task<Unit> PerformVoid(PreviousProcessedShipmentDetails shipment)
        {
            if (shipment == null)
            {
                return Task.FromException<Unit>(new Exception("Could not find a processed shipment from today"));
            }

            if (shipment.Voided)
            {
                return Task.FromException<Unit>(new Exception("The last processed shipment has already been voided"));
            }

            return VoidInternal(shipment.ShipmentID)
                .Match(() => Task.FromResult<Unit>(Unit.Default), ex => Task.FromException<Unit>(ex));
        }

        /// <summary>
        /// 
        /// </summary>
        private Result VoidInternal(long shipmentID)
        {
            if (messageHelper.ShowQuestion("Are you sure you want to void this shipment?") == DialogResult.OK)
            {
                return shippingManager.VoidShipment(shipmentID, shippingErrorManager);
            }

            return Result.FromSuccess();
        }
    }
}
