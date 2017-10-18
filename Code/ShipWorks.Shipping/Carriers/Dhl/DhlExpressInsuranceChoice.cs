using ShipWorks.Shipping.Insurance;
using ShipWorks.Data.Model.EntityClasses;
using System.Reflection;

namespace ShipWorks.Shipping.Carriers.Dhl
{
    /// <summary>
    /// Dhl Express implementation of insurance choice
    /// </summary>
    public class DhlExpressInsuranceChoice : IInsuranceChoice
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DhlExpressInsuranceChoice(ShipmentEntity shipment)
        {
            Shipment = shipment;

            InsurancePennyOne = false;
            InsuranceValue = 0;
            Insured = false;
        }

        /// <summary>
        /// If the package is being insured PennyOne - only applies to FedEx\UPS shipments
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool? InsurancePennyOne { get; set; }

        /// <summary>
        /// The currently configured InsuranceProvider for this insurance choice
        /// </summary>
        [Obfuscation(Exclude = true)]
        public InsuranceProvider InsuranceProvider => InsuranceProvider.ShipWorks;

        /// <summary>
        /// The insured value of the package, if insured
        /// </summary>
        [Obfuscation(Exclude = true)]
        public decimal InsuranceValue { get; set; }

        /// <summary>
        /// Indicates if insurance is on or off
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool Insured { get; set; }

        /// <summary>
        /// The shipment this insurance applies to
        /// </summary>
        public ShipmentEntity Shipment { get; }
    }
}
