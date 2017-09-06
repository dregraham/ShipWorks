using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Reflection;
using System.Threading;
using Interapptive.Shared.Messaging;
using Interapptive.Shared.Utility;
using ShipWorks.Core.UI;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.Api.DTOs;
using ShipWorks.Shipping.Carriers.Amazon.Enums;
using ShipWorks.UI.Controls.MultiValueBinders;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// View model for the AmazonServiceControl
    /// </summary>
    public class AmazonServiceViewModel : INotifyPropertyChanged, IDisposable
    {
        private readonly PropertyChangedHandler handler;
        public event PropertyChangedEventHandler PropertyChanged;
        private GenericMultiValueBinder<ShipmentEntity, double> weightBinder;
        private GenericMultiValueBinder<ShipmentEntity, DateTime> dateBinder;
        private IMultiValue<AmazonDeliveryExperienceType?> deliveryExperienceBinder;
        private GenericMultiValueBinder<ShipmentEntity, AmazonRateTag> shippingServiceBinder;
        private List<AmazonRateTag> servicesAvailable;
        private readonly IDisposable amazonRatesRetrievedIDisposable;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonServiceViewModel(IObservable<IShipWorksMessage> messenger)
        {
            handler = new PropertyChangedHandler(this, () => PropertyChanged);

            amazonRatesRetrievedIDisposable = messenger.OfType<AmazonRatesRetrievedMessage>()
                .ObserveOn(SynchronizationContext.Current)
                .Subscribe(OnAmazonRatesRetrieved);
        }

        /// <summary>
        /// Called when [amazon rates retrieved].
        /// </summary>
        private void OnAmazonRatesRetrieved(AmazonRatesRetrievedMessage amazonRatesRetrievedMessage)
        {
            string selectedServiceId = ShippingService?.ShippingServiceId;

            ServicesAvailable = amazonRatesRetrievedMessage.RateGroup.Rates
                .Select(x => x.Tag)
                .Cast<AmazonRateTag>()
                .DefaultIfEmpty(CreateNoRateTag())
                .ToList();

            ReselectRate(selectedServiceId);
        }

        /// <summary>
        /// Create a no-rate tag
        /// </summary>
        private AmazonRateTag CreateNoRateTag()
        {
            return new AmazonRateTag
            {
                Description = "No rates are available for the shipment.",
                ShippingServiceId = "-1",
                ShippingServiceOfferId = null,
                CarrierName = null
            };
        }

        /// <summary>
        /// Reselect the given rate, if possible
        /// </summary>
        private void ReselectRate(string selectedServiceId)
        {
            if (shippingServiceBinder.IsMultiValued)
            {
                return;
            }

            AmazonRateTag selectedRate = ServicesAvailable.FirstOrDefault(s => s.ShippingServiceId == selectedServiceId);
            if (selectedRate != null)
            {
                ShippingService = selectedRate;
            }
        }

        /// <summary>
        /// Load the given shipments into the view model
        /// </summary>
        public void Load(List<ShipmentEntity> shipments)
        {
            // Build a multi value binder for each of the UI controls.
            dateBinder = new GenericMultiValueBinder<ShipmentEntity, DateTime>(shipments,
                nameof(ShipDate),
                entity => entity.ShipDate,
                (entity, value) => entity.ShipDate = value,
                entity => entity.Processed);

            weightBinder = new GenericMultiValueBinder<ShipmentEntity, double>(shipments,
                nameof(ContentWeight),
                entity => entity.ContentWeight,
                (entity, value) => entity.ContentWeight = value,
                entity => entity.Processed);

            deliveryExperienceBinder = new GenericMultiValueBinder<ShipmentEntity, AmazonDeliveryExperienceType?>(shipments,
                nameof(DeliveryExperience),
                entity => (AmazonDeliveryExperienceType?) entity.Amazon.DeliveryExperience,
                (entity, value) => entity.Amazon.DeliveryExperience = (int) value.GetValueOrDefault(),
                entity => entity.Processed);

            SetupServices(shipments);

            // Wire up the property changed event so we can update rates.
            dateBinder.PropertyChanged += OnPropertyChanged;
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
                entity => entity.Processed);

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
                entity => true);
        }

        /// <summary>
        /// Setups the service multiple.
        /// </summary>
        private void SetupServiceWhenNotOneShipment(List<ShipmentEntity> shipments)
        {
            ServicesAvailable = CreateDefaultAvailableServiceList();

            shippingServiceBinder = new GenericMultiValueBinder<ShipmentEntity, AmazonRateTag>(shipments,
                nameof(ShippingService),
                entity => ServicesAvailable.FirstOrDefault(s => s.ShippingServiceId == entity.Amazon.ShippingServiceID),
                (entity, value) =>
                {
                    if (value?.ShippingServiceId != entity.Amazon.ShippingServiceID)
                    {
                        entity.Amazon.ShippingServiceName = value?.Description ?? string.Empty;
                        entity.Amazon.ShippingServiceID = value?.ShippingServiceId ?? string.Empty;
                        entity.Amazon.CarrierName = value?.CarrierName ?? string.Empty;
                    }
                },
                entity => entity.Processed);
        }

        public List<AmazonRateTag> CreateDefaultAvailableServiceList()
        {
            List<AmazonRateTag> rates = new List<AmazonRateTag>();

            foreach (EnumEntry<AmazonServiceType> service in EnumHelper.GetEnumList<AmazonServiceType>())
            {
                rates.Add(new AmazonRateTag()
                {
                    Description = service.Description,
                    ShippingServiceId = service.ApiValue,
                    CarrierName = service.Description.Split(' ').FirstOrDefault()
                });
            }

            return rates;
        }

        /// <summary>
        /// Creates the tag based on shipment.
        /// </summary>
        public AmazonRateTag CreateTagBasedOnShipment(ShipmentEntity shipment) =>
            new AmazonRateTag
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
            dateBinder.Save();
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
        /// The ship date
        /// </summary>
        [Obfuscation(Exclude = true)]
        public DateTime ShipDate
        {
            get
            {
                return dateBinder.PropertyValue;
            }
            set
            {
                dateBinder.PropertyValue = value;
            }
        }

        /// <summary>
        /// Is ship date multivalued
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool ShipDateIsMultiValued => dateBinder.IsMultiValued;

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
