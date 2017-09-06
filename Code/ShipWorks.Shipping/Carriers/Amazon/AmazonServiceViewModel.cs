using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
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
    public class AmazonServiceViewModel : INotifyPropertyChanged
    {
        private readonly PropertyChangedHandler handler;
        public event PropertyChangedEventHandler PropertyChanged;
        private GenericMultiValueBinder<ShipmentEntity, double> weightBinder;
        private GenericMultiValueBinder<ShipmentEntity, DateTime> dateBinder;
        private IMultiValue<AmazonDeliveryExperienceType?> deliveryExperienceBinder;
        private GenericMultiValueBinder<ShipmentEntity, AmazonRateTag> shippingServiceBinder;
        private List<AmazonRateTag> servicesAvailable;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonServiceViewModel()
        {
            handler = new PropertyChangedHandler(this, () => PropertyChanged);
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
        /// Setups the services.
        /// </summary>
        private void SetupServices(List<ShipmentEntity> shipments)
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
    }
}
