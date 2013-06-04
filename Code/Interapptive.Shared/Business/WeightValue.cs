using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Interapptive.Shared.Business
{
    /// <summary>
    /// Utility class for dealing with weights
    /// </summary>
    public class WeightValue
    {
        double weightLbs;

        /// <summary>
        /// Constructor
        /// </summary>
        public WeightValue(double weightLbs)
        {
            this.weightLbs = weightLbs;
        }

        /// <summary>
        /// The total in pounds including pounds and ounces
        /// </summary>
        public double TotalWeight
        {
            get { return weightLbs; }
        }

        /// <summary>
        /// The pounds part of the weight
        /// </summary>
        public int PoundsOnly
        {
            get
            {
                return (int) Math.Floor(weightLbs);
            }
        }

        /// <summary>
        /// The ounces portion of the weight
        /// </summary>
        public double OuncesOnly
        {
            get
            {
                double whole = Math.Floor(weightLbs);
                double fraction = weightLbs - whole;

                return fraction * 16.0;
            }
        }

        /// <summary>
        /// Get the weight as the total number of ounces
        /// </summary>
        public double TotalOunces
        {
            get
            {
                return weightLbs * 16.0;
            }
        }
    }
}
