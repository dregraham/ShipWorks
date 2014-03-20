using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Interapptive.Shared.Enums;
using Interapptive.Shared.Utility;
using ShipWorks.Data.Model.EntityClasses;
using ShipWorks.Shipping.Carriers.UPS.Enums;

namespace ShipWorks.Shipping.Carriers.UPS.OnLineTools.Api.ElementWriters
{
    /// <summary>
    /// Package weight writer for SurePost rate requests.
    /// </summary>
    public class UpsRateSurePostPackageWeightWriter : UpsSurePostPackageWeightWriter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpsSurePostPackageWeightWriter"/> class.
        /// </summary>
        /// <param name="xmlWriter">The XML writer.</param>
        /// <param name="serviceType">Type of the service.</param>
        /// <exception cref="System.ArgumentException"></exception>
        public UpsRateSurePostPackageWeightWriter(XmlWriter xmlWriter, UpsServiceType serviceType)
            : base(xmlWriter, serviceType)
        {
        }

        /// <summary>
        /// Calculate the weight of the package.  For rating, if the weight is calculated to be 0, we will return 0.1.
        /// </summary>
        public override double CalculateWeight(UpsPackageEntity packageEntity, UpsServicePackageTypeSetting upsSetting)
        {
            double weight = base.CalculateWeight(packageEntity, upsSetting);

            // So that we don't get an error, return the default weight of 0.1 if the weight is calculated to be 0.
            if (weight <= 0)
            {
                weight = 0.1;
            }

            return weight;
        }
    }
}
