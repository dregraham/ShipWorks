using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
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

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// View model for the AmazonServiceControl
    /// </summary>
    public class AmazonServiceViewModel : INotifyPropertyChanged, IDisposable
    {
        private readonly IMessenger messenger;
        private readonly PropertyChangedHandler handler;
        public event PropertyChangedEventHandler PropertyChanged;
        private GenericMultiValueBinder<ShipmentEntity, DateTime> dateMustArriveBy;
        private GenericMultiValueBinder<ShipmentEntity, double> weightBinder;
        private CheckboxMultiValueBinder<ShipmentEntity> carrierWillPickUpBinder;
        private CheckboxMultiValueBinder<ShipmentEntity> sendDeliverByBinder;
        private IMultiValue<AmazonDeliveryExperienceType> deliveryExperienceBinder;
        private GenericMultiValueBinder<ShipmentEntity, KeyValuePair<string, AmazonRateTag>> shippingServiceBinder;
        private List<KeyValuePair<string,AmazonRateTag>> servicesAvailable;
        private readonly MessengerToken amazonRatesRetrievedMessengerToken;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonServiceViewModel(IMessenger messenger)
        {
            this.messenger = messenger;
            handler = new PropertyChangedHandler(() => PropertyChanged);

            amazonRatesRetrievedMessengerToken = messenger.Handle<AmazonRatesRetrievedMessage>(this, OnAmazonRatesRetrieved);
        }

        /// <summary>
        /// Called when [amazon rates retrieved].
        /// </summary>
        private void OnAmazonRatesRetrieved(AmazonRatesRetrievedMessage amazonRatesRetrievedMessage)
        {
            RateGroup rateGroup = amazonRatesRetrievedMessage.RateGroup;

            List<KeyValuePair<string, AmazonRateTag>> services = rateGroup.Rates.Select(r => new KeyValuePair<string, AmazonRateTag>(r.Description, (AmazonRateTag) r.Tag)).ToList();
            services.Insert(0, new KeyValuePair<string, AmazonRateTag>("Please select a service", null));

            ServicesAvailable = services;
        }

        /// <summary>
        /// Load the given shipments into the view model
        /// </summary>
        public void Load(List<ShipmentEntity> shipments)
        {
            ServicesAvailable = new List<KeyValuePair<string, AmazonRateTag>> { new KeyValuePair<string, AmazonRateTag>("Loading Services", null) };

            // Build a multi value binder for each of the UI controls.
            dateMustArriveBy = new GenericMultiValueBinder<ShipmentEntity, DateTime>(shipments,
                nameof(DateMustArriveBy),
                entity => entity.Amazon.DateMustArriveBy,
                (entity, value) => entity.Amazon.DateMustArriveBy = value.Date.AddHours(12));

            weightBinder = new GenericMultiValueBinder<ShipmentEntity, double>(shipments,
                nameof(ContentWeight),
                entity => entity.ContentWeight,
                (entity, value) => entity.ContentWeight = value);

            carrierWillPickUpBinder = new CheckboxMultiValueBinder<ShipmentEntity>(shipments,
                nameof(CarrierWillPickUp),
                entity => entity.Amazon.CarrierWillPickUp,
                (entity, value) => entity.Amazon.CarrierWillPickUp = value);

            sendDeliverByBinder = new CheckboxMultiValueBinder<ShipmentEntity>(shipments,
                nameof(SendDeliverBy),
                entity => entity.Amazon.SendDateMustArriveBy,
                (entity, value) => entity.Amazon.SendDateMustArriveBy = value);

            deliveryExperienceBinder = new GenericMultiValueBinder<ShipmentEntity, AmazonDeliveryExperienceType>(shipments,
                nameof(DeliveryExperience),
                entity => (AmazonDeliveryExperienceType)entity.Amazon.DeliveryExperience,
                (entity, value) => entity.Amazon.DeliveryExperience = (int)value);

            shippingServiceBinder = new GenericMultiValueBinder<ShipmentEntity, KeyValuePair<string, AmazonRateTag>>(shipments,
                nameof(ShippingService),
                entity => ServicesAvailable.FirstOrDefault(s => s.Key == entity.Amazon.ShippingServiceName),
                (entity, value) =>
                {
                    entity.Amazon.ShippingServiceName = value.Value == null ? string.Empty : value.Key;
                    entity.Amazon.ShippingServiceID = value.Value?.ShippingServiceId;
                    entity.Amazon.ShippingServiceOfferID = value.Value?.ShippingServiceOfferId;
                });

            // Wire up the property changed event so we can update rates.
            dateMustArriveBy.PropertyChanged += OnPropertyChanged;
            weightBinder.PropertyChanged += OnPropertyChanged;
            carrierWillPickUpBinder.PropertyChanged += OnPropertyChanged;
            sendDeliverByBinder.PropertyChanged += OnPropertyChanged;
            deliveryExperienceBinder.PropertyChanged += OnPropertyChanged;
            shippingServiceBinder.PropertyChanged += OnPropertyChanged;
        }

        /// <summary>
        /// Event for property changed handling
        /// </summary>
        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e) => 
            PropertyChanged?.Invoke(sender, e);

        /// <summary>
        /// Save
        /// </summary>
        public void Save(List<ShipmentEntity> shipments)
        {
            weightBinder.Save();
            carrierWillPickUpBinder.Save();
            sendDeliverByBinder.Save();
            deliveryExperienceBinder.Save();
            shippingServiceBinder.Save();
        }

        /// <summary>
        /// SendDeliverBy display text
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool SendDeliverBy
        {
            get
            {
                return sendDeliverByBinder.PropertyValue;
            }
            set
            {
                sendDeliverByBinder.PropertyValue = value;
            }
        }

        /// <summary>
        /// SendDeliverBy is multi valued.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool SendDeliverByIsMultiValued => sendDeliverByBinder.IsMultiValued;

        /// <summary>
        /// SendDeliverByCheckState is multi valued.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public CheckState SendDeliverByCheckState => sendDeliverByBinder.CheckStateValue;

        /// <summary>
        /// CarrierWillPickUp display text
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool CarrierWillPickUp
        {
            get
            {
                return carrierWillPickUpBinder.PropertyValue;
            }
            set
            {
                carrierWillPickUpBinder.PropertyValue = value;
            }
        }

        /// <summary>
        /// CarrierWillPickUp is multi valued.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool CarrierWillPickUpIsMultiValued => carrierWillPickUpBinder.IsMultiValued;

        /// <summary>
        /// CarrierWillPickUpCheckState is multi valued.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public CheckState CarrierWillPickUpCheckState => carrierWillPickUpBinder.CheckStateValue;

        /// <summary>
        /// DeliveryExperience display text
        /// </summary>
        [Obfuscation(Exclude = true)]
        public IMultiValue<AmazonDeliveryExperienceType> DeliveryExperience => deliveryExperienceBinder;

        /// <summary>
        /// DateMustArriveBy display text
        /// </summary>
        [Obfuscation(Exclude = true)]
        public string DateMustArriveBy
        {
            get
            {
                if (dateMustArriveBy.PropertyValue >= new DateTime(1980, 1, 1))
                {
                    return dateMustArriveBy.PropertyValue.ToString("(MM/dd/yyyy)");
                }

                return DateTime.Now.AddDays(1).ToString("(MM/dd/yyyy)");
            }
        }

        /// <summary>
        /// DateMustArriveBy is multi valued.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool DateMustArriveByIsMultiValued => dateMustArriveBy.IsMultiValued;

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
        public KeyValuePair<string, AmazonRateTag> ShippingService
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
        public List<KeyValuePair<string, AmazonRateTag>> ServicesAvailable
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
            ShippingService = ServicesAvailable.FirstOrDefault(s => s.Value == rateTag);
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (amazonRatesRetrievedMessengerToken != null)
                {
                    messenger.Remove(amazonRatesRetrievedMessengerToken);
                }
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
