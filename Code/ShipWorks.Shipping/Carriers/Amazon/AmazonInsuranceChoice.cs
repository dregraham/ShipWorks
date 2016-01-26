﻿using ShipWorks.Shipping.Insurance;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Utility;
using System;

namespace ShipWorks.Shipping.Carriers.Amazon
{
    /// <summary>
    /// Amazon specific version of the insurance choice
    /// </summary>
    public class AmazonInsuranceChoice : IInsuranceChoice
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonInsuranceChoice(ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));
            MethodConditions.EnsureArgumentIsNotNull(shipment.Amazon, nameof(shipment.Amazon));
            
            Shipment = shipment;
        }


        /// <summary>
        /// If the package is being insured PennyOne - only applies to FedEx\UPS shipments
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("SonarQube", "S3237:\"value\" parameters should be used", Justification = "This is specific only to Amazon.")]
        public bool? InsurancePennyOne
        {
            get { return Shipment.Amazon.CarrierName == "STAMPS_DOT_COM"; }
            set
            {
                // Since this is a derived field, we'll just ignore the setter
            }
        }

        /// <summary>
        /// The currently configured InsuranceProvider for this insurance choice
        /// </summary>
        public InsuranceProvider InsuranceProvider => InsuranceProvider.ShipWorks;

        /// <summary>
        /// The insured value of the package, if insured
        /// </summary>
        public decimal InsuranceValue
        {
            get { return Shipment.Amazon.InsuranceValue; }
            set { Shipment.Amazon.InsuranceValue = value; }
        }

        /// <summary>
        /// Indicates if insurance is on or off
        /// </summary>
        public bool Insured
        {
            get { return Shipment.Insurance; }
            set { Shipment.Insurance = value; }
        }

        /// <summary>
        /// The shipment this insurance applies to
        /// </summary>
        public ShipmentEntity Shipment { get; }
    }
}