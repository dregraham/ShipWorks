using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.Enums;
using ShipWorks.UI.Controls.MultiValueBinders;
using System.Reflection;
using Interapptive.Shared.Messaging;
using ShipWorks.Core.UI;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;
using ShipWorks.Shipping.Editing.Rating;
using System.Reactive.Linq;
using System.Reactive.Concurrency;
using System.Threading;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// View model for the AmazonServiceControl
    /// </summary>
    public class AmazonServiceViewModel : INotifyPropertyChanged, IDisposable
    {
        private readonly AmazonShipmentType amazonShipmentType;
        private readonly PropertyChangedHandler handler;
        public event PropertyChangedEventHandler PropertyChanged;
        private GenericMultiValueBinder<ShipmentEntity, double> weightBinder;
        private IMultiValue<AmazonDeliveryExperienceType?> deliveryExperienceBinder;
        private GenericMultiValueBinder<ShipmentEntity, AmazonRateTag> shippingServiceBinder;
        private List<AmazonRateTag> servicesAvailable;
        private readonly IDisposable amazonRatesRetrievedIDisposable;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonServiceViewModel(IObservable<IShipWorksMessage> messenger, AmazonShipmentType amazonShipmentType)
        {
            this.amazonShipmentType = amazonShipmentType;
            handler = new PropertyChangedHandler(() => PropertyChanged);

            amazonRatesRetrievedIDisposable = messenger.OfType<AmazonRatesRetrievedMessage>()
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(OnAmazonRatesRetrieved);
        }

        /// <summary>
        /// Called when [amazon rates retrieved].
        /// </summary>
        private void OnAmazonRatesRetrieved(AmazonRatesRetrievedMessage amazonRatesRetrievedMessage)
        {
            List<AmazonRateTag> services = new List<AmazonRateTag>(); 
            foreach (RateResult rate in amazonRatesRetrievedMessage.RateGroup.Rates)
            {
                services.Add((AmazonRateTag)rate.Tag);
            }

            if (!services.Any())
            {
                AmazonRateTag selectedRateTag = new AmazonRateTag { Description = "No rates are available for the shipment.", ShippingServiceId = "-1", ShippingServiceOfferId = null, CarrierName = null };
                services.Insert(0, selectedRateTag);
                ShippingService = selectedRateTag;
            }
            else if (!shippingServiceBinder.IsMultiValued && services.All(s => s.ShippingServiceId != ShippingService?.ShippingServiceId))
            {
                // The selected service id is no longer valid in the rates returned, so select the first one in the list.
                ShippingService = services.First();
            }

            ServicesAvailable = services;
            shippingServiceBinder.PropertyValue = ShippingService;
        }

        /// <summary>
        /// Load the given shipments into the view model
        /// </summary>
        public void Load(List<ShipmentEntity> shipments)
        {
            // Build a multi value binder for each of the UI controls.
            weightBinder = new GenericMultiValueBinder<ShipmentEntity, double>(shipments,
                nameof(ContentWeight),
                entity => entity.ContentWeight,
                (entity, value) => entity.ContentWeight = value,
                (entity) => entity.Processed);

            deliveryExperienceBinder = new GenericMultiValueBinder<ShipmentEntity, AmazonDeliveryExperienceType?>(shipments,
                nameof(DeliveryExperience),
                entity => (AmazonDeliveryExperienceType?)entity.Amazon.DeliveryExperience,
                (entity, value) => entity.Amazon.DeliveryExperience = (int)value.GetValueOrDefault(),
                (entity) => entity.Processed);

            SetupServices(shipments);

            // Wire up the property changed event so we can update rates.
            weightBinder.PropertyChanged += OnPropertyChanged;
            deliveryExperienceBinder.PropertyChanged += OnPropertyChanged;
            shippingServiceBinder.PropertyChanged += OnPropertyChanged;
        }

        /// <summary>
        /// Setups the servicebinder, available services, and
        /// </summary>
        private void SetupServices(List<ShipmentEntity> shipments)
        {
            if (shipments.Count != 1)
            {
                SetupServiceWhenNotOneShipment(shipments);
                return;
            }

            ShipmentEntity shipment = shipments.Single();

            if (shipments.SingleOrDefault()?.Processed ?? false)
            {
                SetupServicesForProcessedShipment(shipment);
            }
            else
            {
                SetupServiceForUnprocessedShipment(shipment);
            }
        }

        /// <summary>
        /// Setups the service for unprocessed shipment.
        /// </summary>
        private void SetupServiceForUnprocessedShipment(ShipmentEntity shipment)
        {
            ServicesAvailable = new List<AmazonRateTag>
            {
                new AmazonRateTag
                {
                    Description = "Loading services...",
                    ShippingServiceId = null,
                    ShippingServiceOfferId = null,
                    CarrierName = null
                }
            };

            shippingServiceBinder = new GenericMultiValueBinder<ShipmentEntity, AmazonRateTag>(new List<ShipmentEntity> { shipment },
                nameof(ShippingService),
                entity => ServicesAvailable.FirstOrDefault(s => s.ShippingServiceId == entity.Amazon.ShippingServiceID),
                (entity, value) =>
                {
                    if (value?.ShippingServiceId != entity.Amazon.ShippingServiceID)
                    {
                        entity.Amazon.ShippingServiceName = value?.Description ?? string.Empty;
                        entity.Amazon.ShippingServiceID = value?.ShippingServiceId ?? string.Empty;
                        entity.Amazon.ShippingServiceOfferID = value?.ShippingServiceOfferId ?? string.Empty;
                        entity.Amazon.CarrierName = value?.CarrierName ?? string.Empty;
                    }
                },
                (entity) => entity.Processed);

            ShippingService = CreateTagBasedOnShipment(shipment);
        }

        /// <summary>
        /// Setups the services for processed shipment.
        /// </summary>
        private void SetupServicesForProcessedShipment(ShipmentEntity shipment)
        {
            var tagBasedOnShipment = CreateTagBasedOnShipment(shipment);

            ServicesAvailable = new List<AmazonRateTag> { tagBasedOnShipment };

            shippingServiceBinder = new GenericMultiValueBinder<ShipmentEntity, AmazonRateTag>(new List<ShipmentEntity> { shipment },
                nameof(ShippingService),
                entity => ServicesAvailable.FirstOrDefault(),
                (entity, value) => { },
                (entity) => true);
        }

        /// <summary>
        /// Setups the service multiple.
        /// </summary>
        private void SetupServiceWhenNotOneShipment(List<ShipmentEntity> shipments)
        {
            // Because of the way Amazon services work, we will always show "multiple" when multiple shipments are loaded,
            // even if the services are the same
            shippingServiceBinder = new GenericMultiValueBinder<ShipmentEntity, AmazonRateTag>(shipments,
                nameof(ShippingService),
                entity => new AmazonRateTag(),
                (entity, value) => { },
                (entity) => true);

            ServicesAvailable = new List<AmazonRateTag>();
        }

        /// <summary>
        /// Creates the tag based on shipment.
        /// </summary>
        public AmazonRateTag CreateTagBasedOnShipment(ShipmentEntity shipment) =>
            new AmazonRateTag()
            {
                Description = shipment.Amazon.ShippingServiceName,
                ShippingServiceId = shipment.Amazon.ShippingServiceID,
                ShippingServiceOfferId = shipment.Amazon.ShippingServiceOfferID,
                CarrierName = shipment.Amazon.CarrierName
            };
        
        /// <summary>
        /// Event for property changed handling
        /// </summary>
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(sender, e);
        }

        /// <summary>
        /// Save
        /// </summary>
        public void Save(List<ShipmentEntity> shipments)
        {
            weightBinder.Save();
            deliveryExperienceBinder.Save();
            shippingServiceBinder.Save();
        }

        /// <summary>
        /// DeliveryExperience display text
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IMultiValue<AmazonDeliveryExperienceType?> DeliveryExperience => deliveryExperienceBinder;

        /// <summary>
        /// ContentWeight display text
        /// </summary>
        [Obfuscation(Exclude = true)]
        public double ContentWeight
        {
            get
            {
                return weightBinder.PropertyValue;
            }
            set
            {
                weightBinder.PropertyValue = value;
            }
        }

        /// <summary>
        /// ContentWeight is multi valued.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool ContentWeightIsMultiValued => weightBinder.IsMultiValued;

        /// <summary>
        /// ShippingServiceName display text
        /// </summary>
        [Obfuscation(Exclude = true)]
        public AmazonRateTag ShippingService
        {
            get
            {
                return shippingServiceBinder.PropertyValue;
            }
            set
            {
                shippingServiceBinder.PropertyValue = value;
                OnPropertyChanged(this, new PropertyChangedEventArgs(nameof(ShippingService)));
            }
        }

        /// <summary>
        /// ShippingServiceName is multi valued.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool ServiceIsMultiValued => shippingServiceBinder.IsMultiValued;

        /// <summary>
        /// ShippingServiceName display text
        /// </summary>
        [Obfuscation(Exclude = true)]
        public List<AmazonRateTag> ServicesAvailable
        {
            get
            {
                return servicesAvailable;
            }
            set
            {
                handler.Set(nameof(ServicesAvailable), ref servicesAvailable, value);
            }
        }

        /// <summary>
        /// Select a specific rate
        /// </summary>
        public void SelectRate(AmazonRateTag rateTag)
        {
            ShippingService = ServicesAvailable.FirstOrDefault(s => s.ShippingServiceId == rateTag.ShippingServiceId);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                amazonRatesRetrievedIDisposable?.Dispose();
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
