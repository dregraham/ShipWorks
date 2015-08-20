using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Interapptive.Shared.Business;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.Shipping.Carriers.Amazon.Enums;
using ShipWorks.Shipping.Editing;
using ShipWorks.UI.Controls;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// View model for the AmazonServiceControl
    /// </summary>
    public class AmazonServiceViewModel
    {
        private GenericMultiValueBinder<ShipmentEntity, DateTime> mustArriveByDateBinder;
        private GenericMultiValueBinder<ShipmentEntity, double> weightBinder;
        private CheckboxMultiValueBinder<ShipmentEntity> carrierWillPickUpBinder;
        private GenericMultiValueBinder<ShipmentEntity, AmazonDeliveryExperienceType> deliveryConfirmationBinder;

        /// <summary>
        /// Load the given shipments into the view model
        /// </summary>
        public void Load(List<ShipmentEntity> shipments)
        {
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
        }

        /// <summary>
        /// CarrierWillPickUp display text
        /// </summary>
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
        public bool ContentWeightIsMultiValued
        {
            get
            {
                return weightBinder.IsMultiValued;
            }
        }
    }
}
