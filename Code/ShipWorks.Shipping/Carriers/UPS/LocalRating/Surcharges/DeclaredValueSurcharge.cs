﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interapptive.Shared.Collections;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.LocalRating;

namespace ShipWorks.Shipping.Carriers.Ups.LocalRating.Surcharges
{
    /// <summary>
    /// Calculates and applies any applicable declared value surcharge
    /// </summary>
    public class DeclaredValueSurcharge : IUpsSurcharge
    {
        private readonly IDictionary<UpsSurchargeType, double> surcharges;

        /// <summary>
        /// Constructor
        /// </summary>
        public DeclaredValueSurcharge(IDictionary<UpsSurchargeType, double> surcharges)
        {
            this.surcharges = surcharges;
        }

        /// <summary>
        /// Apply the surcharge to the service rate based on the shipment
        /// </summary>
        public void Apply(UpsShipmentEntity shipment, IUpsLocalServiceRate serviceRate)
        {
            for (int index = 0; index < shipment.Packages.Count; index++)
            {
                UpsPackageEntity package = shipment.Packages[index];
                CalculateDeclaredValue(package, index + 1, serviceRate);
            }
        }

        /// <summary>
        /// Calculates the declared value.
        /// </summary>
        private void CalculateDeclaredValue(UpsPackageEntity package, int packageNumber, IUpsLocalServiceRate serviceRate)
        {
            decimal declaredValue = package.DeclaredValue;
            if (declaredValue <= 100)
            {
                return;
            }

            decimal perHundredCharge = Math.Ceiling(declaredValue / 100) *
                                      (decimal) surcharges[UpsSurchargeType.DeclaredValuePricePerHundred];

            decimal minimumCharge = (decimal) surcharges[UpsSurchargeType.DeclaredValueMinimumCharge];

            if (perHundredCharge <= minimumCharge)
            {
                AddAmount(UpsSurchargeType.DeclaredValueMinimumCharge, minimumCharge, serviceRate, packageNumber);
            }
            else
            {
                AddAmount(UpsSurchargeType.DeclaredValuePricePerHundred, perHundredCharge, serviceRate, packageNumber);
            }
        }

        /// <summary>
        /// Adds the amount.
        /// </summary>
        private void AddAmount(UpsSurchargeType surchargeType, decimal amount, IUpsLocalServiceRate serviceRate, int packageNumber)
        {
            string surchargeTypeDescription = EnumHelper.GetDescription(surchargeType);
            string surchargeName = $"Package {packageNumber} - {surchargeTypeDescription}";
            serviceRate.AddAmount(amount, surchargeName);
        }
    }
}
