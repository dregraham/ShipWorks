using ShipWorks.Shipping.Insurance;
using ShipWorks.Data.Model.EntityClasses;
using Interapptive.Shared.Utility;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace ShipWorks.Shipping.Carriers.Amazon.SFP
{
    /// <summary>
    /// Amazon specific version of the insurance choice
    /// </summary>
    public class AmazonSFPInsuranceChoice : IInsuranceChoice
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AmazonSFPInsuranceChoice(ShipmentEntity shipment)
        {
            MethodConditions.EnsureArgumentIsNotNull(shipment, nameof(shipment));
            MethodConditions.EnsureArgumentIsNotNull(shipment.AmazonSFP, nameof(shipment.AmazonSFP));

            Shipment = shipment;
        }


        /// <summary>
        /// If the package is being insured PennyOne - only applies to FedEx\UPS shipments
        /// </summary>
        [SuppressMessage("SonarQube", "S3237:\"value\" parameters should be used",
            Justification = "This is specific only to Amazon.")]
        [Obfuscation(Exclude = true)]
        public bool? InsurancePennyOne
        {
            get { return Shipment.AmazonSFP.CarrierName == "STAMPS_DOT_COM" || Shipment.AmazonSFP.CarrierName == "USPS"; }
            set
            {
                // Since this is a derived field, we'll just ignore the setter
            }
        }

        /// <summary>
        /// The currently configured InsuranceProvider for this insurance choice
        /// </summary>
        [Obfuscation(Exclude = true)]
        public InsuranceProvider InsuranceProvider => InsuranceProvider.ShipWorks;

        /// <summary>
        /// The insured value of the package, if insured
        /// </summary>
        [Obfuscation(Exclude = true)]
        public decimal InsuranceValue
        {
            get { return Shipment.AmazonSFP.InsuranceValue; }
            set { Shipment.AmazonSFP.InsuranceValue = value; }
        }

        /// <summary>
        /// Indicates if insurance is on or off
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool Insured
        {
            get { return Shipment.AmazonSFP.Insurance; }
            set { Shipment.AmazonSFP.Insurance = value; }
        }

        /// <summary>
        /// The shipment this insurance applies to
        /// </summary>
        public ShipmentEntity Shipment { get; }
    }
}
