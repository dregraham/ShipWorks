﻿using System;
using System.Reactive;
using System.Threading.Tasks;
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
    public class ShipmentHistoryVoidProcessor : IShipmentHistoryVoidProcessor
    {
        private readonly IShippingManager shippingManager;
        private readonly IShippingErrorManager shippingErrorManager;
        private readonly IMessageHelper messageHelper;
        private readonly IOrderLookupPreviousShipmentLocator shipmentLocator;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentHistoryVoidProcessor(
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
            shippingManager.VoidShipment(shipment.ShipmentID, shippingErrorManager)
                .Map(_ => shipment);

        /// <summary>
        /// Void a processed shipment
        /// </summary>
        public Task<Unit> VoidLast() =>
            shipmentLocator.GetLatestShipmentID()
                .Bind(PerformVoid);

        /// <summary>
        /// Perform the void of a previous shipment
        /// </summary>
        private Task<Unit> PerformVoid(PreviousProcessedShipmentDetails shipment)
        {
            if (shipment.Voided)
            {
                return Task.FromException<Unit>(new Exception("The last processed shipment has already been voided"));
            }

            return shippingManager.VoidShipment(shipment.ShipmentID, shippingErrorManager)
                .Match(x => Task.FromResult<Unit>(Unit.Default), ex => Task.FromException<Unit>(ex));
        }
    }
}
