using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        private GenericMultiValueBinder<ShipmentEntity, DateTime> mustArriveByDateBinder;
        private GenericMultiValueBinder<ShipmentEntity, double> weightBinder;
        private CheckboxMultiValueBinder<ShipmentEntity> carrierWillPickUpBinder;
        private GenericMultiValueBinder<ShipmentEntity, AmazonDeliveryExperienceType> deliveryConfirmationBinder;
        private GenericMultiValueBinder<ShipmentEntity, string> serviceBinder;
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
            mustArriveByDateBinder = new GenericMultiValueBinder<ShipmentEntity, DateTime>(shipments,
                entity => entity.Amazon.DateMustArriveBy,
                (entity, value) => entity.Amazon.DateMustArriveBy = value.Date.AddHours(12));

            weightBinder = new GenericMultiValueBinder<ShipmentEntity, double>(shipments,
                entity => entity.ContentWeight,
                (entity, value) => entity.ContentWeight = value);

            carrierWillPickUpBinder = new CheckboxMultiValueBinder<ShipmentEntity>(shipments,
                entity => entity.Amazon.CarrierWillPickUp,
                (entity, value) => entity.Amazon.CarrierWillPickUp = value);

            deliveryConfirmationBinder = new GenericMultiValueBinder<ShipmentEntity, AmazonDeliveryExperienceType>(shipments,
                entity => (AmazonDeliveryExperienceType)entity.Amazon.DeliveryExperience,
                (entity, value) => entity.Amazon.DeliveryExperience = (int)value);

            serviceBinder = new GenericMultiValueBinder<ShipmentEntity, string>(shipments,
                entity => entity.Amazon.ShippingServiceName,
                (entity, value) => entity.Amazon.ShippingServiceName = value);
        }

        /// <summary>
        /// Save
        /// </summary>
        public void Save(List<ShipmentEntity> shipments)
        {
            mustArriveByDateBinder.Save();
            weightBinder.Save();
            carrierWillPickUpBinder.Save();
            deliveryConfirmationBinder.Save();
            serviceBinder.Save();
        }

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
            set { carrierWillPickUpBinder.PropertyValue = value; }
        }

        /// <summary>
        /// CarrierWillPickUp is multi valued.
        /// </summary>
        public bool CarrierWillPickUpIsMultiValued
        {
            get
            {
                return carrierWillPickUpBinder.IsMultiValued;
            }
        }

        /// <summary>
        /// CarrierWillPickUpCheckState is multi valued.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public CheckState CarrierWillPickUpCheckState
        {
            get
            {
                return carrierWillPickUpBinder.CheckStateValue;
            }
        }

        /// <summary>
        /// DeliveryConfirmation display text
        /// </summary>
        [Obfuscation(Exclude = true)]
        public AmazonDeliveryExperienceType DeliveryConfirmation
        {
            get
            {
                return deliveryConfirmationBinder.PropertyValue;
            }
            set
            {
                deliveryConfirmationBinder.PropertyValue = value; 
            }
        }

        /// <summary>
        /// DeliveryConfirmation is multi valued.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool DeliveryConfirmationIsMultiValued
        {
            get
            {
                return deliveryConfirmationBinder.IsMultiValued;
            }
        }

        /// <summary>
        /// MustArriveByDate display text
        /// </summary>
        [Obfuscation(Exclude = true)]
        public DateTime MustArriveByDate
        {
            get
            {
                if (mustArriveByDateBinder.PropertyValue >= new DateTime(1980, 1, 1))
                {
                    return mustArriveByDateBinder.PropertyValue;
                }

                return DateTime.Now.AddDays(1);
            }
            set
            {
                mustArriveByDateBinder.PropertyValue = value; 
            }
        }

        /// <summary>
        /// MustArriveByDate is multi valued.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool MustArriveByDateIsMultiValued
        {
            get
            {
                return mustArriveByDateBinder.IsMultiValued;
            }
        }

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
            set { weightBinder.PropertyValue = value; }
        }

        /// <summary>
        /// ContentWeight is multi valued.
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool ContentWeightIsMultiValued
        {
            get
            {
                return weightBinder.IsMultiValued;
            }
        }

        /// <summary>
        /// Service display text
        /// </summary>
        public string Service
        {
            get
            {
                return serviceBinder.PropertyValue;
            }
            set { serviceBinder.PropertyValue = value; }
        }

        /// <summary>
        /// Service is multi valued.
        /// </summary>
        public bool ServiceIsMultiValued
        {
            get
            {
                return serviceBinder.IsMultiValued;
            }
        }

        /// <summary>
        /// Service display text
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
