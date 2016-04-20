using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.AddressValidation;
using ShipWorks.Core.Messaging.Messages.Shipping;
using ShipWorks.Data.Model;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Data.Model.HelperClasses;
using ShipWorks.Shipping.Services;
using ShipWorks.Stores;
using ShipWorks.Stores.Content;

namespace ShipWorks.Shipping.Loading
{
    /// <summary>
    /// Loads a shipment for an order.
    /// </summary>
    public class ShipmentLoader : IShipmentLoader
    {
        private readonly Lazy<IPrefetchPath2> orderPrefetchPath = new Lazy<IPrefetchPath2>(CreatePrefetchPath);

        private readonly IStoreTypeManager storeTypeManager;
        private readonly IOrderManager orderManager;
        private readonly IShipmentFactory shipmentFactory;
        private readonly ICarrierShipmentAdapterFactory carrierShipmentAdapterFactory;
        private readonly IValidatedAddressManager validatedAddressManager;
        private readonly IShippingManager shippingManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public ShipmentLoader(IStoreTypeManager storeTypeManager,
                              IOrderManager orderManager,
                              IShipmentFactory shipmentFactory,
                              ICarrierShipmentAdapterFactory carrierShipmentAdapterFactory,
                              IValidatedAddressManager validatedAddressManager,
                              IShippingManager shippingManager)
        {
            this.storeTypeManager = storeTypeManager;
            this.orderManager = orderManager;
            this.shipmentFactory = shipmentFactory;
            this.carrierShipmentAdapterFactory = carrierShipmentAdapterFactory;
            this.validatedAddressManager = validatedAddressManager;
            this.shippingManager = shippingManager;
        }

        /// <summary>
        /// Load all the shipments on a background thread
        /// </summary>
        public async Task<LoadedOrderSelection> Load(long orderID)
        {
            OrderEntity order = null;
            IEnumerable<ICarrierShipmentAdapter> adapters = Enumerable.Empty<ICarrierShipmentAdapter>();

            // Execute the work
            try
            {
                order = orderManager.LoadOrder(orderID, orderPrefetchPath.Value);

                // If we don't mark customs as loaded here, any changes to customs items will be lost when
                // EnsureCustomsLoaded is true.  Since this is called all over the place, this is the simpler solution
                foreach (ShipmentEntity shipment in order.Shipments)
                {
                    shipment.CustomsItemsLoaded = true;
                }

                shipmentFactory.AutoCreateIfNecessary(order);

                await ValidateShipments(order.Shipments).ConfigureAwait(false);
                adapters = CreateUpdatedShipmentAdapters(order);

                return new LoadedOrderSelection(order, adapters, GetDestinationAddressEditable(order));
            }
            catch (Exception ex)
            {
                // In order to get the OrderSelectionChangedHandler to match orders and allow loading to complete
                // we need the order id to be passed along in the LoadedOrderSelection
                return new LoadedOrderSelection(ex, order, adapters, ShippingAddressEditStateType.Editable);
            }
        }

        /// <summary>
        /// Create a collection of shipment adapters that have been fully updated
        /// </summary>
        private IEnumerable<ICarrierShipmentAdapter> CreateUpdatedShipmentAdapters(OrderEntity order)
        {
            List<ICarrierShipmentAdapter> adapters = order.Shipments.Select(carrierShipmentAdapterFactory.Get).ToList();
            foreach (ICarrierShipmentAdapter adapter in adapters)
            {
                adapter.UpdateDynamicData();
                if (adapter.Shipment.IsDirty)
                {
                    shippingManager.SaveShipmentToDatabase(adapter.Shipment, false);
                }
            }
            return adapters;
        }

        /// <summary>
        /// Validate the collection of shipments
        /// </summary>
        private Task ValidateShipments(EntityCollection<ShipmentEntity> shipments)
        {
            Task[] validationTasks = shipments
                .Select(validatedAddressManager.ValidateShipmentAsync)
                .ToArray();

            return TaskEx.WhenAll(validationTasks);
        }

        /// <summary>
        /// Get whether the destination address is editable
        /// </summary>
        private ShippingAddressEditStateType GetDestinationAddressEditable(OrderEntity order)
        {
            ShipmentEntity firstShipment = order.Shipments.FirstOrDefault();

            if (firstShipment != null)
            {
                return storeTypeManager.GetType(order.Store)
                    .ShippingAddressEditableState(order, firstShipment);
            }

            return ShippingAddressEditStateType.Editable;
        }

        /// <summary>
        /// Create the pre-fetch path used to load an order
        /// </summary>
        private static IPrefetchPath2 CreatePrefetchPath()
        {
            IPrefetchPath2 prefetchPath = new PrefetchPath2(EntityType.OrderEntity);

            prefetchPath.Add(OrderEntity.PrefetchPathStore);
            prefetchPath.Add(OrderEntity.PrefetchPathOrderItems);
            IPrefetchPathElement2 shipmentsPath = prefetchPath.Add(OrderEntity.PrefetchPathShipments);

            IPrefetchPathElement2 upsShipmentPath = shipmentsPath.SubPath.Add(ShipmentEntity.PrefetchPathUps);
            upsShipmentPath.SubPath.Add(UpsShipmentEntity.PrefetchPathPackages);

            IPrefetchPathElement2 postalShipmentPath = shipmentsPath.SubPath.Add(ShipmentEntity.PrefetchPathPostal);
            postalShipmentPath.SubPath.Add(PostalShipmentEntity.PrefetchPathUsps);
            postalShipmentPath.SubPath.Add(PostalShipmentEntity.PrefetchPathEndicia);

            IPrefetchPathElement2 iParcelShipmentPath = shipmentsPath.SubPath.Add(ShipmentEntity.PrefetchPathIParcel);
            iParcelShipmentPath.SubPath.Add(IParcelShipmentEntity.PrefetchPathPackages);

            shipmentsPath.SubPath.Add(ShipmentEntity.PrefetchPathOnTrac);

            shipmentsPath.SubPath.Add(ShipmentEntity.PrefetchPathAmazon);

            shipmentsPath.SubPath.Add(ShipmentEntity.PrefetchPathBestRate);

            IPrefetchPathElement2 fedexShipmentPath = shipmentsPath.SubPath.Add(ShipmentEntity.PrefetchPathFedEx);
            fedexShipmentPath.SubPath.Add(FedExShipmentEntity.PrefetchPathPackages);

            shipmentsPath.SubPath.Add(ShipmentEntity.PrefetchPathOther);

            shipmentsPath.SubPath.Add(ShipmentEntity.PrefetchPathInsurancePolicy);
            shipmentsPath.SubPath.Add(ShipmentEntity.PrefetchPathCustomsItems);
            shipmentsPath.SubPath.Add(ShipmentEntity.PrefetchPathValidatedAddress);

            return prefetchPath;
        }
    }
}
