using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using Interapptive.Shared.Business;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.Amazon;
using ShipWorks.UI.Controls;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// View model for the AmazonServiceControl
    /// </summary>
    public class AmazonServiceViewModel
    {
        private TextMultiValueBinder<ShipmentEntity> carrierBinder;
        private TextMultiValueBinder<ShipmentEntity> serviceBinder;
        private TextMultiValueBinder<ShipmentEntity> trackingBinder;
        private TextMultiValueBinder<ShipmentEntity> costBinder;
        private TextMultiValueBinder<ShipmentEntity> shipDateBinder;
        private TextMultiValueBinder<ShipmentEntity> weightBinder;

        /// <summary>
        /// Load the given shipments into the view model
        /// </summary>
        public void Load(List<ShipmentEntity> shipments)
        {
            // Build a multi value binder for each of the UI controls.
            carrierBinder = new TextMultiValueBinder<ShipmentEntity>(shipments,
                entity => entity.Amazon.CarrierName,
                (entity, value) => entity.Amazon.CarrierName = value);

            serviceBinder = new TextMultiValueBinder<ShipmentEntity>(shipments,
                entity => entity.Amazon.ShippingServiceName,
                (entity, value) => entity.Amazon.ShippingServiceName = value);

            trackingBinder = new TextMultiValueBinder<ShipmentEntity>(shipments,
                entity => entity.TrackingNumber,
                (entity, value) => entity.TrackingNumber = value);

            costBinder = new TextMultiValueBinder<ShipmentEntity>(shipments,
                entity => entity.ShipmentCost.ToString(CultureInfo.InvariantCulture),
                (entity, value) => entity.ShipmentCost = decimal.Parse(value));

            shipDateBinder = new TextMultiValueBinder<ShipmentEntity>(shipments,
                entity => entity.ShipDate.ToString(CultureInfo.InvariantCulture),
                (entity, value) => entity.ShipDate = DateTime.Parse(value).Date.AddHours(12));

            weightBinder = new TextMultiValueBinder<ShipmentEntity>(shipments,
                entity => entity.ContentWeight.ToString(CultureInfo.InvariantCulture),
                (entity, value) => entity.ContentWeight = double.Parse(value));
        }

        /// <summary>
        /// Save
        /// </summary>
        public void Save(List<ShipmentEntity> shipments)
        {
            carrierBinder.Save();
            serviceBinder.Save();
            trackingBinder.Save();
            costBinder.Save();
            shipDateBinder.Save();
            weightBinder.Save();
        }

        /// <summary>
        /// Carrier display text
        /// </summary>
        public string Carrier
        {
            get
            {
                return carrierBinder.Text;
            }
            set
            {
                carrierBinder.Text = value; 
            }
        }

        /// <summary>
        /// Carrier is multi valued.
        /// </summary>
        public bool CarrierIsMultiValued
        {
            get
            {
                return carrierBinder.IsMultiValued;
            }
        }

        /// <summary>
        /// Service display text
        /// </summary>
        public string Service
        {
            get
            {
                return serviceBinder.Text;
            }
            set { serviceBinder.Text = value; }
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
        /// Tracking display text
        /// </summary>
        public string Tracking
        {
            get
            {
                return trackingBinder.Text;
            }
            set { trackingBinder.Text = value; }
        }

        /// <summary>
        /// Tracking is multi valued.
        /// </summary>
        public bool TrackingIsMultiValued
        {
            get
            {
                return trackingBinder.IsMultiValued;
            }
        }

        /// <summary>
        /// Cost display text
        /// </summary>
        public decimal Cost
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(costBinder.Text))
                {
                    return decimal.Parse(costBinder.Text);   
                }

                return 0;
            }
            set { costBinder.Text = value.ToString(CultureInfo.InvariantCulture); }
        }

        /// <summary>
        /// Cost is multi valued.
        /// </summary>
        public bool CostIsMultiValued
        {
            get
            {
                return costBinder.IsMultiValued;
            }
        }

        /// <summary>
        /// ShipDate display text
        /// </summary>
        public DateTime ShipDate
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(shipDateBinder.Text))
                {
                    return DateTime.Parse(shipDateBinder.Text);
                }

                return new DateTime(2000, 1, 1);
            }
            set
            {
                shipDateBinder.Text = value.ToString(CultureInfo.InvariantCulture); 
            }
        }

        /// <summary>
        /// ShipDate is multi valued.
        /// </summary>
        public bool ShipDateIsMultiValued
        {
            get
            {
                return shipDateBinder.IsMultiValued;
            }
        }

        /// <summary>
        /// ContentWeight display text
        /// </summary>
        public double ContentWeight
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(weightBinder.Text))
                {
                    return Double.Parse(weightBinder.Text);
                }

                return 0;
            }
            set { weightBinder.Text = value.ToString(CultureInfo.InvariantCulture); }
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
