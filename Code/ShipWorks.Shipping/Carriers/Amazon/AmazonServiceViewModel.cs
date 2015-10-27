using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon.Enums;
using ShipWorks.UI.Controls.MultiValueBinders;
using System.Reflection;
using ShipWorks.Core.UI;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// View model for the AmazonServiceControl
    /// </summary>
    public class AmazonServiceViewModel : INotifyPropertyChanged
    {
        private readonly PropertyChangedHandler handler;
        public event PropertyChangedEventHandler PropertyChanged;
        private GenericMultiValueBinder<ShipmentEntity, DateTime> dateMustArriveBy;
        private GenericMultiValueBinder<ShipmentEntity, double> weightBinder;
        private CheckboxMultiValueBinder<ShipmentEntity> carrierWillPickUpBinder;
        private CheckboxMultiValueBinder<ShipmentEntity> sendDeliverByBinder;
        private IMultiValue<AmazonDeliveryExperienceType> deliveryExperienceBinder;
        private GenericMultiValueBinder<ShipmentEntity, string> shippingServiceNameBinder;
        private List<string> servicesAvailable;

        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonServiceViewModel()
        {
            handler = new PropertyChangedHandler(() => PropertyChanged);
        }

        /// <summary>
        /// Load the given shipments into the view model
        /// </summary>
        public void Load(List<ShipmentEntity> shipments)
        {
            List<string> availableServices = new List<string>() { "Loading Services" };
            availableServices.AddRange(shipments.Where(s => s.Amazon != null).Select(s => s.Amazon.ShippingServiceName));
            ServicesAvailable = availableServices.Distinct().ToList();

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

            shippingServiceNameBinder = new GenericMultiValueBinder<ShipmentEntity, string>(shipments,
                nameof(ShippingServiceName),
                entity => entity.Amazon.ShippingServiceName,
                (entity, value) => entity.Amazon.ShippingServiceName = value);

            // Wire up the property changed event so we can update rates.
            dateMustArriveBy.PropertyChanged += OnPropertyChanged;
            weightBinder.PropertyChanged += OnPropertyChanged;
            carrierWillPickUpBinder.PropertyChanged += OnPropertyChanged;
            sendDeliverByBinder.PropertyChanged += OnPropertyChanged;
            deliveryExperienceBinder.PropertyChanged += OnPropertyChanged;
            shippingServiceNameBinder.PropertyChanged += OnPropertyChanged;
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
            dateMustArriveBy.Save();
            weightBinder.Save();
            carrierWillPickUpBinder.Save();
            sendDeliverByBinder.Save();
            deliveryExperienceBinder.Save();
            shippingServiceNameBinder.Save();
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
        public DateTime DateMustArriveBy
        {
            get
            {
                if (dateMustArriveBy.PropertyValue >= new DateTime(1980, 1, 1))
                {
                    return dateMustArriveBy.PropertyValue;
                }

                return DateTime.Now.AddDays(1);
            }
            set
            {
                dateMustArriveBy.PropertyValue = value; 
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
            set     { weightBinder.PropertyValue = value; }
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
        public string ShippingServiceName
        {
            get
            {
                return shippingServiceNameBinder.PropertyValue;
            }
            set { shippingServiceNameBinder.PropertyValue = value; }
        }

        /// <summary>
        /// ShippingServiceName is multi valued.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool ServiceIsMultiValued => shippingServiceNameBinder.IsMultiValued;

        /// <summary>
        /// ShippingServiceName display text
        /// </summary>
        [Obfuscation(Exclude = true)]
        public List<string> ServicesAvailable
        {
            get
            {
                return servicesAvailable;
            }
            set
            {
                handler.Set(nameof(ServicesAvailable), ref servicesAvailable, value);
                servicesAvailable = value; 
            }
        }
    }
}
