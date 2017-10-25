using ShipWorks.Shipping.Insurance;
using ShipWorks.Data.Model.EntityClasses;
using System.Reflection;
using System.Diagnostics.CodeAnalysis;

namespace ShipWorks.Shipping.Carriers.Dhl
{
    /// <summary>
    /// Dhl Express implementation of insurance choice
    /// </summary>
    public class DhlExpressInsuranceChoice : IInsuranceChoice
    {
        private readonly DhlExpressPackageEntity package;

        /// <summary>
        /// Constructor
        /// </summary>
        public DhlExpressInsuranceChoice(ShipmentEntity shipment, DhlExpressPackageEntity package)
        {
            Shipment = shipment;
            this.package = package;
        }

        /// <summary>
        /// If the package is being insured PennyOne - only applies to FedEx\UPS shipments
        /// </summary>
        [SuppressMessage("SonarQube", "S3237:\"value\" parameters should be used",
            Justification = "This is specific only to DhlExpress.")]
        [Obfuscation(Exclude = true, StripAfterObfuscation = false)]
        public bool? InsurancePennyOne
        {
            get { return true; }
            set
            {
                // always use penny one
            }
        }

        /// <summary>
        /// The currently configured InsuranceProvider for this insurance choice
        /// </summary>
        [Obfuscation(Exclude = true, StripAfterObfuscation = false)]
        public InsuranceProvider InsuranceProvider => InsuranceProvider.ShipWorks;

        /// <summary>
        /// The insured value of the package, if insured
        /// </summary>
        [Obfuscation(Exclude = true, StripAfterObfuscation = false)]
        public decimal InsuranceValue
        {
            get
            {
                return package.InsuranceValue;
            }
            set
            {
                package.InsuranceValue = value;
            }
        }

        /// <summary>
        /// Indicates if insurance is on or off
        /// </summary>
        [Obfuscation(Exclude = true, StripAfterObfuscation = false)]
        public bool Insured
        {
            get
            {
                return package.Insurance;
            }
            set
            {
                package.Insurance = value;
            }
        }

        /// <summary>
        /// The shipment this insurance applies to
        /// </summary>
        public ShipmentEntity Shipment { get; }
    }
}
