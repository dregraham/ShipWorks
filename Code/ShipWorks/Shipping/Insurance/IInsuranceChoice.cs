using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Insurance
{
    /// <summary>
    /// Wraps an entity to provide consist API for accessing insurance information
    /// </summary>
    public interface IInsuranceChoice
    {
        /// <summary>
        /// If the package is being insured PennyOne - only applies to FedEx\UPS shipments
        /// </summary>
        bool? InsurancePennyOne { get; set; }

        /// <summary>
        /// The currently configured InsuranceProvider for this insurance choice
        /// </summary>
        InsuranceProvider InsuranceProvider { get; }

        /// <summary>
        /// The insured value of the package, if insured
        /// </summary>
        decimal InsuranceValue { get; set; }

        /// <summary>
        /// Indicates if insurance is on or off
        /// </summary>
        bool Insured { get; set; }

        /// <summary>
        /// The shipment this insurance applies to.  There may be more than one InsuranceChoice that applies to this shipment if the shipment
        /// has more than one package.
        /// </summary>
        ShipmentEntity Shipment { get; }
    }
}