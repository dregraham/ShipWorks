using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SD.LLBLGen.Pro.ORMSupportClasses;
using ShipWorks.Data.Model.EntityClasses;

namespace ShipWorks.Shipping.Insurance
{
    /// <summary>
    /// Wraps an entity to provide consist API for accessing insurance information
    /// </summary>
    public class InsuranceChoice : IInsuranceChoice
    {
        ShipmentEntity shipment;

        EntityBase2 insuranceFieldEntity;
        string insuranceFieldPrefix;

        EntityBase2 valueFieldEntity;
        string valueFieldPrefix;

        EntityBase2 pennyOneEntity;
        string pennyOneFieldPrefix;

        /// <summary>
        /// Constructor
        /// </summary>
        public InsuranceChoice(ShipmentEntity shipment, EntityBase2 insuranceFieldEntity, EntityBase2 valueFieldEntity, EntityBase2 pennyOneEntity)
        {
            if (shipment == null)
            {
                throw new ArgumentNullException("shipment");
            }

            if (insuranceFieldEntity == null)
            {
                throw new ArgumentNullException("insuranceFieldEntity");
            }

            if (valueFieldEntity == null)
            {
                throw new ArgumentNullException("valueFieldEntity");
            }

            this.shipment = shipment;

            this.insuranceFieldEntity = insuranceFieldEntity;
            this.insuranceFieldPrefix = "";

            this.valueFieldEntity = valueFieldEntity;
            this.valueFieldPrefix = "";

            this.pennyOneEntity = pennyOneEntity;
            this.pennyOneFieldPrefix = "";
        }

        /// <summary>
        /// The shipment this insurance applies to.  There may be more than one InsuranceChoice that applies to this shipment if the shipment
        /// has more than one package.
        /// </summary>
        public ShipmentEntity Shipment
        {
            get { return shipment; }
        }

        /// <summary>
        /// Indicates if insurance is on or off
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool Insured
        {
            get { return (bool) insuranceFieldEntity.Fields[insuranceFieldPrefix + "Insurance"].CurrentValue; }
            set { insuranceFieldEntity.SetNewFieldValue(insuranceFieldPrefix + "Insurance", value); }
        }

        /// <summary>
        /// The currently configured InsuranceProvider for this insurance choice
        /// </summary>
        [Obfuscation(Exclude = true)]
        public InsuranceProvider InsuranceProvider
        {
            get { return (InsuranceProvider) shipment.InsuranceProvider; }
        }

        /// <summary>
        /// The insured value of the package, if insured
        /// </summary>
        [Obfuscation(Exclude = true)]
        public decimal InsuranceValue
        {
            get { return (decimal) valueFieldEntity.Fields[valueFieldPrefix + "InsuranceValue"].CurrentValue; }
            set { valueFieldEntity.SetNewFieldValue(valueFieldPrefix + "InsuranceValue", value); }
        }

        /// <summary>
        /// If the package is being insured PennyOne - only applies to FedEx\UPS shipments
        /// </summary>
        [Obfuscation(Exclude = true)]
        public bool? InsurancePennyOne
        {
            get
            {
                if (pennyOneEntity != null)
                {
                    return (bool) pennyOneEntity.Fields[pennyOneFieldPrefix + "InsurancePennyOne"].CurrentValue;
                }

                return null;
            }
            set
            {
                if (pennyOneEntity == null)
                {
                    throw new InvalidOperationException("The pennyOneEntity has not been set.");
                }

                pennyOneEntity.SetNewFieldValue(pennyOneFieldPrefix + "InsurancePennyOne", value);
            }
        }

        /// <summary>
        /// Are all the insurance shipments FedEx?
        /// </summary>
        public static bool AllFedExShipments(IEnumerable<IInsuranceChoice> choices)
        {
            return choices.All(c => ((ShipmentTypeCode)c.Shipment.ShipmentType) == ShipmentTypeCode.FedEx);
        }

        /// <summary>
        /// Are all the insurance shipments Ups?
        /// </summary>
        public static bool AllUpsShipments(IEnumerable<IInsuranceChoice> choices)
        {
            return choices.All(c => ((ShipmentTypeCode)c.Shipment.ShipmentType == ShipmentTypeCode.UpsOnLineTools || 
                                     (ShipmentTypeCode)c.Shipment.ShipmentType == ShipmentTypeCode.UpsWorldShip));
        }

        /// <summary>
        /// Are all the insurance shipments OnTrac?
        /// </summary>
        public static bool AllOnTracShipments(IEnumerable<IInsuranceChoice> choices)
        {
            return choices.All(c => ((ShipmentTypeCode)c.Shipment.ShipmentType) == ShipmentTypeCode.OnTrac);
        }

        /// <summary>
        /// Are all the insurance shipments iParcel?
        /// </summary>
        public static bool AlliParcelShipments(IEnumerable<IInsuranceChoice> choices)
        {
            return choices.All(c => ((ShipmentTypeCode)c.Shipment.ShipmentType) == ShipmentTypeCode.iParcel);
        }

        /// <summary>
        /// Are all the insurance shipments Endicia?
        /// </summary>
        public static bool AllEndiciaShipments(IEnumerable<IInsuranceChoice> choices)
        {
            return choices.All(c => ((ShipmentTypeCode)c.Shipment.ShipmentType) == ShipmentTypeCode.Endicia);
        }

        /// <summary>
        /// Are all the insurance shipments Usps?
        /// </summary>
        public static bool AllUspsShipments(IEnumerable<IInsuranceChoice> choices)
        {
            return choices.All(c => ((ShipmentTypeCode)c.Shipment.ShipmentType) == ShipmentTypeCode.Usps);
        }
    }
}
